using System;
using Authentication;
using MainClass;

namespace BMS
{
    public partial class Home_Screen : sample_UI
    {
        public Home_Screen()
        {
            InitializeComponent();
        }

        private void roles_button_Click(object sender, EventArgs e)
        {
            if (LoginCodeClass.isAdmin == true)
            {
                ROLES rl = new ROLES();

                CodingSourceClass.ShowWindow(rl, MDI.ActiveForm);
            }
            else
            {
                CodingSourceClass.ShowMsg("You do not have permission to view roles.", "Error");
            }
        }

        private void users_button_Click(object sender, EventArgs e)
        {
            if (LoginCodeClass.isAdmin == true)
            {
                USERS us = new USERS();

                CodingSourceClass.ShowWindow(us, MDI.ActiveForm);
            }
            else
            {
                CodingSourceClass.ShowMsg("You do not have permission to view users.", "Error");
            }
        }

        private void vendors_button_Click(object sender, EventArgs e)
        {
            VENDORS vs = new VENDORS();

            CodingSourceClass.ShowWindow(vs, MDI.ActiveForm);
        }

        private void shops_button_Click(object sender, EventArgs e)
        {
            SHOPS sh = new SHOPS();

            CodingSourceClass.ShowWindow(sh, MDI.ActiveForm);
        }

        private void customers_button_Click(object sender, EventArgs e)
        {
            CUSTOMERS cu = new CUSTOMERS();

            CodingSourceClass.ShowWindow(cu, MDI.ActiveForm);
        }

        private void sales_invoice_button_Click(object sender, EventArgs e)
        {
            SALES_INVOICE si = new SALES_INVOICE();

            CodingSourceClass.ShowWindow(si, MDI.ActiveForm);
        }

        private void sales_return_button_Click(object sender, EventArgs e)
        {
            SALES_RETURN sr = new SALES_RETURN();

            CodingSourceClass.ShowWindow(sr, MDI.ActiveForm);
        }

        private void payment_button_Click(object sender, EventArgs e)
        {
            PAYMENT pay = new PAYMENT();

            CodingSourceClass.ShowWindow(pay, MDI.ActiveForm);
        }

        private void balances_button_Click(object sender, EventArgs e)
        {
            BALANCE bl = new BALANCE();

            CodingSourceClass.ShowWindow(bl, MDI.ActiveForm);
        }

        private void customer_ledger_button_Click(object sender, EventArgs e)
        {
            customer_ledger cl = new customer_ledger();

            CodingSourceClass.ShowWindow(cl,MDI.ActiveForm);
        }

        private void owner_ledger_button_Click(object sender, EventArgs e)
        {
            owner_ledger ol = new owner_ledger();

            CodingSourceClass.ShowWindow(ol, MDI.ActiveForm);
        }

        private void Home_Screen_Load(object sender, EventArgs e)
        {

        }
    }
}
