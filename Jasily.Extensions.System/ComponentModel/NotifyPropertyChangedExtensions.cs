﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.ComponentModel
{
    public static class NotifyPropertyChangedExtensions
    {
        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            string propertyName)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            string propertyName1, string propertyName2)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            handler.Invoke(sender, new PropertyChangedEventArgs(propertyName1));
            handler.Invoke(sender, new PropertyChangedEventArgs(propertyName2));
        }

        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            [NotNull] params string[] propertyNames)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < propertyNames.Length; i++)
            {
                handler(sender, new PropertyChangedEventArgs(propertyNames[i]));
            }
        }

        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            [NotNull] IEnumerable<string> propertyNames)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            foreach (var propertyName in propertyNames)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            [NotNull] params PropertyChangedEventArgs[] eventArgs)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < eventArgs.Length; i++)
            {
                handler(sender, eventArgs[i]);
            }
        }

        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender,
            [NotNull] IEnumerable<PropertyChangedEventArgs> eventArgs)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            foreach (var property in eventArgs)
            {
                handler(sender, property);
            }
        }
    }
}