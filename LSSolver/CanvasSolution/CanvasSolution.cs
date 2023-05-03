using LSPainter.Maths;

namespace LSPainter.LSSolver.Canvas
{
    public abstract class CanvasSolution : ISolution
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

        public long TryChange(CanvasChange change)
        {
            long scoreDiff = 0;

            foreach ((int x, int y) in change.BoundingBox.AsEnumerable())
            {
                
            }
        }

        // This feels wrong, but it works.
        IChange ISolution.GenerateNeighbor() => GenerateNeighbor();
        void ISolution.ApplyChange(IChange change) => ApplyChange((CanvasChange)change);

        long ISolution.TryChange(IChange change) => TryChange((CanvasChange)change);
    }
}