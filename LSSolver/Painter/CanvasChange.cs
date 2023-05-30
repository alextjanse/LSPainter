using LSPainter.Maths;
using LSPainter.Maths.Shapes;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasChange<TSolution> : IChange<TSolution> where TSolution : CanvasSolution
    {
        public abstract BoundingBox BoundingBox { get; }

        public abstract IScore<TSolution> Try(TSolution solution, ISolutionChecker<CanvasSolution> solutionChecker);
        public abstract void Apply(TSolution solution);

        public void DrawShape(TSolution solution, Shape shape, Color color)
        {
            Canvas canvas = solution.Canvas;
            foreach ((int x, int y) in shape.EnumeratePixels(canvas.Width, canvas.Height))
            {
                Color currentColor = canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);
                canvas.SetPixel(x, y, newColor);
            }
        }

        public long TryDrawShape(TSolution solution, CanvasSolutionChecker checker, Shape shape, Color color)
        {
            Canvas canvas = solution.Canvas;

            long scoreDiff = 0;

            foreach ((int x, int y) in shape.EnumeratePixels(canvas.Width, canvas.Height))
            {
                Color currentColor = canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);

                (int rDiff, int gDiff, int bDiff) = checker.GetPixelDiff(x, y, currentColor, newColor);
                
                scoreDiff += pixelScoreDiff;
            }

            return scoreDiff;
        }
    }
}