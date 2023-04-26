using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.Solver;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : ISolution<ShapePainterChange>
    {
        long ISolution<ShapePainterChange>.Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void ISolution<ShapePainterChange>.ApplyChange(ShapePainterChange change)
        {
            throw new NotImplementedException();
        }

        void ISolution<ShapePainterChange>.Draw(Frame frame)
        {
            throw new NotImplementedException();
        }

        long ISolution<ShapePainterChange>.TryChange(ShapePainterChange change)
        {
            throw new NotImplementedException();
        }
    }
}