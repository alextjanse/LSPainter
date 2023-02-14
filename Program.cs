using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LSPainter
{
    public static class Program
    {
        private static void Main()
        {
            string file_path = "./Images/mona_lisa.jpeg";

            ImageHandler image = new ImageHandler("Mona Lisa", file_path);

            using (WindowManager window = new WindowManager(image))
            {
                window.Run();
            }
        }
    }
}
