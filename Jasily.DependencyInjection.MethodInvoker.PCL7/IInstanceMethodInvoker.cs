﻿using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IInstanceMethodInvoker<in T>
    {
        object Invoke(T instance, OverrideArguments arguments);
    }

    public interface IInstanceMethodInvoker<in T, out TResult>
    {
        TResult Invoke(T instance, OverrideArguments arguments);
    }
}
