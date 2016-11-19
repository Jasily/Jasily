using System.IO;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Data
{
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        [NotNull]
        object Deserialize([NotNull] Stream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SerializationException"></exception>
        void Serialize([NotNull] Stream stream, [NotNull] object obj);
    }
}