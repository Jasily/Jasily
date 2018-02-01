using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JetBrains.Annotations;

namespace Jasily.ViewModels
{
    /// <summary>
    /// Default view model for <see cref="DatePicker"/>.
    /// </summary>
    public class DatePickerViewModel : ControlViewModel
    {
        private DateTimeOffset _maxYear;
        private DataTemplate _headerTemplate;
        private string _calendarIdentifier;
        private bool _dayVisible;
        private string _dayFormat;
        private DateTimeOffset _date;
        private bool _yearVisible;
        private DateTimeOffset _minYear;
        private string _yearFormat;
        private bool _monthVisible;
        private string _monthFormat;
        private LightDismissOverlayMode _lightDismissOverlayMode;
        private Orientation _orientation;
        private object _header;

        public DateTimeOffset MaxYear
        {
            get => this._maxYear;
            set => this.SetPropertyRef(ref this._maxYear, value);
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

        public string CalendarIdentifier
        {
            get => this._calendarIdentifier;
            set => this.SetPropertyRef(ref this._calendarIdentifier, value);
        }

        public bool DayVisible
        {
            get => this._dayVisible;
            set => this.SetPropertyRef(ref this._dayVisible, value);
        }

        public string DayFormat
        {
            get => this._dayFormat;
            set => this.SetPropertyRef(ref this._dayFormat, value);
        }

        [PublicAPI]
        public DateTimeOffset Date
        {
            get => this._date;
            set => this.SetPropertyRef(ref this._date, value);
        }

        public DateTimeOffset MinYear
        {
            get => this._minYear;
            set => this.SetPropertyRef(ref this._minYear, value);
        }

        public string YearFormat
        {
            get => this._yearFormat;
            set => this.SetPropertyRef(ref this._yearFormat, value);
        }

        public bool YearVisible
        {
            get => this._yearVisible;
            set => this.SetPropertyRef(ref this._yearVisible, value);
        }

        public Orientation Orientation
        {
            get => this._orientation;
            set => this.SetPropertyRef(ref this._orientation, value);
        }

        public bool MonthVisible
        {
            get => this._monthVisible;
            set => this.SetPropertyRef(ref this._monthVisible, value);
        }

        public string MonthFormat
        {
            get => this._monthFormat;
            set => this.SetPropertyRef(ref this._monthFormat, value);
        }

        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get => this._lightDismissOverlayMode;
            set => this.SetPropertyRef(ref this._lightDismissOverlayMode, value);
        }
    }
}