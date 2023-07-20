using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using LSPainter.DCEL;

namespace LSPainter
{
    public static class Program
    {
        private static void Main()
        {
            string file_path = "./Images/mona_lisa.jpeg";

            ImageHandler image = ImageHandler.FromFile("Mona Lisa", file_path);

            WindowLayout windowLayout = new WindowLayout(2, 2, false, 4);

            using (WindowManager window = new WindowManager(windowLayout, image))
            {
                window.Run();
            }
        }
    }
}
