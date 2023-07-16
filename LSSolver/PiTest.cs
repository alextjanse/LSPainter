namespace LSPainter.LSSolver
{
    public class PiSolution : Solution
    {
        public double Value { get; set; }

        public PiSolution(double value)
        {
            Value = value;
        }

        public override object Clone()
        {
            return new PiSolution(Value);
        }
    }

    public class PiScore : Score<PiSolution>
    {
        public double Value { get; set; }

        public PiScore(double value)
        {
            Value = value;
        }

        public override double ToDouble()
        {
            return Value;
        }

        public override object Clone()
        {
            return new PiScore(Value);
        }
    }

    public class PiChecker : SolutionChecker<PiSolution, PiScore>
    {
        public override PiScore ScoreSolution(PiSolution solution)
        {
            return new PiScore(Math.Abs(solution.Value - double.Pi));
        }
    }

    public abstract class PiOperation : Operation<PiSolution, PiScore, PiChecker>
    {
        
    }

    public class StepOperation : PiOperation
    {
        public double Step { get; set; }

        public StepOperation(double step)
        {
            Step = step;
        }

        public override PiScore Try(PiSolution solution, PiScore currentScore, PiChecker checker)
        {
            return checker.ScoreSolution(new PiSolution(solution.Value + Step));
        }

        public override void Apply(PiSolution solution)
        {
            solution.Value += Step;
        }
    }

    public abstract class PiConstraint : Constraint<PiSolution, PiScore>
    {
        
    }

    public class PiOperationFactory : OperationFactory<PiSolution, PiScore, PiChecker>
    {
        public double MaxStep { get; set; }
        public double Alpha { get; set; }

        public PiOperationFactory(double maxStep)
        {
            MaxStep = maxStep;

            Alpha = 0.99;
        }

        public override Operation<PiSolution, PiScore, PiChecker> Generate()
        {
            return new StepOperation(Randomizer.Range(-MaxStep, MaxStep));
        }

        public override void Update()
        {
            MaxStep *= Alpha;
        }
    }

    public class PiApp
    {
        public Solver<PiSolution, PiScore, PiChecker> solver;

        public PiApp()
        {
            PiSolution solution = new PiSolution(0);
            PiChecker checker = new PiChecker();
            PiOperationFactory operationFactory = new PiOperationFactory(1);

            solver = new Solver<PiSolution, PiScore, PiChecker>(solution, checker, new SimulatedAnnealingSolver(), operationFactory);
        }
    }
}