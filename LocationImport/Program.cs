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
            // https://download.geonames.org/export/dump/
            var folder = @"c:\code\diffractor-util";
            //var citiesInputFileName = System.IO.Path.Combine(folder, "cities1000.txt");
            var citiesInputFileName = System.IO.Path.Combine(folder, "allCountries.txt");
            var statesInputFileName = System.IO.Path.Combine(folder, "admin1CodesASCII.txt");
            var countriesInputFileName = System.IO.Path.Combine(folder, "countryInfo.txt");
            var countriesInputFileName2 = System.IO.Path.Combine(folder, "countries_extra.txt");

            var citiesOutputFileName = System.IO.Path.Combine(folder, "location-places.txt");
            var statesOutputFileName = System.IO.Path.Combine(folder, "location-states.txt");
            var countriesOutputFileName = System.IO.Path.Combine(folder, "location-countries.txt");

            StatesCompacter.Shrink(statesInputFileName, statesOutputFileName);
            CountriesCompacter.Shrink(countriesInputFileName, countriesInputFileName2, countriesOutputFileName);
            CitiesCompacter.Shrink(citiesInputFileName, citiesOutputFileName);

            return 1;
        }
    }
}