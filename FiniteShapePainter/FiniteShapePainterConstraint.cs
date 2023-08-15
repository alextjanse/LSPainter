using LSPainter.LSSolver;

namespace LSPainter.FiniteShapePainter
{
    public class LimitShapesConstraint : Constraint<FiniteShapePainterSolution, FiniteShapePainterScore>
    {
        public int MaxNumberOfShapes { get; set; }

        public LimitShapesConstraint(int maxNumberOfShapes, double penalty, double alpha = 1.1) : base(penalty, alpha)
        {
            MaxNumberOfShapes = maxNumberOfShapes;
        }

        public override double ApplyPenalty(FiniteShapePainterScore score)
        {
            return score.NumberOfShapes > MaxNumberOfShapes ? Penalty * (score.NumberOfShapes - MaxNumberOfShapes) : 0;
        }
    }

    public class BlankPixelConstraint : Constraint<FiniteShapePainterSolution, FiniteShapePainterScore>
    {
        public BlankPixelConstraint(double penalty, double alpha = 1.1) : base(penalty, alpha)
        {
        }

        public override double ApplyPenalty(FiniteShapePainterScore score)
        {
            if (score.BlankPixels < 0) throw new Exception("Something went wrong in the count");

            return score.BlankPixels * Penalty;
        }
    }
}