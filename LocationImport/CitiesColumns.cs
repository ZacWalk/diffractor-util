// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CitiesColumns.cs" company="">
//   
// </copyright>
// <summary>
//   The cities columns.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System;
using System.Data;

#endregion

namespace LocationImport
{
    /// <summary>
    /// The cities columns.
    /// </summary>
    /// <remarks>
    /// Column names from: http://download.geonames.org/export/dump/readme.txt
    /// </remarks>
    internal sealed class CitiesColumns
    {
        #region Column Name Constants

        /// <summary>
        ///   The id column name.
        /// </summary>
        private const string idColumnName = "geonameid";

        /// <summary>
        ///   The name column name.
        /// </summary>
        private const string nameColumnName = "name";

        /// <summary>
        ///   The alternate names column name.
        /// </summary>
        private const string alternateNamesColumnName = "alternatenames";

        /// <summary>
        ///   The latitude column name.
        /// </summary>
        private const string latitudeColumnName = "latitude";

        /// <summary>
        ///   The longitude column name.
        /// </summary>
        private const string longitudeColumnName = "longitude";

        /// <summary>
        ///   The state code column name.
        /// </summary>
        private const string stateCodeColumnName = "admin1 code";

        /// <summary>
        ///   The country code column name.
        /// </summary>
        private const string countryCodeColumnName = "country code";

        #endregion

        #region Columns In File

        /// <summary>
        ///   The column headers.
        /// </summary>
        private static readonly string[] columnHeaders = {
                                                             idColumnName, nameColumnName, "asciiname", 
                                                             alternateNamesColumnName, latitudeColumnName, 
                                                             longitudeColumnName, "feature class", "feature code", 
                                                             countryCodeColumnName, "cc2", stateCodeColumnName, 
                                                             "admin2 code", "admin3 code", "admin4 code", "population", 
                                                             "elevation", "gtopo30", "timezone", "modification date"
                                                         };

        #endregion

        #region Column Positions

        /// <summary>
        ///   The alternate names position.
        /// </summary>
        private readonly int posAlternateNames;

        /// <summary>
        ///   The country code position.
        /// </summary>
        private readonly int posCountryCode;

        /// <summary>
        ///   The id position.
        /// </summary>
        private readonly int posId;

        /// <summary>
        ///   The latitude position.
        /// </summary>
        private readonly int posLatitude;

        /// <summary>
        ///   The longitude position.
        /// </summary>
        private readonly int posLongitude;

        /// <summary>
        ///   The name position.
        /// </summary>
        private readonly int posName;

        /// <summary>
        ///   The state code position.
        /// </summary>
        private readonly int posStateCode;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesColumns"/> class.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        public CitiesColumns( IDataReader sourceDataReader )
        {
            if( sourceDataReader == null )
            {
                throw new ArgumentNullException( "sourceDataReader" );
            }

            posId = sourceDataReader.GetOrdinal( idColumnName );
            posName = sourceDataReader.GetOrdinal( nameColumnName );
            posAlternateNames = sourceDataReader.GetOrdinal( alternateNamesColumnName );
            posLatitude = sourceDataReader.GetOrdinal( latitudeColumnName );
            posLongitude = sourceDataReader.GetOrdinal( longitudeColumnName );
            posStateCode = sourceDataReader.GetOrdinal( stateCodeColumnName );
            posCountryCode = sourceDataReader.GetOrdinal( countryCodeColumnName );
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
        /// The id.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The id.
        /// </returns>
        public int Id( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetInt32( posId );
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
        /// The alternate names.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The alternate names.
        /// </returns>
        public string AlternateNames( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetString( posAlternateNames );
        }

        /// <summary>
        /// The latitude.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The latitude.
        /// </returns>
        public double Latitude( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetDouble( posLatitude );
        }


        /// <summary>
        /// The longitude.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The longitude.
        /// </returns>
        public double Longitude( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetDouble( posLongitude );
        }


        /// <summary>
        /// The state code.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The state code.
        /// </returns>
        public string StateCode( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetString( posStateCode );
        }

        /// <summary>
        /// The country code.
        /// </summary>
        /// <param name="sourceDataReader">
        /// The source data reader.
        /// </param>
        /// <returns>
        /// The country code.
        /// </returns>
        public string CountryCode( IDataReader sourceDataReader )
        {
            return sourceDataReader.GetString( posCountryCode );
        }
    }
}