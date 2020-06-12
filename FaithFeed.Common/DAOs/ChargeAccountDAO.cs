using Dapper;
using FaithFeed.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.ReportingServices.Diagnostics.Internal;

namespace FaithFeed.Common.DAOs {
    public class ChargeAccountDAO {

        private readonly string ConnectionString = GlobalConfig.CnnString;

        public void CreateAccount(ChargeAccountModel newAccount) {

            var parmeters = CreateParameters(newAccount);

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                connection.Execute("dbo.spChargeAccounts_Create", parmeters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EditAccount(ChargeAccountModel updatedAccount) {

            var parameters = CreateParameters(updatedAccount);

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                connection.Execute("dbo.spChargeAccounts_Edit", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public ChargeAccountModel GetAccountInfo(int accountId) {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.QueryFirst<ChargeAccountModel>("dbo.spGetAccountInfo", new { Id = accountId }, commandType: CommandType.StoredProcedure);  
            }
        }

        public List<ChargeAccountModel> GetAllAccounts() {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.Query<ChargeAccountModel>("dbo.spGetAccountInfo_All").OrderBy(a => a.AccountName).ToList();
            }
        }

        public ChargeAccountStatModel GetAccountStats(int accountId) {

            using(IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.QueryFirst<ChargeAccountStatModel>("dbo.spGetAccountStats", new { accountId = accountId }, commandType: CommandType.StoredProcedure);
            }
        }

        public List<int> GetMonthlyAccountsDue() {

            using (IDbConnection connection = new SqlConnection(ConnectionString)) {
                return connection.Query<int>("dbo.spGetMonthlyInvoiceAccounts", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        private DynamicParameters CreateParameters(ChargeAccountModel chargeAccount) {

            var parameters = new DynamicParameters();

            if(chargeAccount.Id != 0) {
                parameters.Add("@id", chargeAccount.Id);
            }
            parameters.Add("@AccountName", chargeAccount.AccountName);
            parameters.Add("@FirstName", chargeAccount.FirstName);
            parameters.Add("@LastName", chargeAccount.LastName);
            parameters.Add("@Address", chargeAccount.Address);
            parameters.Add("@City", chargeAccount.City);
            parameters.Add("@State", chargeAccount.State);
            parameters.Add("@ZipCode", chargeAccount.ZipCode);
            parameters.Add("@PhoneNumber", chargeAccount.PhoneNumber);
            parameters.Add("@AdditionalNotes", chargeAccount.AdditionalNotes);

            return parameters;
        }
    }
}
