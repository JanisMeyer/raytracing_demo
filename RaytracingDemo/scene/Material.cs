using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class Material
    {
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public Vector3 Ambient { get; set; }
        public float PhongExponent { get; set; }
        public float Reflection { get; set; }
        public float Transmission { get; set; }
        public Material()
        {
            this.Diffuse = new Vector3(0.0f, 0.0f, 0.0f);
            this.Specular = new Vector3(0.0f, 0.0f, 0.0f);
            this.Ambient = new Vector3(0.1f, 0.1f, 0.1f);
            this.PhongExponent = 5.0f;
            this.Reflection = 0.0f;
            this.Transmission = 0.0f;
        }
    }
}
