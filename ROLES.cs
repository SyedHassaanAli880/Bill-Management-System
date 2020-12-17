using System;
using System.Collections;
using System.Windows.Forms;
using MainClass;
using DATA_BASE_OPERATIONS;

namespace BMS
{
    public partial class ROLES : sample_CRUD_UI
    {
        bool edit = false;

        int roleID;

        public ROLES()
        {
            InitializeComponent();
        }

        private void roles_dataGridView_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    add_button.Enabled = false;

                    if (edit == true) //updating
                    {
                        save_button.Enabled = true;
                    }
                    else if (edit == false) //adding
                    {
                        save_button.Enabled = false;

                    }

                    DataGridViewRow row = roles_dataGridView.Rows[e.RowIndex];

                    roleID = Convert.ToInt32(row.Cells[0].Value.ToString());

                    roles_textBox.Text = row.Cells[1].Value.ToString();

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

        public void enable_crud_buttons()
        {
            add_button.Enabled = true;

            delete_button.Enabled = true;

            cancel_button.Enabled = true;

            view_button.Enabled = true;

            save_button.Enabled = true;
        }

        public override void cancel_button_Click(object sender, EventArgs e)
        {
            enable_crud_buttons();

            CodingSourceClass.disable_reset(left_panel);
        }
        public override void add_button_Click_1(object sender, EventArgs e)
        {
            CodingSourceClass.enable(left_panel);

            edit = false;

            view_button.Enabled = false;

            delete_button.Enabled = false;
        }

        public override void edit_button_Click(object sender, EventArgs e)
        {
            if(roles_textBox.Text == "" || roles_textBox.Enabled == true)
            {
                CodingSourceClass.ShowMsg("Please select a record to edit.", "Error");

                enable_crud_buttons();

                CodingSourceClass.disable_reset(left_panel);
            }
            else
            {
                CodingSourceClass.enable(left_panel);

                edit = true;

                add_button.Enabled = false; //can't add

                delete_button.Enabled = false; //can't delete

                view_button.Enabled = false; //can't view

                save_button.Enabled = true;
            }
            
        }

        public override void delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (roles_textBox.Text != "" && roles_textBox.Enabled == false)
                {
                        Hashtable ht = new Hashtable();

                        DialogResult dr = MessageBox.Show("Are you sure? ", "Question.....", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                                  //key ,  value
                            ht.Add("@id", roleID);

                            if (SQL_TASKS.insert_update_delete("st_deleteROLES", ht) > 0)
                            {
                                MainClass.CodingSourceClass.ShowMsg(roles_textBox.Text + " deleted successfully from the system.", "Success");

                                CodingSourceClass.disable_reset(left_panel);

                                enable_crud_buttons();

                                LoadRoles();

                                SQL_TASKS.TotalRecords("getTOTALROLES", total_roles_label);
                            }
                            else
                            {
                                CodingSourceClass.ShowMsg("Unable to delete.", "Error");

                                CodingSourceClass.disable_reset(left_panel);

                                enable_crud_buttons();
                            }
                        }
                }
                else
                {
                    CodingSourceClass.ShowMsg("Select a record to delete.", "Error");

                    enable_crud_buttons();

                    CodingSourceClass.disable_reset(left_panel);
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
            CodingSourceClass.disable_reset(left_panel);

            LoadRoles();

            CodingSourceClass.disable_reset(left_panel);

            SQL_TASKS.TotalRecords("getTOTALROLES", total_roles_label);
        }

        public override void save_button_Click(object sender, EventArgs e)
        {
            if (roles_textBox.Text == "")
            {
                CodingSourceClass.ShowMsg("Please enter/select a role.", "Error");

                enable_crud_buttons();

                CodingSourceClass.disable_reset(left_panel);
            }
            else
            {

                if (edit == false) //code for save
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@name", roles_textBox.Text);

                    if (SQL_TASKS.insert_update_delete("st_insertROLES", ht) > 0)
                    {
                        CodingSourceClass.ShowMsg(roles_textBox.Text + " added successfully to the system.", "Success");
                        
                        LoadRoles();

                        enable_crud_buttons();

                        CodingSourceClass.disable_reset(left_panel);

                        SQL_TASKS.TotalRecords("getTOTALROLES", total_roles_label);
                    }
                    else
                    {
                        CodingSourceClass.ShowMsg("Unable to save record.", "Error");

                        enable_crud_buttons();

                        CodingSourceClass.disable_reset(left_panel);
                    }
                }
                else if (edit == true) //code for update
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@name", roles_textBox.Text);

                    ht.Add("@id", roleID);

                    if (SQL_TASKS.insert_update_delete("st_updateROLES", ht) > 0)
                    {
                        CodingSourceClass.ShowMsg(roles_textBox.Text + " updated successfully.", "Success");

                        LoadRoles();

                        enable_crud_buttons();

                        CodingSourceClass.disable_reset(left_panel);
                    }
                    else
                    {
                        CodingSourceClass.ShowMsg("Unable to update record.", "Error");

                        enable_crud_buttons();

                        CodingSourceClass.disable_reset(left_panel);
                    }
                }

            }
            
        }

        public void LoadRoles()
        {
            ListBox lb = new ListBox();

            lb.Items.Add(RoleIDGV);

            lb.Items.Add(RoleGV);

            SQL_TASKS.load_data("st_getROLES", roles_dataGridView,lb);
        }

        

        public override void search_textBox_TextChanged(object sender, EventArgs e)
        {
            if (search_textBox.Text != "")
            {
                ListBox lb = new ListBox();

                lb.Items.Add(RoleIDGV);

                lb.Items.Add(RoleGV);

                Hashtable ht = new Hashtable();

                ht.Add("@data", search_textBox.Text);

                SQL_TASKS.load_data("st_searchROLES", roles_dataGridView, lb, ht);
            }
            else
            {
                LoadRoles();
            }
        }

        private void ROLES_Load(object sender, EventArgs e)
        {
            LoadRoles();

            SQL_TASKS.TotalRecords("getTOTALROLES", total_roles_label);

            CodingSourceClass.disable_reset(left_panel);

            enable_crud_buttons(); //add,delete,update.....
        }

        

        
    }
}
