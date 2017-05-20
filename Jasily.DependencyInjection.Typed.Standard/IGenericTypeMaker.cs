using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Typed
{
    public interface IGenericTypeMaker
    {
        [NotNull]
        Type MakeGenericType([NotNull] params Type[] typeArguments);
    }
}