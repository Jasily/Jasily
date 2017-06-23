namespace Jasily.ComponentModel
{
    public enum ThreadKind
    {
        /// <summary>
        /// The operator can run on any thread.
        /// </summary>
        AnyThread = 0,

        /// <summary>
        /// The first thread for application.
        /// </summary>
        MainThread = 1,

        /// <summary>
        /// The thread which own the UI.
        /// </summary>
        UIThread = 2,

        /// <summary>
        /// 
        /// </summary>
        BackgroundThread = 3
    }
}