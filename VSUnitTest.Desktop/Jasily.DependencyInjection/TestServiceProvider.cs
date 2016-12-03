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
            list.AddSingleton("key", "5");
            list.AddSingleton("value", 8);
            list.AddTransient<KeyValuePair<string, int>>(null);
            var provider = ServiceProvider.Build(list, new ServiceProviderSettings { EnableDebug = true });
            foreach (var _ in Enumerable.Range(0, 10))
            {
                var service = provider.GetService(typeof(KeyValuePair<string, int>));
                Assert.IsNotNull(service);
                Assert.IsInstanceOfType(service, typeof(KeyValuePair<string, int>));
                var kvp = (KeyValuePair<string, int>)service;
                Assert.AreEqual("5", kvp.Key);
                Assert.AreEqual(8, kvp.Value);
            }
        }
    }
}
