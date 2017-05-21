using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.MethodInvoker
{
    [TestClass]
    public class TestPropertyInvoker
    {
        public class Class1
        {
            public int PropertyGetterOnly1 { get; } = 1;

            public int PropertyGetterOnly2 => 1;

            // ReSharper disable once ValueParameterNotUsed
            public int PropertySetterOnly { set {} }
        }

        [TestMethod]
        public void PropertyGetterSetterCanBeNull()
        {
            var property1 = typeof(Class1).GetRuntimeProperties()
                .Single(z => z.Name == nameof(Class1.PropertyGetterOnly1));
            Assert.IsNotNull(property1.GetMethod);
            Assert.IsNull(property1.SetMethod);

            var propertyGetterOnly = typeof(Class1).GetRuntimeProperties()
                .Single(z => z.Name == nameof(Class1.PropertyGetterOnly2));
            Assert.IsNotNull(propertyGetterOnly.GetMethod);
            Assert.IsNull(propertyGetterOnly.SetMethod);

            var propertyPropertySetterOnly = typeof(Class1).GetRuntimeProperties()
                .Single(z => z.Name == nameof(Class1.PropertySetterOnly));
            Assert.IsNotNull(propertyPropertySetterOnly.SetMethod);
            Assert.IsNull(propertyPropertySetterOnly.GetMethod);
        }

        [TestMethod]
        public void MethodInvokerCanAccessPropertyGetter()
        {
            var sc = new ServiceCollection();
            sc.AddMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMethodInvokerFactory<Class1>>();
            Assert.IsNotNull(factory);

            var property = typeof(Class1).GetRuntimeProperties().Single(z => z.Name == nameof(Class1.PropertyGetterOnly1));
            Assert.AreEqual(1, factory.InvokeInstanceMethod(property.GetMethod, new Class1(), provider));
        }
    }
}