using System;

namespace Jasily.Core
{
    public interface IDisposable<out T> : IDisposable
    {
        T DisposeObject { get; }
    }
}