using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    public class Ray
    {
        public Vector3 Origin { get; set; }

        public Vector3 Direction { get; set; }

        public Vector3 this[float t]
        {
            get { return this.Origin + t * this.Direction; }
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this.Direction = Vector3.Normalize(direction);
        }

        public static Ray Transform(Matrix4 transform, Ray ray)
        {
            Vector4 o = Vector4.Transform(new Vector4(ray.Origin, 1.0f), transform);
            Vector4 d = Vector4.Transform(new Vector4(ray.Direction, 0.0f), transform);
            return new Ray(o.Xyz, Vector3.Normalize(d.Xyz));
        }
    }
}
