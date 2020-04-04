using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedSeed
{
    public interface IDataConnection
    {
        ChargeAccountModel CreateAccount(ChargeAccountModel model);

        List<ChargeAccountModel> GetAccountNames_All();

        ChargeAccountModel EditAccount(ChargeAccountModel model);

        ChargeAccountModel GetAccountInfo(int Id);

        InvoiceModel CreateInvoice(InvoiceModel inv);

        void UpdateInvoice(InvoiceModel inv);

        List<InvoiceModel> GetInvoices(int accountId);
    }
}
