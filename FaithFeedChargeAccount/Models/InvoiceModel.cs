using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeed.UI.Models
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
        public InvoiceModel(int ChargeAccountId, string invoiceNumber, double invoiceAmount, bool isPaid, DateTime date, string paymentType, string paidDate)
        {
            AccountId = ChargeAccountId;
            InvoiceNumber = invoiceNumber;
            InvoiceAmount = invoiceAmount;
            IsPaid = isPaid;
            Date = date;
            PaymentType = paymentType;
            PaidDate = paidDate;
        }
        public InvoiceModel(int selectedInvoice, int ChargeAccountId, string invoiceNumber, double invoiceAmount, bool isPaid, DateTime date, string paidDate, string paymentType)
        {
            InvoiceId = selectedInvoice;
            AccountId = ChargeAccountId;
            InvoiceNumber = invoiceNumber;
            InvoiceAmount = invoiceAmount;
            IsPaid = isPaid;
            Date = date;
            PaymentType = paymentType;
            PaidDate = paidDate;
        }
    }
    

}

