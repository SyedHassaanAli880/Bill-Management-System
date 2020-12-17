using System;
using System.Collections;
using System.Data;
using System.Transactions;
using System.Windows.Forms;
using DATA_BASE_OPERATIONS;
using MainClass;
using Authentication;

namespace BMS
{
    public partial class SALES_RETURN : sample_CRUD_UI
    {
        public SALES_RETURN()
        {
            InitializeComponent();
        }
        void enable_crud_buttons()
        {
            add_button.Enabled = true; save_button.Enabled = true;

            cancel_button.Enabled = true; view_button.Enabled = true;
        }

        public override void delete_button_Click(object sender, EventArgs e)
        {

        }

        public void LoadSalesReturn()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(salesreturnIDGV);

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

            Hashtable ht = new Hashtable();

            ht.Add("@data", search_textBox.Text);

            SQL_TASKS.load_data("st_searchSALESRETURN", salesreturn_dataGridView, lb, ht);
        }
        public override void view_button_Click(object sender, EventArgs e)
        {
            LoadSalesReturn();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shop_comboBox_salesreturn.SelectedIndex == -1 || customer_comboBox_salesreturn.SelectedIndex == -1 || sales_invoice_comboBox_salesreturn.SelectedIndex == -1 || total_amount_textBox_salesreturn.Text == "" || amount_paid_textBox_salesreturn.Text == "" || remaining_amount_textBox_salesreturn.Text == "" || amount_to_return_textBox_salesreturn.Text == "" || dateTimePicker_salesreturn.Text == "")
                {
                    throw new Exception("Please enter/select a record.");

                   
                }
                else
                {
                   double debit = 0, credit = 0, bal = 0;

                      using (TransactionScope sc = new TransactionScope())
                      {
                                Hashtable ht = new Hashtable();

                                ht.Add("@salesinvoiceID", Convert.ToInt32(sales_invoice_comboBox_salesreturn.SelectedValue.ToString()));

                                ht.Add("@amount", Convert.ToInt32(amount_to_return_textBox_salesreturn.Text));

                                ht.Add("@date", dateTimePicker_salesreturn.Value);

                                if (LoginCodeClass.isAdmin)
                                {
                                    throw new Exception("Admin cannot perfom current task.");
                                }
                                else
                                { ht.Add("@userID", LoginCodeClass.USERID); }

                                if (SQL_TASKS.insert_update_delete("st_insertSALESRETURN", ht) > 0)
                                {

                                    Hashtable loadht = new Hashtable();

                                    loadht.Add("@custID", Convert.ToInt32(customer_comboBox_salesreturn.SelectedValue.ToString()));

                                    loadht.Add("@shopID", Convert.ToInt32(shop_comboBox_salesreturn.SelectedValue.ToString()));

                                    double[] dd = SQL_TASKS.LoadLastBalance("st_getLASTBALANCES", loadht);

                                    bal = dd[0]; //customer balance

                                    debit = 0;
                                    
                                    credit = Convert.ToDouble(amount_to_return_textBox_salesreturn.Text);

                                    bal += debit - credit;

                                    Hashtable ht2 = new Hashtable();

                                    Int64 salesinvoiceID = Convert.ToInt64(sales_invoice_comboBox_salesreturn.SelectedValue.ToString());

                                    DataRowView drvcustomer = customer_comboBox_salesreturn.SelectedItem as DataRowView;

                                    DataRowView drvshop = shop_comboBox_salesreturn.SelectedItem as DataRowView;

                                    DataRowView drvsalesinvoice = sales_invoice_comboBox_salesreturn.SelectedItem as DataRowView;

                                    ht2.Add("@custID", Convert.ToInt32(customer_comboBox_salesreturn.SelectedValue.ToString()));

                                    ht2.Add("@desc", drvcustomer[1].ToString() + " has return the goods worth Rs. "+amount_to_return_textBox_salesreturn.Text+" on bill # "+ drvsalesinvoice[1].ToString() + " to "+ drvshop[1].ToString() + " shop ");

                                    ht2.Add("@debit", debit);

                                    ht2.Add("@credit", credit);

                                    ht2.Add("@balance", bal);

                                    ht2.Add("@date", dateTimePicker_salesreturn.Value);

                                    ht2.Add("@salesinvoiceid", salesinvoiceID);

                                    SQL_TASKS.insert_update_delete("st_insertCUSTOMERLEDGER", ht2);

                                    bal = dd[1]; //shop balance

                                    debit = Convert.ToDouble(amount_to_return_textBox_salesreturn.Text);

                                    credit = 0;

                                    bal += debit - credit;

                                    Hashtable ht3 = new Hashtable();

                                    ht3.Add("@shopID", Convert.ToInt32(shop_comboBox_salesreturn.SelectedValue.ToString()));

                                    ht3.Add("@desc", drvshop[1].ToString()+" has received Rs. "+amount_to_return_textBox_salesreturn.Text+" as returned from customer "+drvcustomer[1].ToString()+" on bill # "+drvsalesinvoice[1].ToString());

                                    ht3.Add("@debit", debit);

                                    ht3.Add("@credit", credit);

                                    ht3.Add("@balance", bal);

                                    ht3.Add("@date", dateTimePicker_salesreturn.Value);

                                    ht3.Add("@salesinvoiceid", salesinvoiceID);

                                    SQL_TASKS.insert_update_delete("insertVENDORLEDGER", ht3);

                                    CodingSourceClass.ShowMsg("Record added successfully into the system.", "Success");

                                    CodingSourceClass.disable_reset(left_panel);

                                    LoadSalesReturn();

                                    sc.Complete();
                                }
                                else
                                {
                                    throw new Exception("Unable to save record.");
                                }

                      }
                        

                }

            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(salesreturnIDGV);

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

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchSALESRETURN", salesreturn_dataGridView, lb, ht);
            }
            else
            {
                LoadSalesReturn();
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

        private void SALES_RETURN_Load(object sender, EventArgs e)
        {
            try
            {
                CodingSourceClass.disable_reset(left_panel);

                edit_button.Enabled = false; delete_button.Enabled = false;

                LoadSalesReturn();

                SQL_TASKS.LoadList("st_getSHOPS", shop_comboBox_salesreturn, "ID", "Name");

                base.delete_button.Text = "CANNOT DELETE"; base.delete_button.Enabled = false;

                base.edit_button.Text = "CANNOT EDIT"; base.edit_button.Enabled = false;
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        private void customer_comboBox_salesreturn_Enter_1(object sender, EventArgs e)
        {
            if (shop_comboBox_salesreturn.SelectedIndex != -1)
            {
                customer_comboBox_salesreturn.DataSource = null;

                SQL_TASKS.LoadList("st_getCUSTOMERSwrtSHOPS", customer_comboBox_salesreturn, "Customer ID", "Customer Name", "@id", Convert.ToInt32(shop_comboBox_salesreturn.SelectedValue));
            }
        }
        private void sales_invoice_comboBox_salesreturn_Enter(object sender, EventArgs e)
        {
            if (customer_comboBox_salesreturn.SelectedIndex != -1)
            {
                sales_invoice_comboBox_salesreturn.DataSource = null;

                SQL_TASKS.LoadList("st_getsSALESINVOICEwrtCUSTOMER", sales_invoice_comboBox_salesreturn, "ID", "BillNo", "@id", Convert.ToInt32(customer_comboBox_salesreturn.SelectedValue));
            }
        }

        private void sales_invoice_comboBox_salesreturn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (sales_invoice_comboBox_salesreturn.SelectedIndex != -1)
                {

                    Hashtable ht = new Hashtable();

                    ht.Add("@custID", Convert.ToInt32(customer_comboBox_salesreturn.SelectedValue.ToString()));

                    ht.Add("@salesinvoiceID", Convert.ToInt64(sales_invoice_comboBox_salesreturn.SelectedValue.ToString()));

                    double[] arr = SQL_TASKS.SaleInvoiceAmounts("st_getSALESINVOICEAMOUNT", ht);

                    total_amount_textBox_salesreturn.Text = arr[0].ToString();

                    amount_paid_textBox_salesreturn.Text = arr[1].ToString();

                    remaining_amount_textBox_salesreturn.Text = arr[2].ToString();

                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message,"Error");

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }
    }
}
