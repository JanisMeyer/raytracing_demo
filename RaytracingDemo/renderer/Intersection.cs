using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class Intersection
    {
        public float Distance { get; set; }
        public Vector3 Normal { get; set; }
        public Material Material { get; set; }
        public Vector3 Position { get; set; }
        public Intersection()
        {

        }
    }
}
