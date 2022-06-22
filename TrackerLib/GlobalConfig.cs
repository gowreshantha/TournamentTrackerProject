using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLib.DataAccess;

namespace TrackerLib
{
    public class GlobalConfig
    {
        public static IDataConnection Connections { get; private set; }

        public static void InitializeConnections(DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case DatabaseType.Sql:
                    // TODO - Create the SQL Connection
                    SQLConnector sql = new SQLConnector();
                    Connections = sql;
                    break;
                case DatabaseType.TextFile:
                    //TODO - Create the Text Connection
                    TextConnector text = new TextConnector();
                    Connections = text;
                    break;
            }
        }

        public static string GetConnectonString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
