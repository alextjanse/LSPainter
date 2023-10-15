using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using LSPainter.DCEL;
using System.Runtime.Versioning;

namespace LSPainter
{
    public static class Program
    {
        private static void Main()
        {
            PrettyPrint();

            string file_path = "Images/mona_lisa.jpeg";

            ImageHandler image = ImageHandler.FromFile("Mona Lisa", file_path);

            WindowLayout windowLayout = new WindowLayout(1, 1, false, 1);

            using (WindowManager window = new WindowManager(windowLayout, image))
            {
                window.Run();
            }
        }

        private static void PrettyPrint()
        {
            string[] ascii_art = new[]
            {
                @"   __  __             _       _            ",
                @"  / / / _\_ __   __ _(_)_ __ | |_ ___ _ __ ",
                @" / /  \ \| '_ \ / _` | | '_ \| __/ _ \ '__|",
                @"/ /____\ \ |_) | (_| | | | | | ||  __/ |   ",
                @"\____/\__/ .__/ \__,_|_|_| |_|\__\___|_|   ",
                @"         |_|                               ",
            };

            foreach (string line in ascii_art)
            {
                Console.WriteLine(line);
            }
        }
    }
}
