namespace Jasily.ComponentModel.Editor.Conditions
{
    public sealed class WriteIfNotNullOrEmptyAttribute : WriteToObjectConditionAttribute
    {
        public override bool IsMatch(object value) => !string.IsNullOrEmpty(value as string);
    }
}