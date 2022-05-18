using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class Model
    {
        public Geometry Geometry { get; set; }

        public Material Material { get; set; }

        private Matrix4 transform;
        public Matrix4 Transform
        {
            get
            {
                return transform;
            }
            set
            {
                this.transform = value;
                this.InverseTransform = Matrix4.Invert(this.Transform);
                this.NormalTransform = Matrix4.Transpose(this.InverseTransform);
            }
        }

        public Matrix4 InverseTransform { get; private set; }
        public Matrix4 NormalTransform { get; private set; }
        public Model()
        {
            this.Transform = Matrix4.Identity;
        }

        public virtual Material EvaluateMaterialAt(Intersection intersect)
        {
            return this.Material;
        }

        // Check the model for intersections with the given ray
        public bool Intersect(Ray ray, out Intersection intersect)
        {
            Ray transformed_ray = Ray.Transform(this.InverseTransform, ray);
            if(this.Geometry.Intersect(transformed_ray, out intersect)) // check the geometry for intersections
            {
                intersect.Material = this.EvaluateMaterialAt(intersect);

                intersect.Position = Vector4.Transform(new Vector4(intersect.Position, 1.0f), this.Transform).Xyz;
                intersect.Distance = (ray.Origin - intersect.Position).Length;
                intersect.Normal = Vector4.Transform(new Vector4(intersect.Normal, 0.0f), this.NormalTransform).Xyz;
                return true;
            }
            return false;
        }
    }
}
