using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Jasily.Cache.Internal;
using Jasily.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Reflection.Descriptors
{
    [TestClass]
    public class TestTypeDescriptor
    {
        public class Wrapper<T, TProperty> where T : class
        {
            private TProperty property2;

            public Wrapper(T obj)
            {
                this.WrappedObject = obj ?? throw new ArgumentNullException(nameof(obj));
            }

            public T WrappedObject { get; }

            public TProperty Property1 { get; set; }

            public TProperty Property2 {
                get
                {
                    var p = this.Property2;
                    if (p != null) return p;
                    throw new NotImplementedException();
                }
                set { this.property2 = value; } }
        }

        public class Wrapper<T> : Wrapper<T, object> where T : class
        {
            public static readonly Func<Wrapper<T>, T> SelecterFunc = z => z.WrappedObject;

            public Wrapper(T obj)
                : base(obj)
            {
            }
        }

        [TestMethod]
        public void TestField()
        {
            var type = typeof(int);
            var list = type.GetRuntimeFields().ToList();
            Action action1 = () => type.GetRuntimeFields().EmptyForEach();
            action1();
            Action action2 = () => list.EmptyForEach();
            action2();
            Action action3 = () => list.Select(z => z).EmptyForEach();
            action3();
            var wrapper = new Wrapper<Type, List<Wrapper<FieldInfo>>>(type);
            wrapper.Property1 = list.Select(z => new Wrapper<FieldInfo>(z)).ToList();
            wrapper.Property2 = wrapper.Property1;
            Action action4 = () => wrapper.Property1.Select(Wrapper<FieldInfo>.SelecterFunc).EmptyForEach();
            action4();
            Action action5 = () => list.Select(z => z).EmptyForEach();
            action5();
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(10000000, action1);
                var r2 = timer.Test(10000000, action2);
                var r3 = timer.Test(10000000, action3);
                var r4 = timer.Test(10000000, action4);
                var r5 = timer.Test(10000000, action5);
                Console.WriteLine(r1);
                Console.WriteLine(r2);
                Console.WriteLine(r3);
                Console.WriteLine(r4);
                Console.WriteLine(r5);
            }
        }

        [TestMethod]
        public void TestProperty()
        {
            var type = typeof(int);
            var list = type.GetRuntimeProperties().ToList();
            Action action1 = () => type.GetRuntimeProperties().EmptyForEach();
            action1();
            Action action2 = () => list.EmptyForEach();
            action2();
            Action action3 = () => list.Select(z => z).EmptyForEach();
            action3();
            var wrapper = new Wrapper<Type, List<Wrapper<PropertyInfo>>>(type);
            wrapper.Property1 = list.Select(z => new Wrapper<PropertyInfo>(z)).ToList();
            wrapper.Property2 = wrapper.Property1;
            Action action4 = () => wrapper.Property1.Select(Wrapper<PropertyInfo>.SelecterFunc).EmptyForEach();
            action4();
            Action action5 = () => list.Select(z => z).EmptyForEach();
            action5();
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(10000000, action1);
                var r2 = timer.Test(10000000, action2);
                var r3 = timer.Test(10000000, action3);
                var r4 = timer.Test(10000000, action4);
                var r5 = timer.Test(10000000, action5);
                Console.WriteLine(r1);
                Console.WriteLine(r2);
                Console.WriteLine(r3);
                Console.WriteLine(r4);
                Console.WriteLine(r5);
            }
        }

        [TestMethod]
        public void TestMethod()
        {
            var type = typeof(int);
            var list = type.GetRuntimeMethods().ToList();
            Action action1 = () => type.GetRuntimeMethods().EmptyForEach();
            action1();
            Action action2 = () => list.EmptyForEach();
            action2();
            Action action3 = () => list.Select(z => z).EmptyForEach();
            action3();
            var wrapper = new Wrapper<Type, List<Wrapper<MethodInfo>>>(type);
            wrapper.Property1 = list.Select(z => new Wrapper<MethodInfo>(z)).ToList();
            wrapper.Property2 = wrapper.Property1;
            Action action4 = () => wrapper.Property1.Select(Wrapper<MethodInfo>.SelecterFunc).EmptyForEach();
            action4();
            Action action5 = () => list.Select(z => z).EmptyForEach();
            action5();
            using (var timer = new CodeTimer())
            {
                var r1 = timer.Test(1000000, action1);
                var r2 = timer.Test(1000000, action2);
                var r3 = timer.Test(1000000, action3);
                var r4 = timer.Test(1000000, action4);
                var r5 = timer.Test(1000000, action5);
                Console.WriteLine(r1);
                Console.WriteLine(r2);
                Console.WriteLine(r3);
                Console.WriteLine(r4);
                Console.WriteLine(r5);
            }
        }

        [TestMethod]
        public void TestAttribute()
        {
            var type = typeof(int);
            Action action1 = () => type.GetRuntimeMethods().First().GetCustomAttributes<DebuggerDisplayAttribute>().Any();
            action1();
            var typeDescriptor = Default<TypeDescriptor<int>>.Value();
                Default<TypeDescriptor<int>>.Value()
                    .RuntimeMethods()
                    .First()
                    .HasCustomAttribute<DebuggerDisplayAttribute>();
            Action action2 = () => typeDescriptor.RuntimeMethods().First().HasCustomAttribute<DebuggerDisplayAttribute>();
            CodeTimerHelper.FastThen(action2, action1, 10, 1000 * 1000);
        }
    }
}
