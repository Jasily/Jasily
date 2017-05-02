﻿using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.MethodInvoker
{
    [TestClass]
    public class TestMethodInvoker
    {
        public class Class1
        {
            public int Method1()
            {
                return 1;
            }

            public static int Method2()
            {
                return 2;
            }

            public void Method3()
            {
                
            }

            public static void Method4()
            {
                
            }

            public int Method5(int key)
            {
                return 1;
            }

            public static int Method6(int key)
            {
                return 2;
            }

            public void Method7(int key)
            {

            }

            public static void Method8(int key)
            {

            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var sc = new ServiceCollection();
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMethodInvokerFactory<Class1>>();
            Assert.IsNotNull(factory);

            var method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method1));
            Assert.AreEqual(1, factory.InvokeInstanceMethod(method, new Class1(), provider));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method2));
            Assert.AreEqual(2, factory.InvokeStaticMethod(method, provider));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method3));
            Assert.AreEqual(null, factory.InvokeInstanceMethod(method, new Class1(), provider));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method4));
            Assert.AreEqual(null, factory.InvokeStaticMethod(method, provider));

            var args = new OverrideArguments();
            args.AddArgument("key", 1);

            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method5));
            Assert.AreEqual(1, factory.InvokeInstanceMethod(method, new Class1(), provider, args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method6));
            Assert.AreEqual(2, factory.InvokeStaticMethod(method, provider, args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method7));
            Assert.AreEqual(null, factory.InvokeInstanceMethod(method, new Class1(), provider, args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method8));
            Assert.AreEqual(null, factory.InvokeStaticMethod(method, provider, args));
        }
    }

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
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMethodInvokerFactory<Class1>>();
            Assert.IsNotNull(factory);

            var property = typeof(Class1).GetRuntimeProperties().Single(z => z.Name == nameof(Class1.PropertyGetterOnly1));
            Assert.AreEqual(1, factory.InvokeInstanceMethod(property.GetMethod, new Class1(), provider));
        }
    }
}
