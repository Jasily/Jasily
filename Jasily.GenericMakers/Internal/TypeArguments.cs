using System;
using JetBrains.Annotations;

namespace Jasily.GenericMakers.Internal
{
    internal struct TypeArguments : IEquatable<TypeArguments>
    {
        private readonly int _hashCode;
        public readonly Type[] Types;

        public TypeArguments([NotNull] Type[] typeArguments)
        {
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

            this.Types = (Type[])typeArguments.Clone();
            this._hashCode = 0;
            for (var i = 0; i < typeArguments.Length; i++)
            {
                var type = typeArguments[i] ??
                           throw new ArgumentNullException(nameof(typeArguments), "Any element of typeArguments cannot be null.");
                this._hashCode ^= type.GetHashCode();
            }
        }

        public bool Equals(TypeArguments other)
        {
            if (this.Types.Length != other.Types.Length) return false;
            for (var i = 0; i < this.Types.Length; i++)
            {
                if (this.Types[i] != other.Types[i]) return false;
            }
            return true;
        }

        public override bool Equals(object obj) => obj is TypeArguments ta && this.Equals(ta);

        public override int GetHashCode() => this._hashCode;
    }
}