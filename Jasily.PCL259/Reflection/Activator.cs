using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Jasily.Cache;
using Jasily.Reflection.Attributes;

namespace Jasily.Reflection
{
    public class Activator<T> : IActivator
    {
        private static readonly System.Func<T> ActivatorFunc;

        static Activator()
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            object[] entryParams = null;

            var ctors = typeInfo.DeclaredConstructors.ToArray();
            var entryCtor = ctors.SingleOrDefault(z => z.GetCustomAttribute<EntryAttribute>() != null);
            if (entryCtor != null)
            {
                var parameters = entryCtor.GetParameters();
                if (parameters.Length == 0)
                {
                    entryParams = Empty<object>.Array;
                }
                else
                {
                    entryParams = new object[parameters.Length];
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        var dv = parameter.GetCustomAttribute<DefaultValueAttribute>();
                        if (dv != null)
                        {
                            entryParams[i] = dv.Value;
                        }
                        else if (parameter.HasDefaultValue)
                        {
                            entryParams[i] = parameter.DefaultValue;
                        }
                        else if (parameter.ParameterType.GetTypeInfo().IsValueType)
                        {
                            entryParams[i] = Activator.CreateInstance(parameter.ParameterType);
                        }
                        else
                        {
                            entryParams[i] = null;
                        }
                    }
                }
            }
            else
            {
                foreach (var ctor in ctors)
                {
                    var parameters = ctor.GetParameters();
                    if (parameters.Length == 0)
                    {
                        entryCtor = ctor;
                        entryParams = Empty<object>.Array;
                    }
                    else
                    {
                        if (parameters.All(z => z.HasDefaultValue))
                        {
                            if (entryCtor == null)
                            {
                                entryCtor = ctor;
                                entryParams = parameters.Select(z => z.DefaultValue).ToArray();
                            }
                        }
                    }
                }
            }

            Debug.Assert((entryCtor != null && entryParams != null) || (entryCtor == null && entryParams == null));

            if (entryCtor != null)
            {
                var @new = entryParams.Length == 0
                    ? Expression.New(entryCtor)
                    : Expression.New(entryCtor, entryParams.Select(Expression.Constant));

                ActivatorFunc = Expression.Lambda<System.Func<T>>(@new).Compile();
            }
        }

        public T Create()
        {
            if (ActivatorFunc == null) throw new NotSupportedException("cannot create instance.");
            return ActivatorFunc();
        }

        object IActivator.Create() => this.Create();
    }
}