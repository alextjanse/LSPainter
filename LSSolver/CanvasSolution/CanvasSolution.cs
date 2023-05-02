using LSPainter.Maths;

namespace LSPainter.LSSolver.CanvasSolution
{
    public abstract class CanvasSolution : ISolution<CanvasChange>
    {
        public Painting Canvas { get; }
        public long Score { get; protected set; }

        public CanvasSolution(int width, int height)
        {
            Canvas = new Painting(width, height);
        }

        public abstract void ApplyChange(CanvasChange change);

        public abstract CanvasChange GenerateNeighbor();

        public abstract void Iterate();

        public abstract long TryChange(CanvasChange change);
    }
}