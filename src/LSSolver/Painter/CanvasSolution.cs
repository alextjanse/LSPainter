using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasSolution : Solution, ICanvasSolution
    {
        public Canvas Canvas { get; set; }

        public CanvasSolution(Canvas canvas)
        {
            Canvas = canvas;
        }
    }
}