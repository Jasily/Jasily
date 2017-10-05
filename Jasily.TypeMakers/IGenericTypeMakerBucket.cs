using System;
using JetBrains.Annotations;

namespace Jasily.TypeMakers
{
    public interface IGenericTypeMakerBucket
    {
        [NotNull]
        IGenericTypeMaker GetGenericTypeMaker([NotNull] Type type);
    }
}