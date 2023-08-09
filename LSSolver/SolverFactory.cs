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

                    return (ISolver<ICanvasSolution>)solver;
                }
                case SolverType.FiniteShapePainter:
                {
                    FiniteShapePainterSolution solution = new FiniteShapePainterSolution(new Canvas(width, height));
                    FiniteShapePainterChecker checker = new FiniteShapePainterChecker(OriginalImage);
                    
                    ColorPalette palette = new ColorPalette(new Color[]
                        {
                            new Color(94, 66, 65),
                            new Color(73, 35, 34),
                            new Color(54, 19, 13),
                            new Color(95, 51, 42),
                            new Color(126, 68, 54),
                            new Color(86, 43, 26),
                            new Color(122, 88, 42),
                            new Color(135, 101, 53),
                            new Color(170, 111, 35),
                            new Color(156, 80, 28),
                            new Color(106, 50, 15),
                            new Color(130, 60, 26),
                            new Color(193, 136, 49),
                            new Color(220, 164, 69),
                            new Color(94, 76, 36),
                            new Color(234, 192, 92),
                            new Color(145, 122, 68),
                            new Color(104, 95, 62),
                            new Color(117, 125, 88),
                            new Color(142, 140, 89),
                            new Color(157, 156, 102),
                            new Color(67, 67, 43),
                            new Color(116, 112, 67),
                            new Color(48, 42, 28),
                            new Color(84, 61, 81),
                            new Color(55, 41, 54),
                            new Color(43, 31, 43),
                            new Color(78, 93, 72),
                            new Color(67, 79, 57),
                            new Color(92, 106, 80),
                        }
                    );

                    FiniteShapePainterOperationFactory factory = new FiniteShapePainterOperationFactory(width, height, palette);

                    var solver = new Solver<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>(
                        solution,
                        checker,
                        new SimulatedAnnealingAlgorithm(),
                        factory,
                        new Constraint<FiniteShapePainterSolution, FiniteShapePainterScore>[] {
                            new LimitShapesConstraint(100, 100000),
                            new BlankPixelConstraint(1000),
                        }
                    );

                    return (ISolver<ICanvasSolution>)solver;
                }
                default: throw new NotImplementedException();
            }
        }
    }
}