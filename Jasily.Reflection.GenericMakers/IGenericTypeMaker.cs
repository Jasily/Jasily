using System;
using JetBrains.Annotations;

namespace Jasily.Reflection.GenericMakers
{
    public interface IGenericTypeMaker
    {
        [NotNull]
        Type MakeGenericType([NotNull] params Type[] typeArguments);
    }
}