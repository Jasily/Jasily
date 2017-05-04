using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal abstract class AwaitableAdapter
    {
        [NotNull]
        public static IAwaitableAdapter GetAwaitableAdapter(IServiceProvider provider, Type instanceType)
        {
            var awaiterType = instanceType.GetRuntimeMethods()
                .Where(z => z.Name == nameof(Task.GetAwaiter))
                .Where(z => z.GetParameters().Length == 0)
                .Select(z => z.ReturnType)
                .FirstOrDefault(z => z != typeof(void));

            if (awaiterType != null &&
                awaiterType != typeof(void) &&
                typeof(INotifyCompletion).GetTypeInfo().IsAssignableFrom(awaiterType.GetTypeInfo()))
            {
                var resultType = awaiterType.GetRuntimeMethods()
                    .Where(z => z.Name == nameof(TaskAwaiter.GetResult))
                    .Where(z => z.GetParameters().Length == 0)
                    .Select(z => z.ReturnType)
                    .FirstOrDefault();

                if (resultType != null)
                {
                    if (resultType == typeof(void))
                    {
                        return (IAwaitableAdapter)provider.GetRequiredService(typeof(TaskGetAwaiterAdapter<,>)
                            .FastMakeGenericType(instanceType, awaiterType));
                    }
                    else
                    {
                        return (IAwaitableAdapter)provider.GetRequiredService(typeof(TaskGetAwaiterAdapter<,,>)
                            .FastMakeGenericType(instanceType, awaiterType, resultType));
                    }
                }
            }

            return NonTaskAwaiterAdapter.Instance;
        }
    }

    internal abstract class TaskGetAwaiterAdapter : IAwaitableAdapter
    {
        private readonly bool _isValueType;

        protected TaskGetAwaiterAdapter(Type type, Type awaiterType, IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;

            this.GetAwaiterMethod = type.GetRuntimeMethods()
                .FirstOrDefault(z => z.Name == nameof(Task.GetAwaiter) && z.GetParameters().Length == 0);

            if (this.GetAwaiterMethod != null)
            {
                this.IsCompletedMethod = awaiterType
                    .GetRuntimeProperties()
                    .Where(z => z.Name == nameof(TaskAwaiter.IsCompleted))
                    .Where(z => z.PropertyType == typeof(bool))
                    .FirstOrDefault(z => z.GetIndexParameters().Length == 0)?.GetMethod;

                this.GetResultMethod = awaiterType
                    .GetRuntimeMethods()
                    .Where(z => z.Name == nameof(TaskAwaiter.GetResult))
                    .FirstOrDefault(z => z.GetParameters().Length == 0);

                this._isValueType = awaiterType.GetTypeInfo().IsValueType;
            }

            this.IsAwaitable = this.GetAwaiterMethod != null && this.IsCompletedMethod != null && this.GetResultMethod != null;
        }

        protected IServiceProvider ServiceProvider { get; }

        protected MethodInfo GetAwaiterMethod { get; }

        protected MethodInfo IsCompletedMethod { get; }

        protected MethodInfo GetResultMethod { get; }

        public bool IsAwaitable { get; }

        public abstract bool IsCompleted(object instance);

        public abstract object GetResult(object instance);

        public abstract void OnCompleted(object instance, Action continuation);

        protected void VerifyArgument<T>([NotNull] T instance)
        {
            if (!this._isValueType && instance == null) throw new ArgumentNullException(nameof(instance));
            if (!this.IsAwaitable) throw new InvalidOperationException("is NOT awaitable.");
        }
    }

    /// <summary>
    /// adapter for <see cref="Void"/> return value GetResult()
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TAwaiter"></typeparam>
    internal class TaskGetAwaiterAdapter<TInstance, TAwaiter> : TaskGetAwaiterAdapter, 
        IAwaitableAdapter<TInstance>, IAwaitableAdapter<TInstance, object>
    {
        private readonly IInstanceMethodInvoker<TInstance, TAwaiter> _getAwaiterInvoker;
        private readonly IInstanceMethodInvoker<TAwaiter> _awaiterGetResultInvoker;
        private readonly IInstanceMethodInvoker<TAwaiter, bool> _awaiterIsCompletedInvoker;

        public TaskGetAwaiterAdapter(IServiceProvider serviceProvider)
            : base(typeof(TInstance), typeof(TAwaiter), serviceProvider)
        {
            if (this.IsAwaitable)
            {
                this._getAwaiterInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TInstance>>()
                    .GetInstanceMethodInvoker(this.GetAwaiterMethod)
                    .HasResult<TInstance, TAwaiter>();
                this._awaiterGetResultInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                    .GetInstanceMethodInvoker(this.GetResultMethod);
                this._awaiterIsCompletedInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                    .GetInstanceMethodInvoker(this.IsCompletedMethod)
                    .HasResult<TAwaiter, bool>();
            }
        }

        private TAwaiter GetAwaiter(TInstance instance)
        {
            this.VerifyArgument(instance);
            return this._getAwaiterInvoker.Invoke(instance, this.ServiceProvider);
        }

        #region IAwaitableAdapter<T>

        public bool IsCompleted(TInstance instance)
        {
            return this._awaiterIsCompletedInvoker.Invoke(this.GetAwaiter(instance), this.ServiceProvider);
        }

        public void GetResult(TInstance instance)
        {
            this._awaiterGetResultInvoker.Invoke(this.GetAwaiter(instance), this.ServiceProvider, default(OverrideArguments));
        }

        public void OnCompleted(TInstance instance, Action continuation)
        {
            ((INotifyCompletion)this.GetAwaiter(instance)).OnCompleted(continuation);
        }

        #endregion

        #region IAwaitableAdapter

        public override bool IsCompleted(object instance) => this.IsCompleted((TInstance)instance);

        public override object GetResult(object instance)
        {
            this.GetResult((TInstance)instance);
            return null;
        }

        public override void OnCompleted(object instance, Action continuation) => this.OnCompleted((TInstance) instance, continuation);

        #endregion

        object IAwaitableAdapter<TInstance, object>.GetResult(TInstance instance)
        {
            this.GetResult(instance);
            return null;
        }
    }

    internal class TaskGetAwaiterAdapter<TInstance, TAwaiter, TResult> : TaskGetAwaiterAdapter,
        IAwaitableAdapter<TInstance>,
        IAwaitableAdapter<TInstance, TResult>
    {
        private readonly IInstanceMethodInvoker<TInstance, TAwaiter> _getAwaiterInvoker;
        private readonly IInstanceMethodInvoker<TAwaiter, TResult> _awaiterGetResultInvoker;
        private readonly IInstanceMethodInvoker<TAwaiter, bool> _awaiterIsCompletedInvoker;

        public TaskGetAwaiterAdapter(IServiceProvider serviceProvider)
            : base(typeof(TInstance), typeof(TAwaiter), serviceProvider)
        {
            if (this.IsAwaitable)
            {
                this._getAwaiterInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TInstance>>()
                    .GetInstanceMethodInvoker(this.GetAwaiterMethod)
                    .HasResult<TInstance, TAwaiter>();
                this._awaiterGetResultInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                    .GetInstanceMethodInvoker(this.GetResultMethod)
                    .HasResult<TAwaiter, TResult>();
                this._awaiterIsCompletedInvoker = this.ServiceProvider
                    .GetRequiredService<IMethodInvokerFactory<TAwaiter>>()
                    .GetInstanceMethodInvoker(this.IsCompletedMethod)
                    .HasResult<TAwaiter, bool>();
            }
        }

        private TAwaiter GetAwaiter(TInstance instance)
        {
            this.VerifyArgument(instance);
            return this._getAwaiterInvoker.Invoke(instance, this.ServiceProvider);
        }

        #region IAwaitableAdapter<T, TResult>

        public bool IsCompleted(TInstance instance)
        {
            return this._awaiterIsCompletedInvoker.Invoke(this.GetAwaiter(instance), this.ServiceProvider);
        }

        public TResult GetResult(TInstance instance)
        {
            return this._awaiterGetResultInvoker.Invoke(this.GetAwaiter(instance), this.ServiceProvider, default(OverrideArguments));
        }

        void IAwaitableAdapter<TInstance>.GetResult(TInstance instance) => this.GetResult(instance);

        public void OnCompleted(TInstance instance, Action continuation)
        {
            ((INotifyCompletion)this.GetAwaiter(instance)).OnCompleted(continuation);
        }

        #endregion

        #region IAwaitableAdapter

        public override bool IsCompleted(object instance) => this.IsCompleted((TInstance)instance);

        public override object GetResult(object instance) => this.GetResult((TInstance)instance);

        public override void OnCompleted(object instance, Action continuation) => this.OnCompleted((TInstance)instance, continuation);

        #endregion
    }
}
