using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    public abstract class Geometry
    {
        public abstract bool Intersect(Ray ray, out Intersection intersect);
    }
}
