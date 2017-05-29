using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jasily.DependencyInjection.ComplexGenerics;
using Jasily.DependencyInjection.ComplexGenerics.Internal;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    [TestClass]
    public class UnitTest1
    {
        public interface IInterface1<TI1, TI2, TI3>
        {

        }

        public class Class1<TC1, TC2> : IInterface1<TC1, TC2, TC1>
        {

        }

        public class Class2<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
        {

        }

        [TestMethod]
        public void TestDefault()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddComplexGenerics()
                .AddTransient(typeof(Class1<,>).GetTypeInfo().GetInterfaces().Single(), typeof(Class1<,>))
                .AddTransient(typeof(Class2<>).GetTypeInfo().GetInterfaces().Single(), typeof(Class2<>));

            var provider = serviceCollection.BuildServiceProvider();
            Assert.IsInstanceOfType(provider.GetRequiredComplexService<IInterface1<int, int, int>>(),
                typeof(Class1<int, int>));
            Assert.IsInstanceOfType(provider.GetRequiredComplexService<IInterface1<string, int, string>>(),
                typeof(Class1<string, int>));
            Assert.IsNull(provider.GetComplexService<IInterface1<int, double, string>>());

            Assert.IsInstanceOfType(provider.GetRequiredComplexService<IInterface1<string, List<string>, Dictionary<string, HashSet<string>>>>(),
                typeof(Class2<string>));
            Assert.IsNull(provider.GetComplexService<IInterface1<string, List<string>, Dictionary<object, HashSet<string>>>>());
            Assert.IsNull(provider.GetComplexService<IInterface1<string, List<string>, Dictionary<string, HashSet<int>>>>());
            Assert.IsNull(provider.GetComplexService<IInterface1<string, List<int>, Dictionary<string, HashSet<string>>>>());
            Assert.IsNull(provider.GetComplexService<IInterface1<int, List<string>, Dictionary<string, HashSet<string>>>>());
        }

        public class Class3<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
            where TC1 : class
        {

        }

        public class Class4<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
            where TC1 : struct
        {

        }

        public class Class5<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
            where TC1 : class, IDisposable
        {

        }

        public class Class6<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
            where TC1 : class, IDisposable, new()
        {

        }

        public class Class7<TC1> : IInterface1<TC1, List<TC1>, Dictionary<string, HashSet<TC1>>>
            where TC1 : struct, IDisposable
        {

        }

        public class SimpleDisposableClass : IDisposable
        {
            public SimpleDisposableClass(int _) { }

            public void Dispose()
            {

            }
        }

        public class SimpleDisposableClass_New : IDisposable
        {
            public void Dispose()
            {

            }
        }

        public struct SimpleDisposableValue : IDisposable
        {
            public void Dispose()
            {

            }
        }

        [TestMethod]
        public void TestGenericParameterConstraint()
        {
            var serviceCollection = new ServiceCollection();
            var builder = serviceCollection.AddComplexGenerics();
            builder.AddTransient(
                typeof(Class3<>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class3<>));
            builder.AddTransient(
                typeof(Class4<>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class4<>));
            builder.AddTransient(
                typeof(Class5<>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class5<>));
            builder.AddTransient(
                typeof(Class6<>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class6<>));
            builder.AddTransient(
                typeof(Class7<>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class7<>));

            var provider = serviceCollection.BuildServiceProvider();

            Assert.IsInstanceOfType(provider.GetRequiredComplexService<IInterface1<int, List<int>, Dictionary<string, HashSet<int>>>>(),
                typeof(Class4<int>));

            Assert.IsInstanceOfType(
                provider.GetRequiredComplexService<
                    IInterface1<string,
                        List<string>,
                        Dictionary<string, HashSet<string>>>>(), typeof(Class3<string>));

            Assert.IsInstanceOfType(provider.GetRequiredComplexService<
                IInterface1<SimpleDisposableClass_New,
                    List<SimpleDisposableClass_New>,
                    Dictionary<string, HashSet<SimpleDisposableClass_New>>>>(),
                typeof(Class6<SimpleDisposableClass_New>));

            Assert.IsInstanceOfType(
                provider.GetRequiredComplexService<
                    IInterface1<SimpleDisposableClass,
                        List<SimpleDisposableClass>,
                        Dictionary<string, HashSet<SimpleDisposableClass>>>>(),
                typeof(Class5<SimpleDisposableClass>));

            Assert.IsInstanceOfType(
                provider.GetRequiredComplexService<
                    IInterface1<SimpleDisposableValue,
                        List<SimpleDisposableValue>,
                        Dictionary<string, HashSet<SimpleDisposableValue>>>>(),
                typeof(Class7<SimpleDisposableValue>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBuilder()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(
                typeof(Class1<,>).GetTypeInfo().GetInterfaces().Single(),
                typeof(Class2<>));
            serviceCollection.BuildServiceProvider();
        }
    }
}
