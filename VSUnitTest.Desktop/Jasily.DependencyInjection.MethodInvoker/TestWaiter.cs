using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MethodInvoker;
using System.Threading.Tasks;

namespace VSUnitTest.Desktop.Jasily.DependencyInjection.MethodInvoker
{
    [TestClass]
    public class TestWaiter
    {
        [TestMethod]
        public void TestValue()
        {
            var sc = new ServiceCollection();
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var value = 100;
            Assert.AreEqual(100, provider.GetValueOrAwaitableResult(value));
        }

        [TestMethod]
        public void TestTask()
        {
            var sc = new ServiceCollection();
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var task = Task.Run(() => 100);
            Assert.AreEqual(100, provider.GetValueOrAwaitableResult(task));
        }

        [TestMethod]
        public void TestTaskTask()
        {
            var sc = new ServiceCollection();
            sc.UseMethodInvoker();
            var provider = sc.BuildServiceProvider();
            var task = Task.Run<Task<int>>(async () => {
                await Task.Delay(10);
                return 100;
            });
            Assert.AreEqual(100, provider.GetValueOrAwaitableResult(task, true));
        }
    }
}
