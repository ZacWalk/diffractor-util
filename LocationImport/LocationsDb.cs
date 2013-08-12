#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace LocationImport
{
    /// <summary>
    ///   The locations db.
    /// </summary>
    internal static class LocationsDb
    {
        /// <summary>
        ///   The make db.
        /// </summary>
        /// <param name = "citiesFileName">
        ///   The cities file name.
        /// </param>
        /// <param name = "statesFileName">
        ///   The states file name.
        /// </param>
        /// <param name = "countriesFileName">
        ///   The countries file name.
        /// </param>
        /// <returns>
        ///   The make db.
        /// </returns>
        public static int MakeDb(string citiesFileName, string statesFileName, string countriesFileName)
        {
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                       "Diffractor");

            var dbFileName = Path.Combine(appData, "Diffractor.Locations.DB");

            Console.WriteLine("Using cities file: {0}", citiesFileName);
            Console.WriteLine("Using states file: {0}", statesFileName);
            Console.WriteLine("Using countries file: {0}", countriesFileName);
            Console.WriteLine("Using DB: {0}", dbFileName);

            CreateEmptyDb(dbFileName);

            LoadCities(citiesFileName, statesFileName, countriesFileName, dbFileName);

            RunTests(dbFileName);

            return 0;
        }


        /// <summary>
        ///   The run tests.
        /// </summary>
        /// <param name = "dbFileName">
        ///   The db file name.
        /// </param>
        private static void RunTests(string dbFileName)
        {
            using (var connection = CreateConnection(dbFileName))
            {
                connection.Open();

                Test(connection, "Harlow, England");
                Test(connection, "Harlow");
                Test(connection, "Harlow, United Kindgom");

                Test(connection, "London, England");
                Test(connection, "London");
                Test(connection, "London, United Kindgom");
                Test(connection, "London, England, United Kindgom");
            }
        }

        /// <summary>
        ///   The test.
        /// </summary>
        /// <param name = "connection">
        ///   The connection.
        /// </param>
        /// <param name = "query">
        ///   The query.
        /// </param>
        private static void Test(SQLiteConnection connection, string query)
        {
            using (var cmd = connection.CreateCommand())
            {
                const string basicCommand =
                    "Select City.Name As City, State.Name As State, Country.Name As Country FROM City LEFT JOIN State on City.StateId = State.Id LEFT JOIN Country on City.CountryId = Country.Id";


                var qp = query.Split(',');

                var sb = new StringBuilder();
                sb.Append(basicCommand);


                sb.Append(" WHERE ( City.Name LIKE @P0 )");
                if (qp.Length > 1)
                {
                    sb.Append(
                        " OR ( ( City.Name LIKE @P0 ) AND ( ( State.Name LIKE @P1 ) OR ( Country.Name LIKE @P1 ) ) )");
                }

                if (qp.Length > 2)
                {
                    sb.Append(
                        " OR ( ( City.Name LIKE @P0 ) AND ( ( State.Name LIKE @P2 ) OR ( Country.Name LIKE @P2 ) ) )");
                    sb.Append(
                        " OR ( ( City.Name LIKE @P0 ) AND ( State.Name LIKE @P1 ) AND ( Country.Name LIKE @P2 ) )");
                }


                cmd.CommandText = sb.ToString();

                for (var p = 0; p < qp.Length; ++p)
                {
                    var val = string.Format(CultureInfo.InvariantCulture, "%{0}%", qp[p].Trim());

                    cmd.Parameters.AddWithValue(string.Format("@P{0}", p), val);
                }


                Console.WriteLine("Searching for {0}", query);
                using (var rdr = cmd.ExecuteReader())
                {
                    var count = 0;
                    var columns = rdr.FieldCount;
                    while (rdr.Read())
                    {
                        Console.WriteLine("+ Item {0}", ++count);
                        for (var column = 0; column < columns; ++column)
                        {
                            var columnName = rdr.GetName(column);
                            var value = GetString(rdr, column);

                            Console.WriteLine("   {0} = {1}", columnName, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   The get string.
        /// </summary>
        /// <param name = "rdr">
        ///   The rdr.
        /// </param>
        /// <param name = "ordinal">
        ///   The ordinal.
        /// </param>
        /// <returns>
        ///   The get string.
        /// </returns>
        private static string GetString(IDataReader rdr, int ordinal)
        {
            if (rdr.IsDBNull(ordinal))
            {
                return string.Empty;
            }

            return rdr.GetString(ordinal);
        }

        /// <summary>
        ///   The create connection.
        /// </summary>
        /// <param name = "dbFileName">
        ///   The db file name.
        /// </param>
        /// <returns>
        /// </returns>
        private static SQLiteConnection CreateConnection(string dbFileName)
        {
            var csBuilder = new SQLiteConnectionStringBuilder {DataSource = dbFileName};

            return new SQLiteConnection(csBuilder.ConnectionString);
        }

        /// <summary>
        ///   The create empty db.
        /// </summary>
        /// <param name = "dbFileName">
        ///   The db file name.
        /// </param>
        private static void CreateEmptyDb(string dbFileName)
        {
            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }

            SQLiteConnection.CreateFile(dbFileName);

            string[] commands = {
                                    "CREATE TABLE Country (id INTEGER, name NVARCHAR(200), code NVARCHAR(2), CONSTRAINT PK_Country PRIMARY KEY ( Id ) )"
                                    ,
                                    "CREATE TABLE State (id INTEGER, name NVARCHAR(200), code NVARCHAR(5), countryId NVARCHAR(2), CONSTRAINT PK_State PRIMARY KEY ( Id ), CONSTRAINT FK_State_Country FOREIGN KEY ( countryId ) REFERENCES Country ( id ) )"
                                    ,
                                    "CREATE TABLE City (id INTEGER, name NVARCHAR(200), stateId INTEGER, countryId NVARCHAR(2), latitude DOUBLE, longitude DOUBLE, CONSTRAINT PK_Location PRIMARY KEY ( Id ), CONSTRAINT FK_City_State FOREIGN KEY ( stateid ) REFERENCES State ( id ), CONSTRAINT FK_City_Country FOREIGN KEY ( countryid ) REFERENCES Country ( id ) )"
                                    ,
                                    "CREATE TABLE AlternateName ( cityid, name, CONSTRAINT PK_AlternateName PRIMARY KEY ( cityid, name ), CONSTRAINT FK_AlternateName_City FOREIGN KEY ( cityid ) REFERENCES Location ( id ) )"
                                    ,
                                };

            using (var conn = CreateConnection(dbFileName))
            {
                conn.Open();

                foreach (var ddlCommand in commands)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = ddlCommand;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Console.WriteLine("Empty DB Created");
        }

        /// <summary>
        ///   The load single location.
        /// </summary>
        /// <param name = "conn">
        ///   The conn.
        /// </param>
        /// <param name = "id">
        ///   The id.
        /// </param>
        /// <param name = "name">
        ///   The name.
        /// </param>
        /// <param name = "alternateNames">
        ///   The alternate names.
        /// </param>
        /// <param name = "latitude">
        ///   The latitude.
        /// </param>
        /// <param name = "longitude">
        ///   The longitude.
        /// </param>
        /// <param name = "stateId">
        ///   The state id.
        /// </param>
        /// <param name = "countryId">
        ///   The country id.
        /// </param>
        public static void LoadSingleLocation(SQLiteConnection conn,
                                              int id,
                                              string name,
                                              IEnumerable<string> alternateNames,
                                              double latitude,
                                              double longitude,
                                              int stateId,
                                              int countryId)
        {
            // TODO: Consider looking to see if location exists and adding it.
            using (var transaction = conn.BeginTransaction())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO City ( id, name, stateId, countryId, latitude, longitude ) VALUES ( @Id, @Name, @StateId, @CountryId, @Latitude, @Longitude)";

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@StateId", stateId);
                    cmd.Parameters.AddWithValue("@CountryId", countryId);
                    cmd.Parameters.AddWithValue("@Latitude", latitude);
                    cmd.Parameters.AddWithValue("@Longitude", longitude);

                    cmd.ExecuteNonQuery();
                }

                foreach (var alternate in alternateNames)
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO AlternateName ( cityId, name ) VALUES ( @cityId, @Name)";

                        cmd.Parameters.AddWithValue("@cityId", id);
                        cmd.Parameters.AddWithValue("@Name", alternate);

                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        /// <summary>
        ///   The load cities.
        /// </summary>
        /// <param name = "citiesFileName">
        ///   The cities file name.
        /// </param>
        /// <param name = "statesFileName">
        ///   The states file name.
        /// </param>
        /// <param name = "countriesFileName">
        ///   The countries file name.
        /// </param>
        /// <param name = "dbFileName">
        ///   The db file name.
        /// </param>
        private static void LoadCities(string citiesFileName,
                                       string statesFileName,
                                       string countriesFileName,
                                       string dbFileName)
        {
            using (var outputConnection = CreateConnection(dbFileName))
            {
                outputConnection.Open();

                var countryMapping = LoadCountries(outputConnection, countriesFileName);

                Console.WriteLine("Loaded {0} countries", countryMapping.Count);

                var statesMapping = LoadStates(outputConnection, statesFileName, countryMapping);

                Console.WriteLine("Loaded {0} states", statesMapping.Count);

                LoadCities(outputConnection, citiesFileName, statesMapping, countryMapping);
            }
        }

        /// <summary>
        ///   The load countries.
        /// </summary>
        /// <param name = "outputConnection">
        ///   The output connection.
        /// </param>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <returns>
        /// </returns>
        private static Dictionary<string, int> LoadCountries(SQLiteConnection outputConnection, string fileName)
        {
            var mappings = new Dictionary<string, int>();

            using (
                var sourceDataReader = new TabSeparatedValueReader(fileName, Encoding.UTF8,
                                                                   CountriesColumns.ColumnHeaders,
                                                                   Detectors.CommentDetector))
            {
                var columns = new CountriesColumns(sourceDataReader);

                while (sourceDataReader.Read())
                {
                    var id = columns.Id(sourceDataReader);
                    var name = columns.Name(sourceDataReader);
                    var code = columns.IsoCode(sourceDataReader);

                    using (var transaction = outputConnection.BeginTransaction())
                    {
                        using (var cmd = outputConnection.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO Country ( id, name, code ) VALUES ( @Id, @Name, @Code )";

                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@Code", code);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }

                    mappings.Add(code, id);
                }

                return mappings;
            }
        }

        /// <summary>
        ///   The load states.
        /// </summary>
        /// <param name = "outputConnection">
        ///   The output connection.
        /// </param>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "countryMappings">
        ///   The country mappings.
        /// </param>
        /// <returns>
        /// </returns>
        private static Dictionary<string, int> LoadStates(SQLiteConnection outputConnection,
                                                          string fileName,
                                                          Dictionary<string, int> countryMappings)
        {
            var mappings = new Dictionary<string, int>();

            using (
                var sourceDataReader = new TabSeparatedValueReader(fileName, Encoding.UTF8, StatesColumns.ColumnHeaders,
                                                                   Detectors.CommentDetector))
            {
                var columns = new StatesColumns(sourceDataReader);

                var id = 0;
                while (sourceDataReader.Read())
                {
                    ++id;
                    var name = columns.Name(sourceDataReader);
                    var code = columns.Code(sourceDataReader);

                    var ids = code.Split('.');

                    var countryId = countryMappings[ids[0]];


                    using (var transaction = outputConnection.BeginTransaction())
                    {
                        using (var cmd = outputConnection.CreateCommand())
                        {
                            cmd.CommandText =
                                "INSERT INTO State ( id, name, countryId ) VALUES ( @Id, @Name, @CountryId )";

                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@CountryId", countryId);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }

                    mappings.Add(code, id);
                }

                return mappings;
            }
        }

        /// <summary>
        ///   The load cities.
        /// </summary>
        /// <param name = "outputConnection">
        ///   The output connection.
        /// </param>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "stateMappings">
        ///   The state mappings.
        /// </param>
        /// <param name = "countryMappings">
        ///   The country mappings.
        /// </param>
        private static void LoadCities(SQLiteConnection outputConnection,
                                       string fileName,
                                       Dictionary<string, int> stateMappings,
                                       Dictionary<string, int> countryMappings)
        {
            long recordsLoaded = 0;

            using (
                var sourceDataReader = new TabSeparatedValueReader(fileName, Encoding.UTF8, CitiesColumns.ColumnHeaders)
                )
            {
                var columns = new CitiesColumns(sourceDataReader);

                while (sourceDataReader.Read())
                {
                    if (0 == recordsLoaded%10)
                    {
                        Console.Write("\rLoaded {0} cities", recordsLoaded);
                    }


                    var id = columns.Id(sourceDataReader);
                    var name = columns.Name(sourceDataReader);
                    var alternateNames = columns.AlternateNames(sourceDataReader);
                    var latitude = columns.Latitude(sourceDataReader);
                    var longitude = columns.Longitude(sourceDataReader);
                    var stateCode = columns.StateCode(sourceDataReader);
                    var countryCode = columns.CountryCode(sourceDataReader);

                    var stateId = 0;
                    if (!string.IsNullOrEmpty(stateCode))
                    {
                        var code = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", countryCode, stateCode);

                        if (stateMappings.ContainsKey(code))
                        {
                            stateId = stateMappings[code];
                        }
                    }

                    var countryId = countryMappings[countryCode];

                    var splitAlternateNames =
                        (from record in alternateNames.Split(',') select record.Trim()).Distinct();

                    LoadSingleLocation(outputConnection, id, name, splitAlternateNames, latitude, longitude, stateId,
                                       countryId);

                    ++recordsLoaded;
                }

                Console.WriteLine("\rLoaded {0} cities", recordsLoaded);
            }
        }
    }
}