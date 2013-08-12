#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The countries compacter.
    /// </summary>
    internal static class CountriesCompacter
    {
        /// <summary>
        ///   The shrink countries.
        /// </summary>
        /// <param name = "countriesInputFileName">
        ///   The countries input file name.
        /// </param>
        /// <param name = "countriesOutputFileName">
        ///   The countries output file name.
        /// </param>
        /// <returns>
        ///   The shrink countries.
        /// </returns>
        /// <exception cref = "NotImplementedException">
        /// </exception>
        public static int Shrink(string countriesInputFileName, string countriesOutputFileName)
        {
            Console.WriteLine("Using countries file: {0}", countriesInputFileName);
            Console.WriteLine("Producing countries file: {0}", countriesOutputFileName);

            using (var outputFile = new StreamWriter(countriesOutputFileName, false, Encoding.UTF8))
            {
                var loadedCountries = LoadCountries(countriesInputFileName);

                SortCountries(loadedCountries);

                WriteCountries(loadedCountries, outputFile);
            }


            return 0;
        }

        /// <summary>
        ///   The sort countries.
        /// </summary>
        /// <param name = "loadedCities">
        ///   The loaded cities.
        /// </param>
        private static void SortCountries(List<Record> loadedCities)
        {
            Console.Write("\rSorting countries...");
            loadedCities.Sort(new RecordComparer());
            Console.WriteLine("\rSorting countries, completed.");
        }

        /// <summary>
        ///   The write countries.
        /// </summary>
        /// <param name = "loadedCountries">
        ///   The loaded countries.
        /// </param>
        /// <param name = "outputFile">
        ///   The output file.
        /// </param>
        private static void WriteCountries(List<Record> loadedCountries, StreamWriter outputFile)
        {
            var recordsLoaded = 0;
            foreach (var record in loadedCountries)
            {
                if (0 == recordsLoaded%OutputHelpers.ConsoleOutputInterval)
                {
                    Console.Write("\rWriting {0} countries...", recordsLoaded);
                }

                outputFile.Write(record);
                outputFile.Write('\n');
                ++recordsLoaded;
            }

            Console.WriteLine("\rWriting {0} countries, completed.", recordsLoaded);
        }

        /// <summary>
        ///   The load countries.
        /// </summary>
        /// <param name = "countriesInputFileName">
        ///   The countries input file name.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<Record> LoadCountries(string countriesInputFileName)
        {
            var loadedCountries = new List<Record>();

            var recordsLoaded = 0;
            using (
                var sourceDataReader = new TabSeparatedValueReader(countriesInputFileName, Encoding.UTF8,
                                                                   CountriesColumns.ColumnHeaders,
                                                                   Detectors.CommentDetector))
            {
                var columns = new CountriesColumns(sourceDataReader);

                while (sourceDataReader.Read())
                {
                    if (0 == recordsLoaded%OutputHelpers.ConsoleOutputInterval)
                    {
                        Console.Write("\rLoaded {0} countries...", recordsLoaded);
                    }

                    var rec = new Record(columns, sourceDataReader);

                    loadedCountries.Add(rec);

                    ++recordsLoaded;
                }
            }

            Console.WriteLine("\rLoaded {0} countries, completed.", recordsLoaded);
            return loadedCountries;
        }

        #region Nested type: Record

        /// <summary>
        ///   The record.
        /// </summary>
        internal sealed class Record
        {
            /// <summary>
            ///   The code.
            /// </summary>
            private readonly string code;

            /// <summary>
            ///   The name.
            /// </summary>
            private readonly string name;

            /// <summary>
            ///   Initializes a new instance of the <see cref = "Record" /> class.
            /// </summary>
            /// <param name = "columns">
            ///   The columns.
            /// </param>
            /// <param name = "sourceDataReader">
            ///   The source data reader.
            /// </param>
            public Record(CountriesColumns columns, TabSeparatedValueReader sourceDataReader)
            {
                // var id = columns.Id( sourceDataReader );
                name = columns.Name(sourceDataReader);
                code = columns.IsoCode(sourceDataReader);
            }

            /// <summary>
            ///   Gets Name.
            /// </summary>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            ///   Gets Code.
            /// </summary>
            public string Code
            {
                get { return code; }
            }

            /// <summary>
            ///   The to string.
            /// </summary>
            /// <returns>
            ///   The to string.
            /// </returns>
            public override string ToString()
            {
                return string.Format("{0}\t{1}", code, name);
            }
        }

        #endregion

        #region Nested type: RecordComparer

        /// <summary>
        ///   The record comparer.
        /// </summary>
        private sealed class RecordComparer : IComparer<Record>
        {
            #region IComparer<Record> Members

            /// <summary>
            ///   The compare.
            /// </summary>
            /// <param name = "x">
            ///   The x.
            /// </param>
            /// <param name = "y">
            ///   The y.
            /// </param>
            /// <returns>
            ///   The compare.
            /// </returns>
            public int Compare(Record x, Record y)
            {
                var cmp = StringComparer.InvariantCulture.Compare(x.Name, y.Name);
                if (0 == cmp)
                {
                    cmp = StringComparer.InvariantCulture.Compare(x.Code, y.Code);
                }

                return cmp;
            }

            #endregion
        }

        #endregion
    }
}