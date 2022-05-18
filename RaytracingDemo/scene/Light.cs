using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public float Intensity { get; set; }
        public Light()
        {
            this.Position = new Vector3(0.0f, 0.0f, 0.0f);
            this.Intensity = 1.0f;
        }
    }
}
