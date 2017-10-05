using System;
using JetBrains.Annotations;

namespace Jasily.TypeMakers
{
    public interface IGenericTypeMaker
    {
        [NotNull]
        Type MakeGenericType([NotNull] params Type[] typeArguments);
    }
}