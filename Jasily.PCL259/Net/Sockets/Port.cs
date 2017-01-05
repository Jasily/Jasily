using System;

namespace Jasily.Net.Sockets
{
    public struct Port
    {
        public ushort Value { get; }

        public Port(int port)
        {
            if (port > ushort.MaxValue || port < ushort.MinValue) throw new ArgumentException(nameof(port));
            this.Value = (ushort) port;
        }

        public static explicit operator Port(string port) => new Port(int.Parse(port));

        public static explicit operator Port(int port) => new Port(port);

        public override string ToString() => this.Value.ToString();
    }
}