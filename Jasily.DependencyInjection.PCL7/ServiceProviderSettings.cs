using System;
using System.Collections.Generic;


namespace Jasily.DependencyInjection
{
    public struct ServiceProviderSettings
    {
        public const int DefaultCompileAfterCallCount = 2;

        /// <summary>
        /// after this call count, will compile the Expression.
        /// default value is 2.
        /// </summary>
        public int? CompileAfterCallCount { get; set; }

        public bool EnableDebug { get; set; }

        public StringComparer NameComparer { get; set; }

        public List<ResolveLevel> ResolveMode { get; set; }
    }
}