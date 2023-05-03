using LSPainter.LSSolver.Canvas;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolver : CanvasSolver
    {
        static Random random = new Random();

        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        public ShapePainterSolver(ShapePainterSolution initialSolution, CanvasChecker checker) : base(initialSolution, checker)
        {
        }
    }
}