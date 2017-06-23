using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Jasily.Cache.Funcs
{
    public class FuncTemplate<T, TResult>
    {
        private readonly Func<T, TResult> func;
        private readonly Func<Task<T>, Task<TResult>> funcAsync;

        internal FuncTemplate(System.Func<T, TResult> func)
        {
            Debug.Assert(func != null);
            this.func = func;
            this.funcAsync = this.FuncAsync;
        }

        private async Task<TResult> FuncAsync(Task<T> obj) => this.func(await obj.ConfigureAwait(false));

        public static implicit operator Func<T, TResult>(FuncTemplate<T, TResult> template)
            => template.func;

        public static implicit operator Func<Task<T>, Task<TResult>>(FuncTemplate<T, TResult> template)
            => template.funcAsync;
    }
}