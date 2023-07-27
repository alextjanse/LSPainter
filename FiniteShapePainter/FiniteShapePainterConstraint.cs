using LSPainter.LSSolver;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterConstraint : Constraint<FiniteShapePainterSolution, FiniteShapePainterScore>
    {
        public int MaxNumberOfShapes { get; set; }

        public FiniteShapePainterConstraint(int maxNumberOfShapes, double penalty, double alpha = 1.1) : base(penalty, alpha)
        {
            MaxNumberOfShapes = maxNumberOfShapes;
        }

        public override double ApplyPenalty(FiniteShapePainterScore score)
        {
            return score.NumberOfShapes > MaxNumberOfShapes ? Penalty : 0;
        }
    }
}