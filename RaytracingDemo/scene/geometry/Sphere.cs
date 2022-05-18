using OpenTK;
using System;

namespace RaytracingDemo
{
    public class Sphere : Geometry
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public Sphere()
        {
            this.Center = new Vector3(0.0f, 0.0f, 0.0f);
            this.Radius = 1.0f;
        }

        public override bool Intersect(Ray ray, out Intersection intersect)
        {
            intersect = null;
            float t0, t1;
            Vector3 L = this.Center - ray.Origin; 
            float tca = Vector3.Dot(L, ray.Direction); 
            if (tca < 0) 
                return false; 
            float d2 = Vector3.Dot(L, L) - tca * tca; 
            if (d2 > this.Radius) 
                return false; 
            float thc = (float)Math.Sqrt(this.Radius - d2); 
            t0 = tca - thc; 
            t1 = tca + thc;
            float t = -1.0f;
            if (t0 > t1 && t1 > 0)
                t = t1;
            else if (t0 > 0 && t0 > 0 )
                t = t0;
            if (t < 0)
                return false;
            intersect = new Intersection()
            {
                Distance = t,
                Position = ray[t],
                Normal = Vector3.Normalize(ray[t] - this.Center)
            };
            return true;
        }
    }
}
