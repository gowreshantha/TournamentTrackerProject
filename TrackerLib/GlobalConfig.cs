using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLib.DataAccess;

namespace TrackerLib
{
    public class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case DatabaseType.Sql:
                    // TODO - Create the SQL Connection
                    SQLConnector sql = new SQLConnector();
                    Connection = sql;
                    break;
                case DatabaseType.TextFile:
                    //TODO - Create the Text Connection
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
