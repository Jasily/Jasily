using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// exception when a required member cannot be resolve.
    /// </summary>
    public class MemberResolveException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        internal MemberResolveException(MemberInfo member)
        {
            this.Member = member;
        }

        /// <summary>
        /// which member cannot be resolve.
        /// </summary>
        public MemberInfo Member { get; }
    }
}
