namespace ExecuteAutomation.Reqnroll.Dynamics;

public class DynamicInstanceComparisonException : Exception
{
    public DynamicInstanceComparisonException(IList<string> diffs)
        : base("There were some difference between the table and the instance")
    {
        Differences = diffs;
    }

    public IList<string> Differences { get; private set; }
}