using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection
{
    public class MemberResolveException : Exception
    {
        public MemberResolveException(MemberInfo member)
        {
            this.Member = member;
        }

        public MemberInfo Member { get; }
    }
}
