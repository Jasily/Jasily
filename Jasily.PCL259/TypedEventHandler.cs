namespace Jasily
{
    public delegate void TypedEventHandler<in T, in TEventArgs>(T sender, TEventArgs e);

    public delegate void TypedEventHandler<in T>(T sender);
}