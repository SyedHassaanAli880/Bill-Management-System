using System;
using System.Collections;
using System.Data.SqlClient;
using Authentication;
using MainClass;

namespace BMS
{
    public partial class login : sample_UI
    {
        SqlConnection con = new SqlConnection(CodingSourceClass.connection());

        SqlCommand cmd = new SqlCommand();

        public login()
        {
            InitializeComponent();
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (username_textBox.Text != "" && password_textBox.Text != "")
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@username", username_textBox.Text);

                    ht.Add("@password", password_textBox.Text);

                    if (other_radioButton.Checked == true)
                    {
                        if (LoginCodeClass.getlogindetails("st_getAuthenticationDetails", ht, false))
                        {
                            LoginCodeClass.isAdmin = false;

                            Home_Screen hs = new Home_Screen();

                            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);

                            LoginCodeClass.set_logged(true);
                        }
                        else { throw new Exception("No user with username " + username_textBox.Text + " found."); }


                    }
                    else if (admin_radioButton.Checked == true)
                    {
                        if (LoginCodeClass.getlogindetails("st_getADMINDetails", ht, true))
                        {
                            LoginCodeClass.isAdmin = true;

                            Home_Screen hs = new Home_Screen();

                            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);

                            LoginCodeClass.set_logged(true);
                        }
                        else { throw new Exception("Admin not found with username " + username_textBox.Text); }
                    }
                    else
                    {
                        throw new Exception("Please select wether you are admin or other.");
                    }
                }
                else
                { CodingSourceClass.ShowMsg("Please enter username and password.","Error"); LoginCodeClass.set_logged(false); }
                
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                LoginCodeClass.set_logged(false);
            }

        }
        private void change_password_button_login_Click(object sender, EventArgs e)
        {
            CHANGE_PASSWORD cp = new CHANGE_PASSWORD();

            CodingSourceClass.ShowWindow(cp, MDI.ActiveForm);
        }

        private void forgot_password_button_login_Click(object sender, EventArgs e)
        {
            FORGOT_PASSWORD fp = new FORGOT_PASSWORD();

            CodingSourceClass.ShowWindow(fp, MDI.ActiveForm);
        }
    }
}
