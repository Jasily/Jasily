using JetBrains.Annotations;

namespace Jasily.Core
{
    /// <summary>
    /// the object has default key use for Dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGetKey<out T>
    {
        /// <summary>
        /// get default key from object.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        T GetKey();
    }
}