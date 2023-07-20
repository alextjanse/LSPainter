using LSPainter.LSSolver.Painter;
using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter
    }

    public class SolverFactory
    {
        public ImageHandler OriginalImage { get; }

        public SolverFactory(ImageHandler originalImage)
        {
            OriginalImage = originalImage;
        }

        public ISolver<ICanvasSolution> CreateCanvasSolver(SolverType type)
        {
            int width = OriginalImage.Width;
            int height = OriginalImage.Height;

            switch (type)
            {
                case SolverType.ShapePainter:
                    ShapePainterSolution solution = new ShapePainterSolution(new Canvas(width, height));
                    ShapePainterChecker checker = new ShapePainterChecker(OriginalImage);
                    ShapePainterOperationFactory factory = new ShapePainterOperationFactory(width, height);

                    var solver = new Solver<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>(solution, checker, new SimulatedAnnealingAlgorithm(), factory);

                    return (ISolver<ICanvasSolution>)solver;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}