using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib
{
    public class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool database, bool textFiles)
        {
            if (database)
            {
                // TODO - Create the SQL Connection
                SQLConnector sql = new SQLConnector();
                Connections.Add(sql);
            }

            if (textFiles)
            {
                //TODO - Create the Text Connection
                TextConnection text = new TextConnection();
                Connections.Add(text);
            }
        }
    }
}
