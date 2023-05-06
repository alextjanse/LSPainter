using LSPainter.Maths;
using LSPainter.Maths.Shapes;

namespace LSPainter.LSSolver.Painter
{
    public interface ICanvasSolution : ISolution
    {
        Canvas Canvas { get; }
    }

    public abstract class CanvasSolution : ICanvasSolution
    {
        public Canvas Canvas { get; }
        public long Score { get; protected set; }
        protected CanvasComparer comparer;

        public CanvasSolution(int width, int height, CanvasComparer comparer)
        {
            Canvas = new Canvas(width, height);
            this.comparer = comparer;
        }

        protected abstract CanvasChange GenerateCanvasChange();
        protected abstract long TryChange(CanvasChange change);
        protected abstract void ApplyChange(CanvasChange change);

        public IChange GenerateNeighbor() => GenerateCanvasChange();
        public long TryChange(IChange change) => TryChange((CanvasChange)change);
        public void ApplyChange(IChange change) => ApplyChange((CanvasChange)change);

        public void DrawShape(Shape shape, Color color)
        {
            foreach ((int x, int y) in shape.EnumeratePixels(Canvas.Width, Canvas.Height))
            {
                Color currentColor = Canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);
                Canvas.SetPixel(x, y, newColor);
            }
        }

        public long TryDrawShape(Shape shape, Color color)
        {
            long scoreDiff = 0;

            foreach ((int x, int y) in shape.EnumeratePixels(Canvas.Width, Canvas.Height))
            {
                Color currentColor = Canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);

                long pixelScoreDiff = comparer.PixelScoreDiff(x, y, currentColor, newColor);
                scoreDiff += pixelScoreDiff;
            }

            return scoreDiff;
        }
    }
}