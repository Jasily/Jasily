using System;

namespace Jasily.Interfaces
{
    public interface IDisposable<out T> : IDisposable
    {
        T DisposeObject { get; }
    }
}