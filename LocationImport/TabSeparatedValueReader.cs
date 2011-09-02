// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabSeparatedValueReader.cs" company="">
//   
// </copyright>
// <summary>
//   The tab separated value reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    /// The tab separated value reader.
    /// </summary>
    public sealed class TabSeparatedValueReader : SeparatedValueReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabSeparatedValueReader"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fileEncoding">
        /// The file encoding.
        /// </param>
        /// <param name="columnHeaders">
        /// The column headers.
        /// </param>
        /// <param name="isLineComment">
        /// The is line comment.
        /// </param>
        public TabSeparatedValueReader( string fileName, 
                                        Encoding fileEncoding, 
                                        IEnumerable< string > columnHeaders, 
                                        Func< string, bool > isLineComment )
            : base( fileName, fileEncoding, '\t', columnHeaders, isLineComment )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabSeparatedValueReader"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fileEncoding">
        /// The file encoding.
        /// </param>
        /// <param name="columnHeaders">
        /// The column headers.
        /// </param>
        public TabSeparatedValueReader( string fileName, Encoding fileEncoding, IEnumerable< string > columnHeaders )
            : base( fileName, fileEncoding, '\t', columnHeaders )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabSeparatedValueReader"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fileEncoding">
        /// The file encoding.
        /// </param>
        /// <param name="isLineComment">
        /// The is line comment.
        /// </param>
        public TabSeparatedValueReader( string fileName, Encoding fileEncoding, Func< string, bool > isLineComment )
            : base( fileName, fileEncoding, '\t', isLineComment )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabSeparatedValueReader"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fileEncoding">
        /// The file encoding.
        /// </param>
        public TabSeparatedValueReader( string fileName, Encoding fileEncoding )
            : base( fileName, fileEncoding, '\t' )
        {
        }
    }
}