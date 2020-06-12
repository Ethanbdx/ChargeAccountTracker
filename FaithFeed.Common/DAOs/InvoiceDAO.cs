using FaithFeed.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace FaithFeed.Common.DAOs {
    public class InvoiceDAO {

        private readonly string ConnectionString = GlobalConfig.CnnString;

        public void CreateInvoice(InvoiceModel newInvoice) {

            var parameters = CreateParamters(newInvoice);

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                connection.Execute("dbo.spCreateInvoice", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateInvoice(InvoiceModel updatedInvoice) {

            var parameters = CreateParamters(updatedInvoice);

            using (IDbConnection connection = new SqlConnection(ConnectionString)) {
                connection.Execute("dbo.spUpdateInvoice", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<InvoiceModel> GetAccountInvoices(int accountId) {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.Query<InvoiceModel>("dbo.spGetInvoices", new { accountId = accountId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void DeleteInvoice(int invoiceId) {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                connection.Execute("dbo.spDeleteInvoice", new { invoiceId = invoiceId }, commandType: CommandType.StoredProcedure);
            }
        }

        public List<InvoiceModel> GetMonthlyInvoices(int accountId) {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.Query<InvoiceModel>("dbo.spGetMonthlyInvoices", new { accountId = accountId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        private DynamicParameters CreateParamters(InvoiceModel invoice) {

            var parameters = new DynamicParameters();

            if(invoice.InvoiceId != 0) {
                parameters.Add("@SelectedInvoice", invoice.InvoiceId);
            }
            parameters.Add("@InvoiceNumber", invoice.InvoiceNumber);
            parameters.Add("@AccountId", invoice.AccountId);
            parameters.Add("@InvoiceAmount", invoice.InvoiceAmount);
            parameters.Add("@Date", invoice.Date);
            parameters.Add("@IsPaid", invoice.IsPaid);
            parameters.Add("@PaymentType", invoice.PaymentType);
            parameters.Add("@PaidDate", invoice.PaidDate);

            return parameters;
        }
    }
}
