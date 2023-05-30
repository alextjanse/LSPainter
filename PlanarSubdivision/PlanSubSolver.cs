using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;
using LSPainter.LSSolver;

namespace LSPainter.PlanarSubdivision
{
    public class PlanSubScore : CanvasScore
    {
        public long Vertices { get; }
        public long PixelScore { get; }

        public PlanSubScore(long vertices, long pixelScore) : base(pixelScore)
        {
            Vertices = vertices;
        }

        public override IScore<CanvasSolution> Add(IScore<CanvasSolution> score)
        {
            PlanSubScore other = (PlanSubScore)score;

            IScore<CanvasSolution> pixelDiff = base.Add(other);

            return new PlanSubScore(Vertices + other.Vertices, pixelDiff.GetCleanScore());
        }
    }

    public class PlanSubConstraints : PlainCanvasConstraints
    {
        public long MaxVertices { get; }

        public long Penalty { get; }

        public PlanSubConstraints(long maxVertices = 100, long penalty = 1000)
        {
            MaxVertices = maxVertices;
            Penalty = penalty;
        }

        public long EvaluateScore(PlanSubScore score)
        {
            long output = score.PixelScore;

            if (score.Vertices > MaxVertices)
            {
                output += (MaxVertices - score.Vertices) * Penalty;
            }

            return output;
        }
    }

    public class PlanSubSolver : CanvasSolver
    {
        public PlanSubSolver(
            PlanSubSolution initialSolution,
            PlanSubConstraints constraints,
            CanvasSolutionChecker checker
        ) : base(
            initialSolution,
            constraints,
            checker
        )
        {
        }
    }
}