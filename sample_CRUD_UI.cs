using System;
using MainClass;

namespace BMS
{
    public partial class sample_CRUD_UI : sample_UI
    {
        public sample_CRUD_UI()
        {
            InitializeComponent();
        }

        public virtual void edit_button_Click(object sender, EventArgs e)
        {

        }

        public virtual void delete_button_Click(object sender, EventArgs e)
        {

        }

        public virtual void view_button_Click(object sender, EventArgs e)
        {

        }

        public virtual void save_button_Click(object sender, EventArgs e)
        {

        }

        public virtual void search_textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void back_button_Click(object sender, EventArgs e)
        {
            Home_Screen hs = new Home_Screen();

            CodingSourceClass.ShowWindow(hs, MDI.ActiveForm);
        }

        public virtual void cancel_button_Click(object sender, EventArgs e)
        {

        }

        public virtual void add_button_Click_1(object sender, EventArgs e)
        {

        }
    }
}
