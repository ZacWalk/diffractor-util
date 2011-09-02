// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatesColumns.cs" company="">
//   
// </copyright>
// <summary>
//   The states columns.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System;
using System.Data;

#endregion

namespace LocationImport
{
    /// <summary>
    /// The states columns.
    /// </summary>
    internal sealed class StatesColumns
    {
        #region Column Name Constants

        /// <summary>
        ///   The code column name.
        /// </summary>
        private const string codeColumnName = "Code";

        /// <summary>
        ///   The name column name.
        /// </summary>
        private const string nameColumnName = "Name";

        #endregion

        #region Columns In File

        // Column names from: CountryInfo.txt
        /// <summary>
        ///   The column headers.
        /// </summary>
        private static readonly string[] columnHeaders = { codeColumnName, nameColumnName };

        #endregion

        #region Column Positions

        /// <summary>
        ///   The code position.
        /// </summary>
        private readonly int posCode;

        /// <summary>
        ///   The name position.
        /// </summary>
        private readonly int posName;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StatesColumns"/> class.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public StatesColumns( IDataReader sourceDataReader )
        {
            if( sourceDataReader == null )
            {
                throw new ArgumentNullException( "sourceDataReader" );
            }

            posName = sourceDataReader.GetOrdinal( nameColumnName );
            posCode = sourceDataReader.GetOrdinal( codeColumnName );
        }

        /// <summary>
        ///   The column headers.
        /// </summary>
        public static string[] ColumnHeaders
        {
            get
            {
                return columnHeaders;
            }
        }

        /// <summary>
        /// The name.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The name.
        /// </returns>
        public string Name( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetString( posName );
        }

        /// <summary>
        /// The code.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The code.
        /// </returns>
        public string Code( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetString( posCode );
        }
    }
}