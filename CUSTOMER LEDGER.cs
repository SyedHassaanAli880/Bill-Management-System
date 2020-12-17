using System;
using System.Collections;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using DATA_BASE_OPERATIONS;
using MainClass;

namespace BMS
{
    public partial class customer_ledger : sample_UI
    {
        public customer_ledger()
        {
            InitializeComponent();
        }
        
        ReportDocument rd = new ReportDocument();
        private void load_button_ledger_Click(object sender, EventArgs e)
        {
            try
            {
                if (shop_comboBox_ledger.SelectedIndex == -1 || customers_comboBox_ledger.SelectedIndex == -1 || (complete_ledger_radioButton.Checked == false && range_radioButton.Checked == false))
                {
                    CodingSourceClass.ShowMsg("Please enter/select a record.", "Error");
                }
                else if(shop_comboBox_ledger.SelectedIndex != -1 && customers_comboBox_ledger.SelectedIndex != -1)
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@custID", Convert.ToInt32(customers_comboBox_ledger.SelectedValue.ToString()));

                    ht.Add("@shopID", Convert.ToInt32(shop_comboBox_ledger.SelectedValue.ToString()));

                    string path = Application.StartupPath + "\\Reports\\CustomerLedger.rpt";

                    if (range_radioButton.Checked == true && complete_ledger_radioButton.Checked == false)
                    {
                        ht.Add("@from", FROM_dateTimePicker.Value);

                        ht.Add("@to", TO_dateTimePicker.Value);

                        SQL_TASKS.LoadReport("st_getCUSTOMERLEDGERwrtRANGE", crystalReportViewer1, ht, rd, path);
                    }
                    else if(range_radioButton.Checked == false && complete_ledger_radioButton.Checked == true)
                    {
                        SQL_TASKS.LoadReport("st_getCUSTOMERLEDGER", crystalReportViewer1, ht, rd, path);
                    }
                }
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message, "Error");

                if (rd != null)
                {
                    rd.Close();
                }
            }
        }

        private void customers_comboBox_ledger_Enter(object sender, EventArgs e)
        {
            if (shop_comboBox_ledger.SelectedIndex != -1)
            {
                customers_comboBox_ledger.DataSource = null;

                SQL_TASKS.LoadList("st_getCUSTOMERSwrtSHOPS", customers_comboBox_ledger, "Customer ID", "Customer Name", "@id", Convert.ToInt32(shop_comboBox_ledger.SelectedValue));
            }
        }

        private void customer_ledger_Load(object sender, EventArgs e)
        {
            SQL_TASKS.LoadList("st_getSHOPS", shop_comboBox_ledger, "ID", "Name");
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, this, MDI.ActiveForm);
        }

        private void range_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (range_radioButton.Checked == true && complete_ledger_radioButton.Checked == false)
            {
                FROM_dateTimePicker.Visible = true; TO_dateTimePicker.Visible = true;
            }
            else if (range_radioButton.Checked == false && complete_ledger_radioButton.Checked == true)
            {
                FROM_dateTimePicker.Visible = false; TO_dateTimePicker.Visible = false;
            }
        }
    }
}
