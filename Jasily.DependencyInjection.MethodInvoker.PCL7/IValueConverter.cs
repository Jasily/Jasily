using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IValueConverter<out TResult>
    {
        bool CanConvertFrom([CanBeNull] object value);

        TResult Convert([CanBeNull] object value);
    }
}
