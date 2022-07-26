using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLib.DataAccess;

namespace TrackerLib
{
    public class GlobalConfig
    {
        public const string PrizeFile = "PrizeModels.csv";
        public const string PersonFile = "PersonModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "Tournament.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchupEntryModels.csv";


        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case DatabaseType.Sql:
                    SQLConnector sql = new SQLConnector();
                    Connection = sql;
                    break;
                case DatabaseType.TextFile:
                    TextConnector text = new TextConnector();
                    Connection = text;
                    break;
               default:
                    break;
            }
        }

        public static string GetConnectonString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
