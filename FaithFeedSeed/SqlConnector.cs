using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FaithFeedSeed;
using Dapper;

namespace FaithFeedSeed
{
    public class SqlConnector : IDataConnection
    {
        public const string db = "ChargeAccountInvoices";

        /// <summary>
        /// Create an Charge Account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                p.Add("@AccountId", inv.ChargeAccount);
                p.Add("@InvoiceAmount", inv.InvoiceAmount);
                p.Add("@Date", inv.Date);
                p.Add("@IsPaid", inv.IsPaid);
                p.Add("@PaymentType", inv.PaymentType);
                p.Add("@PaidDate", inv.PaidDate);

                connection.Execute("dbo.spCreateInvoice", p, commandType: CommandType.StoredProcedure);
                return inv;
            }
        }

        /// <summary>
        /// Edit an Charge Account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Getting Charge Account Info
        /// </summary>
        /// <param name="accname"></param>
        /// <returns></returns>
        public List<ChargeAccountModel> GetAccountInfo(ChargeAccountModel accname)
        {
            List<ChargeAccountModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                var e = new DynamicParameters();
                p.Add("@SelectedAccountName", accname.AccountName);
                p.Add("@FirstName","", direction: ParameterDirection.Output);
                p.Add("@LastName", "", direction: ParameterDirection.Output);
                p.Add("@Address", "", direction: ParameterDirection.Output);
                p.Add("@City", "", direction: ParameterDirection.Output);
                p.Add("@State", "", direction: ParameterDirection.Output);
                p.Add("@ZipCode", "", direction: ParameterDirection.Output);
                p.Add("@PhoneNumber", "", direction: ParameterDirection.Output);
                p.Add("@AdditionalNotes", "", direction: ParameterDirection.Output);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                output = connection.Query<ChargeAccountModel>("dbo.spGetAccountInfo", p, commandType: CommandType.StoredProcedure).ToList();
                accname.FirstName = p.Get<string>("@FirstName");
                accname.LastName = p.Get<string>("@LastName");
                accname.Address = p.Get<string>("@Address");
                accname.City = p.Get<string>("@City");
                accname.State = p.Get<string>("@State");
                accname.ZipCode = p.Get<string>("@ZipCode");
                accname.PhoneNumber = p.Get<string>("@PhoneNumber");
                accname.AdditionalNotes = p.Get<string>("@AdditionalNotes");
                accname.Id = p.Get<int>("@Id");
            }
            return output;
            
        }
        /// <summary>
        /// Getting All Account Names
        /// </summary>
        /// <returns></returns>
        public List<ChargeAccountModel> GetAccountNames_All()
        {
            List<ChargeAccountModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<ChargeAccountModel>("dbo.spGetAccountNames_All").ToList();
            }
            return output;
        }

        public InvoiceModel UpdateInvoice(InvoiceModel inv)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@SelectedInvoice", inv.Id);
                p.Add("@InvoiceNumber", inv.InvoiceNumber);
                p.Add("@AccountId", inv.ChargeAccount);
                p.Add("@InvoiceAmount", inv.InvoiceAmount);
                p.Add("@Date", inv.Date);
                p.Add("@IsPaid", inv.IsPaid);
                p.Add("@PaymentType", inv.PaymentType);
                p.Add("@PaidDate", inv.PaidDate);
                connection.Execute("dbo.spUpdateInvoices", p, commandType: CommandType.StoredProcedure);
                return inv;
            }
        }
    }
    }

