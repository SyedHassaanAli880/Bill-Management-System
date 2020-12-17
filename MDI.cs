using System;
using System.IO;
using System.Windows.Forms;
using MainClass;
using Authentication;


namespace BMS
{
    public partial class MDI : Form
    {
        public MDI()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MDI_Load(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\bms_connect";
            
            if(File.Exists(path))
            {
                login lg = new login();

                CodingSourceClass.ShowWindow(lg, this);
            }
            else
            {
                SETTINGS ss = new SETTINGS();

                CodingSourceClass.ShowWindow(ss, this);
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SETTINGS set = new SETTINGS();

            CodingSourceClass.ShowWindow(set, this);
        }
        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(LoginCodeClass.get_logged() == true)
            {
                login lg = new login();

                CodingSourceClass.ShowWindow(lg, MDI.ActiveForm);
            }
            else
            {
                CodingSourceClass.ShowMsg("Already logged out!", "Error");
            }
        }

    }
}
