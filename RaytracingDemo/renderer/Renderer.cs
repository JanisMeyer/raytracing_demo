using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    public class Renderer
    {
        public int RecursionDepth { get; set; }
        public Renderer()
        {
            this.RecursionDepth = 5;
        }

        // Trace recursively up to the given depth using the provided ray
        public Vector4 TraceRecursively(Camera camera, Ray ray, Scene scene, int depth)
        {
            if (depth > this.RecursionDepth)
                return new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

            Vector4 color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            if(this.CastRay(ray, scene, out Intersection intersect))
            {
                // Evaluate the color at the given intersection
                color += new Vector4(EvaluatePhong(scene, camera, intersect), 0.0f);
                if (intersect.Material.Reflection > 0.0f) // Evaluate reflection (recursively cast new ray)
                {
                    Vector3 reflect_dir = Vector3.Normalize(ray.Direction - 2.0f * Vector3.Dot(intersect.Normal, ray.Direction) * intersect.Normal);
                    color += this.TraceRecursively(camera, new Ray(intersect.Position + 0.0001f * reflect_dir, reflect_dir), scene, depth + 1);
                }
                if (intersect.Material.Transmission > 0.0f)
                {
                    
                    color += new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
            return color;
        }

        // Evaluate the color at a point in the scene using the Phong Model
        public Vector3 EvaluatePhong(Scene scene, Camera camera, Intersection intersect)
        {
            Vector3 color = new Vector3(0.0f, 0.0f, 0.0f);
            foreach (Light light in scene.Lights)
            {
                Vector3 contrib = new Vector3(0.0f, 0.0f, 0.0f);

                Vector3 L = Vector3.Normalize(light.Position - intersect.Position);
                float dist = (light.Position - intersect.Position).Length;
                Ray s_ray = new Ray(intersect.Position + 0.0001f * L, L);

                bool visible = !this.CastShadowRay(s_ray, scene, dist);
                if (visible)
                {
                    Vector3 V = Vector3.Normalize(camera.Position - intersect.Position);
                    Vector3 L_R = Vector3.Normalize(-2.0f * Vector3.Dot(intersect.Normal, -L) * intersect.Normal - L);

                    Vector3 diffuse = intersect.Material.Diffuse * Math.Max(0.0f, Vector3.Dot(intersect.Normal, L));
                    contrib += diffuse;

                    Vector3 specular = intersect.Material.Specular * Math.Max(0.0f, (float)Math.Pow(Vector3.Dot(V, L_R), intersect.Material.PhongExponent));
                    contrib += specular;
                }

                Vector3 ambient = intersect.Material.Ambient;
                contrib += ambient;

                contrib = light.Intensity * contrib / (dist * dist);
                color += contrib;
            }
            return color;
        }

        // Cast a ray into the scene and check for intersections
        public bool CastRay(Ray ray, Scene scene, out Intersection intersect)
        {
            intersect = null;
            Intersection _intersect;
            foreach(Model model in scene)
            {
                if(model.Intersect(ray, out _intersect))
                {
                    if((intersect == null || _intersect.Distance < intersect.Distance) && _intersect.Distance > 0.0f)
                    {
                        intersect = _intersect;
                    }
                }
            }
            return intersect != null;
        }

        // Cast a shadow ray (i.e. check for no intersection)
        public bool CastShadowRay(Ray ray, Scene scene, float max)
        {
            foreach(Model model in scene)
                if (model.Intersect(ray, out Intersection intersection))
                    if(intersection.Distance < max && intersection.Distance > 0.0f)
                        return true;
            return false;
        }

        // Render the scene
        public byte[] RenderScene(Scene scene, Camera camera, int width, int height)
        {
            float inv_w = 1.0f / width;
            float inv_h = 1.0f / height;
            byte[] image = new byte[width * height * 4];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float x = 2.0f * (i + 0.5f) * inv_w - 1.0f;
                    float y = 2.0f * (j + 0.5f) * inv_h - 1.0f;
                    Ray ray = camera.Cast(x, y);
                    Vector4 color = TraceRecursively(camera, ray, scene, 0);
                    for (int k = 0; k < 4; k++)
                    {
                        image[(j * width + i) * 4 + k] = (byte)(Math.Min(Math.Pow(color[k], 1.0f/2.2f), 1.0f) * 255.0f);
                    }
                }
            }
            return image;
        }
    }
}
