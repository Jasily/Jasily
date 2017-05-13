using System;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class VoidAwaiterAdapter<TAwaiter> : AwaiterAdapter<TAwaiter>
    {
        private readonly IInstanceMethodInvoker<TAwaiter> _getResultInvoker;

        public VoidAwaiterAdapter(AwaitableInfo info, IServiceProvider serviceProvider)
            : base(info, serviceProvider)
        {
            this._getResultInvoker = this.ServiceProvider.GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                .GetInstanceMethodInvoker(this.AwaitableInfo.GetResultMethod);
        }

        public void GetResult(TAwaiter awaiter)
            => this._getResultInvoker.Invoke(awaiter, this.ServiceProvider, default(OverrideArguments));
    }
}