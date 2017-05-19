using System.Threading.Tasks;
using Jasily.DependencyInjection.MethodInvoker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    [TestClass]
    public class TestAwaiterAdapter
    {
        [TestMethod]
        public void CanAwaitValueType()
        {
            async Task Async()
            {
                var sc = new ServiceCollection();
                sc.UseAwaiterAdapter();
                var provider = sc.BuildServiceProvider();
                await provider.Async(100);
            }

            Async().GetAwaiter().GetResult();
        }

        [TestMethod]
        public void CanAwaitTask()
        {
            async Task<int> Async()
            {
                var sc = new ServiceCollection();
                sc.UseAwaiterAdapter();
                var provider = sc.BuildServiceProvider();
                return await provider.Async(Task.Run(() => 5)).HasResultAsync<int>();
            }

            Assert.AreEqual(5, Async().GetAwaiter().GetResult());
        }

        [TestMethod]
        public void CanAwaitTaskObject()
        {
            async Task<object> Async()
            {
                var sc = new ServiceCollection();
                sc.UseAwaiterAdapter();
                var provider = sc.BuildServiceProvider();
                return await provider.Async(Task.Run(() => "5")).HasResultAsync<object>();
            }

            Assert.AreEqual("5", Async().GetAwaiter().GetResult());
        }

        [TestMethod]
        public void CanUnpackAnyAwaitable()
        {
            var sc = new ServiceCollection();
            sc.UseAwaiterAdapter();
            var provider = sc.BuildServiceProvider();

            var task0 = Task.Run(() => { });
            var adapter = provider.GetAwaitableAdapter<Task>();
            Assert.AreEqual(true, adapter.IsAwaitable);
            Assert.AreEqual(null, adapter.GetResult(task0));

            var task1 = Task.Run(() => 100);
            Assert.AreEqual(100, provider.GetValueOrAwaitableResult(task1));

            var task2 = Task.Run<Task<int>>(async () => {
                await Task.Delay(10);
                return 500;
            });
            Assert.AreEqual(500, provider.GetValueOrAwaitableResult(task2, true));
        }
    }
}
