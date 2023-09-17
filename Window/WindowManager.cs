using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using LSPainter.DCEL;
using LSPainter.LSSolver;

namespace LSPainter
{
    public interface IUpdatable
    {
        void Update();
    }

    public interface IIterable
    {
        void Iterate();
    }
    
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
        ImageHandler original;
        SolverManager solverManager;
        PictureFrameManager pictureFrameManager;

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

            pictureFrameManager = new PictureFrameManager(windowLayout, original, solverManager.EnumerateCanvases());

            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");

            DCELSolution dcel = new DCELSolution(original.Width, original.Height);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            shader.Load();
            shader.Use();

            pictureFrameManager.Load(shader);
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

            pictureFrameManager.Update();
            pictureFrameManager.Draw();

            SwapBuffers();
        }
    }
}
