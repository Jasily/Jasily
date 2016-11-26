using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Interfaces
{
    public class OrderableComparer : Comparer<IOrderable>
    {
        public override int Compare([NotNull] IOrderable x, [NotNull] IOrderable y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return x.GetOrderCode().CompareTo(y.GetOrderCode());
        }
    }
}