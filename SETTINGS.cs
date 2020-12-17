using System;
using MainClass;
using Authentication;

namespace BMS
{
    public partial class SETTINGS : sample_UI //settings inheriting sample_UI
    {
        public SETTINGS()
        {
            InitializeComponent();
        }

        private void integrated_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            //windows authentication
            if(integrated_checkBox_settings.Checked)
            {
                username_textBox_settings.Text = "";
                password_textBox_settings.Text = "";

                username_textBox_settings.AllowDrop = true;
                password_textBox_settings.AllowDrop = true;

                username_textBox_settings.Enabled = false;
                password_textBox_settings.Enabled = false;
            }
            else //sql server authentication
            {
                username_textBox_settings.AllowDrop = false;
                password_textBox_settings.AllowDrop = false;

                username_textBox_settings.Enabled = true;
                password_textBox_settings.Enabled = true;
            }
        }
       

        private void save_settings_button_Click(object sender, EventArgs e)
        {
            try
            {
                bool windowsAuth;

                if (integrated_checkBox_settings.Checked)
                {
                    if (server_name_textBox_settings.Text != "" && database_textBox_settings.Text != "")
                    {
                        windowsAuth = true;

                        CodingSourceClass.createFile("\\bms_connect", windowsAuth, server_name_textBox_settings.Text, database_textBox_settings.Text);

                        login log = new login();

                        CodingSourceClass.ShowWindow(log, this, MDI.ActiveForm);
                    }
                    else
                    {
                        CodingSourceClass.ShowMsg("Please fill all required fields", "Error");
                    }
                }
                else
                {
                    if (server_name_textBox_settings.Text != "" && database_textBox_settings.Text != "" && username_textBox_settings.Text != "" && password_textBox_settings.Text != "")
                    {
                        windowsAuth = false;

                        bool result = CodingSourceClass.createFile("\\bms_connect", windowsAuth, server_name_textBox_settings.Text, database_textBox_settings.Text, username_textBox_settings.Text, password_textBox_settings.Text);

                        if(result == true)
                        {
                            login log = new login();

                            CodingSourceClass.ShowWindow(log, this, MDI.ActiveForm);

                            LoginCodeClass.set_logged(true);
                        }
                        
                    }
                    else
                    {
                        LoginCodeClass.set_logged(false);

                        CodingSourceClass.ShowMsg("Please fill all required fields", "Error");
                    }
                }
            }
            catch(Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");
            }
               
            
        }
    }
}
