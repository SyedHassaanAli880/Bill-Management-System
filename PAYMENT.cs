using System;
using System.Collections;
using System.Data;
using System.Transactions;
using System.Windows.Forms;
using Authentication;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class PAYMENT : sample_CRUD_UI
    {
        bool edit = false; int paymentID,salesinvoiceID,userID,customerID;

        string postedby;

        public PAYMENT()
        {
            InitializeComponent();
        }
        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (true)
                {
                    Hashtable ht = new Hashtable();

                    DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        ht.Add("@payid", paymentID);

                        if (SQL_TASKS.insert_update_delete("st_deletePAYMENTS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully from the system.", "Success");

                            enable_crud_buttons();

                            CodingSourceClass.disable_reset(left_panel);

                            LoadPayments();
                        }
                        else
                        {
                            CodingSourceClass.ShowMsg("Unable to delete.An error occured.", "Error");

                            enable_crud_buttons();

                            CodingSourceClass.disable_reset(left_panel);
                        }

                    }
                }
                else
                {
                    throw new Exception("Select a record to delete.");
                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }
        void enable_crud_buttons()
        {
            add_button.Enabled = true; save_button.Enabled = true;

            cancel_button.Enabled = true; view_button.Enabled = true;
        }

        public void LoadPayments()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(paymentsIDGV);

            lb.Items.Add(dateGV);

            lb.Items.Add(salesinvoiceIDGV);

            lb.Items.Add(userIDGV);

            lb.Items.Add(userGV);

            lb.Items.Add(billnoGV);

            lb.Items.Add(customernameGV);

            lb.Items.Add(customerIDGV);

            lb.Items.Add(shopIDGV);

            lb.Items.Add(shopGV);

            lb.Items.Add(amountGV);

            lb.Items.Add(ChequenoGV);

            lb.Items.Add(bankGV);

            lb.Items.Add(clearingdateGV);

            SQL_TASKS.load_data("st_getPAYMENTS", payments_dataGridView, lb);
        }
        public override void view_button_Click(object sender, EventArgs e)
        {
            LoadPayments();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shop_comboBox_payments.SelectedIndex == -1 || customer_comboBox_payments.SelectedIndex == -1 || sales_invoice_comboBox_payments.SelectedIndex == -1 || total_amount_textBox_payments.Text == "" || amount_paid_textBox_payments.Text == "" || remaining_amount_textBox_payments.Text == "" || amount_to_pay_textBox_payments.Text == "" || (cash_radioButton_sales_invoice.Checked == false && cheque_radioButton.Checked == false))
                {
                    throw new Exception("Please enter/select a record.");
                }
                else if((shop_comboBox_payments.SelectedIndex != -1 && customer_comboBox_payments.SelectedIndex != -1 && sales_invoice_comboBox_payments.SelectedIndex != -1 && total_amount_textBox_payments.Text != "" && amount_paid_textBox_payments.Text != "" && remaining_amount_textBox_payments.Text != "" && amount_to_pay_textBox_payments.Text != "" && cheque_radioButton.Checked == true) && (clearing_date_dateTimePicker.Text == "" || bank_and_branch_textBox.Text == "" || cheque_no_textBox.Text == ""))
                {
                    throw new Exception("Please enter/select a record.");

                }
                else
                {
                    if (Convert.ToDouble(amount_to_pay_textBox_payments.Text) > Convert.ToDouble(total_amount_textBox_payments.Text))
                    {
                        throw new Exception("Amount to pay cannot be greater than total amount.");
                    }
                    else
                    {
                        double debit = 0, credit = 0, bal = 0;

                        using (TransactionScope sc = new TransactionScope())
                        {
                            Hashtable ht = new Hashtable();

                            if (cash_radioButton_sales_invoice.Checked == true)
                            {
                                ht.Add("@salesinvoiceid", Convert.ToInt32(sales_invoice_comboBox_payments.SelectedValue.ToString()));

                                ht.Add("@amount", Convert.ToInt32(amount_to_pay_textBox_payments.Text));

                                ht.Add("@date", dateTimePicker_payments.Value);

                                if (LoginCodeClass.isAdmin)
                                {
                                    throw new Exception("Admin cannot enter payments.");
                                }
                                else
                                { ht.Add("@userID", LoginCodeClass.USERID); }

                                SQL_TASKS.insert_update_delete("st_insertPAYMENTSwrtCASH", ht);
                            }
                            else
                            {
                                ht.Add("@salesinvoiceid", Convert.ToInt32(sales_invoice_comboBox_payments.SelectedValue.ToString()));

                                ht.Add("@amount", Convert.ToInt32(amount_to_pay_textBox_payments.Text));

                                ht.Add("@date", dateTimePicker_payments.Value);

                                if (LoginCodeClass.isAdmin)
                                {
                                    ht.Add("@userID", LoginCodeClass.idOFadmin);
                                }
                                else
                                { ht.Add("@userID", LoginCodeClass.USERID); }

                                ht.Add("@cheque",cheque_no_textBox.Text);

                                ht.Add("@bank", bank_and_branch_textBox.Text);

                                ht.Add("@clearing", clearing_date_dateTimePicker.Value);

                                SQL_TASKS.insert_update_delete("st_insertPAYMENTSwrtCHEQUE", ht);
                            }

                                Hashtable loadht = new Hashtable();

                                loadht.Add("@custID", Convert.ToInt32(customer_comboBox_payments.SelectedValue.ToString()));

                                loadht.Add("@shopID", Convert.ToInt32(shop_comboBox_payments.SelectedValue.ToString()));

                                double[] dd = SQL_TASKS.LoadLastBalance("st_getLASTBALANCES", loadht);

                                bal = dd[0]; //customer balance

                                debit = 0;

                                credit = Convert.ToDouble(amount_to_pay_textBox_payments.Text);

                                bal += debit - credit;

                                Hashtable ht2 = new Hashtable();

                                Int64 salesinvoiceID = Convert.ToInt64(sales_invoice_comboBox_payments.SelectedValue.ToString());

                                DataRowView drvcustomer = customer_comboBox_payments.SelectedItem as DataRowView;

                                DataRowView drvshop = shop_comboBox_payments.SelectedItem as DataRowView;

                                DataRowView drvsalesinvoice = sales_invoice_comboBox_payments.SelectedItem as DataRowView;

                                ht2.Add("@custID", Convert.ToInt32(customer_comboBox_payments.SelectedValue.ToString()));

                                ht2.Add("@desc", drvcustomer[1].ToString() + " has done the payment of Rs. " + amount_to_pay_textBox_payments.Text + " on bill # " + drvsalesinvoice[1].ToString() + " to " + drvshop[1].ToString());

                                ht2.Add("@debit", debit);

                                ht2.Add("@credit", credit);

                                ht2.Add("@balance", bal);

                                ht2.Add("@date", dateTimePicker_payments.Value);

                                ht2.Add("@salesinvoiceid", salesinvoiceID);

                                SQL_TASKS.insert_update_delete("st_insertCUSTOMERLEDGER", ht2);

                                bal = dd[1]; //shop balance

                                debit = Convert.ToDouble(amount_to_pay_textBox_payments.Text);

                                credit = 0;

                                bal += debit - credit;

                                Hashtable ht3 = new Hashtable();

                                ht3.Add("@shopID", Convert.ToInt32(shop_comboBox_payments.SelectedValue.ToString()));

                                ht3.Add("@desc", drvshop[1].ToString() + " has received Rs. " + amount_to_pay_textBox_payments.Text + " as paid from customer " + drvcustomer[1].ToString() + " on bill # " + drvsalesinvoice[1].ToString());

                                ht3.Add("@debit", debit);

                                ht3.Add("@credit", credit);

                                ht3.Add("@balance", bal);

                                ht3.Add("@date", dateTimePicker_payments.Value);

                                ht3.Add("@salesinvoiceid", salesinvoiceID);

                                SQL_TASKS.insert_update_delete("insertVENDORLEDGER", ht3);

                                Hashtable ht4 = new Hashtable();

                                if (Convert.ToDouble(amount_to_pay_textBox_payments.Text) == Convert.ToDouble(total_amount_textBox_payments.Text))
                                {
                                    //complete payment

                                    ht4.Add("@status", 2);
                                }
                                else if (Convert.ToDouble(amount_to_pay_textBox_payments.Text) < Convert.ToDouble(total_amount_textBox_payments.Text))
                                {
                                    //partial payment

                                    ht4.Add("@status", 1);
                                }
                                /*else //if status == 2
                                {
                                    ht4.Add("@status", 0);
                                }*/

                                ht4.Add("@salesinvoiceid", salesinvoiceID);

                                CodingSourceClass.ShowMsg("Record added successfully into the system.", "Success");

                                CodingSourceClass.disable_reset(left_panel);

                                LoadPayments();

                                sc.Complete();
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");
            }

        }

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(paymentsIDGV); //0

                lb.Items.Add(dateGV); //1 

                lb.Items.Add(salesinvoiceIDGV); //2

                lb.Items.Add(userIDGV); //3

                lb.Items.Add(userGV);  //4  

                lb.Items.Add(billnoGV); //5

                lb.Items.Add(customernameGV); //6

                lb.Items.Add(customerIDGV); //7

                lb.Items.Add(shopIDGV); //8

                lb.Items.Add(shopGV); //9

                lb.Items.Add(amountGV); //10

                lb.Items.Add(ChequenoGV); //11

                lb.Items.Add(bankGV); //12

                lb.Items.Add(clearingdateGV); //13

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchPAYMENTS", payments_dataGridView, lb, ht);
            }
            else
            {
                LoadPayments();
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);
        }

        public override void cancel_button_Click(object sender, EventArgs e)
        {
            CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
        }

        public override void add_button_Click_1(object sender, EventArgs e)
        {
            CodingSourceClass.enable_reset(left_panel);
        }

        private void PAYMENT_Load(object sender, EventArgs e)
        {
            try
            {
                edit_button.Enabled = false;

                LoadPayments();

                SQL_TASKS.LoadList("st_getSHOPS", shop_comboBox_payments, "ID", "Name");

                cheque_no_textBox.Visible = false; cheque_no_label.Visible = false;

                bank_and_branch_label.Visible = false; bank_and_branch_textBox.Visible = false;

                clearing_date_label.Visible = false; clearing_date_dateTimePicker.Visible = false;

                base.edit_button.Text = "CANNOT EDIT"; 

                CodingSourceClass.disable_reset(left_panel);

            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        private void customer_comboBox_payments_Enter(object sender, EventArgs e)
        {
            if (shop_comboBox_payments.SelectedIndex != -1)
            {
                customer_comboBox_payments.DataSource = null;

                SQL_TASKS.LoadList("st_getCUSTOMERSwrtSHOPS", customer_comboBox_payments, "Customer ID", "Customer Name", "@id", Convert.ToInt32(shop_comboBox_payments.SelectedValue));
            }
        }

        private void sales_invoice_comboBox_payments_Enter(object sender, EventArgs e)
        {
            if (customer_comboBox_payments.SelectedIndex != -1)
            {
                sales_invoice_comboBox_payments.DataSource = null;

                SQL_TASKS.LoadList("st_getsSALESINVOICEwrtCUSTOMER", sales_invoice_comboBox_payments, "ID", "BillNo", "@id", Convert.ToInt32(customer_comboBox_payments.SelectedValue));
            }
        }

        private void sales_invoice_comboBox_payments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sales_invoice_comboBox_payments.SelectedIndex != -1)
                {

                    Hashtable ht = new Hashtable();

                    ht.Add("@custID", Convert.ToInt32(customer_comboBox_payments.SelectedValue.ToString()));

                    ht.Add("@salesinvoiceID", Convert.ToInt64(sales_invoice_comboBox_payments.SelectedValue.ToString()));

                    double[] arr = SQL_TASKS.SaleInvoiceAmounts("st_getSALESINVOICEAMOUNT", ht);

                    total_amount_textBox_payments.Text = arr[0].ToString();

                    amount_paid_textBox_payments.Text = arr[1].ToString();

                    remaining_amount_textBox_payments.Text = arr[2].ToString();

                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }

        private void cheque_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (cheque_radioButton.Checked == true)
            {
                cheque_no_textBox.Visible = true; cheque_no_label.Visible = true;

                bank_and_branch_label.Visible = true; bank_and_branch_textBox.Visible = true;

                clearing_date_label.Visible = true; clearing_date_dateTimePicker.Visible = true;

                cheque_no_textBox.Clear();

                bank_and_branch_textBox.Clear();

                clearing_date_dateTimePicker.Text = "";
            }
            else
            {
                cheque_no_textBox.Clear();

                bank_and_branch_textBox.Clear();

                clearing_date_dateTimePicker.Text = "";

                cheque_no_textBox.Visible = false; cheque_no_label.Visible = false;

                bank_and_branch_label.Visible = false; bank_and_branch_textBox.Visible = false;

                clearing_date_label.Visible = false; clearing_date_dateTimePicker.Visible = false;
            }
        }

        private void payments_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    add_button.Enabled = false;

                    if (edit == true) //updating member
                    {
                        save_button.Enabled = true;
                    }
                    else if (edit == false) //adding member
                    {
                        save_button.Enabled = false;

                    }

                    DataGridViewRow row = payments_dataGridView.Rows[e.RowIndex];

                    paymentID = Convert.ToInt32(row.Cells[0].Value.ToString());

                    customer_comboBox_payments.Text = row.Cells["customernameGV"].Value.ToString();

                    sales_invoice_comboBox_payments.Text = row.Cells["billnoGV"].Value.ToString();

                    shop_comboBox_payments.Text = row.Cells["shopGV"].Value.ToString();

                    customerID = Convert.ToInt32(row.Cells["customerIDGV"].Value);

                    salesinvoiceID = Convert.ToInt32(row.Cells["salesinvoiceIDGV"].Value);

                    if (/*sales_invoice_comboBox_payments.SelectedIndex != -1*/true)
                    {

                        Hashtable ht = new Hashtable();

                        ht.Add("@custID", customerID);

                        ht.Add("@salesinvoiceID", salesinvoiceID);

                        double[] arr = SQL_TASKS.SaleInvoiceAmounts("st_getSALESINVOICEAMOUNT", ht);

                        total_amount_textBox_payments.Text = arr[0].ToString();

                        amount_paid_textBox_payments.Text = arr[1].ToString();

                        remaining_amount_textBox_payments.Text = arr[2].ToString();

                    }

                    dateTimePicker_payments.Value = Convert.ToDateTime(row.Cells[1].Value);

                    userID = Convert.ToInt32(row.Cells[3].Value.ToString());

                    postedby = row.Cells[4].Value.ToString();

                    amount_to_pay_textBox_payments.Text = row.Cells[10].Value.ToString();

                    cheque_no_textBox.Text = row.Cells[11].Value.ToString();

                    bank_and_branch_textBox.Text = row.Cells[12].Value.ToString();

                    clearing_date_dateTimePicker.Text = row.Cells["clearingdateGV"].Value.ToString();

                    if (cheque_no_textBox.Text == "" && bank_and_branch_textBox.Text == "")
                    {
                        cash_radioButton_sales_invoice.Checked = true;
                    }
                    else
                    {
                        cheque_radioButton.Checked = true;
                    }

                    CodingSourceClass.disable(left_panel);

                    delete_button.Enabled = true;

                    edit_button.Enabled = true;

                    save_button.Enabled = false;

                    cancel_button.Enabled = true;

                    view_button.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                enable_crud_buttons();

                CodingSourceClass.disable_reset(left_panel);
            }
        }
    }
}
