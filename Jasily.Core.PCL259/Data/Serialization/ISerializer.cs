﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Core.Data.Serialization
{
    /// <summary>
    /// interface for any kind serializer 
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="stream"/> is null.</exception>
        /// <returns></returns>
        [NotNull]
        object Deserialize([NotNull] Stream stream, [CanBeNull] Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="stream"/> or <paramref name="obj"/> is null.</exception>
        void Serialize([NotNull] Stream stream, [NotNull] object obj);
    }
}
