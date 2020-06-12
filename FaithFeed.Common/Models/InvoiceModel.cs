using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeed.Common.Models
{
    public class InvoiceModel
    {
        /// <summary>
        /// Represents a standard invoice
        /// </summary>
        /// 
        public int InvoiceId { get; set; }
        public int AccountId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public double InvoiceAmount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentType { get; set; }
        public string PaidDate { get; set; }

        public InvoiceModel()
        {

        }
    }
    
}

