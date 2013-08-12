#region

using System;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///   The main.
        /// </summary>
        /// <param name = "args">
        ///   The args.
        /// </param>
        /// <returns>
        ///   The main.
        /// </returns>
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                return Usage();
            }

            if (StringComparer.InvariantCultureIgnoreCase.Equals(args[0], "MAKEDB"))
            {
                if (args.Length != 4)
                {
                    return Usage();
                }


                var citiesFileName = args[1];
                var statesFileName = args[2];
                var countriesFileName = args[3];

                return LocationsDb.MakeDb(citiesFileName, statesFileName, countriesFileName);
            }

            if (StringComparer.InvariantCultureIgnoreCase.Equals(args[0], "SHRINKPLACES"))
            {
                if (args.Length != 3)
                {
                    return Usage();
                }

                var citiesInputFileName = args[1];
                var citiesOutputFileName = args[2];

                return CitiesCompacter.Shrink(citiesInputFileName, citiesOutputFileName);
            }

            if (StringComparer.InvariantCultureIgnoreCase.Equals(args[0], "SHRINKSTATES"))
            {
                if (args.Length != 3)
                {
                    return Usage();
                }

                var statesInputFileName = args[1];
                var statesOutputFileName = args[2];

                return StatesCompacter.Shrink(statesInputFileName, statesOutputFileName);
            }

            if (StringComparer.InvariantCultureIgnoreCase.Equals(args[0], "SHRINKCOUNTRIES"))
            {
                if (args.Length != 3)
                {
                    return Usage();
                }

                var countriesInputFileName = args[1];
                var countriesOutputFileName = args[2];

                return CountriesCompacter.Shrink(countriesInputFileName, countriesOutputFileName);
            }

            return Usage();
        }


        /// <summary>
        ///   The usage.
        /// </summary>
        /// <returns>
        ///   The usage.
        /// </returns>
        private static int Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  LocationImport CitiesFile StatesFile CountriesFile");
            Console.WriteLine();
            Console.WriteLine(" e.g.:");
            Console.WriteLine(
                " LocationImport MAKEDB c:\\temp\\cities1000.txt c:\\temp\\admin1CodesASCII.txt c:\\temp\\CountryInfo.txt");
            Console.WriteLine();
            return 1;
        }
    }
}