using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedSeed
{
    public class ChargeAccountModel
    {
        /// <summary>
        /// Represents standard charge account information
        /// </summary>
        
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalNotes { get; set; }
        public int Id { get; set; }

        public ChargeAccountModel(int accountId, string accountName, string firstName, string lastName, string address, string city, string state, string zipCode, string phoneNumber, string additionalNotes)
        {
            Id = accountId;
            AccountName = accountName;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            PhoneNumber = phoneNumber;
            AdditionalNotes = additionalNotes;
        }
        public ChargeAccountModel()
        {

        }
        public ChargeAccountModel(string selectedAccount)
        {
            AccountName = selectedAccount;
        }
       
    }
    
}
