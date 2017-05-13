using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class GenericAwaitableAdapter<TInstance, TAwaiter, TResult> : AwaitableAdapter<TInstance, TAwaiter>,
        IAwaitableAdapter<TInstance>,
        IAwaitableAdapter<TInstance, TResult>
    {
        private readonly GenericAwaiterAdapter<TAwaiter, TResult> _awaiterAdapter;

        public GenericAwaitableAdapter(AwaitableInfo info, IServiceProvider serviceProvider)
            : base(info, serviceProvider)
        {
            this._awaiterAdapter = new GenericAwaiterAdapter<TAwaiter, TResult>(info, serviceProvider);
        }

        protected override AwaiterAdapter<TAwaiter> AwaiterAdapter => this._awaiterAdapter;

        public TResult GetResult(TInstance instance) => this._awaiterAdapter.GetResult(this.GetAwaiter(instance));

        void IAwaitableAdapter<TInstance>.GetResult(TInstance instance) => this.GetResult(instance);

        public override object GetResult(object instance) => this.GetResult((TInstance)instance);

        public override void OnCompleted(object instance, Action continuation) => this.OnCompleted((TInstance)instance, continuation);
    }
}
