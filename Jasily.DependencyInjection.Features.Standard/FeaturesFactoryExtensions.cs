using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features
{
    public static class FeaturesFactoryExtensions
    {
        [NotNull]
        public static TFeature CreateRequiredFeature<T, TFeature>([NotNull] IFeaturesFactory<T> factory,
            [NotNull] T source, bool inherit)
            where T : class
            where TFeature : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return factory.TryCreate<TFeature>(source, inherit) ?? throw new NotSupportedException();
        }
    }
}