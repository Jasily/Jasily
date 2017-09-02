using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Typed
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGenericTypeFactory
    {
        [NotNull]
        IGenericTypeMaker GetTypeMaker([NotNull] Type type);
    }
}