using System;
using System.Runtime.CompilerServices;
using Jasily.DependencyInjection.MethodInvoker;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class AwaiterAdapter
    {
        protected AwaitableInfo AwaitableInfo { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected AwaiterAdapter([NotNull] AwaitableInfo info, [NotNull] IServiceProvider serviceProvider)
        {
            this.AwaitableInfo = info ?? throw new ArgumentNullException(nameof(info));
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }

    internal abstract class AwaiterAdapter<TAwaiter> : AwaiterAdapter
    {
        private readonly IInstanceMethodInvoker<TAwaiter, bool> _isCompletedInvoker;

        protected AwaiterAdapter(AwaitableInfo info, IServiceProvider serviceProvider)
            : base(info, serviceProvider)
        {
            this._isCompletedInvoker = this.ServiceProvider
                .GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                .GetInstanceMethodInvoker(this.AwaitableInfo.IsCompletedGetMethod)
                .HasResult<TAwaiter, bool>();
        }

        public bool IsCompleted(TAwaiter awaiter)
            => this._isCompletedInvoker.Invoke(awaiter, this.ServiceProvider);

        public void OnCompleted(TAwaiter awaiter, Action continuation)
        {
            ((INotifyCompletion)awaiter).OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(TAwaiter awaiter, Action continuation)
        {
            if (this.AwaitableInfo.UnsafeOnCompletedMethod != null)
            {
                ((ICriticalNotifyCompletion)awaiter).UnsafeOnCompleted(continuation);
            }
            else
            {
                this.OnCompleted(awaiter, continuation);
            }
        }
    }
}