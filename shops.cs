using System;
using System.Collections;
using System.Windows.Forms;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class SHOPS : sample_CRUD_UI
    {
        bool edit = false; int shopID;

        public SHOPS()
        {
            InitializeComponent();
        }

        private void shops_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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

                    DataGridViewRow row = shops_dataGridView.Rows[e.RowIndex];

                    shopID = Convert.ToInt32(row.Cells["ShopIDGV"].Value.ToString());

                    shop_name_textBox.Text = Convert.ToString(row.Cells["ShopnameGV"].Value.ToString());

                    vendors_comboBox_shops.SelectedValue = row.Cells["VendorIDGV"].Value;

                    shop_branch_address_textBox.Text = Convert.ToString(row.Cells["BranchnameGV"].Value.ToString());

                    managers_comboBox_shops.SelectedValue = row.Cells["ManagerIDGV"].Value;

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

                CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
            }
        }

        void LoadShops()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(ShopIDGV); //0

            lb.Items.Add(ShopnameGV); //1

            lb.Items.Add(VendorIDGV); //2

            lb.Items.Add(VendorGV); //3
                
            lb.Items.Add(BranchnameGV); //4

            lb.Items.Add(ManagerIDGV); //5

            lb.Items.Add(ManagerGV); //6

            SQL_TASKS.load_data("st_getSHOPS", shops_dataGridView, lb); 
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
            if (shop_name_textBox.Text == "" || vendors_comboBox_shops.SelectedIndex == -1 || shop_branch_address_textBox.Text == "" || managers_comboBox_shops.SelectedIndex == -1)
            {
                CodingSourceClass.ShowMsg("Please select a record to edit.", "Error");
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

        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shop_name_textBox.Text != "" || vendors_comboBox_shops.SelectedIndex != -1 || shop_branch_address_textBox.Text != "" || managers_comboBox_shops.SelectedIndex != -1)
                {
                    Hashtable ht = new Hashtable();

                    DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        ht.Add("@id", shopID);

                        if (SQL_TASKS.insert_update_delete("st_deleteSHOPS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully from the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadShops();
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

                CodingSourceClass.disable_reset(left_panel);

                enable_crud_buttons();
            }
        }

        public override void view_button_Click(object sender, EventArgs e)
        {
            LoadShops();
        }

        

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (shop_name_textBox.Text == "" || vendors_comboBox_shops.SelectedIndex == -1 || shop_branch_address_textBox.Text == "" || managers_comboBox_shops.SelectedIndex == -1)
                {
                    throw new Exception("Please enter/select a record.");

                }
                else
                {
                    if (edit == false) //code for save
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", shop_name_textBox.Text);

                        ht.Add("@shopvendorID", Convert.ToInt32(vendors_comboBox_shops.SelectedValue.ToString()));

                        ht.Add("@branchaddress", shop_branch_address_textBox.Text);

                        ht.Add("@managerID", Convert.ToInt32(managers_comboBox_shops.SelectedValue.ToString()));
                        
                        if (SQL_TASKS.insert_update_delete("st_insertSHOPS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg(shop_name_textBox.Text + " added successfully into the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadShops();
                        }
                        else
                        {throw new Exception("Unable to save record.");}
                    }
                    else if (edit == true) //code for update
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", shop_name_textBox.Text);

                        ht.Add("@shopvendorID", Convert.ToInt32(vendors_comboBox_shops.SelectedValue.ToString()));

                        ht.Add("@branchaddress", shop_branch_address_textBox.Text);

                        ht.Add("@managerID", Convert.ToInt32(managers_comboBox_shops.SelectedValue.ToString()));

                        ht.Add("@shopID", shopID);

                        if (SQL_TASKS.insert_update_delete("st_updateSHOPS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg(shop_name_textBox.Text + " updated successfully.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadShops();
                        }
                        else
                        {
                            throw new Exception("Unable to update record.");
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

                lb.Items.Add(ShopIDGV);

                lb.Items.Add(ShopnameGV);

                lb.Items.Add(VendorIDGV);

                lb.Items.Add(VendorGV);

                lb.Items.Add(BranchnameGV);

                lb.Items.Add(ManagerIDGV);

                lb.Items.Add(ManagerGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchSHOPS", shops_dataGridView, lb, ht);
            }
            else
            {
                LoadShops();
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);
        }

        public override void cancel_button_Click(object sender, EventArgs e)
        {
            CodingSourceClass.disable_reset(left_panel);

            enable_crud_buttons();
        }

        public override void add_button_Click_1(object sender, EventArgs e)
        {
            CodingSourceClass.enable_reset(left_panel);
        }

        private void shops_Load(object sender, EventArgs e)
        {
            SQL_TASKS.LoadList("st_getVENDORSdata", vendors_comboBox_shops, "ID", "Vendor Name");

            SQL_TASKS.LoadList("st_getUSERSwrtMANAGERS", managers_comboBox_shops, "ID","Name");

            CodingSourceClass.enable_reset(left_panel);

            enable_crud_buttons();

            LoadShops();
        }

        
    }
}
