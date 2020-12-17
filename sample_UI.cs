using System;
using System.Windows.Forms;
using Authentication;

namespace BMS
{
    public partial class sample_UI : Form
    {
        public sample_UI()
        {
            InitializeComponent();
        }

        private void sample_UI_Load(object sender, EventArgs e)
        {
            if (LoginCodeClass.isAdmin == true)
            {
                if (LoginCodeClass.usernameOFadmin == "" || LoginCodeClass.usernameOFadmin == null)
                {
                    user_label.Text = "ADMIN";
                }
                else
                {
                    user_label.Text = "ADMIN: " + LoginCodeClass.usernameOFadmin;
                }
            }
            else
            {
                if (LoginCodeClass.NAME == "" || LoginCodeClass.NAME == null)
                {
                    user_label.Text = "USER";
                }
                else
                {
                    user_label.Text = LoginCodeClass.NAME;
                }
            }
            
        }
    }
}
