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
    public partial class frmCustomDetailList : Form
    {
        String bag;
        MySqlConnection baglanti;

        string cl_id;

        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

        DataTable dt = new DataTable();
        DataSet dataSet;
        MySqlDataAdapter adapter;
        MySqlCommand command = new MySqlCommand();
        MySqlCommandBuilder cmdb;
        public frmCustomDetailList()
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

        private void frmCustomDetailList_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cl_id) > 0)
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                String sqlName = "SELECT CONCAT(CL.NAME, ' ', CL.SURNAME) AS PNAME FROM customer_list CL where CL.ID = " + Convert.ToInt32(cl_id);
                MySqlCommand komutName = new MySqlCommand(sqlName, baglanti);
                lblNAME.Text = komutName.ExecuteScalar().ToString();
            }
        }
        public void ArrangeForm( string lvCL_ID){

            cl_id = lvCL_ID;
            Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string yr3, m3, d3;
            string yr, m, d, yr2, m2, d2;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

            //

            customer_list.DataSource = null;
            String sql = "";
           
            if (checkBox1.Checked == true)
            {
                 sql = "SELECT SLC.KISI_ADI, SLC.ODEMETIPI, SLC.* FROM stock_list_case SLC WHERE SLC.CL_ID = " + Convert.ToInt32(cl_id) +
                      " AND DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

            }
            else
            {
                 sql = "SELECT SLC.KISI_ADI, SLC.ODEMETIPI, SLC.* FROM stock_list_case SLC WHERE SLC.CL_ID = " + Convert.ToInt32(cl_id) + " AND DATE BETWEEN '"
                              + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";

            }

            //sqlCon.ConnectSql();
          
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt = new DataTable();
            dataSet = new DataSet();
            adapter = new MySqlDataAdapter();//MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            //baglanti.Open();
            adapter.Fill(dt);
            adapter.Fill(dataSet, "stock_list_case");
            customer_list.DataSource = dt; // dataSet.Tables[0];//
            baglanti.Close();

            customer_list.Columns[0].HeaderText = "Kişi/Firma Adı";
            customer_list.Columns[1].HeaderText = "Ödeme Tipi";
            customer_list.Columns[2].HeaderText = "detayId";
            customer_list.Columns[3].HeaderText = "CL_ID";
            customer_list.Columns[4].HeaderText = "TL";
            customer_list.Columns[4].DefaultCellStyle.Format = "N";

            customer_list.Columns[5].HeaderText = "Dolar";
            customer_list.Columns[5].DefaultCellStyle.Format = "N";
            customer_list.Columns[6].HeaderText = "Euro";
            customer_list.Columns[6].DefaultCellStyle.Format = "N";
            customer_list.Columns[7].HeaderText = "Kaç Adet?";
            customer_list.Columns[8].HeaderText = "Ödeme Tipi";
            customer_list.Columns[9].HeaderText = "Kişi/Firma Adı";
            customer_list.Columns[10].HeaderText = "Stok Kodu";
            customer_list.Columns[11].HeaderText = "Tarih";

            customer_list.Columns[2].Visible = false;
            customer_list.Columns[3].Visible = false;
            customer_list.Columns[8].Visible = false;
            customer_list.Columns[9].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            customer_list.DataSource = null;

            string yr3, m3, d3;
            string yr, m, d, yr2, m2, d2;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;


            String sql2 = "";

            if (checkBox1.Checked == true) {
                sql2 = "SELECT concat(sl.NAME,' ',concat('(',sl.CODE,')')) as SNAME, CONCAT(CL.NAME, ' ', CL.SURNAME) AS FNAME, dsc.* from daily_sale_case dsc"+
                        " JOIN stock_list sl ON DSC.SL_ID = SL.ID JOIN customer_list CL ON CL.ID = DSC.CL_ID where cl.ID = " + Convert.ToInt32(cl_id) +
                        " AND  dsc.DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

            }
            else{
                sql2 = "SELECT concat(sl.NAME,' ',concat('(',sl.CODE,')')) as SNAME, CONCAT(CL.NAME, ' ', CL.SURNAME) AS FNAME, dsc.* from daily_sale_case dsc"+
                        " JOIN stock_list sl ON DSC.SL_ID = SL.ID JOIN customer_list CL ON CL.ID = DSC.CL_ID where cl.ID = " + Convert.ToInt32(cl_id) + " AND dsc.DATE BETWEEN '"
                                + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" ;


            }
                

            DataTable dt2 = new DataTable();

            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand();

            command2.CommandText = sql2;
            command2.Connection = baglanti;
            adapter2.SelectCommand = command2;

            //baglanti.Open();
            adapter2.Fill(dt2);
            //adapter2.Fill(dataSet, "daily_sale_case");
            customer_list.DataSource = dt2;
            baglanti.Close();

            customer_list.Columns[0].HeaderText = "Stok Adı";
            customer_list.Columns[1].HeaderText = "Müşteri";
            customer_list.Columns[2].HeaderText = "ID";
            customer_list.Columns[3].HeaderText = "CL_ID";
            customer_list.Columns[4].HeaderText = "SL_ID";
            customer_list.Columns[5].HeaderText = "TL";
            customer_list.Columns[5].DefaultCellStyle.Format = "N";
            customer_list.Columns[6].HeaderText = "Dolar";
            customer_list.Columns[6].DefaultCellStyle.Format = "N";
            customer_list.Columns[7].HeaderText = "Euro";
            customer_list.Columns[7].DefaultCellStyle.Format = "N";
            customer_list.Columns[8].HeaderText = "Adet Sayısı";
            customer_list.Columns[9].HeaderText = "Ödeme Tipi";
            customer_list.Columns[10].HeaderText = "Tarih";

            customer_list.Columns[1].Visible = false;
            customer_list.Columns[2].Visible = false;
            customer_list.Columns[3].Visible = false;
            customer_list.Columns[4].Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            customer_list.DataSource = null;
            String sql3 = "";
            string yr3, m3, d3;
            string yr, m, d, yr2, m2, d2;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

       

            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            if (checkBox1.Checked == true)
            {
               sql3 = "SELECT PC.* FROM customer_list CL JOIN payments PC ON PC.CL_ID = CL.ID where cl.ID = " + Convert.ToInt32(cl_id) +
                    " AND  PC.DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";
            }
            else
            {
                sql3 = "SELECT PC.* FROM customer_list CL JOIN payments PC ON PC.CL_ID = CL.ID where cl.ID = " + Convert.ToInt32(cl_id) + " AND PC.DATE BETWEEN '"
                                + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";
            }
            
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt3 = new DataTable();

            MySqlDataAdapter adapter3 = new MySqlDataAdapter();
            MySqlCommand command3 = new MySqlCommand();

            command3.CommandText = sql3;
            command3.Connection = baglanti;
            adapter3.SelectCommand = command3;

            //baglanti.Open();
            adapter3.Fill(dt3);
           // adapter.Fill(dataSet, "payments");
            customer_list.DataSource = dt3;
            baglanti.Close();

            customer_list.Columns[0].HeaderText = "Numarası";
            customer_list.Columns[1].HeaderText = "Ödeme Şekli";
            customer_list.Columns[2].HeaderText = "Ödeme Tipi";
            customer_list.Columns[3].HeaderText = "TL";
            customer_list.Columns[3].DefaultCellStyle.Format = "N";
            customer_list.Columns[4].HeaderText = "Euro";
            customer_list.Columns[4].DefaultCellStyle.Format = "N";
            customer_list.Columns[5].HeaderText = "Dolar";
            customer_list.Columns[5].DefaultCellStyle.Format = "N";
            customer_list.Columns[6].HeaderText = "Müşteri_id";
            customer_list.Columns[7].HeaderText = "Müşteri";
            customer_list.Columns[8].HeaderText = "Tarih";

            
            customer_list.Columns[0].Visible = false;
            customer_list.Columns[6].Visible = false;
            customer_list.Columns[7].Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
