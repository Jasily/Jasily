﻿using System.Text;
using Jasily.Features.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Features.Standard.Test.DependencyInjection
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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

            var sb1 = provider.GetRequiredService<IFeaturesFactory<object>>().TryCreateFeature<StringBuilder>(obj, true);
            Assert.IsNotNull(sb1);
            Assert.AreEqual(obj.ToString(), sb1.ToString());

            var sb2 = provider.GetRequiredService<IFeaturesFactory<ServiceCollection>>().TryCreateFeature<StringBuilder>(obj, true);
            Assert.IsNotNull(sb2);
            Assert.AreEqual(obj.ToString(), sb2.ToString());

            Assert.AreNotSame(sb1, sb2);
        }
    }
}
