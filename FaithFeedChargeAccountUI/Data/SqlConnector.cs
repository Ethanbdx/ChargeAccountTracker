using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FaithFeedSeed;
using Dapper;
using FaithFeedChargeAccountUI.Models;

namespace FaithFeedSeed
{
    public class SqlConnector : IDataConnection
    {
        public const string db = "ChargeAccountInvoices";

        public ChargeAccountModel CreateAccount(ChargeAccountModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@AccountName", model.AccountName);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@Address", model.Address);
                p.Add("@City", model.City);
                p.Add("@State", model.State);
                p.Add("@ZipCode", model.ZipCode);
                p.Add("@PhoneNumber", model.PhoneNumber);
                p.Add("@AdditionalNotes", model.AdditionalNotes);
                connection.Execute("dbo.spChargeAccounts_Create", p, commandType: CommandType.StoredProcedure);
                return model;

            }
        }

        public InvoiceModel CreateInvoice(InvoiceModel inv)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@InvoiceNumber", inv.InvoiceNumber);
                p.Add("@AccountId", inv.AccountId);
                p.Add("@InvoiceAmount", inv.InvoiceAmount);
                p.Add("@Date", inv.Date);
                p.Add("@IsPaid", inv.IsPaid);
                p.Add("@PaymentType", inv.PaymentType);
                p.Add("@PaidDate", inv.PaidDate);
                connection.Execute("dbo.spCreateInvoice", p, commandType: CommandType.StoredProcedure);
                return inv;
            }
        }

        public ChargeAccountModel EditAccount(ChargeAccountModel model)
        {

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@id", model.Id);
                p.Add("@AccountName", model.AccountName);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@Address", model.Address);
                p.Add("@City", model.City);
                p.Add("@State", model.State);
                p.Add("@ZipCode", model.ZipCode);
                p.Add("@PhoneNumber", model.PhoneNumber);
                p.Add("@AdditionalNotes", model.AdditionalNotes);
                connection.Execute("dbo.spChargeAccounts_Edit", p, commandType: CommandType.StoredProcedure);
                return model;
            }
        }

        public ChargeAccountModel GetAccountInfo(int Id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                ChargeAccountModel output;
                output = connection.QueryFirst<ChargeAccountModel>("dbo.spGetAccountInfo", new { Id = Id }, commandType: CommandType.StoredProcedure);
                return output;
            }
        }

        public List<ChargeAccountModel> GetAccountInfo_All()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                List<ChargeAccountModel> output;
                output = connection.Query<ChargeAccountModel>("dbo.spGetAccountInfo_All").ToList();
                return output;
            }
        }

        public void UpdateInvoice(InvoiceModel inv)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@SelectedInvoice", inv.InvoiceId);
                p.Add("@InvoiceNumber", inv.InvoiceNumber);
                p.Add("@AccountId", inv.AccountId);
                p.Add("@InvoiceAmount", inv.InvoiceAmount);
                p.Add("@Date", inv.Date);
                p.Add("@IsPaid", inv.IsPaid);
                p.Add("@PaymentType", inv.PaymentType);
                p.Add("@PaidDate", inv.PaidDate);
                connection.Execute("dbo.spUpdateInvoice", p, commandType: CommandType.StoredProcedure);
            }
        }
        public List<InvoiceModel> GetInvoices(int accId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                List<InvoiceModel> invoices = connection.Query<InvoiceModel>("dbo.spGetInvoices", new { accountId = accId }, commandType: CommandType.StoredProcedure).ToList();
                return invoices;
            }
        }

        public ChargeAccountStatModel GetChargeAccountStat(int accountId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                ChargeAccountStatModel stats = connection.QueryFirst<ChargeAccountStatModel>("dbo.spGetAccountStats", new { accountId = accountId }, commandType: CommandType.StoredProcedure);
                return stats;
            }
        }
    }
}

