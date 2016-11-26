using System;
using Jasily.Interfaces;

namespace Jasily.Net.Sockets
{
    public struct Port : IInitializedValueType
    {
        public int Value { get; }

        public Port(int port)
        {
            if (!IsValueVaild(port)) throw new ArgumentException(nameof(port));
            this.Value = port;
        }

        public bool IsInitialized => this.Value != 0;

        public static explicit operator Port(string port) => new Port(int.Parse(port));

        public static explicit operator Port(int port) => new Port(port);

        public override string ToString() => this.Value.ToString();

        public static bool IsValueVaild(int port) => port > 0 && port < 65536;
    }
}