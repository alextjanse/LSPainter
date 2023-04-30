namespace LSPainter.Solver
{
    public class CanvasSolutionChecker<TChange> : SolutionChecker<CanvasSolution<TChange>, TChange>
        where TChange : Change
    {
        ImageHandler original;
        public CanvasSolutionChecker(ImageHandler original)
        {
            this.original = original;
        }

        public override long GetScoreDiff(CanvasSolution<TChange> solution, TChange change)
        {
            throw new NotImplementedException();
        }

        public (int, int, int) PixelDiff(int x, int y, Color solutionColor)
        {
            Color originalColor = original.GetPixel(x, y);
            return Color.Diff(solutionColor, originalColor);
        }

        public override long ScoreSolution(CanvasSolution<TChange> solution)
        {
            throw new NotImplementedException();
        }
    }
}