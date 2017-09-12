using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Jasily.Features;
using Jasily.Features.DependencyInjection;

namespace Jasily.Features.Standard.Test
{
    [TestClass]
    public class TestBasicCall
    {
        private FeaturesProvider CreateFeaturesProvider()
        {
            var builder = new FeaturesBuilder();
            builder.RegisterFeature<string, StringBuilder>(z => new StringBuilder(z.Source), false);
            return builder.Build();
        }

        [TestMethod]
        public void TestFactoryNotNull()
        {
            var provider = this.CreateFeaturesProvider();

            Assert.IsNotNull(provider.GetFeaturesFactory<string>());
            Assert.IsNotNull(provider.GetFeaturesFactory<StringBuilder>());
            Assert.IsNotNull(provider.GetFeaturesFactory<TestBasicCall>());
        }

        [TestMethod]
        public void TestGetFeature()
        {
            var provider = this.CreateFeaturesProvider();

            // registered
            {
                var factory = provider.GetFeaturesFactory<string>();
                var sb = factory.TryCreateFeature<StringBuilder>("50", false);
                Assert.IsNotNull(sb);
                Assert.AreEqual("50", sb.ToString());
            }

            // unregistered
            {
                var factory = provider.GetFeaturesFactory<string>();
                Assert.IsNull(factory.TryCreateFeature<string>("50", false));
                Assert.IsNull(factory.TryCreateFeature<string>("50", false));
            }

            // unregistered factory
            {
                var factory = provider.GetFeaturesFactory<StringBuilder>();
                Assert.IsNull(factory.TryCreateFeature<string>(new StringBuilder(), false));
                Assert.IsNull(factory.TryCreateFeature<string>(new StringBuilder(), false));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInstanceCanNotBeNull()
        {
            var provider = this.CreateFeaturesProvider();
            var factory = provider.GetFeaturesFactory<string>();
            factory.TryCreateFeature<StringBuilder>(null, false);
        }
    }
}
