using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedSeed
{
    public class InvoiceModel
    {
        /// <summary>
        /// Represents a standard invoice
        /// </summary>
        /// 
        public int ChargeAccount { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceAmount { get; set; }
        public string Date { get; set; }
        public bool IsPaid { get; set; }
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public string PaidDate { get; set; }

        public InvoiceModel()
        {

        }
        public InvoiceModel(int ChargeAccountId, string invoiceNumber, double invoiceAmount, bool isPaid, string date, string paymentType, string paidDate)
        {
            ChargeAccount = ChargeAccountId;
            InvoiceNumber = invoiceNumber;
            InvoiceAmount = invoiceAmount;
            IsPaid = isPaid;
            Date = date;
            PaymentType = paymentType;
            PaidDate = paidDate;
        }
        public InvoiceModel(int selectedInvoice, int ChargeAccountId, string invoiceNumber, double invoiceAmount, bool isPaid, string date, string paidDate, string paymentType)
        {
            Id = selectedInvoice;
            ChargeAccount = ChargeAccountId;
            InvoiceNumber = invoiceNumber;
            InvoiceAmount = invoiceAmount;
            IsPaid = isPaid;
            Date = date;
            PaymentType = paymentType;
            PaidDate = paidDate;
        }
    }
    

}

