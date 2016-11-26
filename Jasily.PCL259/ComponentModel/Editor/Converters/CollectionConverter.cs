using System.Collections.Generic;
using Jasily.Interfaces;

namespace Jasily.ComponentModel.Editor.Converters
{
    public class CollectionConverter<T, TElement> : CollectionConverter<T, T, TElement>
        where T : class, ICollection<TElement>, new()
    {
    }

    public class CollectionConverter<TIn, TOut, TElement> : TwoWayConverter<TIn, TOut>
        where TIn : class, ICollection<TElement>, new()
        where TOut : class, ICollection<TElement>, new()
    {
        public override bool CanConvert(TIn value) => true;

        public override TOut Convert(TIn value)
        {
            if (value == null) return null;
            var o = new TOut();
            o.AddRange(value);
            return o;
        }

        public override TIn ConvertBack(TOut value)
        {
            if (value == null) return null;
            var o = new TIn();
            o.AddRange(value);
            return o;
        }

        public override bool CanConvertBack(TOut value) => true;
    }
}