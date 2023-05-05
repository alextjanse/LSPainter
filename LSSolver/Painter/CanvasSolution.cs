using LSPainter.Maths;

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
    }
}