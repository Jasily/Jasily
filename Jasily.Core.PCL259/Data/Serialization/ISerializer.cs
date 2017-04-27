using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Core.Data.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [NotNull]
        object Deserialize([NotNull] Stream stream, [CanBeNull] Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        void Serialize([NotNull] Stream stream, [NotNull] object obj);
    }
}
