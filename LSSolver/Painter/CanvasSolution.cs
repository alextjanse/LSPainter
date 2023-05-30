using LSPainter.Maths;
using LSPainter.Maths.Shapes;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasSolution : ISolution
    {
        public Canvas Canvas { get; }

        public CanvasSolution(int width, int height)
        {
            Canvas = new Canvas(width, height);
        }

        public abstract void Update();

        public abstract IChange<ISolution> GenerateNeighbor();
    }
}