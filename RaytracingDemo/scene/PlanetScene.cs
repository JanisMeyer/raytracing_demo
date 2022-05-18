using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RaytracingDemo
{
    public class PlanetScene : Scene
    {
        public PlanetScene()
        {
            Model m = new PlanetModel("../../8k_earth_daymap.jpg");
            m.Transform = Matrix4.CreateRotationY(-1.25f) * Matrix4.CreateRotationX(-(float)Math.PI / 4.0f) * Matrix4.CreateRotationZ((float)Math.PI);
            this.Add(m);

            m = new PlanetModel("../../8k_sun.jpg");
            m.Transform = Matrix4.CreateTranslation(2.5f, 0.0f, 0.0f);
            this.Add(m);

            m = new PlanetModel("../../8k_moon.jpg");
            m.Transform = Matrix4.CreateTranslation(-2.5f, 0.0f, 0.0f);
            this.Add(m);

            this.Lights.Add(new Light()
            {
                Position = new Vector3(0.0f, 1.0f, 2.0f),
                Intensity = 1.0f
            });
        }
    }
}
