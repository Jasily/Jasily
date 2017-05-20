using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Typed
{
    public interface IGenericTypeFactory
    {
        [NotNull]
        IGenericTypeMaker GetTypeMaker([NotNull] Type type);
    }
}