using System;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    /// <summary>
    /// adapter for <see cref="Void"/> return value GetResult()
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TAwaiter"></typeparam>
    internal class VoidAwaitableAdapter<TInstance, TAwaiter> : AwaitableAdapter<TInstance, TAwaiter>, 
        IAwaitableAdapter<TInstance>, IAwaitableAdapter<TInstance, object>
    {
        private readonly VoidAwaiterAdapter<TAwaiter> _awaiterAdapter;

        public VoidAwaitableAdapter(AwaitableInfo info, IServiceProvider serviceProvider)
            : base(info, serviceProvider)
        {
            this._awaiterAdapter = new VoidAwaiterAdapter<TAwaiter>(info, serviceProvider);
        }

        protected override AwaiterAdapter<TAwaiter> AwaiterAdapter => this._awaiterAdapter;

        public void GetResult(TInstance instance) => this._awaiterAdapter.GetResult(this.GetAwaiter(instance));

        public override object GetResult(object instance)
        {
            this.GetResult((TInstance)instance);
            return null;
        }

        object IAwaitableAdapter<TInstance, object>.GetResult(TInstance instance)
        {
            this.GetResult(instance);
            return null;
        }
    }
}