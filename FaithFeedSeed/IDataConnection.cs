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

        List<ChargeAccountModel> GetAccountInfo(ChargeAccountModel accname);

        InvoiceModel CreateInvoice(InvoiceModel inv);

        InvoiceModel UpdateInvoice(InvoiceModel inv);
    }
}
