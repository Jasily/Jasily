using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.Converters.StringValueConverters
{
    public abstract class BaseStringConverter
    {
        private static readonly IReadOnlyCollection<Type> SourceType = new ReadOnlyCollection<Type>(new[] { typeof(string) });

        public IEnumerable<Type> ConvertSourceType => SourceType;
    }

    public abstract class BaseStringConverter<T> : BaseStringConverter,
        IStringValueConverter<T>
    {
        public T Convert(string value)
        {
            try
            {
                return this.ConvertCore(value);
            }
            catch (ConvertException)
            {
                throw;
            }
            catch (Exception e)
            {
                var msg = string.IsNullOrWhiteSpace(e.Message)
                    ? $"connot convert value <{value}> to type <{typeof(T).Name}>"
                    : $"connot convert value <{value}> to type <{typeof(T).Name}>: {e.Message}";
                throw new ConvertException(msg, e);
            }
        }

        public bool TryConvert(string value, out T result)
        {
            try
            {
                return this.TryConvertCore(value, out result);
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        protected abstract T ConvertCore([NotNull] string value);

        protected virtual bool TryConvertCore(string value, out T result)
        {
            try
            {
                result = this.ConvertCore(value);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        public bool TryConvert(string source, Type destType, out object value)
        {
            if (destType != typeof(T)) throw new NotSupportedException();
            
            if (this.TryConvert(source, out var result))
            {
                value = result;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public object Convert(string source, Type destType)
        {
            if (destType != typeof(T)) throw new NotSupportedException();

            return this.Convert(source);
        }

        public bool CanConvertTo(Type sourceType, Type destType)
        {
            return sourceType == typeof(string) && destType == typeof(T);
        }

        bool IValueConverter.TryConvert(object source, Type destType, out object value)
        {
            if (source is string s)
            {
                return this.TryConvert(s, destType, out value);
            }

            throw new NotSupportedException();
        }

        object IValueConverter.Convert(object source, Type destType)
        {
            if (source is string s)
            {
                return this.Convert(s, destType);
            }

            throw new NotSupportedException();
        }
    }
}
