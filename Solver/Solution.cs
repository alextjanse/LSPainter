namespace LSPainter.Solver
{
    public abstract class Solution : Texture
    {
        public ulong Score { get; protected set; }

        public abstract void Draw(Frame frame);
    }
}