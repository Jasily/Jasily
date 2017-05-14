using System.Threading.Tasks;
using Jasily.DependencyInjection.AwaiterAdapter.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    [TestClass]
    public class TestAwaitableInfo
    {
        [TestMethod]
        public void TestBuildAwaitableInfoForTask()
        {
            var info = AwaitableInfo.TryBuild(typeof(Task));
            Assert.IsNotNull(info);
        }
    }
}