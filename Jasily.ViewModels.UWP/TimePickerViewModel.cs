using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="TimePicker"/>.
    /// </summary>
    public class TimePickerViewModel : ControlViewModel
    {
        private string _clockIdentifier;
        private LightDismissOverlayMode _lightDismissOverlayMode;
        private TimeSpan _time;
        private int _minuteIncrement;
        private DataTemplate _headerTemplate;
        private object _header;

        [PublicAPI]
        public TimeSpan Time
        {
            get => this._time;
            set => this.SetPropertyRef(ref this._time, value);
        }

        public int MinuteIncrement
        {
            get => this._minuteIncrement;
            set => this.SetPropertyRef(ref this._minuteIncrement, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => this._headerTemplate;
            set => this.SetPropertyRef(ref this._headerTemplate, value);
        }

        public object Header
        {
            get => this._header;
            set => this.SetPropertyRef(ref this._header, value);
        }

        public string ClockIdentifier
        {
            get => this._clockIdentifier;
            set => this.SetPropertyRef(ref this._clockIdentifier, value);
        }

        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get => this._lightDismissOverlayMode;
            set => this.SetPropertyRef(ref this._lightDismissOverlayMode, value);
        }
    }
}