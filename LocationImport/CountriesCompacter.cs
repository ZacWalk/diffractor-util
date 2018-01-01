#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static int Shrink(string countriesInputFileName, string countriesInputFileName2, string countriesOutputFileName)
        {
            Console.WriteLine("Using countries file: {0}", countriesInputFileName);
            Console.WriteLine("Producing countries file: {0}", countriesOutputFileName);

            using (var outputFile = new StreamWriter(countriesOutputFileName, false, Encoding.UTF8))
            {
                var loadedCountries = LoadCountries(countriesInputFileName, countriesInputFileName2);

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
        private static List<Record> LoadCountries(string countriesInputFileName, string countriesInputFileName2)
        {
            var loadedCountries = new Dictionary<string, Record>(StringComparer.OrdinalIgnoreCase);

            var recordsLoaded = 0;
            var maxAlts = 0;

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

                    var rec = new Record();
                    rec.name = columns.Name(sourceDataReader);
                    var code = rec.code = columns.IsoCode(sourceDataReader);

                    loadedCountries[code] = rec;
                    ++recordsLoaded;
                }
            }

            // Alternative names stored in different file
            var lines = File.ReadAllLines(countriesInputFileName2, Encoding.UTF8);

            // "name";"nativeName";"tld";"cca2";"ccn3";"cca3";"currency";"callingCode";"capital";"altSpellings";"relevance";"region";"subregion";"language";"languageCodes";"translations";"population";"latlng";"demonym";"borders"

            foreach(var line in lines)
            {
                var parts = line.Split(';');
                var code = parts[3].Trim('"'); // country code
                var alts = parts[9].Trim('"'); // alt names

                if (loadedCountries.ContainsKey(code))
                {
                    if (code == "US")
                    {
                        // few extas for usa
                        alts = alts + " United States,United States of America,America,the States,US,U.S.,USA,U.S.A.";
                    }

                    var altParts = alts.Split(',').Skip(1).Distinct().ToArray();
                    loadedCountries[code].alts = altParts;
                    if (altParts.Length > maxAlts) maxAlts = altParts.Length;
                }
            }

            Console.WriteLine("\rLoaded {0} countries, completed.", recordsLoaded);
            Console.WriteLine("\r       {0} maxumum alt names.", maxAlts);
            var results = loadedCountries.Values.ToList<Record>();
            results.Sort(delegate (Record c1, Record c2) { return String.Compare(c1.code, c2.code); });
            return results;
        }

        #region Nested type: Record

        /// <summary>
        ///   The record.
        /// </summary>
        internal sealed class Record
        {
            public string code;
            public string name;
            public string[] alts;
           
            public override string ToString()
            {
                var parts = alts != null ? string.Join("\t", alts) : "";
                return string.Format("{0}\t{1}\t{2}", code, name, parts);
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
                var cmp = StringComparer.InvariantCulture.Compare(x.name, y.name);
                if (0 == cmp)
                {
                    cmp = StringComparer.InvariantCulture.Compare(x.code, y.code);
                }

                return cmp;
            }

            #endregion
        }

        #endregion
    }
}