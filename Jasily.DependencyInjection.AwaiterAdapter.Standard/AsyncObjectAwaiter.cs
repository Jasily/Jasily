using System;
using System.Runtime.CompilerServices;
using Jasily.DependencyInjection.AwaiterAdapter.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct AsyncObjectAwaiter<T> : INotifyCompletion
    {
        private readonly T _instance;
        private readonly IAwaitableAdapter _adapter;

        internal AsyncObjectAwaiter(T instance, IAwaitableAdapter adapter)
        {
            this._instance = instance;
            this._adapter = adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                if (this._adapter == null) throw new InvalidOperationException();
                if (!this._adapter.IsAwaitable) return true;
                return ((IAwaitableAdapter<T>) this._adapter).IsCompleted(this._instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetResult()
        {
            if (this._adapter == null) throw new InvalidOperationException();
            if (!this._adapter.IsAwaitable) return;
            ((IAwaitableAdapter<T>)this._adapter).GetResult(this._instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="continuation"></param>
        public void OnCompleted([NotNull] Action continuation)
        {
            if (this._adapter == null) throw new InvalidOperationException();
            if (continuation == null) throw new ArgumentNullException(nameof(continuation));
            if (!this._adapter.IsAwaitable) return;
            ((IAwaitableAdapter<T>)this._adapter).OnCompleted(this._instance, continuation);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public struct AsyncObjectAwaiter<T, TResult> : INotifyCompletion
    {
        private readonly T _instance;
        private readonly IAwaitableAdapter _adapter;

        internal AsyncObjectAwaiter(T instance, IAwaitableAdapter adapter)
        {
            this._instance = instance;
            this._adapter = adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                if (this._adapter == null) throw new InvalidOperationException();
                if (!this._adapter.IsAwaitable) return true;
                return ((IAwaitableAdapter<T, TResult>)this._adapter).IsCompleted(this._instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TResult GetResult()
        {
            if (this._adapter == null) throw new InvalidOperationException();
            if (!this._adapter.IsAwaitable) return default(TResult);
            return ((IAwaitableAdapter<T, TResult>)this._adapter).GetResult(this._instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="continuation"></param>
        public void OnCompleted([NotNull] Action continuation)
        {
            if (this._adapter == null) throw new InvalidOperationException();
            if (continuation == null) throw new ArgumentNullException(nameof(continuation));
            if (!this._adapter.IsAwaitable) return;
            ((IAwaitableAdapter<T, TResult>)this._adapter).OnCompleted(this._instance, continuation);
        }
    }
}