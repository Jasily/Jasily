using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public class ResolveResult
    {
        public static readonly ResolveResult None = new ResolveResult();

        private ResolveResult()
        {
        }

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