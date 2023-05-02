public interface ISolver<in TSolution, in TChange, out TSolutionChecker>
    where TSolution : ISolution<TSolutionChecker, TChange>
    where TChange : IChange<TSolution>
    where TSolutionChecker : ISolutionChecker<TSolution, TChange>
{
    TSolution Solve(TSolutionChecker checker, TSolution initialSolution);
}

public interface ISolution<out TSolutionChecker, in TChange>
    where TSolutionChecker : ISolutionChecker<ISolution<TSolutionChecker, TChange>, TChange>
    where TChange : IChange<ISolution<TSolutionChecker, TChange>>
{
    TChange GenerateChange();
    void ApplyChange(TChange change);
    ISolution<TSolutionChecker, TChange> Clone();
}

public interface ISolutionChecker<in TSolution, in TChange>
    where TSolution : ISolution<ISolutionChecker<TSolution, TChange>, TChange>
    where TChange : IChange<TSolution>
{
    bool IsFeasible(TSolution solution);
    long ScoreDiff(TSolution solution, TChange change);
}

public interface IChange<in TSolution>
{
    void Apply(TSolution solution);
}
