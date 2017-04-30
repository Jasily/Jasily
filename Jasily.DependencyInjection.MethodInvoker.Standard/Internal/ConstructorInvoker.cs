using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class ConstructorInvoker<T> : BaseInvoker,
        IStaticMethodInvoker, IStaticMethodInvoker<T>
    {
        private Func<IServiceProvider, OverrideArguments, T> func;

        public ConstructorInvoker(IInternalMethodInvokerFactory factory, ConstructorInfo constructor)
            : base(factory, constructor)
        {
            this.Constructor = constructor;
            this.func = this.ImplFunc();
        }

        public ConstructorInfo Constructor { get; }

        private T InvokeMethod(IServiceProvider serviceProvider, OverrideArguments args)
        {
            var a = this.Parameters.Length == 0 ? null : this.ResolveArguments(serviceProvider, args);

            try
            {
                return (T)this.Constructor.Invoke(a);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        public T Invoke(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return this.func(serviceProvider, arguments);
        }

        object IStaticMethodInvoker.Invoke(IServiceProvider serviceProvider, OverrideArguments arguments)
        {
            return this.Invoke(serviceProvider, arguments);
        }

        private Func<IServiceProvider, OverrideArguments, T> ImplFunc()
        {
#if DEBUG
            if (CompileImmediately) return this.CompileFunc();
#endif
            if (CompileThreshold <= 0) return this.CompileFunc();
            var count = 0;
            return (p, args) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.func, this.CompileFunc()));
                }

                return this.InvokeMethod(p, args);
            };
        }

        private Func<IServiceProvider, OverrideArguments, T> CompileFunc()
        {
            Expression body = this.Parameters.Length == 0
                ? Expression.New(this.Constructor)
                : Expression.New(this.Constructor, this.ResolveArgumentsExpressions());

            return Expression.Lambda<Func<IServiceProvider, OverrideArguments, T>>(body,
                    ParameterServiceProvider, ParameterOverrideArguments
                ).Compile();
        }

        public IStaticMethodInvoker<TResult2> CastTo<TResult2>()
        {
            var smi = this as IStaticMethodInvoker<TResult2>;
            if (smi != null) return smi;
            throw new InvalidOperationException();
        }
    }
}
