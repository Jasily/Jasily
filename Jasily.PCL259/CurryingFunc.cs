using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily
{
    public static class CurryingFunc
    {
        internal const string NotInitializedMessage = "struct is not initialized";

        public static CurryingFunc<T1, TResult> Currying<T1, TResult>(
            [NotNull] Func<T1, TResult> func)
        {
            return new CurryingFunc<T1, TResult>(func);
        }

        public static CurryingFunc<T1, T2, TResult> Currying<T1, T2, TResult>(
            [NotNull] Func<T1, T2, TResult> func)
        {
            return new CurryingFunc<T1, T2, TResult>(func);
        }

        public static CurryingFunc<T1, T2, T3, TResult> Currying<T1, T2, T3, TResult>(
            [NotNull] Func<T1, T2, T3, TResult> func)
        {
            return new CurryingFunc<T1, T2, T3, TResult>(func);
        }
    }

    public struct CurryingFunc<T1, TResult>
    {
        private readonly Func<T1, TResult> function;

        [NotNull]
        public Func<T1, TResult> Function => this.function ?? throw new InvalidOperationException(CurryingFunc.NotInitializedMessage);

        public CurryingFunc([NotNull] Func<T1, TResult> func)
        {
            this.function = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Func<TResult> Currying(T1 argument)
        {
            var func = this.Function;
            return () => func(argument);
        }

        public TResult Invoke(T1 value) => this.Function(value);
    }

    public struct CurryingFunc<T1, T2, TResult>
    {
        private readonly Func<T1, T2, TResult> function;

        [NotNull]
        public Func<T1, T2, TResult> Function => this.function ?? throw new InvalidOperationException(CurryingFunc.NotInitializedMessage);

        public CurryingFunc([NotNull] Func<T1, T2, TResult> func)
        {
            this.function = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Func<TResult> Currying(T1 argument1, T2 argument2)
        {
            var func = this.Function;
            return () => func(argument1, argument2);
        }

        public CurryingFunc<T2, TResult> Currying(T1 argument)
        {
            var func = this.Function;
            return CurryingFunc.Currying<T2, TResult>(z => func(argument, z));
        }

        public TResult Invoke(T1 value1, T2 value2) => this.Function(value1, value2);
    }

    public struct CurryingFunc<T1, T2, T3, TResult>
    {
        private readonly Func<T1, T2, T3, TResult> function;

        [NotNull]
        public Func<T1, T2, T3, TResult> Function => this.function ?? throw new InvalidOperationException(CurryingFunc.NotInitializedMessage);

        public CurryingFunc([NotNull] Func<T1, T2, T3, TResult> func)
        {
            this.function = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Func<TResult> Currying(T1 argument1, T2 argument2, T3 argument3)
        {
            var func = this.Function;
            return () => func(argument1, argument2, argument3);
        }

        public CurryingFunc<T3, TResult> Currying(T1 argument1, T2 argument2)
        {
            var func = this.Function;
            return CurryingFunc.Currying<T3, TResult>(z => func(argument1, argument2, z));
        }

        public CurryingFunc<T2, T3, TResult> Currying(T1 argument1)
        {
            var func = this.Function;
            return CurryingFunc.Currying<T2, T3, TResult>((z, x) => func(argument1, z, x));
        }

        public TResult Invoke(T1 value1, T2 value2, T3 value3) => this.Function(value1, value2, value3);
    }
}
