using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LSPainter
{
    public struct WindowLayout
    {
        public int Rows { get; }
        public int Columns { get; }
        public bool ShowOriginal { get; }
        public int NCanvases { get; }

        public WindowLayout(int rows, int columns, bool showOriginal, int nCanvases)
        {
            Rows = rows;
            Columns = columns;
            ShowOriginal = showOriginal;
            NCanvases = nCanvases;

            int nFrames = Rows * Columns;

            if (nFrames > 32)
            {
                throw new Exception("Invalid window layout: too many frames to store on Textures!");
            }
        }
    }

    public class WindowManager : GameWindow
    {
        Shader shader;
        int vertexBufferObject;
        int vertexArrayObject;
        int elementBufferObject;

        Painting painting;

        ImageHandler image;
        FrameManager frameManager;

        public WindowManager(WindowLayout windowLayout, ImageHandler image) :
            base(GameWindowSettings.Default, new NativeWindowSettings()
                                            {
                                                Size = (image.Width * windowLayout.Columns, image.Height * windowLayout.Rows),
                                                Title = image.Title,
                                                Flags = ContextFlags.ForwardCompatible
                                            })
        {
            this.image = image;
            painting = new Painting(image.Width, image.Height);

            frameManager = new FrameManager(windowLayout, image, painting);

            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, frameManager.Vertices.Length * sizeof(float), frameManager.Vertices, BufferUsageHint.StreamDraw);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, frameManager.Indices.Length * sizeof(uint), frameManager.Indices, BufferUsageHint.StreamDraw);

            shader.Load();
            shader.Use();

            var vertexSize = 5 * sizeof(float);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, vertexSize, 3 * sizeof(float));

            frameManager.Load(shader);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            frameManager.Update();

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            frameManager.Draw();

            SwapBuffers();
        }
    }
}
