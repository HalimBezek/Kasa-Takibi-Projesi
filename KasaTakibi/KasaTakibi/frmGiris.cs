using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.fonts;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace WindowsFormsApplication12
{
    public partial class frmGiris : Form
    {
        public DataGridView lgvdtgridSalesList;
        String bag;
        MySqlConnection baglanti;
        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

        public frmGiris()
        {
            InitializeComponent();
            build.Server = "127.0.0.1";//	localhost
            build.UserID = "root";
            build.Password = "12345678";
            build.Database = "case_follow";
            build.Port = 3306;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bag = build.ToString();
            baglanti = new MySqlConnection(bag);

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
            }
            catch
            {
                MessageBox.Show("Veritabanı bağlantısı oluşturulamadı.", "Hata Mesajı");
            }
            if (baglanti.State == ConnectionState.Open)
            {
                AddForm frmAdd = new AddForm();

                frmAdd.Show();

                this.Hide();
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ae-robotic.com");
        }
    }
}
