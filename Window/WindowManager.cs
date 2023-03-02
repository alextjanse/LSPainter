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
        }
    }

    public class WindowManager : GameWindow
    {
        Shader shader;
        int vertexBufferObject;
        int vertexArrayObject;
        int elementBufferObject;

        ImageHandler original;
        SolverManager solverManager;
        FrameManager frameManager;

        public WindowManager(WindowLayout windowLayout, ImageHandler original) :
            base
            (
                new GameWindowSettings()
                {
                    UpdateFrequency = 0,
                    RenderFrequency = 60,
                },
                new NativeWindowSettings()
                {
                    Size = (original.Width * windowLayout.Columns, original.Height * windowLayout.Rows),
                    Title = original.Title,
                    Flags = ContextFlags.ForwardCompatible
                }
            )
        {
            this.original = original;

            solverManager = new SolverManager(original, 1);

            frameManager = new FrameManager(windowLayout, original, solverManager.Instances[0].Painting);

            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            
            // vertexArrayObject = GL.GenVertexArray();
            // GL.BindVertexArray(vertexArrayObject);

            // vertexBufferObject = GL.GenBuffer();
            // GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            // GL.BufferData(BufferTarget.ArrayBuffer, frameManager.Vertices.Length * sizeof(float), frameManager.Vertices, BufferUsageHint.StreamDraw);

            // elementBufferObject = GL.GenBuffer();
            // GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            // GL.BufferData(BufferTarget.ElementArrayBuffer, frameManager.Indices.Length * sizeof(uint), frameManager.Indices, BufferUsageHint.StreamDraw);
            shader.Load();
            shader.Use();

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

            solverManager.Iterate();

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            frameManager.Update();
            frameManager.Draw();

            SwapBuffers();
        }
    }
}
