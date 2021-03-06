﻿namespace Jasily.ComponentModel.Editor.Converters
{
    public sealed class EmptyToNullStringConverter : ToStringConverter<string>
    {
        public override string ConvertBack(string value) => string.IsNullOrEmpty(value) ? null : value;
    }
}