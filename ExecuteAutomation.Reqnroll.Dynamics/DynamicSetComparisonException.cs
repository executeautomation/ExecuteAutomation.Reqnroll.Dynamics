namespace ExecuteAutomation.Reqnroll.Dynamics;

public class DynamicSetComparisonException : Exception
{
    public DynamicSetComparisonException(string message) : base(message)
    {
        Differences = new List<string>();
    }

    public DynamicSetComparisonException(string message, IList<string> differences) : base(message)
    {
        Differences = differences;
    }

    public IList<string> Differences { get; private set; }
}