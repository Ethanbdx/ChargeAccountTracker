using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedChargeAccountUI.Models
{
    public class ChargeAccountStatModel
    {
        public double TotalOwed { get; set; }
        public double MonthlySales { get; set; }
        public double YTDSales { get; set; }
        public int AverageDays { get; set; }
    }
}
