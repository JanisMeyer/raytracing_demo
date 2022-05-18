using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class PlanetModel : Model
    {
        private Bitmap texture; 
        public PlanetModel(string texture)
        {
            this.Geometry = new Sphere()
            {
                Center = new Vector3(0.0f, 0.0f, 0.0f),
                Radius = 1.0f
            };
            this.texture = new Bitmap(Image.FromFile(texture));
        }

        public override Material EvaluateMaterialAt(Intersection intersect)
        {
            float lon = (float)Math.Atan2(intersect.Normal.Z, intersect.Normal.X) * (1.0f / (float)Math.PI);
            float lat = (float)Math.Acos(intersect.Normal.Y) * (1.0f / (float)Math.PI);
            Color px = this.texture.GetPixel((int)((lon * 0.5f + 0.5f) * this.texture.Width), (int)((1.0f - lat) * this.texture.Height));
            float r = (float)Math.Pow(px.R / 255.0f, 2.2f);
            float g = (float)Math.Pow(px.G / 255.0f, 2.2f);
            float b = (float)Math.Pow(px.B / 255.0f, 2.2f);
            return new Material()
            {
                Diffuse = new Vector3(r, g, b)
            };
        }
    }
}
