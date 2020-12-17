using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMS
{
    public partial class BALANCE : sample_UI
    {
        public BALANCE()
        {
            InitializeComponent();
        }

        private void back_button_balance_Click(object sender, EventArgs e)
        {
           Home_Screen log = new Home_Screen();

            MainClass.CodingSourceClass.ShowWindow(log, MDI.ActiveForm);
        }
    }
}
