using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    public class FixedTaskScheduler : TaskScheduler
    {
        private int executorCount;
        private readonly Queue<Task> queuedTasks = new Queue<Task>();
        private readonly TaskFactory taskFactory;

        public FixedTaskScheduler(int maxThread)
        {
            if (maxThread < 1) throw new ArgumentOutOfRangeException(nameof(maxThread));
            this.MaximumConcurrencyLevel = maxThread;
            this.taskFactory = new TaskFactory(Default);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (this.queuedTasks)
            {
                return this.queuedTasks.ToArray();
            }
        }

        protected override void QueueTask(Task task)
        {
            bool create;
            lock (this.queuedTasks)
            {
                this.queuedTasks.Enqueue(task);
                create = this.executorCount < this.MaximumConcurrencyLevel;
                if (create) this.executorCount++;
            }
            if (create)
            {
                this.StartNewTask();
            }
        }

        protected override bool TryDequeue(Task task)
        {
            lock (this.queuedTasks)
            {
                var array = this.queuedTasks.ToArray();
                if (!array.Contains(task)) return false;
                this.queuedTasks.Clear();
                foreach (var t in array.Where(z => z != task))
                {
                    this.queuedTasks.Enqueue(t);
                }
                return true;
            }
        }

        private void StartNewTask()
        {
            this.taskFactory.StartNew(() =>
            {
                while (true)
                {
                    Task task;
                    lock (this.queuedTasks)
                    {
                        if (this.queuedTasks.Count == 0)
                        {
                            this.executorCount--;
                            return;
                        }
                        task = this.queuedTasks.Dequeue();
                    }
                    this.TryExecuteTask(task);
                }
            });
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued && !this.TryDequeue(task)) return false;
            return this.TryExecuteTask(task);
        }

        public override int MaximumConcurrencyLevel { get; }
    }
}