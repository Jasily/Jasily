using System.Collections.Generic;
using System.Linq;
using Jasily.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSUnitTest.Desktop.Jasily.DependencyInjection
{
    [TestClass]
    public class TestServiceProvider
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = ServiceProvider.CreateServiceCollection();
            var obj1 = "5";
            var obj2 = new object();
            list.AddSingletonInstance("key", obj1);
            list.AddSingletonInstance("value", obj2);
            list.AddType<KeyValuePair<string, object>>(ServiceLifetime.Scoped, null);
            var provider = ServiceProvider.Build(list, new ServiceProviderSettings { EnableDebug = true });
            foreach (var _ in Enumerable.Range(0, 10))
            {
                var service = provider.GetService(typeof(KeyValuePair<string, object>));
                Assert.IsNotNull(service);
                Assert.IsInstanceOfType(service, typeof(KeyValuePair<string, object>));
                var kvp = (KeyValuePair<string, object>)service;
                Assert.AreEqual(obj1, kvp.Key);
                Assert.AreEqual(obj2, kvp.Value);
            }
        }
    }
}
