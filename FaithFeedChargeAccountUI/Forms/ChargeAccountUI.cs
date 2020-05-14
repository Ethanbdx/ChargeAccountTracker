using FaithFeedSeed;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using FaithFeedChargeAccountUI.Models;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace FaithFeedChargeAccountUI
{

    public partial class ChargeAccountUI : MaterialForm
    {
        private ChargeAccountModel selectedAccount;
        private InvoiceModel selectedInvoice;
        public ChargeAccountUI()
        {   
            InitializeComponent();
            PopulateDropDown();
        }
        #region Event Handlers

        private void CreateNewAccount_Click(object sender, EventArgs e)
        { 
            if (InvoicesGroupBox.Visible) {
                InvoicesGroupBox.Visible = false;
                btnPrintInvoice.Visible = false;
            }
            SelectedChargeAccount.SelectedIndex = -1;
            ClearFields();
            EditFields();
            Editbtn.Visible = false;
            Savebtn.Visible = false;
            Createbtn.Visible = true;
            ChargeAccountGroupBox.Visible = true;
            AccountNameValue.ReadOnly = false;
        }
        private void Editbtn_Click(object sender, EventArgs e)
        {
            EditFields();
            Editbtn.Visible = false;
            Savebtn.Visible = true;
        }
        //Selection changes on account name dropdown
        private void SelectedChargeAccount_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ReadOnlyFields();
            Createbtn.Visible = false;
            DisplayFields();
            ClearInvoiceFields();
        }
        //Creating an invoice from the selectedID. 
        public void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (selectedAccount.Id == 0)
            {
                MessageBox.Show("You must select an account first.");
            }
            else
            {
                InvoiceUI form = new InvoiceUI(selectedAccount.Id);
                form.Show();
            }
        }
        //Valdiation for new invoices.
        private void CreateInvoiceBtn_Click(object sender, EventArgs e)
        {
            if(CreateInvoiceBtn.Text == "Create Invoice")
            {
                if (ValidateInvoice())
                {
                    InvoiceModel inv = new InvoiceModel()
                    {
                        AccountId = selectedAccount.Id,
                        InvoiceNumber = InvoiceNumberValue.Text,
                        InvoiceAmount = Double.Parse(InvoiceAmountValue.Text),
                        IsPaid = InvoiceStatus(InvoiceStatusValue.Text),
                        Date = InvoiceDatePicker.Value
                    };
                    if (txtPaymentType.Visible)
                    {
                        inv.PaymentType = txtPaymentType.Text;
                        inv.PaidDate = dtpPaidDate.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        inv.PaymentType = "";
                        inv.PaidDate = "";
                    }
                    GlobalConfig.Connections.CreateInvoice(inv);
                    FillDataGrid();
                    ClearInvoiceFields();
                    PopulateAccountStats(selectedAccount.Id);
                    MessageBox.Show("New Invoice Created!");
                }
                else
                {
                    MessageBox.Show("Invoice information is invalid");
                }
            } 
            else
            {
                if (ValidateInvoice())
                {
                    InvoiceModel inv = selectedInvoice;
                    inv.Date = InvoiceDatePicker.Value;
                    inv.InvoiceNumber = InvoiceNumberValue.Text;
                    inv.InvoiceAmount = double.Parse(InvoiceAmountValue.Text);
                    inv.IsPaid = InvoiceStatus(InvoiceStatusValue.Text);
                    if (txtPaymentType.Visible)
                    {
                        inv.PaymentType = txtPaymentType.Text;
                        inv.PaidDate = dtpPaidDate.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        inv.PaymentType = "";
                        inv.PaidDate = "";
                    }
                    GlobalConfig.Connections.UpdateInvoice(inv);
                    FillDataGrid();
                    ClearInvoiceFields();
                    PopulateAccountStats(selectedAccount.Id);
                    MessageBox.Show("Sucessfully Upated Invoice!");
                    CreateInvoiceBtn.Size = new Size(430, 68);

                } 
                else
                {
                    MessageBox.Show("Invoice information couldn't be validated.");
                }
            }
        }
        //Validation for the newly created account
        private void Createbtn_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                ChargeAccountModel model = new ChargeAccountModel(
                -1,
                AccountNameValue.Text,
                FirstNameValue.Text,
                LastNameValue.Text,
                AddressValue.Text,
                CityValue.Text,
                StateValue.Text,
                ZipValue.Text,
                PhoneNumberValue.Text,
                AdditionalNotesValue.Text);
                GlobalConfig.Connections.CreateAccount(model);
                AccountNameValue.ReadOnly = true;
                ClearFields();
                ReadOnlyFields();
                MessageBox.Show("Sucessfully created new charge account!");
                PopulateDropDown();
            }
            else
            {
                MessageBox.Show("Please review and correct information.");
            }
        }
        //Validation for edited accounts.
        private void Savebtn_Click(object sender, EventArgs e)
        {
            var CurrentAccount = SelectedChargeAccount.SelectedIndex;
            if (ValidateForm())
            {
                ChargeAccountModel model = new ChargeAccountModel(
                selectedAccount.Id,
                AccountNameValue.Text,
                FirstNameValue.Text,
                LastNameValue.Text,
                AddressValue.Text,
                CityValue.Text,
                StateValue.Text,
                ZipValue.Text,
                PhoneNumberValue.Text,
                AdditionalNotesValue.Text);
                GlobalConfig.Connections.EditAccount(model);
                PopulateDropDown();
                ReadOnlyFields();
                MessageBox.Show("Sucessfully edited information");
                SelectedChargeAccount.SelectedIndex = CurrentAccount;
                PopulateFields(model.Id);
                Savebtn.Visible = false;
                Editbtn.Visible = true;
            }
            else
            {
                MessageBox.Show("Hmmm, something isn't quite right.");
            }
        }
        #endregion
        #region Methods 
        //Valdiation logic for invoices
        private bool ValidateInvoice()
        {
            bool output = true;
            if(InvoiceNumberValue.Text.Length == 0)
            {
                output = false;
            }
            if(InvoiceAmountValue.Text.Length == 0)
            {
                output = false;
            }
            if (InvoiceDatePicker.Text.Length == 0)
            {
                output = false;
            }
            if (InvoiceStatusValue.Text.Length == 0)
            {
                output = false;
            }
            return output;
        }
        //Validation logic for account
        private bool ValidateForm()
        {
            bool output = true;

            if (AccountNameValue.Text.Length == 0 || AccountNameValue.Text.Length > 20)
            {
                output = false;
            }
            if (FirstNameValue.Text.Length == 0 || FirstNameValue.Text.Length > 15)
            {
                output = false;
            }
            if (LastNameValue.Text.Length == 0 || LastNameValue.Text.Length > 20)
            {
                output = false;
            }
            if (AccountNameValue.Text.Length == 0 || AddressValue.Text.Length > 40)
            {
                output = false;
            }
            if (CityValue.Text.Length == 0 || CityValue.Text.Length > 20)
            {
                output = false;
            }
            if (StateValue.Text.Length == 0 || StateValue.Text.Length > 2)
            {
                output = false;
            }
            if (ZipValue.Text.Length == 0 || ZipValue.Text.Length > 5)
            {
                output = false;
            }
            if (PhoneNumberValue.Text.Length == 0 || PhoneNumberValue.Text.Length > 15)
            {
                output = false;
            }
            return output;
        }
        private void ReadOnlyFields()
        {
            AccountNameValue.ReadOnly = true;
            FirstNameValue.ReadOnly = true;
            LastNameValue.ReadOnly = true;
            AddressValue.ReadOnly = true;
            CityValue.ReadOnly = true;
            StateValue.ReadOnly = true;
            ZipValue.ReadOnly = true;
            PhoneNumberValue.ReadOnly = true;
            AdditionalNotesValue.ReadOnly = true;
        }
        private void EditFields()
        {
            AccountNameValue.ReadOnly = false;
            FirstNameValue.ReadOnly = false;
            LastNameValue.ReadOnly = false;
            AddressValue.ReadOnly = false;
            CityValue.ReadOnly = false;
            StateValue.ReadOnly = false;
            ZipValue.ReadOnly = false;
            PhoneNumberValue.ReadOnly = false;
            AdditionalNotesValue.ReadOnly = false;
        }
        private void ClearFields()
        {
            AccountNameValue.Text = "";
            FirstNameValue.Text = "";
            LastNameValue.Text = "";
            AddressValue.Text = "";
            CityValue.Text = "";
            StateValue.Text = "";
            ZipValue.Text = "";
            PhoneNumberValue.Text = "";
            AdditionalNotesValue.Text = "";
            ChargeAccountGroupBox.Text = "Charge Account Details";
        }
        private void DisplayFields()
        {
           
            if (!ChargeAccountGroupBox.Visible || !InvoicesGroupBox.Visible)
            {
                ChargeAccountGroupBox.Visible = true;
                InvoicesGroupBox.Visible = true;
                btnPrintInvoice.Visible = true;
            }
            Editbtn.Visible = true;
            ChargeAccountGroupBox.Text = "Charge Account Details";
            PopulateFields((int)SelectedChargeAccount.SelectedValue);
            FillDataGrid();
        }
        private void PopulateFields(int accountId)
        {
           
            ChargeAccountModel account = GlobalConfig.Connections.GetAccountInfo(accountId);
            selectedAccount = account;
            AccountNameValue.Text = account.AccountName;
            FirstNameValue.Text = account.FirstName;
            LastNameValue.Text = account.LastName;
            AddressValue.Text = account.Address;
            CityValue.Text = account.City;
            StateValue.Text = account.State;
            ZipValue.Text = account.ZipCode;
            PhoneNumberValue.Text = account.PhoneNumber;
            AdditionalNotesValue.Text = account.AdditionalNotes;
            if (ChargeAccountGroupBox.Text == "Charge Account Details")
            {
                ChargeAccountGroupBox.Text += " - Id (" + account.Id + ")";
            }
            PopulateAccountStats(accountId);
        }
        private void PopulateDropDown()
        {
            var selAcct = SelectedChargeAccount;
            selAcct.DisplayMember = "AccountName";
            selAcct.ValueMember = "Id";
            selAcct.DataSource = GlobalConfig.Connections.GetAccountInfo_All();
            selAcct.SelectedIndex = -1;
        }
        private void FillDataGrid()
        {
            var idg = InvoicesDataGrid;
            var cols = idg.Columns;
            idg.ReadOnly = true;
            idg.DataSource = GlobalConfig.Connections.GetInvoices(selectedAccount.Id);
            cols[1].Visible = false;
            cols[2].Visible = false;
            cols[0].Width = 65;
            cols[3].HeaderText = "Invoice Number";
            cols[3].Width = 85;
            cols[4].Width = 110;
            cols[5].HeaderText = "Invoice Amount";
            cols[5].DefaultCellStyle.Format = "C2";
            cols[5].Width = 110;
            cols[6].HeaderText = "Paid";
            cols[6].Width = 100;
            cols[7].HeaderText = "Payment Type";
            cols[7].Width = 160;
            cols[8].HeaderText = "Date Paid";
            cols[8].Width = 130;
        }
        private void InvoicesDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            List<InvoiceModel> invoices = (List<InvoiceModel>)senderGrid.DataSource;
            selectedInvoice = invoices[e.RowIndex];

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                InvoiceNumberValue.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                InvoiceDatePicker.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                InvoiceAmountValue.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                if (InvoicesDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString() == "True")
                {
                    InvoiceStatusValue.SelectedIndex = 1;
                    lblPaymentType.Visible = true;
                    lblDatePaid.Visible = true;
                    txtPaymentType.Visible = true;
                    dtpPaidDate.Visible = true;
                    txtPaymentType.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
                    dtpPaidDate.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[8].Value.ToString();
                }
                else if (InvoicesDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString() == "False")
                {
                    InvoiceStatusValue.SelectedIndex = 0;
                    dtpPaidDate.Text = "";
                    txtPaymentType.Text = "";
                }
                CreateInvoiceBtn.Text = "Update Invoice";
                CreateInvoiceBtn.Size = new Size(212, 68);
            }
        }
        private void PopulateAccountStats(int accountId)
        {
            ChargeAccountStatModel accountStats = GlobalConfig.Connections.GetChargeAccountStat(accountId);
            TotalOwedLabel.Text = accountStats.TotalOwed.ToString("C2");
            MonthlySalesLabel.Text = accountStats.MonthlySales.ToString("C2");
            YTDSalesLabel.Text = accountStats.YTDSales.ToString("C2");
            AvgDayLabel.Text = accountStats.AverageDays.ToString();
        }
        private bool InvoiceStatus(string status)
        {
            if(status == "Paid")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ClearInvoiceFields()
        {
            InvoiceNumberValue.Text = "";
            InvoiceAmountValue.Text = "";
            InvoiceDatePicker.ResetText();
            InvoiceStatusValue.SelectedIndex = -1;
            CreateInvoiceBtn.Text = "Create Invoice";
        }

        #endregion

        private void InvoiceStatusValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(InvoiceStatusValue.SelectedIndex == 1)
            {  
                lblPaymentType.Visible = true;
                lblDatePaid.Visible = true;
                txtPaymentType.Visible = true;
                dtpPaidDate.Text = null;
                dtpPaidDate.Visible = true;
            } else
            {
                dtpPaidDate.Text = null;
                txtPaymentType.Text = null;
                lblPaymentType.Visible = false;
                lblDatePaid.Visible = false;
                txtPaymentType.Visible = false;
                dtpPaidDate.Visible = false;
            }
        }

        private void monthlyInvoices_Click(object sender, EventArgs e)
        {
            //TODO: LOOP AND PRINT EVERY DUE INVOICE
            MessageBox.Show("Pretending like I'm printing every account's invoices");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //TODO: File Dialog for DB Change
            if(keyData == (Keys.Alt | Keys.D))
            {
                MessageBox.Show("Database edit mode.");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InvoiceNumberValue_TextChanged(object sender, EventArgs e)
        {
            if (InvoiceNumberValue.Text == "")
            {
                InvoiceStatusLabel.Visible = false;
                InvoiceStatusValue.Visible = false;
            }
            else
            {
                InvoiceStatusLabel.Visible = true;
                InvoiceStatusValue.Visible = true;
            }
        }
        private void DeleteInvoiceBtn_Click(object sender, EventArgs e)
        {
            GlobalConfig.Connections.DeleteInvoice(selectedInvoice.InvoiceId);
            ClearInvoiceFields();
            FillDataGrid();
            PopulateAccountStats(selectedAccount.Id);
        }
    }
}

