#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The comma separated value reader.
    /// </summary>
    public sealed class CommaSeparatedValueReader : SeparatedValueReader
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CommaSeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "columnHeaders">
        ///   The column headers.
        /// </param>
        public CommaSeparatedValueReader(string fileName, Encoding fileEncoding, IEnumerable<string> columnHeaders)
            : base(fileName, fileEncoding, ',', columnHeaders)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CommaSeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "columnHeaders">
        ///   The column headers.
        /// </param>
        /// <param name = "isLineComment">
        ///   The is line comment.
        /// </param>
        public CommaSeparatedValueReader(string fileName,
                                         Encoding fileEncoding,
                                         IEnumerable<string> columnHeaders,
                                         Func<string, bool> isLineComment)
            : base(fileName, fileEncoding, ',', columnHeaders, isLineComment)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CommaSeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        public CommaSeparatedValueReader(string fileName, Encoding fileEncoding)
            : base(fileName, fileEncoding, ',')
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CommaSeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "isLineComment">
        ///   The is line comment.
        /// </param>
        public CommaSeparatedValueReader(string fileName, Encoding fileEncoding, Func<string, bool> isLineComment)
            : base(fileName, fileEncoding, ',', isLineComment)
        {
        }
    }
}