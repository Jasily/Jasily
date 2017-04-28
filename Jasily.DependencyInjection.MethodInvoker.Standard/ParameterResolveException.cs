using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public class ParameterResolveException : Exception
    {
        public ParameterResolveException(ParameterInfo parameter)
        {
            this.Parameter = parameter;
        }

        public ParameterInfo Parameter { get; }
    }
}
