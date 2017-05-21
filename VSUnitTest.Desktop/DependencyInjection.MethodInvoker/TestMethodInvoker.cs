using System;
using System.Linq;
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
            sc.AddMethodInvoker();
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

        public abstract class Class2
        {
            public abstract int AbstractMethod();

            public virtual int VirutualMethod() => 1;
        }

        public class Class2SubClass : Class2
        {
            public override int AbstractMethod() => 2;

            public override int VirutualMethod() => 2;

            public override string ToString() => "2";
        }

        public class Class2SubClassSubClass : Class2SubClass
        {
            public override int AbstractMethod() => 3;

            public override int VirutualMethod() => 3;
        }

        [TestMethod]
        public void TestInheritMethod()
        {
            var sc = new ServiceCollection();
            sc.AddMethodInvoker();
            var provider = sc.BuildServiceProvider();

            void AssertExpected<T>(IMethodInvokerFactory factory, T instance, object value1, object value2, object value3)
            {
                Assert.IsNotNull(factory);

                MethodInfo method;
                IObjectMethodInvoker invoker;

                method = typeof(T).GetRuntimeMethods().Single(z => z.Name == nameof(Class2SubClass.AbstractMethod));
                invoker = factory.GetObjectMethodInvoker(method);
                Assert.AreEqual(value1, invoker.Invoke(instance, provider));

                method = typeof(T).GetRuntimeMethods().Single(z => z.Name == nameof(Class2.VirutualMethod));
                invoker = factory.GetObjectMethodInvoker(method);
                Assert.AreEqual(value2, invoker.Invoke(instance, provider));

                method = typeof(T).GetRuntimeMethods().Single(z => z.Name == nameof(Class2.ToString));
                invoker = factory.GetObjectMethodInvoker(method);
                Assert.AreEqual(value3, invoker.Invoke(instance, provider));
            }

            var obj1 = new Class2SubClass();
            var f = (IMethodInvokerFactory) provider.GetService<IMethodInvokerFactory<Class2SubClass>>();
            AssertExpected<Class2>(f, obj1, 2, 2, "2");
            AssertExpected<Class2SubClass>(f, obj1, 2, 2, "2");

            var obj2 = new Class2SubClassSubClass();
            f = provider.GetService<IMethodInvokerFactory<Class2SubClassSubClass>>();
            AssertExpected<Class2>(f, obj2, 3, 3, "2");
            AssertExpected<Class2SubClass>(f, obj2, 3, 3, "2");
            AssertExpected<Class2SubClassSubClass>(f, obj2, 3, 3, "2");
        }
    }
}
