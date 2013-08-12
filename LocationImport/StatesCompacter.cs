#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The states compacter.
    /// </summary>
    internal class StatesCompacter
    {
        /// <summary>
        ///   The shrink states.
        /// </summary>
        /// <param name = "statesInputFileName">
        ///   The states input file name.
        /// </param>
        /// <param name = "statesOutputFileName">
        ///   The states output file name.
        /// </param>
        /// <returns>
        ///   The shrink states.
        /// </returns>
        /// <exception cref = "NotImplementedException">
        /// </exception>
        public static int Shrink(string statesInputFileName, string statesOutputFileName)
        {
            Console.WriteLine("Using states file: {0}", statesInputFileName);
            Console.WriteLine("Producing states file: {0}", statesOutputFileName);

            using (var outputFile = new StreamWriter(statesOutputFileName, false, Encoding.UTF8))
            {
                var loadedCountries = LoadStates(statesInputFileName);

                SortStates(loadedCountries);

                WriteStates(loadedCountries, outputFile);
            }


            return 0;
        }

        /// <summary>
        ///   The sort states.
        /// </summary>
        /// <param name = "loadedStates">
        ///   The loaded states.
        /// </param>
        private static void SortStates(List<Record> loadedStates)
        {
            Console.Write("\rSorting countries...");
            loadedStates.Sort(new RecordComparer());
            Console.WriteLine("\rSorting states, completed.");
        }

        /// <summary>
        ///   The write states.
        /// </summary>
        /// <param name = "loadedStates">
        ///   The loaded states.
        /// </param>
        /// <param name = "outputFile">
        ///   The output file.
        /// </param>
        private static void WriteStates(List<Record> loadedStates, StreamWriter outputFile)
        {
            var recordsLoaded = 0;
            foreach (var record in loadedStates)
            {
                if (0 == recordsLoaded%OutputHelpers.ConsoleOutputInterval)
                {
                    Console.Write("\rWriting {0} states...", recordsLoaded);
                }

                outputFile.Write(record);
                outputFile.Write('\n');
                ++recordsLoaded;
            }

            Console.WriteLine("\rWriting {0} states, completed.", recordsLoaded);
        }

        /// <summary>
        ///   The load states.
        /// </summary>
        /// <param name = "statesInputFileName">
        ///   The states input file name.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<Record> LoadStates(string statesInputFileName)
        {
            var loadedStates = new List<Record>();

            var recordsLoaded = 0;
            using (
                var sourceDataReader = new TabSeparatedValueReader(statesInputFileName, Encoding.UTF8,
                                                                   StatesColumns.ColumnHeaders,
                                                                   Detectors.CommentDetector))
            {
                var columns = new StatesColumns(sourceDataReader);

                while (sourceDataReader.Read())
                {
                    if (0 == recordsLoaded%OutputHelpers.ConsoleOutputInterval)
                    {
                        Console.Write("\rLoaded {0} states...", recordsLoaded);
                    }

                    var rec = new Record(columns, sourceDataReader);

                    loadedStates.Add(rec);

                    ++recordsLoaded;
                }
            }

            Console.WriteLine("\rLoaded {0} states, completed.", recordsLoaded);
            return loadedStates;
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
            public Record(StatesColumns columns, TabSeparatedValueReader sourceDataReader)
            {
                // var id = columns.Id( sourceDataReader );
                name = columns.Name(sourceDataReader);
                code = columns.Code(sourceDataReader);
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