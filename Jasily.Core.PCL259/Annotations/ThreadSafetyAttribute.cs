using System;

namespace Jasily.Core.Annotations
{
    /// <summary>
    /// Indicates the method can invoke in mulit thread.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ThreadSafetyAttribute : AnnotationAttribute
    {
        
    }
}