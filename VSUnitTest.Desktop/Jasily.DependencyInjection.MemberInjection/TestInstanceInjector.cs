using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jasily.DependencyInjection.MemberInjection;
using Jasily.DependencyInjection.MemberInjection.AutoInjection;
using System.Text;
using System.IO;

namespace VSUnitTest.Desktop.Jasily.DependencyInjection.MemberInjection
{
    [TestClass]
    public class TestInstanceInjector
    {
        public class TestClass1
        {
            [Inject]
            public StringBuilder Property { get; set; }

            [Inject]
            public string Field;

            [Inject(false)]
            public StringBuilder OptionalProperty { get; set; }

            [Inject(false)]
            public string OptionalField;

            [Inject(false)]
            public MemoryStream OptionalProperty2 { get; set; }

            [Inject(false)]
            public Encoding OptionalField2;

            public StringBuilder IgnoreProperty { get; set; }
            
            public string IgnoreField;
        }

        [TestMethod]
        public void TestDefaultAction()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<StringBuilder>();
            sc.AddSingleton("123");
            sc.UseMemberInjector();
            sc.UseInstanceInjector();
            var provider = sc.BuildServiceProvider();
            var injector = provider.GetService<IInstanceInjector<TestClass1>>();
            var instance = new TestClass1();
            injector.Inject(instance);
            Assert.IsNotNull(instance.Property);
            Assert.AreEqual("123", instance.Field);
            Assert.IsNotNull(instance.OptionalProperty);
            Assert.AreEqual("123", instance.OptionalField);
            Assert.IsNull(instance.OptionalProperty2);
            Assert.IsNull(instance.OptionalField2);
            Assert.IsNull(instance.IgnoreProperty);
            Assert.IsNull(instance.IgnoreField);
        }

        [ExpectedException(typeof(MemberResolveException))]
        [TestMethod]
        public void TestDefault_UnResolve()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            sc.UseInstanceInjector();
            var provider = sc.BuildServiceProvider();
            var injector = provider.GetService<IInstanceInjector<TestClass1>>();
            var instance = new TestClass1();
            injector.Inject(instance);
            Assert.IsNotNull(instance.Property);
            Assert.AreEqual("123", instance.Field);
            Assert.IsNotNull(instance.OptionalProperty);
            Assert.AreEqual("123", instance.OptionalField);
            Assert.IsNull(instance.OptionalProperty2);
            Assert.IsNull(instance.OptionalField2);
            Assert.IsNull(instance.IgnoreProperty);
            Assert.IsNull(instance.IgnoreField);
        }

        public class TestClass2
        {
            [Inject]
            public StringBuilder Property { get; }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void TestError()
        {
            var sc = new ServiceCollection();
            sc.UseMemberInjector();
            sc.UseInstanceInjector();
            var provider = sc.BuildServiceProvider();
            var injector = provider.GetService<IInstanceInjector<TestClass2>>();
        }
    }
}
