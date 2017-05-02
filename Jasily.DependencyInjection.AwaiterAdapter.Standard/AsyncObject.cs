using System;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct AsyncObject<T>
    {
        private readonly T _instance;
        private readonly ITaskAdapter<T> _adapter;

        internal AsyncObject(T instance, ITaskAdapter<T> adapter)
        {
            this._instance = instance;
            this._adapter = adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AsyncObjectAwaiter<T> GetAwaiter()
        {
            if (this._adapter == null) throw new InvalidOperationException();
            return new AsyncObjectAwaiter<T>(this._instance, this._adapter);
        }

        public AsyncObject<T, TResult> HasResultAsync<TResult>()
        {
            if (this._adapter == null) throw new InvalidOperationException();
            return new AsyncObject<T, TResult>(this._instance, this._adapter);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public struct AsyncObject<T, TResult>
    {
        private readonly T _instance;
        private readonly ITaskAdapter<T> _adapter;

        internal AsyncObject(T instance, ITaskAdapter<T> adapter)
        {
            this._instance = instance;
            this._adapter = adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AsyncObjectAwaiter<T, TResult> GetAwaiter()
        {
            if (this._adapter == null) throw new InvalidOperationException();
            return new AsyncObjectAwaiter<T, TResult>(this._instance, this._adapter);
        }
    }
}