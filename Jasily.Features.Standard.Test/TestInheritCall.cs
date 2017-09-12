using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Features.Standard.Test
{
    [TestClass]
    public class TestInheritCall
    {
        private FeaturesProvider CreateFeaturesProvider()
        {
            var builder = new FeaturesBuilder();
            builder.RegisterFeature<A, string>(z => "A", false);
            builder.RegisterFeature<A, StringBuilder>(z => new StringBuilder("A"), false);
            builder.RegisterFeature<B, string>(z => "B", false);
            builder.RegisterFeature<B, List<string>>(z => new List<string> { "B" }, false);
            return builder.Build();
        }

        class A { }

        class B : A { }

        [TestMethod]
        public void TestGetFeatureInherit()
        {
            var provider = this.CreateFeaturesProvider();
            var factoryA = provider.GetFeaturesFactory<A>();
            var factoryB = provider.GetFeaturesFactory<B>();
            
            // string
            Assert.AreEqual("A", factoryA.TryCreateFeature<string>(new A(), true));
            Assert.AreEqual("A", factoryA.TryCreateFeature<string>(new B(), true));
            Assert.AreEqual("B", factoryB.TryCreateFeature<string>(new B(), true));

            // StringBuilder
            Assert.AreEqual(null, factoryB.TryCreateFeature<StringBuilder>(new B(), false));
            Assert.AreEqual("A", factoryB.TryCreateFeature<StringBuilder>(new B(), true)?.ToString());
        }
    }
}