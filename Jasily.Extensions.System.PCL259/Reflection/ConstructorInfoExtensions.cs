using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class ConstructorInfoExtensions
    {
        public static Func<object[], object> Compile([NotNull] this ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            var parameters = constructor.GetParameters();
            var lambdaArgument = Expression.Parameter(typeof(object[]));
            var constructorArguments = new Expression[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                Expression arg = Expression.ArrayIndex(lambdaArgument, Expression.Constant(i));
                arg = parameter.ParameterType == typeof(object)
                    ? arg
                    : Expression.Convert(arg, parameter.ParameterType);
                constructorArguments[i] = arg;
            }
            Expression invoker = Expression.New(constructor, constructorArguments);

            if (constructor.DeclaringType != typeof(object))
            {
                invoker = Expression.Convert(invoker, typeof(object));
            }

            return Expression.Lambda<Func<object[], object>>(invoker, lambdaArgument).Compile();
        }

        public static TDelegate Compile<TDelegate>([NotNull] this ConstructorInfo constructor)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            var parameters = constructor.GetParameters();
            var lambdaArguments = new ParameterExpression[parameters.Length];
            var constructorArguments = new Expression[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                constructorArguments[i] = lambdaArguments[i] = Expression.Parameter(parameter.ParameterType);
            }
            Expression invoker = Expression.New(constructor, constructorArguments);
            return Expression.Lambda<TDelegate>(invoker, lambdaArguments).Compile();
        }
    }
}