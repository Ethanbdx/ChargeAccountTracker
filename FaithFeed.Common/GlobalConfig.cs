using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeed.Common
{
    public static class GlobalConfig
    {
        public static string CnnString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\Database\FaithFeedSeed.mdf;Integrated Security=True";
    }  
}
