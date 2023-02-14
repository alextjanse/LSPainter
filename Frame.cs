using OpenTK.Graphics.OpenGL4;
using OpenTK;
using SixLabors.ImageSharp;

namespace LSPainter
{
    public class Frame
    {
        // Top-Left = (x, y), Bottom-Left = (x + w, y + h)
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }

        public float[] Vertices { get; }

        public uint[] Indices { get; }

        ImageHandler image;

        public Frame(ImageHandler image, float x, float y, float width, float height)
        {
            this.image = image;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            Vertices = new float[] {
                X + Width, Y,          0.0f, 1.0f, 1.0f,
                X + Width, Y + Height, 0.0f, 1.0f, 0.0f,
                X,         Y + Height, 0.0f, 0.0f, 0.0f,
                X,         Y,          0.0f, 0.0f, 1.0f
            };

            Indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };
        }

        public void Draw()
        {

        }
    }
}