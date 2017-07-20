using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication12
{
    public partial class AddPerson : Form
    {
        public AddPerson()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
            && !char.IsSeparator(e.KeyChar);
        }

        private void tbCode_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void tbCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
           && !char.IsSeparator(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (((tbName.Text.Trim() == "") || (tbSurname.Text.Trim() == "")) && (tbfirmname.Text.Trim() == ""))
            {
                MessageBox.Show("Tam ad giriniz ve ya Firma adını giriniz", "Bilgilendirme Mesajı");

            }
            else
            {
                SqlClass sqlConn = new SqlClass();
                sqlConn.AddPerson(tbName.Text, tbSurname.Text, tbfirmname.Text, tbTelNum.Text, tbadres.Text);

                
                tbName.Text = " ";
                tbSurname.Text = " ";
                tbfirmname.Text = " ";
                tbTelNum.Text = " ";
                tbadres.Text = " ";
                label2.Visible = true;
            }
        }

        private void AddPerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            //AddForm Addfrm = new AddForm();
            //Addfrm.FullCombobax();
        }

        private void tbTelNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
                label2.Visible = false;
        }

        private void tbfirmname_TextChanged(object sender, EventArgs e)
        {
                label2.Visible = false;   
        }

        private void AddPerson_Load(object sender, EventArgs e)
        {

        }

    }
}
