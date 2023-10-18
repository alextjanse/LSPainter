namespace LSPainter.LSSolver
{
    /// <summary>
    /// Contains the score of a solution and its parameters that can be
    /// used by Constraints.
    /// </summary>
    public abstract class Score<TSolution> : ICloneable where TSolution : Solution
    {
        /// <summary>
        /// The "raw" solution score, e.g. total pixel score, with no penalties.
        /// </summary>
        public abstract double ToDouble();
        public abstract object Clone();
    }
}