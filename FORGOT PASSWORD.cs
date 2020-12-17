using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;
using MainClass;
using System.Collections;
using DATA_BASE_OPERATIONS;

namespace BMS
{
    public partial class FORGOT_PASSWORD : Form
    {
        SqlConnection sq = new SqlConnection(CodingSourceClass.connection());

        int x;

        public FORGOT_PASSWORD()
        {
            InitializeComponent();
        }

        private void back_button_forgotPassword_Click(object sender, EventArgs e)
        {
            login log = new login();

            CodingSourceClass.ShowWindow(log, MDI.ActiveForm);
        }

        private void send_code_button_Click(object sender, EventArgs e)
        {
            if (email_address_textBox.Text == "")
            {
                CodingSourceClass.ShowMsg("Please enter email address.", "error");
            }
            else
            {
                try
                {
                    Random r = new Random();

                    x = r.Next(1, 1000000); //Generating 6-digit verification code

                    SmtpClient cl = new SmtpClient("smtp.gmail.com");

                    cl.Port = 587;

                    cl.Credentials = new System.Net.NetworkCredential("gymmanagement491@gmail.com", "syedkamranali90.");

                    cl.EnableSsl = true;

                    MailMessage mail = new MailMessage("gymmanagement491@gmail.com", email_address_textBox.Text, "VERIFICATION CODE", "Your confirmation code is " + x.ToString() + ".");

                    cl.Send(mail);

                    MessageBox.Show("A code is sent to " + email_address_textBox.Text + ".Enter code to verify it.","CODE SENT...",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    CodingSourceClass.ShowMsg(ex.Message, "error");
                }
            }
        }

        private void verify_code_button_Click(object sender, EventArgs e)
        {
            if (x.ToString() == verification_code_textBox.Text)
            {
                disabled_new_details_label.Text = "ENABLED";

                disable_control_panel.Enabled = true;
            }
            else
            {
                CodingSourceClass.ShowMsg("Incorrect code.", "error");
            }
        }

        public void clear()
        {
            email_address_textBox.Text = "";

            verification_code_textBox.Text = "";

            new_password_textBox.Text = "";

            current_username_textBox.Text = "";

            admin_radioButton.Checked = false; other_radioButton.Checked = false;

            disable_control_panel.Enabled = false; disabled_new_details_label.Text = "DISABLED";
        }

        private void save_new_details_button_Click(object sender, EventArgs e)
        {
            try
            {

                if (new_password_textBox.Text == "")
                {
                    throw new Exception("Password cannot be blank.");
                }
                else
                {
                    SqlCommand sqlcmd = new SqlCommand();

                    if (admin_radioButton.Checked) //insert new password of admin
                    {
                        Hashtable ht = new Hashtable();

                        ht.Add("@username", current_username_textBox.Text);

                        ht.Add("@password", new_password_textBox.Text);

                        if (SQL_TASKS.insert_update_delete("st_updateAdminPASSWORD", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("New Password added successfully.", "Success");

                            clear();
                        }
                        else
                        {
                            throw new Exception("An error occured during the process.");
                        }

                    }
                    else if (other_radioButton.Checked && admin_radioButton.Checked == false)
                    {
                        Hashtable ht = new Hashtable(); //insert new password of user
                        
                        ht.Add("@username", current_username_textBox.Text);

                        ht.Add("@password", new_password_textBox.Text);

                        if (SQL_TASKS.insert_update_delete("st_updateUSERPASSWORD", ht) > 0)
                        {
                            CodingSourceClass.ShowMsg("New Password added successfully.", "Success");

                            clear();
                        }
                        else
                        {
                            throw new Exception("An error occured during the process.");
                        }
                    }
                    else
                    {
                        throw new Exception("Please select if you are admin or other.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                sq.Close();
            }
        
            
        }

        private void FORGOT_PASSWORD_Load(object sender, EventArgs e)
        {
            disable_control_panel.Enabled = false;

            disabled_new_details_label.Text = "DISABLED";
        }

        private void back_button_forgotpassword_Click(object sender, EventArgs e)
        {
            login log = new login();

            CodingSourceClass.ShowWindow(log, MDI.ActiveForm);
        }
    }
}
