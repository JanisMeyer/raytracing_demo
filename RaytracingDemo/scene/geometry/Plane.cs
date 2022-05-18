using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    class Plane : Geometry
    {
        public Vector3 Normal { get; set; }
        public float Distance { get; set; }

        public Plane()
        {
            this.Normal = new Vector3(1.0f, 0.0f, 0.0f);
            this.Distance = 0.0f;
        }

        public override bool Intersect(Ray ray, out Intersection intersect)
        {
            intersect = null;
            float dn = Vector3.Dot(ray.Direction, this.Normal);
            if (dn == 0) // Parallel
                return false;
            float t = (this.Distance - Vector3.Dot(ray.Origin, this.Normal)) / dn;
            if (t < 0.0f)
                return false;
            intersect = new Intersection()
            {
                Distance = t,
                Position = ray[t],
                Normal = this.Normal
            };
            return true;
        }
    }
}
