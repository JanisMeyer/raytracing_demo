using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    public class Camera
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.ViewMatrix = Matrix4.LookAt(this.Position, this.Position + this.Direction, new Vector3(0.0f, 1.0f, 0.0f));
            }
        }

        private Vector3 direction;
        public Vector3 Direction
        {
            get { return this.direction; }
            set
            {
                this.direction = Vector3.Normalize(value);
                this.ViewMatrix = Matrix4.LookAt(this.Position, this.Position + this.Direction, new Vector3(0.0f, 1.0f, 0.0f));
            }
        }

        private Matrix4 viewMatrix;
        public Matrix4 ViewMatrix
        {
            get
            {
                return this.viewMatrix;
            }
            private set
            {
                this.viewMatrix = value;
                this.InverseViewMatrix = Matrix4.Invert(this.ViewMatrix);
            }
        }

        public Matrix4 InverseViewMatrix { get; private set; }
        public float Near { get; set; }

        private float scale;
        private float fieldOfView;
        public float FieldOfView
        {
            get { return this.fieldOfView; }
            set
            {
                this.fieldOfView = value;
                this.scale = (float)Math.Tan(this.FieldOfView * Math.PI/180.0 * 0.5);
            }
        }
        public float AspectRatio { get; set; }
        public Camera()
        {
            this.Position = new Vector3(0.0f, 0.0f, 0.0f);
            this.Direction = new Vector3(0.0f, 0.0f, -1.0f);
            this.Near = 1.0f;
            this.FieldOfView = 90.0f;
            this.AspectRatio = 1.0f;
        }

        // Use the camera to cast a ray through the given pixel
        public Ray Cast(float x, float y)
        {   
            Vector3 target = new Vector3(Vector4.Transform(new Vector4(x * this.scale * this.AspectRatio, y * this.scale, -this.Near, 1.0f), this.InverseViewMatrix));
            return new Ray(this.Position, Vector3.Normalize(target - this.Position));
        }
    }
}
