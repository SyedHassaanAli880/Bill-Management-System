using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DATA_BASE_OPERATIONS;
using MainClass;
using System.Security.Cryptography;
using System.Collections;

namespace BMS
{
    public partial class CHANGE_PASSWORD : Form
    {
        SqlConnection sq = new SqlConnection(CodingSourceClass.connection());

        public CHANGE_PASSWORD()
        {
            InitializeComponent();
        }

        public void clear()
        {
            old_password_textBox.Text = "";

            username_textBox.Text = "";

            new_password_textBox_change.Text = "";

            new_password_panel_change.Enabled = false;

            disabled_new_password_label.Text = "DISABLED";
        }

        private void back_button_changePassword_Click(object sender, EventArgs e)
        {
            login log = new login();

            CodingSourceClass.ShowWindow(log, MDI.ActiveForm);
        }

        string usernameGLOBAL;
        private void verify_oldpassword_button_Click(object sender, EventArgs e)
        {
            
                try
                {
                if (username_textBox.Text != "" && old_password_textBox.Text != "" && admin_radioButton.Checked == true && other_radioButton.Checked == false)
                {
                    string password;

                    sq.Open();

                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = sq;

                    SqlDataReader dr;

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@username", username_textBox.Text);

                    cmd.Parameters.AddWithValue("@password", old_password_textBox.Text);

                    cmd.CommandText = "st_getADMIN";

                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            usernameGLOBAL = (dr[0].ToString()); password = (dr[1].ToString());

                            if (password == old_password_textBox.Text && usernameGLOBAL == username_textBox.Text)
                            {
                                new_password_panel_change.Enabled = true;

                                disabled_new_password_label.Text = "ENABLED";
                            }
                            else
                            {
                                throw new Exception("Wrong username or password.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No user found with these details.");
                    }
                }
                else if (username_textBox.Text != "" && old_password_textBox.Text != "" && admin_radioButton.Checked == false && other_radioButton.Checked == true)
                {   
                    string password;

                    sq.Open();

                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = sq;

                    SqlDataReader dr;

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@username", username_textBox.Text);

                    cmd.Parameters.AddWithValue("@password", old_password_textBox.Text);

                    cmd.CommandText = "st_getUSEREMAILandPASSWORD";

                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            usernameGLOBAL = (dr[0].ToString()); password = (dr[1].ToString());

                            if (password == old_password_textBox.Text && usernameGLOBAL == username_textBox.Text)
                            {
                                new_password_panel_change.Enabled = true;

                                disabled_new_password_label.Text = "ENABLED";
                            }
                            else
                            {
                                CodingSourceClass.ShowMsg("Wrong username or password.", "Error");

                                sq.Close();

                                clear();

                                new_password_panel_change.Enabled = false;

                                disabled_new_password_label.Text = "DISABLED";
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No user found with these details.");
                    }
                }
                else { throw new Exception("Please select all details properly."); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    sq.Close();

                    clear();

                    new_password_panel_change.Enabled = false;

                    disabled_new_password_label.Text = "DISABLED";
                }
        }

        private void save_new_details_button_change_Click(object sender, EventArgs e)
        {
            try
            {
                if (new_password_textBox_change.Text != "" && admin_radioButton.Checked == true && other_radioButton.Checked == false)
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@username", usernameGLOBAL);

                    ht.Add("@password", new_password_textBox_change.Text);

                    if (SQL_TASKS.insert_update_delete("st_updateAdminPASSWORD", ht) > 0)
                    {
                        CodingSourceClass.ShowMsg("Password changed successfully.", "Success");

                        clear(); sq.Close();
                    }
                    else
                    {
                        throw new Exception("An error occured.");
                    }
                }
                else if (new_password_textBox_change.Text != "" && admin_radioButton.Checked == false && other_radioButton.Checked == true)
                {

                    Hashtable ht = new Hashtable();

                    ht.Add("@username", usernameGLOBAL);

                    ht.Add("@password",new_password_textBox_change.Text);

                    if (SQL_TASKS.insert_update_delete("st_updateUSERPASSWORD", ht) > 0)
                    {
                        CodingSourceClass.ShowMsg("Password changed successfully.", "Success");

                        clear(); sq.Close();
                    }
                    else
                    {
                        throw new Exception("An error occured.");
                    }
                }
                else
                {
                    throw new Exception("Please fill all required fields properly.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                sq.Close();
            }
        }

        private void CHANGE_PASSWORD_Load(object sender, EventArgs e)
        {
            new_password_panel_change.Enabled = false;
        }
    }
}
