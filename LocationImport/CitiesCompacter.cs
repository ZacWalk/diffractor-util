// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CitiesCompacter.cs" company="">
//   
// </copyright>
// <summary>
//   The cities compacter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    /// The cities compacter.
    /// </summary>
    internal static class CitiesCompacter
    {
        /// <summary>
        /// The shrink cities.
        /// </summary>
        /// <param name="citiesInputFileName">
        /// The cities input file name.
        /// </param>
        /// <param name="citiesOutputFileName">
        /// The cities output file name.
        /// </param>
        /// <returns>
        /// The shrink cities.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static int Shrink( string citiesInputFileName, string citiesOutputFileName )
        {
            Console.WriteLine( "Using cities file: {0}", citiesInputFileName );
            Console.WriteLine( "Producing cities file: {0}", citiesOutputFileName );

            using( var outputFile = new StreamWriter( citiesOutputFileName, false, Encoding.UTF8 ) )
            {
                var loadedCities = LoadCities( citiesInputFileName );

                SortCities( loadedCities );

                WriteCities( loadedCities, outputFile );
            }


            Console.WriteLine( "Shrinking complete." );

            return 0;
        }

        /// <summary>
        /// The sort cities.
        /// </summary>
        /// <param name="loadedCities">
        /// The loaded cities.
        /// </param>
        private static void SortCities( List< Record > loadedCities )
        {
            Console.Write( "\rSorting cities..." );
            loadedCities.Sort( new RecordComparer() );
            Console.WriteLine( "\rSorting cities, completed." );
        }

        /// <summary>
        /// The write cities.
        /// </summary>
        /// <param name="loadedCities">
        /// The loaded cities.
        /// </param>
        /// <param name="outputFile">
        /// The output file.
        /// </param>
        private static void WriteCities( List< Record > loadedCities, StreamWriter outputFile )
        {
            long recordsLoaded = 0;
            foreach( var record in loadedCities )
            {
                outputFile.Write( record );
                outputFile.Write( '\n' );
                if( 0 == recordsLoaded % OutputHelpers.ConsoleOutputInterval )
                {
                    Console.Write( "\rWrote {0} cities...", recordsLoaded );
                }

                ++recordsLoaded;
            }

            Console.WriteLine( "\rWrote {0} cities, completed.", recordsLoaded );
        }

        /// <summary>
        /// The load cities.
        /// </summary>
        /// <param name="citiesInputFileName">
        /// The cities input file name.
        /// </param>
        /// <returns>
        /// </returns>
        private static List< Record > LoadCities( string citiesInputFileName )
        {
            var ids = new List< int >();

            long recordsLoaded = 0;
            var loadedCities = new List< Record >();

            using(
                var sourceDataReader = new TabSeparatedValueReader( citiesInputFileName, Encoding.UTF8, 
                                                                    CitiesColumns.ColumnHeaders ) )
            {
                var columns = new CitiesColumns( sourceDataReader );

                while( sourceDataReader.Read() )
                {
                    if( 0 == recordsLoaded % OutputHelpers.ConsoleOutputInterval )
                    {
                        Console.Write( "\rLoaded {0} cities...", recordsLoaded );
                    }

                    var rec = new Record( columns, sourceDataReader );

                    loadedCities.Add( rec );

                    ++recordsLoaded;
                }
            }

            Console.WriteLine( "\rLoaded {0} cities, completed.", recordsLoaded );

            return loadedCities;
        }

        #region Nested type: Record

        /// <summary>
        /// The record.
        /// </summary>
        private sealed class Record
        {
            /// <summary>
            ///   The alternate names.
            /// </summary>
            private readonly string alternateNames;

            /// <summary>
            ///   The country code.
            /// </summary>
            private readonly string countryCode;

            /// <summary>
            /// The id.
            /// </summary>
            private readonly int id;

            /// <summary>
            ///   The latitude.
            /// </summary>
            private readonly double latitude;

            /// <summary>
            ///   The longitude.
            /// </summary>
            private readonly double longitude;

            /// <summary>
            ///   The name.
            /// </summary>
            private readonly string name;

            /// <summary>
            ///   The state code.
            /// </summary>
            private readonly string stateCode;

            /// <summary>
            /// Initializes a new instance of the <see cref="Record"/> class.
            /// </summary>
            /// <param name="columns">
            /// The columns.
            /// </param>
            /// <param name="sourceDataReader">
            /// The source data reader.
            /// </param>
            public Record( CitiesColumns columns, IDataReader sourceDataReader )
            {
                id = columns.Id( sourceDataReader );
                name = columns.Name( sourceDataReader );
                alternateNames = CompactAlternateNames( name, columns.AlternateNames( sourceDataReader ) );
                latitude = columns.Latitude( sourceDataReader );
                longitude = columns.Longitude( sourceDataReader );
                stateCode = columns.StateCode( sourceDataReader );
                countryCode = columns.CountryCode( sourceDataReader );
            }

            /// <summary>
            ///   The name.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            ///   The state code.
            /// </summary>
            public string StateCode
            {
                get
                {
                    return stateCode;
                }
            }

            /// <summary>
            ///   The country code.
            /// </summary>
            public string CountryCode
            {
                get
                {
                    return countryCode;
                }
            }

            /// <summary>
            ///   The longitude.
            /// </summary>
            public double Longitude
            {
                get
                {
                    return longitude;
                }
            }

            /// <summary>
            ///   The latitude.
            /// </summary>
            public double Latitude
            {
                get
                {
                    return latitude;
                }
            }

            /// <summary>
            ///   The alternate names.
            /// </summary>
            public string AlternateNames
            {
                get
                {
                    return alternateNames;
                }
            }

            /// <summary>
            /// Gets Id.
            /// </summary>
            public int Id
            {
                get
                {
                    return id;
                }
            }

            /// <summary>
            /// The compact alternate names.
            /// </summary>
            /// <param name="actualName">
            /// The actual name.
            /// </param>
            /// <param name="otherNames">
            /// The other names.
            /// </param>
            /// <returns>
            /// The compact alternate names.
            /// </returns>
            private static string CompactAlternateNames( string actualName, string otherNames )
            {
                if( string.IsNullOrEmpty( otherNames ) )
                {
                    return otherNames;
                }

                var candidates = otherNames.Split( ',' );

                var sb = new StringBuilder();
                foreach( var name in candidates )
                {
                    if( StringComparer.InvariantCultureIgnoreCase.Equals( actualName, name ) )
                    {
                        continue;
                    }

                    if( sb.Length != 0 )
                    {
                        sb.Append( "," );
                    }

                    sb.Append( name );
                }

                return sb.ToString();
            }

            /// <summary>
            /// The to string.
            /// </summary>
            /// <returns>
            /// The to string.
            /// </returns>
            public override string ToString()
            {
                return string.Format( "{0:0.#####}\t{1:0.#####}\t{2}\t{3}\t{4}\t{5}", Latitude, Longitude, Name, 
                                      StateCode, CountryCode, AlternateNames );
            }
        }

        #endregion

        #region Nested type: RecordComparer

        /// <summary>
        /// The record comparer.
        /// </summary>
        private sealed class RecordComparer : IComparer< Record >
        {
            #region IComparer<Record> Members

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The compare.
            /// </returns>
            public int Compare( Record x, Record y )
            {
                var cmp = StringComparer.InvariantCulture.Compare( x.Name, y.Name );
                if( 0 == cmp )
                {
                    cmp = StringComparer.InvariantCulture.Compare( x.StateCode, y.StateCode );
                }

                if( 0 == cmp )
                {
                    cmp = StringComparer.InvariantCulture.Compare( x.CountryCode, y.CountryCode );
                }

                if( 0 == cmp )
                {
                    cmp = Comparer< double >.Default.Compare( x.Latitude, y.Latitude );
                }

                if( 0 == cmp )
                {
                    cmp = Comparer< double >.Default.Compare( x.Longitude, y.Longitude );
                }

                if( 0 == cmp )
                {
                    cmp = StringComparer.InvariantCulture.Compare( x.AlternateNames, y.AlternateNames );
                }

                return cmp;
            }

            #endregion
        }

        #endregion
    }
}