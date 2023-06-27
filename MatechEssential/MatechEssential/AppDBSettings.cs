using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;


namespace MatechEssential
{
    public enum DbEngines{
        MSSQL,
        MySQL
    }
    public static class AppDBSettings
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings[0].ConnectionString; } }
        [DefaultValue(DbEngines.MySQL)]
        public static DbEngines DatabaseEngine { get; set; }
    }
}
