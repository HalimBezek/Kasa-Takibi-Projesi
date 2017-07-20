using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class frmPaymentList : Form
    {
        String bag;
        MySqlConnection baglanti;

        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
        public frmPaymentList()
        {
           
            InitializeComponent();


            build.Server = "127.0.0.1";//	localhost
            build.UserID = "root";
            build.Password = "12345678";
            build.Database = "case_follow";
            build.Port = 3306;

            bag = build.ToString();
            baglanti = new MySqlConnection(bag);

            baglanti.Open();
        
        }

        private void frmPaymentList_Load(object sender, EventArgs e)
        {
            /*SqlClass sqlCon = new SqlClass();
            sqlCon.ListData(dtgridPaymentList, DateTime.Now, DateTime.Now);
            */
            string yr3, m3, d3;

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

            String SQLTEXT = " SELECT * FROM payments Where DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + 
                "'" + yr3 + "-" + m3 + "-" + d4 + "'";

            String sql = SQLTEXT;//"SELECT * FROM stock_list Where DATE BETWEEN @tar1 and @tar2 ";
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            //if (baglanti.State =Close )  
            //   baglanti.Open();
            adapter.Fill(dt);
            dtgridPaymentList.DataSource = dt;
            baglanti.Close();



            dtgridPaymentList.Columns[0].HeaderText = "Numarası";
            dtgridPaymentList.Columns[1].HeaderText = "Ödeme Şekli";
            dtgridPaymentList.Columns[2].HeaderText = "Ödeme Tipi";

            dtgridPaymentList.Columns[3].HeaderText = "TL";
            dtgridPaymentList.Columns[3].DefaultCellStyle.Format = "N";

            dtgridPaymentList.Columns[4].HeaderText = "Euro";
            dtgridPaymentList.Columns[4].DefaultCellStyle.Format = "N";

            dtgridPaymentList.Columns[5].HeaderText = "Dolar";
            dtgridPaymentList.Columns[5].DefaultCellStyle.Format ="N";

            dtgridPaymentList.Columns[6].HeaderText = "Müşteri_id";
            dtgridPaymentList.Columns[7].HeaderText = "Müşteri";
            dtgridPaymentList.Columns[8].HeaderText = "Tarih";

            dtgridPaymentList.Columns[0].Visible = false;
            dtgridPaymentList.Columns[6].Visible = false;
            dtgridPaymentList.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnQueryStock_Click(object sender, EventArgs e)
        {
            string yr, m, d, yr2, m2, d2;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            String SQLTEXT = " SELECT * FROM payments Where DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

            String sql = SQLTEXT;//"SELECT * FROM stock_list Where DATE BETWEEN @tar1 and @tar2 ";
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            //if (baglanti.State =Close )  
            //   baglanti.Open();
            adapter.Fill(dt);
            dtgridPaymentList.DataSource = dt;
            baglanti.Close();

            dtgridPaymentList.Columns[0].HeaderText = "Numarası";
            dtgridPaymentList.Columns[1].HeaderText = "Ödeme Şekli";
            dtgridPaymentList.Columns[2].HeaderText = "Ödeme Tipi";
            dtgridPaymentList.Columns[3].HeaderText = "TL";
            dtgridPaymentList.Columns[3].DefaultCellStyle.Format = "N";
            dtgridPaymentList.Columns[4].HeaderText = "Euro";
            dtgridPaymentList.Columns[4].DefaultCellStyle.Format = "N";
            dtgridPaymentList.Columns[5].HeaderText = "Dolar";
            dtgridPaymentList.Columns[5].DefaultCellStyle.Format = "N";
            dtgridPaymentList.Columns[6].HeaderText = "Müşteri_id";
            dtgridPaymentList.Columns[7].HeaderText = "Müşteri";
            dtgridPaymentList.Columns[8].HeaderText = "Tarih";

            dtgridPaymentList.Columns[0].Visible = false;
            dtgridPaymentList.Columns[6].Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
