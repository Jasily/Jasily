using System;
using System.Collections.Generic;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily
{
    public struct ChainReleaser<T> : IDisposable
    {
        private readonly List<IDisposable> _objs;

        private ChainReleaser(T obj, [CanBeNull] List<IDisposable> list)
        {
            this._objs = list;
            if (obj is IDisposable x)
            {
                if (this._objs == null) this._objs = new List<IDisposable>();
                this._objs.Add(x);
            }
            this.Value = obj;
        }

        public T Value { get; }

        public ChainReleaser<TNext> Next<TNext>(TNext obj)
        {
            return new ChainReleaser<TNext>(obj, this._objs);
        }

        public void Dispose()
        {
            if (this._objs == null) return;
            this._objs.Reverse();
            this._objs.ForEach(z => z.Dispose());
        }

        public static implicit operator ChainReleaser<T>(T obj) => Releaser.CreateChainReleaser(obj);
    }
}