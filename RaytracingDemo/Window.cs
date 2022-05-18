using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    class Window : GameWindow
    {

        private int program;
        private int vertexArray;
        private int indexBuffer;
        private int texture;

        private Renderer renderer;
        private Camera camera;
        private Scene scene;

        public Window()
            : base(1580, 760, GraphicsMode.Default, "Raytracing Demo", GameWindowFlags.Default, DisplayDevice.Default)
        {
            this.X = 0;
            this.Y = 0;
            this.renderer = new Renderer(); // Software-Renderer
            this.camera = new Camera()
            {
                Position = new Vector3(0.0f, 1.0f, 7.5f),
                Direction = new Vector3(0.0f, -0.25f, -1.0f),
                AspectRatio = (float)this.ClientSize.Width / (float)this.ClientSize.Height,
                FieldOfView = 45.0f
            };
            this.scene = new Scene();

            // Construct Scene of several spheres, planes and lights
            this.scene.Add(new Model()
            {
                Geometry = new Sphere(),
                Material = new Material()
                {
                    Diffuse = new Vector3(0.0f, 1.0f, 0.0f)
                }
            });

            this.scene.Add(new Model()
            {
                Geometry = new Sphere(),
                Material = new Material()
                {
                    Diffuse = new Vector3(0.0f, 1.0f, 1.0f)
                },
                Transform = Matrix4.CreateTranslation(0.0f, -1.0f, 2.0f)
            });

            this.scene.Add(new Model()
            {
                Geometry = new Sphere(),
                Material = new Material()
                {
                    Diffuse = new Vector3(1.0f, 0.0f, 0.0f)
                },
                Transform = Matrix4.CreateTranslation(-2.5f, 0.0f, 0.0f)
            });

            this.scene.Add(new Model()
            {
                Geometry = new Sphere(),
                Material = new Material()
                {
                    Diffuse = new Vector3(0.0f, 0.0f, 1.0f)
                },
                Transform = Matrix4.CreateTranslation(2.5f, 0.0f, 0.0f)
            });

            this.scene.Add(new Model()
            {
                Geometry = new Sphere(),
                Material = new Material()
                {
                    Diffuse = new Vector3(0.0f, 1.0f, 0.0f)
                }
            });

            this.scene.Add(new Model()
            {
                Geometry = new Plane()
                {
                    Normal = new Vector3(0.0f, 1.0f, 0.0f),
                    Distance = -2.0f
                },
                Material = new Material()
                {
                    Diffuse = new Vector3(1.0f, 1.0f, 0.0f),
                    Reflection = 1.0f
                }
            });

            this.scene.Add(new Model()
            {
                Geometry = new Plane()
                {
                    Normal = new Vector3(0.0f, 0.0f, 1.0f),
                    Distance = -10.0f
                },
                Material = new Material()
                {
                    Diffuse = new Vector3(1.0f, 1.0f, 1.0f),
                    Reflection = 1.0f
                }
            });

            this.scene.Lights.Add(new Light()
            {
                Position = new Vector3(1.0f, 1.0f, 1.0f)
            });

            this.scene.Lights.Add(new Light()
            {
                Position = new Vector3(-1.0f, 1.0f, 2.0f)
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Create GPU Program with simple Vertex and Fragment shaders
            int vs = GL.CreateShader(ShaderType.VertexShader);
            using (StreamReader sr = new StreamReader(new FileStream("shaders/VertexShader.glsl", FileMode.Open), Encoding.UTF8))
                GL.ShaderSource(vs, sr.ReadToEnd());
            GL.CompileShader(vs);

            int fs = GL.CreateShader(ShaderType.FragmentShader);
            using (StreamReader sr = new StreamReader(new FileStream("shaders/FragmentShader.glsl", FileMode.Open), Encoding.UTF8))
                GL.ShaderSource(fs, sr.ReadToEnd());
            GL.CompileShader(fs);

            this.program = GL.CreateProgram();
            GL.AttachShader(program, vs);
            GL.AttachShader(program, fs);
            GL.LinkProgram(program);

            // Simple rectangle with texture on it to display software-rendered screen
            float[] pos = new float[]
            {
                -1.0f, -1.0f, 0.0f,
                1.0f, -1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                -1.0f, 1.0f, 0.0f
            };

            float[] tc = new float[]
            {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f, 1.0f,
                0.0f, 1.0f
            };

            int[] indices = new int[]
            {
                0, 1, 3, 
                1, 2, 3
            };

            this.vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);

            int posBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, posBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, pos.Length * sizeof(float), pos, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            int tcBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, tc.Length * sizeof(float), tc, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            this.indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            this.texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, this.ClientSize.Width, this.ClientSize.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            int loc = GL.GetUniformLocation(this.program, "tex");

            // Render Scene to Texture with Software Raytracer
            byte[] image = renderer.RenderScene(this.scene, this.camera, this.ClientSize.Width, this.ClientSize.Height);
            SaveImage(image, this.ClientSize.Width, this.ClientSize.Height);

            GL.BindTexture(TextureTarget.Texture2D, this.texture);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image);
            GL.UseProgram(this.program);
            GL.Uniform1(loc, 0);

            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.Blend);
        }

        private void SaveImage(byte[] image, int width, int height) {
            Bitmap bitmap = new Bitmap(width, height);
            for (int j = 0; j < height; j++) {
                for (int i = 0; i < width; i++) {
                    bitmap.SetPixel(i, height - j - 1,
                        Color.FromArgb(image[(j * width + i) * 4 + 3], image[(j * width + i) * 4 + 0], image[(j * width + i) * 4 + 1], image[(j * width + i) * 4 + 2]));
                }
            }
            bitmap.Save("output.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Simply render the rectangle with its texture

            GL.Viewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.UseProgram(this.program);
            GL.BindVertexArray(this.vertexArray);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.indexBuffer);

            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            this.SwapBuffers();
        }

        protected override void OnDisposed(EventArgs e)
        {
            base.OnDisposed(e);

            GL.DeleteProgram(this.program);
            GL.DeleteVertexArray(this.vertexArray);
            GL.DeleteBuffer(this.indexBuffer);
            GL.DeleteTexture(this.texture);
        }
    }
}
