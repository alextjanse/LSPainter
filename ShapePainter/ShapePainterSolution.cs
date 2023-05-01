using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : CanvasSolution<ShapePainterChange>
    {
        public ShapePainterSolution(int width, int height) : base(width, height)
        {
            Score = 0;
        }
    }
}