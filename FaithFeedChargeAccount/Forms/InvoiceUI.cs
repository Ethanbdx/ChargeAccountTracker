using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FaithFeed.UI
{
    
    public partial class InvoiceUI : Form
    {
        public int accountId { get; set; }
        public InvoiceUI(int id)
        {
            accountId = id;
            InitializeComponent();
        }
        
        private void InvoiceUI_Load(object sender, EventArgs e)
        {
            //var DataTable = new Invoices.InvoicesDataTable();
            //var TableAdapter = new Reports.InvoicesTableAdapters.InvoicesTableAdapter();
            //TableAdapter.Fill(DataTable, accountId);
            //var rds1 = new ReportDataSource("Invoices", DataTable as DataTable);
            //var myDataTable = new ChargeAccounts.ChargeAccountsDataTable();
            //var myTableAdapter = new Reports.ChargeAccountsTableAdapters.ChargeAccountsTableAdapter();
            //myTableAdapter.Fill(myDataTable, accountId);
            //var rds = new ReportDataSource("ChargeAccounts", myDataTable as DataTable);
            //reportViewer1.LocalReport.DataSources.Clear();
            //reportViewer1.LocalReport.DataSources.Add(rds);
            //reportViewer1.LocalReport.DataSources.Add(rds1);
            //reportViewer1.RefreshReport();
        }
    }
}
