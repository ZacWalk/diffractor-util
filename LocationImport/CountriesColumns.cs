#region

using System;
using System.Data;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The countris columns.
    /// </summary>
    /// <remarks>
    ///   Column names from: CountryInfo.txt
    /// </remarks>
    internal sealed class CountriesColumns
    {
        #region Column Name Constants

        /// <summary>
        ///   The id column name.
        /// </summary>
        private const string idColumnName = "ISO-Numeric";

        /// <summary>
        ///   The name column name.
        /// </summary>
        private const string nameColumnName = "Country";

        /// <summary>
        ///   The iso code column name.
        /// </summary>
        private const string isocodeColumnName = "ISO";

        #endregion

        #region Columns In File

        /// <summary>
        ///   The column headers.
        /// </summary>
        private static readonly string[] columnHeaders = {
                                                             isocodeColumnName, "ISO3", idColumnName, "fips",
                                                             nameColumnName, "Capital", "Area(in sq km)", "Population",
                                                             "Continent", "tld", "CurrencyCode", "CurrencyName", "Phone",
                                                             "Postal Code", "Format	Postal Code Regex", "Languages",
                                                             "geonameid", "neighbours", "EquivalentFipsCode"
                                                         };

        #endregion

        #region Column Positions

        /// <summary>
        ///   The id position.
        /// </summary>
        private readonly int posId;

        /// <summary>
        ///   The ISO code position.
        /// </summary>
        private readonly int posIsoCode;

        /// <summary>
        ///   The name position.
        /// </summary>
        private readonly int posName;

        #endregion

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CountriesColumns" /> class.
        /// </summary>
        /// <param name = "sourceDataReader">
        ///   The source data reader.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        public CountriesColumns(IDataReader sourceDataReader)
        {
            if (sourceDataReader == null)
            {
                throw new ArgumentNullException("sourceDataReader");
            }

            posId = sourceDataReader.GetOrdinal(idColumnName);
            posName = sourceDataReader.GetOrdinal(nameColumnName);
            posIsoCode = sourceDataReader.GetOrdinal(isocodeColumnName);
        }

        /// <summary>
        ///   The column headers.
        /// </summary>
        public static string[] ColumnHeaders
        {
            get { return columnHeaders; }
        }

        /// <summary>
        ///   The id.
        /// </summary>
        /// <param name = "sourceDataReader">
        ///   The source data reader.
        /// </param>
        /// <returns>
        ///   The id.
        /// </returns>
        public int Id(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetInt32(posId);
        }

        /// <summary>
        ///   The name.
        /// </summary>
        /// <param name = "sourceDataReader">
        ///   The source data reader.
        /// </param>
        /// <returns>
        ///   The name.
        /// </returns>
        public string Name(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posName);
        }

        /// <summary>
        ///   The ISO code.
        /// </summary>
        /// <param name = "sourceDataReader">
        ///   The source data reader.
        /// </param>
        /// <returns>
        ///   The ISO code.
        /// </returns>
        public string IsoCode(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posIsoCode);
        }
    }
}