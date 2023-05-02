/* using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter,
    }



    public class SolverFactory
    {
        /* 
        TODO: this function generic. I'm screwing up the whole inheritance
        with the TSolutionChecker<TChange>, TSolution, TChange stuff, need
        to make it generic, but also explicit.
        
        public static ISolver CreateSolver(ImageHandler original, SolverType type)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    return new ShapePainterSolver(original);
                default:
                    throw new Exception("Unknown type");
            }
        }
    }
} */