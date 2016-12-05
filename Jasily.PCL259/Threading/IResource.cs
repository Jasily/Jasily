namespace Jasily.Threading
{
    public interface IResource
    {
        int CurrentCount { get; }

        int Release(int count = 1);

        Releaser<int> Acquire(int count = 1);
    }
}