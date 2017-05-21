using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.Features
{
    [TestClass]
    public class TestFeatures
    {
        [TestMethod]
        public void TestFeaturesNormal()
        {
            var sc = new ServiceCollection();
            var obj = new ServiceCollection();
            sc.AddFeatures()
                .RegisterFeature<object, StringBuilder>(z =>
                {
                    Assert.AreSame(obj, z.Source);
                    Assert.IsNotNull(z.ServiceProvider);
                    return new StringBuilder(obj.ToString());
                });
            var provider = sc.BuildServiceProvider();

            var sb1 = provider.GetRequiredService<IFeaturesFactory<object>>().TryCreate<StringBuilder>(obj, true);
            Assert.IsNotNull(sb1);
            Assert.AreEqual(obj.ToString(), sb1.ToString());

            var sb2 = provider.GetRequiredService<IFeaturesFactory<ServiceCollection>>().TryCreate<StringBuilder>(obj, true);
            Assert.IsNotNull(sb2);
            Assert.AreEqual(obj.ToString(), sb2.ToString());

            Assert.AreNotSame(sb1, sb2);
        }
    }
}
