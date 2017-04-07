using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public struct ResolveResult
    {
        public ResolveResult(object value)
        {
            this.HasValue = true;
            this.Value = value;
        }

        public bool HasValue { get; }

        [CanBeNull]
        public object Value { get; }
    }
}