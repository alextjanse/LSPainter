using LSPainter.LSSolver.Painter;
using LSPainter.ShapePainter;
using LSPainter.FiniteShapePainter;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter,
        FiniteShapePainter
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
                {
                    ShapePainterSolution solution = new ShapePainterSolution(new Canvas(width, height));
                    ShapePainterChecker checker = new ShapePainterChecker(OriginalImage);
                    ShapePainterOperationFactory factory = new ShapePainterOperationFactory(width, height);

                    var solver = new Solver<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>(
                        solution,
                        checker,
                        new SimulatedAnnealingAlgorithm(),
                        factory
                    );

                    return solver;
                }
                case SolverType.FiniteShapePainter:
                {
                    FiniteShapePainterSolution solution = new FiniteShapePainterSolution(new Canvas(width, height));
                    FiniteShapePainterChecker checker = new FiniteShapePainterChecker(OriginalImage);

                    FiniteShapePainterOperationFactory factory = new FiniteShapePainterOperationFactory(width, height);

                    var solver = new Solver<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>(
                        solution,
                        checker,
                        new SimulatedAnnealingAlgorithm(),
                        factory,
                        new Constraint<FiniteShapePainterSolution, FiniteShapePainterScore>[] {
                            new LimitShapesConstraint(1000, 1e6),
                            new BlankPixelConstraint(1000),
                        }
                    );

                    return solver;
                }
                default: throw new NotImplementedException();
            }
        }
    }
}