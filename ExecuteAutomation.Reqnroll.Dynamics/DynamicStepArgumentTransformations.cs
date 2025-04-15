using Reqnroll;

namespace ExecuteAutomation.Reqnroll.Dynamics;

[Binding]
public class DynamicStepArgumentTransformations
{
    [StepArgumentTransformation]
    public IEnumerable<object> TransformToEnumerable(Table table)
    {
        return table.CreateDynamicSet();
    }

    [StepArgumentTransformation]
    public IList<object> TransformToList(Table table)
    {
        return table.CreateDynamicSet().ToList<object>();
    }

    [StepArgumentTransformation]
    public dynamic TransformToDynamicInstance(Table table)
    {
        return table.CreateDynamicInstance();
    }
    
    [StepArgumentTransformation]
    public dynamic TransformToNestedDynamicInstance(Table table)
    {
        return table.CreateNestedDynamicInstance();
    }
    
    [StepArgumentTransformation]
    public async Task<dynamic> TransformToDynamicInstanceAsync(Table table)
    {
        return await table.CreateDynamicInstanceAsync();
    }
    
    [StepArgumentTransformation]
    public async Task<IEnumerable<dynamic>> TransformToEnumerableAsync(Table table)
    {
        return await table.CreateDynamicSetAsync();
    }
}