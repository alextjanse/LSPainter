using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasSolution : Solution
    {
        public Canvas Canvas { get; set; }

        public CanvasSolution(Canvas canvas)
        {
            Canvas = canvas;
        }
    }
}