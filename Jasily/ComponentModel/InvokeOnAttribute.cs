using System;
using Jasily.Core.Annotations;

namespace Jasily.ComponentModel
{
    /// <summary>
    /// Indicates the method should invoke in thread.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InvokeOnAttribute : AnnotationAttribute
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="thread"></param>
        public InvokeOnAttribute(ThreadKind thread) { }
    }
}