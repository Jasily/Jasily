using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class AwaitableInfo
    {
        private static readonly TypeInfo NotifyCompletionTypeInfo = typeof(INotifyCompletion).GetTypeInfo();
        private static readonly TypeInfo CriticalNotifyCompletionTypeInfo = typeof(ICriticalNotifyCompletion).GetTypeInfo();

        private AwaitableInfo(
            [NotNull] Type awaitableType, [NotNull] MethodInfo getAwaiterMethod,
            [NotNull] Type awaiterType, [NotNull] MethodInfo getResultMethod, [NotNull] MethodInfo isCompletedGetMethod,
            [NotNull] MethodInfo onCompletedMethod)
        {
            Debug.Assert(awaitableType != null);
            Debug.Assert(getAwaiterMethod != null);
            Debug.Assert(awaiterType != null);
            Debug.Assert(getResultMethod != null);
            Debug.Assert(isCompletedGetMethod != null);
            Debug.Assert(onCompletedMethod != null);

            this.AwaitableType = awaitableType;
            this.GetAwaiterMethod = getAwaiterMethod;
            this.AwaiterType = awaiterType;
            this.GetResultMethod = getResultMethod;
            this.IsCompletedGetMethod = isCompletedGetMethod;
            this.OnCompletedMethod = onCompletedMethod;

            this.ResultType = this.GetResultMethod.ReturnType;
        }

        [NotNull]
        public Type AwaitableType { get; }

        [NotNull]
        public MethodInfo GetAwaiterMethod { get; }

        [NotNull]
        public Type AwaiterType { get; }

        [NotNull]
        public MethodInfo GetResultMethod { get; }

        [NotNull]
        public MethodInfo IsCompletedGetMethod { get; }

        [NotNull]
        public MethodInfo OnCompletedMethod { get; }

        [NotNull]
        public Type ResultType { get; }

        [CanBeNull]
        public MethodInfo UnsafeOnCompletedMethod { get; private set; }

        [CanBeNull]
        public MethodInfo ConfigureAwaitMethod { get; private set; }

        [CanBeNull]
        public AwaitableInfo ConfigureAwaitAwaitableInfo { get; private set; }

        public static AwaitableInfo TryBuild(Type type)
        {
            Debug.Assert(type != null);
            return TryBuild(type, new Dictionary<Type, AwaitableInfo>());
        }

        private static AwaitableInfo TryBuild([NotNull] Type type, [NotNull] Dictionary<Type, AwaitableInfo> typeChains)
        {
            if (typeChains.TryGetValue(type, out var r))
            {
                return r;
            }

            var runtimeMethods = type.GetRuntimeMethods().ToArray();

            var getAwaiterMethodInfo = runtimeMethods.FirstOrDefault(z =>
                z.Name == nameof(Task.GetAwaiter) &&
                z.ReturnType != typeof(void) &&
                z.GetParameters().Length == 0);
            if (getAwaiterMethodInfo == null) return null;

            var awaiterType = getAwaiterMethodInfo.ReturnType;

            var isCompletedMethod = awaiterType
                .GetRuntimeProperties()
                .Where(z => z.Name == nameof(TaskAwaiter.IsCompleted) && z.PropertyType == typeof(bool))
                .SingleOrDefault(z => z.GetIndexParameters().Length == 0)?.GetMethod;
            if (isCompletedMethod == null) return null;

            var getResultMethodInfo = awaiterType
                .GetRuntimeMethods()
                .FirstOrDefault(z => z.Name == nameof(TaskAwaiter.GetResult) && z.GetParameters().Length == 0);
            if (getResultMethodInfo == null) return null;

            var awaiterTypeInfo = awaiterType.GetTypeInfo();

            if (!NotifyCompletionTypeInfo.IsAssignableFrom(awaiterTypeInfo)) return null;

            var onCompletedMethodInfo = awaiterTypeInfo.GetRuntimeInterfaceMap(typeof(INotifyCompletion)).TargetMethods
                .Single(z => z.Name == nameof(INotifyCompletion.OnCompleted));

            var info = new AwaitableInfo(
                type, getAwaiterMethodInfo,
                awaiterType, getResultMethodInfo, isCompletedMethod,
                onCompletedMethodInfo);
            typeChains.Add(type, info);

            if (CriticalNotifyCompletionTypeInfo.IsAssignableFrom(awaiterTypeInfo))
            {
                info.UnsafeOnCompletedMethod = awaiterTypeInfo
                    .GetRuntimeInterfaceMap(typeof(ICriticalNotifyCompletion)).TargetMethods
                    .SingleOrDefault(z => z.Name == nameof(ICriticalNotifyCompletion.UnsafeOnCompleted));
            }

            var configureAwaitMethodInfo = runtimeMethods.FirstOrDefault(z =>
                z.Name == nameof(Task.ConfigureAwait) &&
                z.ReturnType != typeof(void) &&
                z.GetParameters().Length == 0);
            var ps = configureAwaitMethodInfo?.GetParameters();
            if (ps?.Length == 1 && ps[0].ParameterType == typeof(bool))
            {
                var nextInfo = TryBuild(configureAwaitMethodInfo.ReturnType, typeChains);
                if (nextInfo != null)
                {
                    info.ConfigureAwaitMethod = configureAwaitMethodInfo;
                    info.ConfigureAwaitAwaitableInfo = nextInfo;
                }
            }

            return info;
        }


    }
}