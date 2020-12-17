using System;
using System.Collections;
using System.Windows.Forms;
using DATA_BASE_OPERATIONS;
using System.Transactions;
using Authentication;
using MainClass;

namespace BMS
{
    public partial class SALES_INVOICE : sample_CRUD_UI
    {
        bool edit = false;

        int salesinvoiceID,shopID,customerID,userID;

        public SALES_INVOICE()
        {
            InitializeComponent();
        }

        private void salesinvoice_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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

                    DataGridViewRow row = salesinvoice_dataGridView.Rows[e.RowIndex];

                    salesinvoiceID = Convert.ToInt32(row.Cells["salesIDGV"].Value.ToString());

                    shops_comboBox_sales_invoice.Text = row.Cells["shopGV"].Value.ToString();

                    shopID = Convert.ToInt32(row.Cells["shopIDGV"].Value);

                    customers_comboBox_sales_invoice.Text = row.Cells["customernameGV"].Value.ToString();

                    customerID = Convert.ToInt32(row.Cells["customerIDGV"].Value);

                    order_details_textBox.Text = row.Cells["detailsGV"].Value.ToString();

                    amount_textBox_sales_invoice.Text = row.Cells["amountGV"].Value.ToString();

                    string typ = row.Cells["typeGV"].Value.ToString();

                    if (typ == "Cash")
                    {
                        cash_radioButton_sales_invoice.Checked = true;
                    }
                    else
                    {
                        credit_card_radioButton_sales_invoice.Checked = true;
                    }

                    userID = Convert.ToInt32(row.Cells["userIDGV"].Value);

                    bill_no_textBox_salesinvoice.Text = row.Cells["billnoGV"].Value.ToString();

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

