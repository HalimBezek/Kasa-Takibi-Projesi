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
    public partial class frmStockList : Form
    {      String bag;
           MySqlConnection baglanti;
           
           MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
           
            
        public frmStockList()
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

        private void frmStockList_Load(object sender, EventArgs e)
        {
           /* SqlClass sqlCon = new SqlClass();
            sqlCon.ListData(dtgrdStockList, DateTime.Now,DateTime.Now);
            */
            string yr3, m3, d3;
            
            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();
            int d4 = Convert.ToInt32(d3) + 1;

            String SQLTEXT = " SELECT * FROM stock_list Where DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " +
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
            dtgrdStockList.DataSource = dt;
            baglanti.Close();

            dtgrdStockList.Columns[0].HeaderText = "Numarası";
            dtgrdStockList.Columns[1].HeaderText = "Stok Adı";
            dtgrdStockList.Columns[2].HeaderText = "Stok Kodu";
            dtgrdStockList.Columns[3].HeaderText = "Kayıt Tarihi";
         ///   dtgrdStockList.Columns[4].HeaderText = "Alan Firma/Kişi";
          //  dtgrdStockList.Columns[5].HeaderText = "Fiyatı";
         //   dtgrdStockList.Columns[6].HeaderText = "Ödeme Cinsi";
         //   dtgrdStockList.Columns[7].HeaderText = "Kayıt Tarihi";

            dtgrdStockList.Columns[0].Visible = false;
            dtgrdStockList.AllowUserToAddRows = false;
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

            String SQLTEXT = " SELECT * FROM stock_list Where DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND "+"'" + yr2 + "-" + m2 + "-" + d2 + "'";

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
            dtgrdStockList.DataSource = dt;
            baglanti.Close();

            dtgrdStockList.Columns[0].HeaderText = "Numarası";
            dtgrdStockList.Columns[1].HeaderText = "Stok Adı";
            dtgrdStockList.Columns[2].HeaderText = "Stok Kodu";
            
            dtgrdStockList.Columns[0].Visible = false;
            dtgrdStockList.AllowUserToAddRows = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string yr, m, d, yr2, m2, d2;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            String SQLTEXT = " SELECT sl.* FROM stock_list sl Where sl.NAME LIKE '" + tbName.Text + "%'" + " AND sl.DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

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
            dtgrdStockList.DataSource = dt;
            baglanti.Close();

            dtgrdStockList.Columns[0].HeaderText = "Numarası";
            dtgrdStockList.Columns[1].HeaderText = "Stok Adı";
            dtgrdStockList.Columns[2].HeaderText = "Stok Kodu";


            dtgrdStockList.Columns[0].Visible = false;
            dtgrdStockList.AllowUserToAddRows = false;
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }



        private void dtgrdStockList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            String lvSL_CODE;
            try
            {
                lvSL_CODE = dtgrdStockList.CurrentRow.Cells["CODE"].Value.ToString();
            }
            catch
            {
                lvSL_CODE = "0";
            }

            SqlClass sqlCon = new SqlClass();

            String bag;
            DataSet dSet = new DataSet();



            SqlClass connect = new SqlClass();
            connect.ConnectSql();
            String sql2 = "SELECT SLC.KISI_ADI, SLC.ODEMETIPI, SLC.* FROM stock_list_case SLC WHERE SLC.CODE =  " + "'" + lvSL_CODE + "'";

            DataTable dtable2 = new DataTable();

            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand();




            bag = build.ToString();
            baglanti = new MySqlConnection(bag);

            command2.CommandText = sql2;
            command2.Connection = baglanti;
            adapter2.SelectCommand = command2;

            baglanti.Open();
            adapter2.Fill(dtable2);
            adapter2.Fill(dSet);
            dtgrdDetail.DataSource = dtable2;
            baglanti.Close();
            // sqlCon.ListData(dtgrdStockList, DateTime.Now, DateTime.Now);

            dtgrdDetail.Columns[0].HeaderText = "Kişi/Firma Adı";
            dtgrdDetail.Columns[1].HeaderText = "Ödeme Tipi";
            dtgrdDetail.Columns[2].HeaderText = "detayId";
            dtgrdDetail.Columns[3].HeaderText = "CL_ID";
            dtgrdDetail.Columns[4].HeaderText = "TL";
            dtgrdDetail.Columns[4].DefaultCellStyle.Format = "N";
            dtgrdDetail.Columns[5].HeaderText = "Dolar";
            dtgrdDetail.Columns[5].DefaultCellStyle.Format = "N";
            dtgrdDetail.Columns[6].HeaderText = "Euro";
            dtgrdDetail.Columns[6].DefaultCellStyle.Format = "N";
            dtgrdDetail.Columns[7].HeaderText = "Ödeme Tipi";
            dtgrdDetail.Columns[8].HeaderText = "Kaç Adet?";
            dtgrdDetail.Columns[9].HeaderText = "Kişi/Firma Adı";
            dtgrdDetail.Columns[10].HeaderText = "Stok Kodu";
            dtgrdDetail.Columns[11].HeaderText = "Tarih";
            dtgrdDetail.Columns[12].HeaderText = "SL_ID";

            dtgrdDetail.Columns[1].Visible = false;

            dtgrdDetail.Columns[2].Visible = false;
            dtgrdDetail.Columns[3].Visible = false;
            dtgrdDetail.Columns[10].Visible = false;
            dtgrdDetail.Columns[9].Visible = false;
            dtgrdDetail.Columns[12].Visible = false;
            dtgrdDetail.AllowUserToAddRows = false;

        }


    }
}
