using System;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Data
{
    public interface ISerializerProvider
    {
        [NotNull]
        ISerializer Create([NotNull] Type type);
    }
}