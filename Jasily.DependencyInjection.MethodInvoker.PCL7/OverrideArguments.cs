using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public struct OverrideArguments
    {
        private Dictionary<string, object> data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="key"/> is <see cref="null"/>.</exception>
        public void AddArgument([NotNull] string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (this.data == null) this.data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.data.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="key"/> is <see cref="null"/>.</exception>
        /// <returns></returns>
        public bool TryGetValue([NotNull] string key, out object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (this.data != null)
            {
                return this.data.TryGetValue(key, out value);
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <param name="provider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="parameter"/> is <see cref="null"/>.</exception>
        /// <returns></returns>
        public bool TryGetValue<T>([NotNull] ParameterInfo parameter, out T value, [CanBeNull] IServiceProvider provider)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (this.data != null)
            {
                var result = this.data.TryGetValue(parameter.Name, out var ret);
                if (result)
                {
                    if (ret is T)
                    {
                        value = (T)ret;
                        return true;
                    }

                    var converter = provider?.GetService<IValueConverter<T>>();
                    if (converter?.CanConvertFrom(ret) == true)
                    {
                        value = converter.Convert(ret);
                        return true;
                    }

                    converter = provider.GetService<IValueConverterFactory>()?.GetValueConverter<T>();
                    if (converter?.CanConvertFrom(ret) == true)
                    {
                        value = converter.Convert(ret);
                        return true;
                    }

                    throw new InvalidOperationException();
                }
            }
            
            value = default(T);
            return false;
        }
    }
}
