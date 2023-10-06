using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : CanvasSolution
    {
        public ShapePainterSolution(Canvas canvas) : base(canvas)
        {
        }

        public override object Clone()
        {
            Canvas canvasClone = (Canvas)Canvas.Clone();

            return new ShapePainterSolution(canvasClone);
        }
    }
}