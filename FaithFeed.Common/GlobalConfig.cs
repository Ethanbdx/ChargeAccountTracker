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
        public static string CnnString = $@"Data Source=localhost\SQLEXPRESS03;AttachDbFilename=E:\source\repos\ChargeAccountTracker\Database\FaithFeedSeed.mdf;Integrated Security=True";
    }  
}
