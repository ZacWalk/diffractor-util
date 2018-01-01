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
            var folder = @"D:\Development\diffractor-util";
            //var citiesInputFileName = System.IO.Path.Combine(folder, "cities1000.txt");
            var citiesInputFileName = System.IO.Path.Combine(folder, "allCountries.txt");
            var statesInputFileName = System.IO.Path.Combine(folder, "admin1CodesASCII.txt");
            var countriesInputFileName = System.IO.Path.Combine(folder, "countryInfo.txt");
            var countriesInputFileName2 = System.IO.Path.Combine(folder, "countries_extra.txt");

            var citiesOutputFileName = System.IO.Path.Combine(folder, "locations.txt");
            var statesOutputFileName = System.IO.Path.Combine(folder, "states.txt");
            var countriesOutputFileName = System.IO.Path.Combine(folder, "countries.txt");

            StatesCompacter.Shrink(statesInputFileName, statesOutputFileName);
            CountriesCompacter.Shrink(countriesInputFileName, countriesInputFileName2, countriesOutputFileName);
            CitiesCompacter.Shrink(citiesInputFileName, citiesOutputFileName);

            return 1;
        }
    }
}