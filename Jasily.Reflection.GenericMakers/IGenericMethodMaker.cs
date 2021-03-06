﻿using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection.GenericMakers
{
    public interface IGenericMethodMaker
    {
        [NotNull]
        MethodInfo MakeGenericMethod([NotNull] params Type[] typeArguments);
    }
}