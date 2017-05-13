using System;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class GenericAwaiterAdapter<TAwaiter, TResult> : AwaiterAdapter<TAwaiter>
    {
        private readonly IInstanceMethodInvoker<TAwaiter, TResult> _getResultInvoker;

        public GenericAwaiterAdapter(AwaitableInfo info, IServiceProvider serviceProvider)
            : base(info, serviceProvider)
        {
            this._getResultInvoker = this.ServiceProvider.GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                .GetInstanceMethodInvoker(this.AwaitableInfo.GetResultMethod)
                .HasResult<TAwaiter, TResult>();
        }

        public TResult GetResult(TAwaiter awaiter)
            => this._getResultInvoker.Invoke(awaiter, this.ServiceProvider, default(OverrideArguments));
    }
}