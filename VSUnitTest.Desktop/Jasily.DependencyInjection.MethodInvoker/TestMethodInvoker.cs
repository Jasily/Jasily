using System;
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
            var invoker = provider.GetService<IMethodInvoker<Class1>>();
            Assert.IsNotNull(invoker);

            var method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method1));
            Assert.AreEqual(1, invoker.InvokeInstanceMethod(method, new Class1()));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method2));
            Assert.AreEqual(2, invoker.InvokeStaticMethod(method));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method3));
            Assert.AreEqual(null, invoker.InvokeInstanceMethod(method, new Class1()));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method4));
            Assert.AreEqual(null, invoker.InvokeStaticMethod(method));

            var args = new OverrideArguments();
            args.AddArgument("key", 1);

            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method5));
            Assert.AreEqual(1, invoker.InvokeInstanceMethod(method, new Class1(), args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method6));
            Assert.AreEqual(2, invoker.InvokeStaticMethod(method, args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method7));
            Assert.AreEqual(null, invoker.InvokeInstanceMethod(method, new Class1(), args));
            method = typeof(Class1).GetRuntimeMethods().Single(z => z.Name == nameof(Class1.Method8));
            Assert.AreEqual(null, invoker.InvokeStaticMethod(method, args));
        }
    }
}
