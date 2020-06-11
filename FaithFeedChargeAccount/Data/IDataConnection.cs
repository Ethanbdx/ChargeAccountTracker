using FaithFeed.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeed.UI
{
    public interface IDataConnection
    {
        ChargeAccountModel CreateAccount(ChargeAccountModel model);

        List<ChargeAccountModel> GetAccountInfo_All();

        ChargeAccountModel EditAccount(ChargeAccountModel model);

        ChargeAccountModel GetAccountInfo(int Id);

        InvoiceModel CreateInvoice(InvoiceModel inv);

        void UpdateInvoice(InvoiceModel inv);

        void DeleteInvoice(int invoiceId);

        ChargeAccountStatModel GetChargeAccountStat(int accountId);

        List<InvoiceModel> GetInvoices(int accountId);
        List<int> GetMonthlyInvoiceAccounts();
    }
}
