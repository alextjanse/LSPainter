using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter
{
    public class Canvas : Texture, ICloneable
    {
        Image<Rgba32> image;

        public Canvas(int width, int height) : base(width, height)
        {
            image = new Image<Rgba32>(Width, Height, Color.Black);

            image.CopyPixelDataTo(Data);
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = 4 * (y * Width + x);

            Data[index + 0] = color.R;
            Data[index + 1] = color.G;
            Data[index + 2] = color.B;
            Data[index + 3] = color.A;
        }

        public override void Update()
        {
            // Upload Data
            base.Update();
        }

        public object Clone()
        {
            Canvas newCanvas = new Canvas(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newCanvas.SetPixel(x, y, GetPixel(x, y));
                }
            }

            return newCanvas;
        }
    }
}