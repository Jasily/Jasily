using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jasily.DependencyInjection.MemberInjection;
using System.Text;

namespace VSUnitTest.Desktop.Jasily.DependencyInjection.MemberInjection
{
    [TestClass]
    public class TestMemberInjector
    {
        public class TestClass1
        {
            public StringBuilder Property { get; set; }

            public string Field;
        }

        [TestMethod]
        public void TestProperty()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<StringBuilder>();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetProperty("Property")).Inject(instance, false);
            Assert.IsNotNull(instance.Property);
        }

        [TestMethod]
        public void TestProperty_UnResolve()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetProperty("Property")).Inject(instance, false);
            Assert.IsNull(instance.Property);
        }

        [TestMethod]
        public void TestProperty_Required()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<StringBuilder>();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetProperty("Property")).Inject(instance, true);
            Assert.IsNotNull(instance.Property);
        }

        [ExpectedException(typeof(MemberResolveException))]
        [TestMethod]
        public void TestProperty_RequiredUnResolve()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetProperty("Property")).Inject(instance, true);
            Assert.IsNotNull(instance.Property);
        }

        [TestMethod]
        public void TestField()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton("123");
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetField("Field")).Inject(instance, false);
            Assert.AreEqual("123", instance.Field);
        }

        [TestMethod]
        public void TestField_UnResolve()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetField("Field")).Inject(instance, false);
            Assert.IsNull(instance.Field);
        }

        [TestMethod]
        public void TestField_Required()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton("123");
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetField("Field")).Inject(instance, true);
            Assert.AreEqual("123", instance.Field);
        }

        [ExpectedException(typeof(MemberResolveException))]
        [TestMethod]
        public void TestField_RequiredUnResolve()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            var provider = sc.BuildServiceProvider();
            var factory = provider.GetService<IMemberInjectorFactory<TestClass1>>();
            var instance = new TestClass1();
            factory.GetMemberInjector(typeof(TestClass1).GetField("Field")).Inject(instance, true);
            Assert.AreEqual("123", instance.Field);
        }
    }
}
