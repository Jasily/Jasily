using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Windows.ApplicationModel.Background;

namespace Jasily.ApplicationModel.Background
{
    public abstract class BackgroundTaskRegister
    {
        private readonly Type taskEntryPoint;

        protected BackgroundTaskRegister([NotNull] string taskName, [NotNull] Type taskEntryPoint)
        {
            if (taskName == null) throw new ArgumentNullException(nameof(taskName));
            if (taskEntryPoint == null) throw new ArgumentNullException(nameof(taskEntryPoint));
            this.TaskName = taskName;
            this.taskEntryPoint = taskEntryPoint;
        }

        protected string TaskName { get; }

        private void Register()
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = this.TaskName,
                TaskEntryPoint = this.taskEntryPoint.FullName
            };
            this.Configuration(builder);
            builder.Register();
        }

        protected abstract void Configuration(BackgroundTaskBuilder builder);

        public static Task<bool> TryRegister(bool removeOther, params BackgroundTaskRegister[] registers)
            => TryRegister(removeOther, (IEnumerable<BackgroundTaskRegister>)registers);

        public static async Task<bool> TryRegister(bool removeOther, IEnumerable<BackgroundTaskRegister> registers)
        {
            if (registers == null) throw new ArgumentNullException(nameof(registers));

            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.Denied)
            {
                return false;
            }

            var dict = registers.ToDictionary(z => z.TaskName);

            foreach (var task in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (!dict.Remove(task.Name) && removeOther)
                {
                    task.Unregister(false);
                }
            }


            foreach (var kvp in dict) // newest.
            {
                kvp.Value.Register();
            }

            return true;
        }

        public static BackgroundTaskRegister Create<T>(string taskName, Action<BackgroundTaskBuilder> builder)
            where T : IBackgroundTask
            => new BackgroundTaskRegisterImpl<T>(taskName, builder);

        private sealed class BackgroundTaskRegisterImpl<T> : BackgroundTaskRegister where T : IBackgroundTask
        {
            private readonly Action<BackgroundTaskBuilder> builder;

            public BackgroundTaskRegisterImpl([NotNull] string taskName, [NotNull] Action<BackgroundTaskBuilder> builder)
                : base(taskName, typeof(T))
            {
                if (builder == null) throw new ArgumentNullException(nameof(builder));
                this.builder = builder;
            }

            protected override void Configuration(BackgroundTaskBuilder builder) => this.builder(builder);
        }
    }
}