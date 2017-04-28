using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal interface IInternalMemberInjectorFactory
    {
        IServiceProvider ServiceProvider { get; }

        bool IsValueType { get; }

        MemberInjector InternalGetMemberInjector(MemberInfo member);
    }
}
