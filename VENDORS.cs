using System;
using System.Windows.Forms;
using System.Collections;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class VENDORS : sample_CRUD_UI
    {
        public VENDORS()
        {
            InitializeComponent();
        }

        bool edit = false;

        int vendorID;

        void LoadVendors()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(VendorIDGV);

            lb.Items.Add(NameGV);

            lb.Items.Add(PhoneGV);

            lb.Items.Add(AddressGV);

            SQL_TASKS.load_data("st_getVENDORSdata", vendors_dataGridView, lb);
        }



        private void vendors_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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

                    DataGridViewRow row = vendors_dataGridView.Rows[e.RowIndex];

                    vendorID = Convert.ToInt32(row.Cells[0].Value.ToString());

                    vendor_name_textBox.Text = row.Cells[1].Value.ToString();

                    vendor_phone_textBox.Text = row.Cells[2].Value.ToString();

                    vendor_address_textBox.Text = row.Cells[3].Value.ToString();

                    delete_button.Enabled = true;

                    edit_button.Enabled = true;

                    save_button.Enabled = false;

                    cancel_button.Enabled = true;

                    view_button.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MainClass.CodingSourceClass.ShowMsg(ex.Message, "Error");
            }

        }

            void enable_crud_buttons()
            {
                add_button.Enabled = true;

                delete_button.Enabled = true;

                cancel_button.Enabled = true;

                view_button.Enabled = true;

                save_button.Enabled = true;
            }


        public override void edit_button_Click(object sender, EventArgs e)
        {
            if (vendor_name_textBox.Text == "" || vendor_phone_textBox.Text == "" || vendor_address_textBox.Text == "")
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

        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (vendor_name_textBox.Text != "" && vendor_phone_textBox.Text != "" && vendor_address_textBox.Text != "")
                {
                    Hashtable ht = new Hashtable();

                    DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        ht.Add("@id", vendorID);

                        if (SQL_TASKS.insert_update_delete("st_deleteVENDORS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully from the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadVendors();
                        }
                        else
                        { throw new Exception("An error occured."); }
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

            LoadVendors();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (vendor_name_textBox.Text == "" || vendor_phone_textBox.Text == "" || vendor_address_textBox.Text == "")
                {
                    throw new Exception("Please enter/select a record.");

                    
                }
                else
                {
                    if (edit == false) //code for save
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", vendor_name_textBox.Text);

                        ht.Add("@phone", vendor_phone_textBox.Text);

                        ht.Add("@address", vendor_address_textBox.Text);

                        if (SQL_TASKS.insert_update_delete("st_insertVENDORS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg(vendor_name_textBox.Text + " added successfully into the system.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            enable_crud_buttons();

                            LoadVendors();
                        }
                        else
                        {
                            throw new Exception("Unable to save record.");

                        }
                    }
                    else if (edit == true) //code for update
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", vendor_name_textBox.Text);

                        ht.Add("@phone", vendor_phone_textBox.Text);

                        ht.Add("@address", vendor_address_textBox.Text);

                        ht.Add("@id", vendorID);

                        if (SQL_TASKS.insert_update_delete("st_updateVENDORS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record updated successfully.", "Success");

                            CodingSourceClass.disable_reset(left_panel);

                            LoadVendors();
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

                LoadVendors();
            }
        }

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(VendorIDGV);

                lb.Items.Add(NameGV);

                lb.Items.Add(PhoneGV);

                lb.Items.Add(AddressGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchVENDORS", vendors_dataGridView, lb, ht);
            }
            else
            {
                LoadVendors();
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

        private void Vendors_Load(object sender, EventArgs e)
        {
            LoadVendors();

            CodingSourceClass.disable_reset(left_panel); enable_crud_buttons();
        }
    }
}
