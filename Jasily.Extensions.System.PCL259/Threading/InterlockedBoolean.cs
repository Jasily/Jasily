namespace System.Threading
{
    public struct InterlockedBoolean
    {
        private const int True = 1;
        private const int False = 0;

        private int value;

        public InterlockedBoolean(bool value)
        {
            this.value = value ? True : False;
        }

        public bool Value => this.value == True;

        public bool Read()
            => Volatile.Read(ref this.value) == True;

        public void Write(bool value)
            => Volatile.Write(ref this.value, value ? True : False);

        public bool CompareExchange(bool value, bool compared)
            => Interlocked.CompareExchange(ref this.value, value ? True : False, compared ? True : False) == True;

        public static implicit operator bool(InterlockedBoolean value) => value.Value;
    }
}