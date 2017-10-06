using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.GenericMakers
{
    public interface IGenericMakerBucket
    {
        [NotNull]
        IGenericTypeMaker GetGenericTypeMaker([NotNull] Type type);

        [NotNull]
        IGenericMethodMaker GetGenericMethodMaker(MethodInfo method);
    }
}