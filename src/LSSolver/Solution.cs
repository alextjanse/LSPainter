namespace LSPainter.LSSolver
{
    public interface ISolution : ICloneable
    {

    }

    public interface ICanvasSolution : ISolution
    {
        Canvas Canvas { get; }
    }

    /// <summary>
    /// Stores the solution. Gets altered by the operations.
    /// </summary>
    public abstract class Solution : ISolution
    {
        public abstract object Clone();
    }
}