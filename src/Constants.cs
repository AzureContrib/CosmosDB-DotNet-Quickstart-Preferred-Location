using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CosmosDb.QuickStart
{
    public class Constants
    {

        public static readonly string database = ConfigurationManager.AppSettings["database"];
        public static readonly string collection = ConfigurationManager.AppSettings["collection"];

        public static readonly string endpoint = ConfigurationManager.AppSettings["endpoint"];
        public static readonly string authKey = ConfigurationManager.AppSettings["authKey"];

        public static readonly string connectionMode = ConfigurationManager.AppSettings["connectionMode"];
        public static readonly string connectionProtocol = ConfigurationManager.AppSettings["connectionProtocol"];

        public static readonly int maxConnectionLimit = Convert.ToInt32(ConfigurationManager.AppSettings["maxConnectionLimit"]);
    }
}