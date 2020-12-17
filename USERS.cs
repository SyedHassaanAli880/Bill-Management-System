using System;
using System.Collections;
using System.Windows.Forms;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class USERS : sample_CRUD_UI
    {
        bool edit = false;

        int userID;

        public USERS()
        {
            InitializeComponent();
        }

        
        public void LoadUsers()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(UserIDGV);

            lb.Items.Add(NameGV);

            lb.Items.Add(UsernameGV);

            lb.Items.Add(PasswordGV);

            lb.Items.Add(PhoneGV);

            lb.Items.Add(AddressGV);

            lb.Items.Add(RoleIDGV);

            lb.Items.Add(RoleGV);

            SQL_TASKS.load_data("st_getUSERS", users_dataGridView, lb);
        }

        public void enable_crud_buttons()
        {
            add_button.Enabled = true;

            delete_button.Enabled = true;

            cancel_button.Enabled = true;

            view_button.Enabled = true;

            save_button.Enabled = true;
        }

        private void users_dataGridView_CellClick_1(object sender, DataGridViewCellEventArgs e)
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

                    DataGridViewRow row = users_dataGridView.Rows[e.RowIndex];

                    userID = Convert.ToInt32(row.Cells[0].Value.ToString());

                    name_textBox.Text = row.Cells[1].Value.ToString();

                    username_textBox.Text = row.Cells[2].Value.ToString();

                    password_textBox.Text = row.Cells[3].Value.ToString();

                    phone_textBox.Text = row.Cells[4].Value.ToString();

                    address_textBox.Text = row.Cells[5].Value.ToString();

                    roles_comboBox.SelectedValue = row.Cells[6].Value;

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

        public override void edit_button_Click(object sender, EventArgs e)
        {
            if(name_textBox.Text == "" || phone_textBox.Text == "" || address_textBox.Text == "" || password_textBox.Text == "" || username_textBox.Text == "" || roles_comboBox.SelectedIndex == -1)
            {
                CodingSourceClass.ShowMsg("Please select a record to edit.", "Error");

                enable_crud_buttons();

                CodingSourceClass.disable_reset(left_panel);
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
                if (name_textBox.Text != "" && phone_textBox.Text != "" && address_textBox.Text != "" && password_textBox.Text != "" && username_textBox.Text != "" && roles_comboBox.SelectedIndex != -1)
                {
                    Hashtable ht = new Hashtable();

                    DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        ht.Add("@id", userID);

                        if (SQL_TASKS.insert_update_delete("st_deleteUSERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record deleted successfully from the system.", "Success");
                            
                            enable_crud_buttons();

                            CodingSourceClass.disable_reset(left_panel);

                            LoadUsers();
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

        public override void view_button_Click(object sender, EventArgs e)
        {
            enable_crud_buttons();

            CodingSourceClass.disable_reset(left_panel);

            LoadUsers();
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (name_textBox.Text == "" || phone_textBox.Text == "" || address_textBox.Text == "" || password_textBox.Text == "" || username_textBox.Text == "" || roles_comboBox.SelectedIndex == -1)
                {
                    throw new Exception("Please enter/select a record.");
                }
                else
                {
                    

                    if (edit == false) //code for save
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", name_textBox.Text);

                        ht.Add("@username", username_textBox.Text);

                        ht.Add("@password", password_textBox.Text);

                        ht.Add("@phone", phone_textBox.Text);

                        ht.Add("@address", address_textBox.Text);
                        
                        ht.Add("@roleID", (Convert.ToInt32(roles_comboBox.SelectedValue.ToString())));

                        if (SQL_TASKS.insert_update_delete("st_insertUSERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg(name_textBox.Text + " added successfully to the system.", "Success");
                            
                            enable_crud_buttons();

                            CodingSourceClass.disable_reset(left_panel);

                            LoadUsers();
                        }
                        else
                        {
                            throw new Exception("Unable to save record.An error occured.");
                        }
                    }
                    else if (edit == true) //code for update
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@name", name_textBox.Text);

                        ht.Add("@username", username_textBox.Text);

                        ht.Add("@password", password_textBox.Text);

                        ht.Add("@phone", address_textBox.Text);

                        ht.Add("@address", phone_textBox.Text);

                        ht.Add("@roleID", Convert.ToInt32(roles_comboBox.SelectedValue.ToString()));

                        ht.Add("@id", userID);

                        if (SQL_TASKS.insert_update_delete("st_updateUSERS", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("Record updated successfully into the system.", "Success");

                            enable_crud_buttons();

                            CodingSourceClass.disable_reset(left_panel);

                            LoadUsers();
                        }
                        else
                        {
                            throw new Exception("Unable to update.An error occured");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message,"Error");

                enable_crud_buttons();

                CodingSourceClass.disable_reset(left_panel);
            }

        }

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(UserIDGV);

                lb.Items.Add(NameGV);

                lb.Items.Add(UsernameGV);

                lb.Items.Add(PasswordGV);

                lb.Items.Add(PhoneGV);

                lb.Items.Add(AddressGV);

                lb.Items.Add(RoleIDGV);

                lb.Items.Add(RoleGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchUSERS", users_dataGridView, lb, ht);
            }
            else
            {
                LoadUsers();
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
            CodingSourceClass.enable_reset(left_panel);

            edit = false; //can't edit at the time of adding

            view_button.Enabled = false; //can't view

            delete_button.Enabled = false; //can't delete
        }

        private void USERS_Load(object sender, EventArgs e)
        {
            LoadUsers();

            SQL_TASKS.LoadList("st_getROLESdata",roles_comboBox,"ID","Role");
            
            enable_crud_buttons();

            CodingSourceClass.disable_reset(left_panel);
        }

        
    }
}
