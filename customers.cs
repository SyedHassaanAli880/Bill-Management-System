using System;
using System.Collections;
using System.Windows.Forms;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class CUSTOMERS : sample_CRUD_UI
    {
        bool edit = false;

        int customerID;

        public CUSTOMERS()
        {
            InitializeComponent();
        }

        private void customers_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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

                    DataGridViewRow row = customers_dataGridView.Rows[e.RowIndex];

                    customerID = Convert.ToInt32(row.Cells[0].Value.ToString());

                    customer_name_textBox.Text = row.Cells["CustomernameGV"].Value.ToString();

                    shops_comboBox_customers.SelectedValue = Convert.ToInt32(row.Cells["ShopIDGV"].Value.ToString());

                    email_textBox_customers.Text = row.Cells["CustomeremailGV"].Value.ToString();

                    phone_textBox_customers.Text = row.Cells["CustomerphoneGV"].Value.ToString();

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

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        public void LoadCustomers()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(CustomerIDGV);

            lb.Items.Add(CustomernameGV);

            lb.Items.Add(CustomerphoneGV);

            lb.Items.Add(ShopIDGV);

            lb.Items.Add(ShopnameGV);

            lb.Items.Add(CustomeremailGV);

            SQL_TASKS.load_data("st_getCUSTOMERS", customers_dataGridView, lb);
        }

        public void enable_crud_buttons()
        {
            add_button.Enabled = true;

            delete_button.Enabled = true;

            cancel_button.Enabled = true;

            view_button.Enabled = true;

            save_button.Enabled = true;
        }
        public override void edit_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shops_comboBox_customers.SelectedIndex == -1)
                {
                    throw new Exception("Please select a record to edit.");
                }
                else
                {
                    CodingSourceClass.enable(left_panel);

                    edit = true; //editing option enabled

                    add_button.Enabled = false; //can't add

                    delete_button.Enabled = false; //can't delete

                    view_button.Enabled = false; //can't view

                    save_button.Enabled = true; //can save after editing
                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }

        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shops_comboBox_customers.SelectedIndex != -1)
                {
                    Hashtable ht = new Hashtable();

                    DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        ht.Add("@id", customerID);

                        if (SQL_TASKS.insert_update_delete("st_deleteCUSTOMERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully from the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadCustomers();
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

        public override void view_button_Click(object sender, EventArgs e)
        {
            enable_crud_buttons();

            CodingSourceClass.disable_reset(left_panel);

            LoadCustomers();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shops_comboBox_customers.SelectedIndex == -1)
                {
                    throw new Exception("Please enter/select a record.");

                }
                else
                {
                    if (edit == false) //code for save
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", customer_name_textBox.Text);

                        ht.Add("@phone", phone_textBox_customers.Text);

                        ht.Add("@shopID", Convert.ToInt32(shops_comboBox_customers.SelectedValue.ToString()));

                        ht.Add("@email", email_textBox_customers.Text);

                        if (SQL_TASKS.insert_update_delete("st_insertCUSTOMERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg(customer_name_textBox.Text + " added successfully to the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadCustomers();
                        }
                        else
                        {
                            throw new Exception("Unable to save record.");

                         
                        }
                    }
                    else if (edit == true) //code for update
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", customer_name_textBox.Text);

                        ht.Add("@phone", phone_textBox_customers.Text);

                        ht.Add("@shopID", Convert.ToInt32(shops_comboBox_customers.SelectedValue.ToString()));

                        ht.Add("@email", email_textBox_customers.Text);

                        ht.Add("@id", customerID);

                        if (SQL_TASKS.insert_update_delete("st_updateCUSTOMERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record updated successfully into the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            LoadCustomers();
                        }
                        else
                        {
                            throw new Exception("Unable to update.");
                         
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(CustomerIDGV);

                lb.Items.Add(CustomernameGV);

                lb.Items.Add(CustomerphoneGV);

                lb.Items.Add(ShopIDGV);

                lb.Items.Add(CustomeremailGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchCUSTOMERS", customers_dataGridView, lb, ht);
            }
            else
            {
                LoadCustomers();
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);
        }

        public override void cancel_button_Click(object sender, EventArgs e)
        {
            customer_name_textBox.Enabled = false; shops_comboBox_customers.Enabled = false;

            email_textBox_customers.Enabled = false; phone_textBox_customers.Enabled = false;

            customer_name_textBox.Text = ""; shops_comboBox_customers.SelectedIndex = -1;

            email_textBox_customers.Text = ""; phone_textBox_customers.Text = "";

            enable_crud_buttons();
        }

        public override void add_button_Click_1(object sender, EventArgs e)
        {
            CodingSourceClass.enable(left_panel);

            edit = false; //can't edit at the time of adding

            view_button.Enabled = false; //can't view

            delete_button.Enabled = false; //can't delete
        }

        private void customers_Load(object sender, EventArgs e)
        {
            SQL_TASKS.LoadList("st_getSHOPS", shops_comboBox_customers, "ID", "Name");

            CodingSourceClass.disable_reset(left_panel);

            enable_crud_buttons();

            LoadCustomers();
        }
    }
}
