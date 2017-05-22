using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Reflection
{
    public static class MethodInfoExtensions
    {
        public static Func<object, object[], object> CompileFunc([NotNull] this MethodInfo method)
            => new MethodExpressionBuilder(method).CompileFunc();

        public static Action<object, object[]> CompileAction([NotNull] this MethodInfo method)
            => new MethodExpressionBuilder(method).CompileAction();

        /// <summary>
        /// For static method, e.g. : string.IsNullOrEmpty => Func&lt;string, bool&gt;.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static TDelegate Compile<TClass, TDelegate>([NotNull] this MethodInfo method)
            => new MethodExpressionBuilder(method).Compile<TClass, TDelegate>();

        private class MethodExpressionBuilder
        {
            private readonly MethodInfo method;
            private readonly ParameterInfo[] methodParameters;
            private readonly int argumentsOffset;
            private readonly bool isStatic;

            public MethodExpressionBuilder([NotNull] MethodInfo method)
            {
                this.method = method ?? throw new ArgumentNullException(nameof(method));
                this.methodParameters = method.GetParameters();
                this.isStatic = method.IsStatic;
                this.argumentsOffset = this.isStatic ? 0 : 1;
            }

            public TDelegate Compile<TClass, TDelegate>()
            {
                var lambdaArguments = new ParameterExpression[this.methodParameters.Length + this.argumentsOffset];
                var methodArguments = new Expression[this.methodParameters.Length];
                for (var i = 0; i < this.methodParameters.Length; i++)
                {
                    var arg = Expression.Parameter(this.methodParameters[i].ParameterType);
                    lambdaArguments[i + this.argumentsOffset] = arg;
                    methodArguments[i] = arg;
                }
                Expression invoker;
                if (this.isStatic)
                {
                    invoker = Expression.Call(this.method, methodArguments);
                }
                else
                {
                    lambdaArguments[0] = Expression.Parameter(typeof(TClass));
                    invoker = Expression.Call(lambdaArguments[0], this.method, methodArguments);
                }

                return Expression.Lambda<TDelegate>(invoker, lambdaArguments).Compile();
            }

            public Func<object, object[], object> CompileFunc()
            {
                if (this.method.ReturnType == typeof(void)) throw new InvalidOperationException();
                return this.CompileObject<Func<object, object[], object>>(false);
            }

            public Action<object, object[]> CompileAction()
                => this.CompileObject<Action<object, object[]>>(true);

            private TDelegate CompileObject<TDelegate>(bool returnVoid)
            {
                var lambdaArguments = new[] { Expression.Parameter(typeof(object)), Expression.Parameter(typeof(object[])) };
                var methodArguments = new Expression[this.methodParameters.Length];
                for (var i = 0; i < this.methodParameters.Length; i++)
                {
                    var arg = Expression.ArrayIndex(lambdaArguments[1], Expression.Constant(i));
                    methodArguments[i] = Expression.Convert(arg, this.methodParameters[i].ParameterType);
                }

                Expression invoker;
                if (this.isStatic)
                {
                    invoker = Expression.Call(this.method, methodArguments);
                }
                else
                {
                    Expression instance = lambdaArguments[0];
                    if (this.method.ReturnType != typeof(object))
                        instance = Expression.Convert(instance, this.method.DeclaringType);
                    invoker = Expression.Call(instance, this.method, methodArguments);
                }

                if (returnVoid)
                {
                    return Expression.Lambda<TDelegate>(invoker, lambdaArguments).Compile();
                }
                else
                {
                    if (this.method.ReturnType != typeof(object))
                        invoker = Expression.Convert(invoker, typeof(object));
                    return Expression.Lambda<TDelegate>(invoker, lambdaArguments).Compile();
                }
            }
        }
    }
}