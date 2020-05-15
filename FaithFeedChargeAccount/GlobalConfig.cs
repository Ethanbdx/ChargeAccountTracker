using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedSeed
{
    public static class GlobalConfig
    {
        public static IDataConnection Connections { get; private set; } 
        public static void InitializeConnections(bool database)
        {
            if (database)
            {
                SqlConnector sql = new SqlConnector();
                Connections = sql;
            }
        }
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }  
}