                enable_crud_buttons(); CodingSourceClass.disable_reset(left_panel);
            }
        }

        public void LoadSalesInvoice()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(salesIDGV);

            lb.Items.Add(shopIDGV);

            lb.Items.Add(customerIDGV);

            lb.Items.Add(detailsGV);

            lb.Items.Add(amountGV);

            lb.Items.Add(typeGV);

            lb.Items.Add(userIDGV);

            lb.Items.Add(billnoGV);

            lb.Items.Add(statusGV);

            lb.Items.Add(customernameGV);

            lb.Items.Add(shopGV);

            lb.Items.Add(userGV);

            SQL_TASKS.load_data("st_getSALESINVOICE", salesinvoice_dataGridView, lb);
        }

        public void enable_crud_buttons()
        {
            add_button.Enabled = true;

            delete_button.Enabled = true;

            cancel_button.Enabled = true;

            view_button.Enabled = true;

            save_button.Enabled = true;
        }

        private void SALES_INVOICE_Load(object sender, EventArgs e)
        {
            try
            {
                base.edit_button.Enabled = false;

                base.edit_button.Text = "CANNOT EDIT";

                LoadSalesInvoice();

                SQL_TASKS.LoadList("st_getSHOPS", shops_comboBox_sales_invoice, "ID", "Name");

                CodingSourceClass.disable_reset(left_panel);
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");
            }
        }

        private void customers_comboBox_sales_invoice_Enter(object sender, EventArgs e)
        {
            if(shops_comboBox_sales_invoice.SelectedIndex != -1)
            {
                customers_comboBox_sales_invoice.DataSource = null;

                SQL_TASKS.LoadList("st_getCUSTOMERSwrtSHOPS", customers_comboBox_sales_invoice, "Customer ID", "Customer Name","@id",Convert.ToInt32(shops_comboBox_sales_invoice.SelectedValue));
            }
            
        }

        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shops_comboBox_sales_invoice.SelectedIndex == -1 || order_details_textBox.Text == "" || amount_textBox_sales_invoice.Text == "")
                {
                    throw new Exception("Please enter/select a record.");
                }
                else
                {
                    
                    DialogResult dr = MessageBox.Show("SaleInvoice " +bill_no_textBox_salesinvoice.Text+ "is associated with Payments,Sales Return and Vendor/Owner Ledger.\nDeleting this invoice will delete the particular data related to that invoice from Payments,Sales Return and Vendor/Owner Ledger.\nAlso particular data of Customer Leder which is associated with Payments will be deleted.\nWe recommend you to not delete this.\nAre you sure you want to delete?","!!!WARNING!!!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                    
                    if (DialogResult.Yes == dr)
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@salesinvoiceID", salesinvoiceID);

                        if (SQL_TASKS.insert_update_delete("st_deleteSALESINVOICE", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully.", "Success");

                            CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();

                            LoadSalesInvoice();
                        }
                        else
                        {
                            throw new Exception("Unabled to delete record.");
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message,"Error");

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        public override void view_button_Click(object sender, EventArgs e)
        {
            LoadSalesInvoice();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shops_comboBox_sales_invoice.SelectedIndex == -1 || customers_comboBox_sales_invoice.SelectedIndex == -1 || order_details_textBox.Text == "" || dateTimePicker_sales_invoice.Text == "" || amount_textBox_sales_invoice.Text == "" || (cash_radioButton_sales_invoice.Checked == false && credit_card_radioButton_sales_invoice.Checked == false))
                {
                    throw new Exception("Please enter/select a record.");
                }
                else
                {
                    //0 is for crash and 1 is for debit 

                    Int16? type = cash_radioButton_sales_invoice.Checked ? Convert.ToInt16(0) : Convert.ToInt16(1);

                    if (type != null)
                    {
                        double debit = 0, credit = 0, bal = 0;

                        if (edit == false) //code for save
                        {
                            using (TransactionScope sc = new TransactionScope())
                            {
                                Hashtable ht = new Hashtable();

                                ht.Add("@shopID", Convert.ToInt32(shops_comboBox_sales_invoice.SelectedValue.ToString()));

                                ht.Add("@customerID", Convert.ToInt32(customers_comboBox_sales_invoice.SelectedValue.ToString()));

                                ht.Add("@details", order_details_textBox.Text);

                                ht.Add("@amount", amount_textBox_sales_invoice.Text);

                                ht.Add("@type", type);

                                if (LoginCodeClass.isAdmin)
                                {
                                    throw new Exception("Admin cannot make salesinvoice.");
                                }
                                else
                                { ht.Add("@userID", LoginCodeClass.USERID); }

                                ht.Add("@billno", bill_no_textBox_salesinvoice.Text);

                                ht.Add("@status",0); 

                                //0 = not paid, 1 = partially paid, 2 = completely paid

                                if (SQL_TASKS.insert_update_delete("st_insertSALESINVOICE", ht) > 0)
                                {

                                    Hashtable loadht = new Hashtable();

                                    loadht.Add("@custID", Convert.ToInt32(customers_comboBox_sales_invoice.SelectedValue.ToString()));

                                    loadht.Add("@shopID", Convert.ToInt32(shops_comboBox_sales_invoice.SelectedValue.ToString()));

                                    double[] dd = SQL_TASKS.LoadLastBalance("st_getLASTBALANCES", loadht);

                                    bal = dd[0]; //customer balance

                                    debit = Convert.ToDouble(amount_textBox_sales_invoice.Text);

                                    credit = 0;

                                    bal += debit - credit;

                                    Hashtable ht2 = new Hashtable();

                                    ht2.Add("@custID", Convert.ToInt32(customers_comboBox_sales_invoice.SelectedValue.ToString()));

                                    ht2.Add("@desc", order_details_textBox.Text);

                                    ht2.Add("@debit", debit);

                                    ht2.Add("@credit", credit);

                                    ht2.Add("@balance", bal);

                                    ht2.Add("@date", dateTimePicker_sales_invoice.Value);

                                    ht2.Add("@salesinvoiceid", salesinvoiceID);

                                    SQL_TASKS.insert_update_delete("st_insertCUSTOMERLEDGER", ht2);

                                    bal = dd[1]; //shop balance

                                    debit = 0;

                                    credit = Convert.ToDouble(amount_textBox_sales_invoice.Text);

                                    bal += debit - credit;

                                    Hashtable ht3 = new Hashtable();

                                    ht3.Add("@shopID", Convert.ToInt32(shops_comboBox_sales_invoice.SelectedValue.ToString()));

                                    ht3.Add("@desc", order_details_textBox.Text);

                                    ht3.Add("@debit", debit);

                                    ht3.Add("@credit", credit);

                                    ht3.Add("@balance", bal);

                                    ht3.Add("@date", dateTimePicker_sales_invoice.Value);

                                    ht3.Add("@salesinvoiceid", salesinvoiceID);

                                    SQL_TASKS.insert_update_delete("insertVENDORLEDGER", ht3);

                                    CodingSourceClass.ShowMsg("Record added successfully into the system.", "Success");

                                    CodingSourceClass.disable_reset(left_panel);

                                    LoadSalesInvoice();

                                    sc.Complete();
                                }
                                else 
                                {
                                    throw new Exception("Unable to save record.");
                                }

                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Please fill all fields.");

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

                lb.Items.Add(salesIDGV);

                lb.Items.Add(customerIDGV);

                lb.Items.Add(customernameGV);

                lb.Items.Add(shopIDGV);

                lb.Items.Add(shopGV);

                lb.Items.Add(detailsGV);

                lb.Items.Add(amountGV);

                lb.Items.Add(typeGV);

                lb.Items.Add(userGV);

                lb.Items.Add(userIDGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchSALESINVOICE", salesinvoice_dataGridView, lb, ht);
            }
            else
            {
                LoadSalesInvoice();
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);
        }

        public override void cancel_button_Click(object sender, EventArgs e)
        {
            enable_crud_buttons();

            CodingSourceClass.disable_reset(left_panel);
        }

        public override void add_button_Click_1(object sender, EventArgs e)
        {
            CodingSourceClass.enable(left_panel);

            edit = false; //can't edit at the time of adding

            view_button.Enabled = false; //can't view

            delete_button.Enabled = false; //can't delete
        }

        private void shops_comboBox_sales_invoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            customers_comboBox_sales_invoice.SelectedIndex = -1;
        }

    }
}
