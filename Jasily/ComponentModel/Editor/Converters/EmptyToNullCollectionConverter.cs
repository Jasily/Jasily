using System.Collections.Generic;

namespace Jasily.ComponentModel.Editor.Converters
{
    public class EmptyToNullCollectionConverter<TIn, TOut, TElement> : CollectionConverter<TIn, TOut, TElement>
        where TIn : class, ICollection<TElement>, new()
        where TOut : class, ICollection<TElement>, new()
    {
        public override TIn ConvertBack(TOut value)
            => value?.Count == 0 ? null : base.ConvertBack(value);
    }
}