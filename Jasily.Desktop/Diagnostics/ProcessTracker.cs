using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.Diagnostics
{
    public sealed class ProcessTracker : IDisposable
    {
        private readonly SynchronizationContext context;
        private readonly SendOrPostCallback startedCallback;
        private readonly SendOrPostCallback stopedCallback;
        public event EventHandler<int> ProcessStarted;
        public event EventHandler<int> ProcessStoped;
        private ProcessWatcher watcher;

        public ProcessTracker()
        {
            this.context = SynchronizationContext.Current;
            this.startedCallback = id => this.ProcessStarted?.Invoke(this, (int) id);
            this.stopedCallback = id => this.ProcessStoped?.Invoke(this, (int) id);
        }

        public void Start()
        {
            ProcessWatcher watcher = new ManagementProcessWatcher();
            if (!watcher.Start())
            {
                watcher.Stop();
                watcher = new LoopProcessWatcher(200);
                watcher.Start();
            }
            this.watcher = watcher;
            watcher.ProcessStarted += this.Watcher_ProcessStarted;
            watcher.ProcessStoped += this.Watcher_ProcessStoped;
        }

        public void Stop()
        {
            var watcher = this.watcher;
            if (watcher != null)
            {
                watcher.ProcessStarted -= this.Watcher_ProcessStarted;
                watcher.ProcessStoped -= this.Watcher_ProcessStoped;
            }
        }

        private void Watcher_ProcessStarted(object sender, int e) => this.context.Post(this.startedCallback, e);

        private void Watcher_ProcessStoped(object sender, int e) => this.context.Post(this.stopedCallback, e);

        private abstract class ProcessWatcher
        {
            public event EventHandler<int> ProcessStarted;

            public event EventHandler<int> ProcessStoped;

            protected void OnProcessStarted(int processId) => this.ProcessStarted?.Invoke(this, processId);

            protected void OnProcessStoped(int processId) => this.ProcessStoped?.Invoke(this, processId);

            public abstract bool Start();

            public abstract void Stop();
        }

        private class ManagementProcessWatcher : ProcessWatcher
        {
            private readonly ManagementEventWatcher startWatcher;
            private readonly ManagementEventWatcher stopWatcher;

            public ManagementProcessWatcher()
            {
                this.startWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                this.startWatcher.EventArrived += this.OnProcessStarted;

                this.stopWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
                this.stopWatcher.EventArrived += this.OnProcessStoped;
            }

            public override bool Start()
            {
                try
                {
                    this.startWatcher.Start();
                    this.stopWatcher.Start();
                    return true;
                }
                catch (ManagementException)
                {
                    return false;
                }
            }

            private static void Stop(ManagementEventWatcher watcher)
            {
                try
                {
                    watcher.Stop();
                }
                catch
                {
#if DEBUG
                    throw;
#endif
                }
            }

            public override void Stop()
            {
                Stop(this.startWatcher);
                Stop(this.stopWatcher);
            }

            private static int From(EventArrivedEventArgs e) => int.Parse((string) e.NewEvent.Properties["ProcessId"].Value);

            private void OnProcessStoped(object sender, EventArrivedEventArgs e) => this.OnProcessStoped(From(e));

            private void OnProcessStarted(object sender, EventArrivedEventArgs e) => this.OnProcessStarted(From(e));
        }

        private class LoopProcessWatcher : ProcessWatcher
        {
            private readonly int delay;
            private bool isStoped;

            public LoopProcessWatcher(int delay)
            {
                this.delay = delay;
            }


            private static int[] GetProcesseIds() => Process.GetProcesses().Select(z => z.Id).ToArray();

            public override bool Start()
            {
                Task.Run(async () =>
                {
                    var ids = GetProcesseIds();
                    while (!this.isStoped)
                    {
                        await Task.Delay(this.delay).ConfigureAwait(false);
                        var cur = GetProcesseIds();
                        foreach (var i in cur.Except(ids).ToArray()) this.OnProcessStarted(i);
                        foreach (var i in ids.Except(cur).ToArray()) this.OnProcessStoped(i);
                        ids = cur;
                    }
                });
                return true;
            }

            public override void Stop() => this.isStoped = true;
        }

        public void Dispose() => this.Stop();
    }
}