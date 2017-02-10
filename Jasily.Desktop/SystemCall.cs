using System;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace Jasily
{
    public static class SystemCall
    {
        // ReSharper disable once InconsistentNaming
        private const string CMDFileName = "cmd.exe";

        [NotNull]
        public static Process CreateProcess([NotNull] string command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = CMDFileName,
                    Arguments = "/C " + command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
        }

        public static void Fire([NotNull] string command)
        {
            using (var process = CreateProcess(command))
            {
                process.Start();
            }
        }

        public static int Call([NotNull] string command)
        {
            using (var process = CreateProcess(command))
            {
                process.Start();
                process.WaitForExit();
                return process.ExitCode;
            }
        }

        [NotNull]
        public static ProcessResult OutputReadToEnd([NotNull] string command)
        {
            using (var process = CreateProcess(command))
            {
                var sb = new StringBuilder();
                process.OutputDataReceived += (sender, args) =>
                {
                    sb.AppendLine(args.Data); // is thread-safely ?
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                return new ProcessResult(process.ExitCode, sb.ToString());
            }
        }

        public struct ProcessResult
        {
            public int ExitCode { get; }

            public string Output { get; }

            public ProcessResult(int exitCode, string output)
            {
                this.ExitCode = exitCode;
                this.Output = output;
            }
        }
    }
}