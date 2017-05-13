using System;
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
            [NotNull] MethodInfo onCompletedMethod, [CanBeNull] MethodInfo unsafeOnCompletedMethod)
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
            this.OnCompletedMethod = onCompletedMethod;
            this.UnsafeOnCompletedMethod = unsafeOnCompletedMethod;
            this.GetResultMethod = getResultMethod;
            this.IsCompletedGetMethod = isCompletedGetMethod;

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

        [CanBeNull]
        public MethodInfo UnsafeOnCompletedMethod { get; }

        [NotNull]
        public Type ResultType { get; }

        public static AwaitableInfo TryBuild(Type type)
        {
            var getAwaiterMethodInfo = type.GetRuntimeMethods().FirstOrDefault(z =>
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

            var unsafeOnCompletedMethodInfo = CriticalNotifyCompletionTypeInfo.IsAssignableFrom(awaiterTypeInfo)
                ? awaiterTypeInfo
                    .GetRuntimeInterfaceMap(typeof(ICriticalNotifyCompletion)).TargetMethods
                    .SingleOrDefault(z => z.Name == nameof(ICriticalNotifyCompletion.UnsafeOnCompleted))
                : null;

            return new AwaitableInfo(
                type, getAwaiterMethodInfo,
                awaiterType, getResultMethodInfo, isCompletedMethod,
                onCompletedMethodInfo, unsafeOnCompletedMethodInfo);
        }
    }
}