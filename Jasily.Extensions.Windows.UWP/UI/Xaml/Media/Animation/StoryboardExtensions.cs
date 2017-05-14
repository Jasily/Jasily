using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Jasily.Extensions.Windows.UI.Xaml.Media.Animation
{
    public static class StoryboardExtensions
    {
        public static Task PlayAsync(this Storyboard storyboard)
        {
            if (storyboard == null) throw new ArgumentNullException(nameof(storyboard));

            var tcs = new TaskCompletionSource<bool>();

            EventHandler<object> onComplete = null;
            onComplete = (s, e) =>
            {
                storyboard.Completed -= onComplete;
                tcs.SetResult(true);
            };
            storyboard.Completed += onComplete;

            storyboard.Begin();
            return tcs.Task;
        }
    }
}