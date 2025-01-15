namespace ExecuteAutomation.Reqnroll.Dynamics;

public class DynamicSetComparisonException : Exception
{
    public DynamicSetComparisonException(string message) : base(message)
    {
    }

    public DynamicSetComparisonException(string message, IList<string> differences) : base(message)
    {
        Differences = differences;
    }

    public IList<string> Differences { get; private set; }
}