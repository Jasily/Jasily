﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MethodInvoker;
using System.Reflection;
using System.Linq;

namespace VSUnitTest.Desktop.Jasily.DependencyInjection.MethodInvoker
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
        }

        [TestMethod]
        public void TestMethod1()
        {
            var sc = new ServiceCollection();
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var invoker = provider.GetService<IMethodInvoker<Class1>>();
            Assert.IsNotNull(invoker);
            var method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method1));
            Assert.AreEqual(1, invoker.Invoke(method, new Class1()));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method2));
            Assert.AreEqual(2, invoker.Invoke(method, new Class1()));
        }
    }
}
