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

namespace FaithFeedChargeAccountUI
{

    public partial class ChargeAccountUI : MaterialForm
    {
        private List<ChargeAccountModel> ChargeAccountList = new List<ChargeAccountModel>();
        public ChargeAccountUI()
        {   
            InitializeComponent();
            WireUpDropdown();
        }
        #region Event Handlers
        /// <summary>
        /// Populate the Invoice Fields to the right of the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoicesDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            
            if(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                InvoiceNumberValue.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                InvoiceDatePicker.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                InvoiceAmountValue.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPaymentType.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                dtpPaidDate.Text = InvoicesDataGrid.Rows[e.RowIndex].Cells[6].Value.ToString();
                if (InvoicesDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString() == "True")
                {
                    InvoiceStatusValue.SelectedIndex = 1;
                    lblPaymentType.Visible = true;
                    lblDatePaid.Visible = true;
                    txtPaymentType.Visible = true;
                    dtpPaidDate.Visible = true;
                }
                else if(InvoicesDataGrid.Rows[e.RowIndex].Cells[4].Value.ToString() == "False")
                {
                    InvoiceStatusValue.SelectedIndex = 0;
                    dtpPaidDate.Text = null;
                    txtPaymentType.Text = null;
                }
                CreateInvoiceBtn.Text = "Update Invoice";
                var select = "SELECT InvoiceId from dbo.Invoices WHERE InvoiceNumber =" + Int32.Parse(InvoiceNumberValue.Text) + "";
                using (SqlConnection conn = new SqlConnection(GlobalConfig.CnnString("ChargeAccountInvoices")))
                {
                    SqlCommand cmd = new SqlCommand(select, conn);
                    try
                    {
                        conn.Open();
                        selectedInvoice = (int)cmd.ExecuteScalar();

                    } catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Making the form in the state of creating a new account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createANewChargeAccountToolStripMenuItem_Click(object sender, EventArgs e)
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
            Savebtn.Enabled = true;
        }
        //Selection changes on account name dropdown
        private void SelectedChargeAccount_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ReadOnlyFields();
            Createbtn.Visible = false;
            Savebtn.Visible = true;
            DisplayFields();
            ClearInvoiceFields();
        }
        //Creating an invoice from the selectedID. 
        public void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("You must select an account first.");
            }
            else
            {
                InvoiceUI form = new InvoiceUI(selectedId);
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
                    InvoiceModel inv = new InvoiceModel(
                    selectedId,
                    InvoiceNumberValue.Text,
                    Double.Parse(InvoiceAmountValue.Text),
                    InvoiceStatus(InvoiceStatusValue.Text),
                    InvoiceDatePicker.Text,
                    txtPaymentType.Text,
                    dtpPaidDate.Text);
                    GlobalConfig.Connections.CreateInvoice(inv);
                    FillDataGrid();
                    ClearInvoiceFields();
                    MessageBox.Show("New Invoice Created!");
                }
                else
                {
                    MessageBox.Show("Invoice information is invalid");
                }
            } else
            {
                if (ValidateInvoice())
                {
                    InvoiceModel inv = new InvoiceModel(
                    selectedInvoice,
                    selectedId,
                    InvoiceNumberValue.Text,
                    Double.Parse(InvoiceAmountValue.Text),
                    InvoiceStatus(InvoiceStatusValue.Text),
                    InvoiceDatePicker.Text,
                    dtpPaidDate.Text,
                    txtPaymentType.Text);
                    GlobalConfig.Connections.UpdateInvoice(inv);
                    FillDataGrid();
                    ClearInvoiceFields();
                    MessageBox.Show("Sucessfully Upated Invoice!");
                } else
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
                WireUpDropdown();
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
                selectedId,
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
                WireUpDropdown();
                ReadOnlyFields();
                Savebtn.Enabled = false;
                MessageBox.Show("Sucessfully edited information");
                SelectedChargeAccount.SelectedIndex = CurrentAccount;
                PopulateFields();
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
            PopulateFields();
            FillDataGrid();
        }
        public int selectedId { get; set; }
        private void PopulateFields()
        {
            
            ChargeAccountModel accname = new ChargeAccountModel(SelectedChargeAccount.Text);
            GlobalConfig.Connections.GetAccountInfo(accname);
            AccountNameValue.Text = accname.AccountName;
            FirstNameValue.Text = accname.FirstName;
            LastNameValue.Text = accname.LastName;
            AddressValue.Text = accname.Address;
            CityValue.Text = accname.City;
            StateValue.Text = accname.State;
            ZipValue.Text = accname.ZipCode;
            PhoneNumberValue.Text = accname.PhoneNumber;
            AdditionalNotesValue.Text = accname.AdditionalNotes;
            if (ChargeAccountGroupBox.Text == "Charge Account Details")
            {
                ChargeAccountGroupBox.Text += " - Id (" + accname.Id + ")";
                selectedId = accname.Id;
            } 
        }
        private void WireUpDropdown()
        {
            SelectedChargeAccount.DataSource = ChargeAccountList = GlobalConfig.Connections.GetAccountNames_All();
            SelectedChargeAccount.DisplayMember = "AccountName";
            SelectedChargeAccount.SelectedIndex = -1;
        }
        private void FillDataGrid()
        {
            var select = "SELECT InvoiceNumber AS [Invoice Number], Date, InvoiceAmount AS [Invoice Amount], IsPaid AS [Paid], PaymentType AS [Payment Type], PaidDate AS [Paid Date] FROM dbo.Invoices WHERE AccountID =" + selectedId + " ORDER BY IsPaid ASC, Date ASC";
            var c = new SqlConnection(GlobalConfig.CnnString("ChargeAccountInvoices"));
            var dataAdapter = new SqlDataAdapter(select, c);
            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            InvoicesDataGrid.ReadOnly = true;
            InvoicesDataGrid.DataSource = ds.Tables[0];
            InvoicesDataGrid.Columns[0].Width = 65;
            InvoicesDataGrid.Columns[1].Width = 85;
            InvoicesDataGrid.Columns[2].Width = 110;
            InvoicesDataGrid.Columns[3].Width = 110;
            InvoicesDataGrid.Columns[4].Width = 100;
            InvoicesDataGrid.Columns[5].Width = 160;
            InvoicesDataGrid.Columns[6].Width = 130;
            InvoicesDataGrid.Columns["Invoice Amount"].DefaultCellStyle.Format = "C";
        }
        
        private int selectedInvoice;
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
    }
}

