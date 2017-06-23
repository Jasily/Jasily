namespace Jasily.ComponentModel.Editor.Conditions
{
    public sealed class WriteIfNotNullOrWhiteSpaceAttribute : WriteToObjectConditionAttribute
    {
        public override bool IsMatch(object value) => !string.IsNullOrWhiteSpace(value as string);
    }
}