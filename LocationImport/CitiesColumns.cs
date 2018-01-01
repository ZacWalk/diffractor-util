#region

using System;
using System.Data;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The cities columns.
    /// </summary>
    /// <remarks>
    ///   Column names from: http://download.geonames.org/export/dump/readme.txt
    /// </remarks>
    internal sealed class CitiesColumns
    {
        #region Column Name Constants

        private const string idColumnName = "geonameid";
        private const string nameColumnName = "name";
        private const string alternateNamesColumnName = "alternatenames";
        private const string latitudeColumnName = "latitude";
        private const string longitudeColumnName = "longitude";
        private const string stateCodeColumnName = "admin1 code";
        private const string featureCodeName = "feature code";
        private const string featureClassName = "feature class";
        private const string countryCodeColumnName = "country code";
        private const string populationColumnName = "population";

        #endregion

        #region Columns In File

        private static readonly string[] columnHeaders = {
                                                             idColumnName, nameColumnName, "asciiname",
                                                             alternateNamesColumnName, latitudeColumnName,
                                                             longitudeColumnName, featureClassName, featureCodeName,
                                                             countryCodeColumnName, "cc2", stateCodeColumnName,
                                                             "admin2 code", "admin3 code", "admin4 code", populationColumnName,
                                                             "elevation", "gtopo30", "timezone", "modification date"
                                                         };

        #endregion

        #region Column Positions

        private readonly int posAlternateNames;
        private readonly int posCountryCode;
        private readonly int posId;
        private readonly int posLatitude;
        private readonly int posLongitude;
        private readonly int posName;
        private readonly int posFeatureClass;
        private readonly int posFeatureCode;
        private readonly int posStateCode;
        private readonly int posPopulation;

        #endregion

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CitiesColumns" /> class.
        /// </summary>
        /// <param name = "sourceDataReader">
        ///   The source data reader.
        /// </param>
        public CitiesColumns(IDataReader sourceDataReader)
        {
            if (sourceDataReader == null)
            {
                throw new ArgumentNullException("sourceDataReader");
            }

            posId = sourceDataReader.GetOrdinal(idColumnName);
            posName = sourceDataReader.GetOrdinal(nameColumnName);
            posAlternateNames = sourceDataReader.GetOrdinal(alternateNamesColumnName);
            posLatitude = sourceDataReader.GetOrdinal(latitudeColumnName);
            posLongitude = sourceDataReader.GetOrdinal(longitudeColumnName);
            posStateCode = sourceDataReader.GetOrdinal(stateCodeColumnName);
            posCountryCode = sourceDataReader.GetOrdinal(countryCodeColumnName);
            posPopulation = sourceDataReader.GetOrdinal(populationColumnName);
            posFeatureCode = sourceDataReader.GetOrdinal(featureCodeName);
            posFeatureClass = sourceDataReader.GetOrdinal(featureClassName);
        }

        public static string[] ColumnHeaders
        {
            get { return columnHeaders; }
        }

        public int Id(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetInt32(posId);
        }
        
        public string Name(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posName);
        }

        public string AlternateNames(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posAlternateNames);
        }

        public double Latitude(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetDouble(posLatitude);
        }

        public double Longitude(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetDouble(posLongitude);
        }

        public double Population(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetDouble(posPopulation);
        }

        public string StateCode(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posStateCode);
        }
        
        public string CountryCode(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posCountryCode);
        }

        public string FeatureCode(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posFeatureCode);
        }

        public string FeatureClass(IDataReader sourceDataReader)
        {
            return sourceDataReader.GetString(posFeatureClass);
        }
    }
}