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
    public partial class frmCusumerCase : Form
    {
       public String lvCL_ID;
        String bag;
        MySqlConnection baglanti;
           
        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
        //  String sql = "SELECT * FROM customer_list "; // +DataListesi;
        //if (DataGridList.Name == "dtgridSalesList")
        //    sql = sql + " ORDER BY  SALE_CODE";
        DataTable dt = new DataTable();
        DataSet dataSet;
        MySqlDataAdapter adapter;
        MySqlCommand command = new MySqlCommand();
        MySqlCommandBuilder cmdb ;
        
           
        public frmCusumerCase()
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


        private void frmCusumerCase_Load(object sender, EventArgs e)
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
            
            SqlClass sqlCon = new SqlClass();
            //sqlCon.ListData(customer_list, DateTime.Now, DateTime.Now);

            sqlCon.ConnectSql();
            String sql = "SELECT CL.*, CL.ID AS CL_ID FROM customer_list CL Where CL.DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";


                //" SELECT CL.*, CC.*,cl.DATE FROM customer_case cc INNER JOIN customer_list  cl ON cl.ID = cc.CL_ID";
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt = new DataTable();

            
            adapter = new MySqlDataAdapter();//MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            //baglanti.Open();
            adapter.Fill(dt);
            customer_list.DataSource = dt;
            baglanti.Close();

            customer_list.Columns[0].HeaderText = "ID";
            customer_list.Columns[1].HeaderText = "Adı";
            customer_list.Columns[2].HeaderText = "Soyadı";
            customer_list.Columns[3].HeaderText = "Adres";
            customer_list.Columns[4].HeaderText = "Telefon Numarası";
            customer_list.Columns[5].HeaderText = "Firma Adı";
            customer_list.Columns[6].HeaderText = "Kayıt Tarihi";
            customer_list.Columns[7].HeaderText = "IDW";

            customer_list.Columns[0].Visible = false;
            customer_list.Columns[6].Visible = false;
            customer_list.Columns[7].Visible = false;
            customer_list.AllowUserToAddRows = false;

           // SqlClass sqlCon = new SqlClass();
            //sqlCon.ListData(customer_list, DateTime.Now, DateTime.Now);


            //sqlCon.ConnectSql();
            String sqlTotal2 = "SELECT CC.ODEMETIPI,SUM(CC.TL) AS TL, SUM(CC.EURO) AS EURO, SUM(CC.DOLAR) AS DOLAR FROM customer_case cc WHERE DATE BETWEEN '"
                                + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + " GROUP BY CC.ODEMETIPI";
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dtTotal2 = new DataTable();

            MySqlDataAdapter adapterTotal2 = new MySqlDataAdapter();
            MySqlCommand commandTotal2 = new MySqlCommand();

            commandTotal2.CommandText = sqlTotal2;
            commandTotal2.Connection = baglanti;
            adapterTotal2.SelectCommand = commandTotal2;

            //baglanti.Open();
            adapterTotal2.Fill(dtTotal2);
            grdCase_Total.DataSource = dtTotal2;
            baglanti.Close();

            grdCase_Total.Columns[0].HeaderText = "Ödeme Tipi";
            grdCase_Total.Columns[1].HeaderText = "TL";
            grdCase_Total.Columns[1].DefaultCellStyle.Format = "N";
            grdCase_Total.Columns[2].HeaderText = "€";
            grdCase_Total.Columns[2].DefaultCellStyle.Format = "N";
            grdCase_Total.Columns[3].HeaderText = "$";
            grdCase_Total.Columns[3].DefaultCellStyle.Format = "N";

          //  customer_list.Columns[0].Visible = false;
            



            String id;
            int lvD=0;int lvT=0;int lvE=0;
     
            }

        private void btnQueryStock_Click(object sender, EventArgs e)
        {
            //string yr, m, d, yr2, m2, d2;

            //yr = dateTimePicker1.Value.Year.ToString();
            //m = dateTimePicker1.Value.Month.ToString();
            //d = dateTimePicker1.Value.Day.ToString();
            //yr2 = dateTimePicker2.Value.Year.ToString();
            //m2 = dateTimePicker2.Value.Month.ToString();
            //d2 = dateTimePicker2.Value.Day.ToString();

            //String SQLTEXT = " SELECT * FROM customer_list Where DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

            //String sql = SQLTEXT;//"SELECT * FROM stock_list Where DATE BETWEEN @tar1 and @tar2 ";
            ////if (DataGridList.Name == "dtgridSalesList")
            ////    sql = sql + " ORDER BY  SALE_CODE";
            //DataTable dt = new DataTable();

            //MySqlDataAdapter adapter = new MySqlDataAdapter();
            //MySqlCommand command = new MySqlCommand();

            //command.CommandText = sql;
            //command.Connection = baglanti;
            //adapter.SelectCommand = command;

            ////if (baglanti.State =Close )  
            ////   baglanti.Open();
            //adapter.Fill(dt);
            //customer_list.DataSource = dt;
            //baglanti.Close();

            //customer_list.Columns[0].HeaderText = "Numarası";
            //customer_list.Columns[1].HeaderText = "Adı";
            //customer_list.Columns[2].HeaderText = "Soyadı";
            //customer_list.Columns[3].HeaderText = "Firma Adı";
            //customer_list.Columns[4].HeaderText = "Telefon Numarası";
            //customer_list.Columns[5].HeaderText = "Adres";
            //customer_list.Columns[6].HeaderText = "tl";
            //customer_list.Columns[7].HeaderText = "$";
            //customer_list.Columns[7].ValueType = typeof(double);
            //customer_list.Columns[8].HeaderText = "€";
            //customer_list.Columns[9].HeaderText = "Tarih";
            //customer_list.Columns[9].HeaderText = "Ödeme Tipi";

            //customer_list.Columns[0].Visible = false;

        }

     
        private void customer_list_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          String a =   customer_list.TabIndex.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

    
        private void button1_Click_2(object sender, EventArgs e)
        {
            lvCL_ID = "0";
            try
            {
                if (customer_list.CurrentRow.Cells["CL_ID"].Value.ToString() == "")
                    lvCL_ID = "0";
                else
                  lvCL_ID = customer_list.CurrentRow.Cells["CL_ID"].Value.ToString();

            }
            catch
            {
                lvCL_ID = "0";
            }
            if (Convert.ToInt32(lvCL_ID) > 0)
            {
                frmCustomDetailList detail = new frmCustomDetailList();
                detail.ArrangeForm(lvCL_ID);
                // detail.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string yr, m, d;

            yr = DateTime.Now.Year.ToString();
            m = DateTime.Now.Month.ToString();
            d = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d) + 1;
            

            DialogResult result = new DialogResult();

            result = MessageBox.Show("Veriyi güncellemek istiyormusunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //baglanti = new MySqlConnection(bag);
            if (result == DialogResult.Yes)
            { 
                try{
                   
                    cmdb = new MySqlCommandBuilder(adapter);
          /*  DataSet uds = new DataSet();
            uds = dataSet.GetChanges(DataRowState.Modified);
            
            adapter.Update(uds);*/

                    adapter.Update(dataSet, "customer_case");
                    if (baglanti.State == ConnectionState.Closed)
                      baglanti.Open();

                    String sqlTotal2 = "SELECT CC.ODEMETIPI,SUM(CC.TL) AS TL, SUM(CC.EURO) AS EURO, SUM(CC.DOLAR) AS DOLAR FROM customer_case cc WHERE DATE BETWEEN '"
                                + yr + "-" + m + "-" + d + "' AND " + "'" + yr + "-" + m + "-" + d4 + "'" + "GROUP BY CC.ODEMETIPI";
                    //if (DataGridList.Name == "dtgridSalesList")
                    //    sql = sql + " ORDER BY  SALE_CODE";
                    DataTable dtTotal2 = new DataTable();

                    MySqlDataAdapter adapterTotal2 = new MySqlDataAdapter();
                    MySqlCommand commandTotal2 = new MySqlCommand();

                    commandTotal2.CommandText = sqlTotal2;
                    commandTotal2.Connection = baglanti;
                    adapterTotal2.SelectCommand = commandTotal2;

                    //baglanti.Open();
                    adapterTotal2.Fill(dtTotal2);
                    grdCase_Total.DataSource = dtTotal2;
                    baglanti.Close();

                    grdCase_Total.Columns[0].HeaderText = "Ödeme Tipi";
                    grdCase_Total.Columns[1].HeaderText = "TL";
                    grdCase_Total.Columns[1].DefaultCellStyle.Format = "N";
                    grdCase_Total.Columns[2].HeaderText = "€";
                    grdCase_Total.Columns[2].DefaultCellStyle.Format = "N";
                    grdCase_Total.Columns[3].HeaderText = "$";
                    grdCase_Total.Columns[3].DefaultCellStyle.Format = "N";




                }
                catch (global::System.Exception ex)
                {
                    MessageBox.Show((ex.Message + (" : " + "Bir hata olustu, lütfen daha sonra tekrar deneyiniz...")), "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            
         
        }

        private void customer_list_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            String lvCL_ID;
            string yr, m, d;

            yr = DateTime.Now.Year.ToString();
            m = DateTime.Now.Month.ToString();
            d = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d) + 1;
            //dataSet = null;
            try
            {
                lvCL_ID = customer_list.CurrentRow.Cells["ID"].Value.ToString();
            }
            catch
            {
                lvCL_ID = "0";
            }


            String sqlTotal3 = "SELECT * FROM customer_case CC where CC.CL_ID = " + "'" + lvCL_ID + "'" + " and DATE BETWEEN '"
                                + yr + "-" + m + "-" + d + "' AND " + "'" + yr + "-" + m + "-" + d4 + "'";
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dtTotal3 = new DataTable();
            dataSet = new DataSet();

             adapter = new MySqlDataAdapter();
            MySqlCommand commandTotal3 = new MySqlCommand();

            commandTotal3.CommandText = sqlTotal3;
            commandTotal3.Connection = baglanti;
            adapter.SelectCommand = commandTotal3;
            if (baglanti.State == ConnectionState.Closed)
              baglanti.Open();

            adapter.Fill(dtTotal3);
            adapter.Fill(dataSet, "customer_case");
            grdCusCasDetail.DataSource = dataSet.Tables[0];//dtTotal3;
            baglanti.Close();

            grdCusCasDetail.Columns[0].HeaderText = "ID";
            grdCusCasDetail.Columns[1].HeaderText = "CL_ID";
            grdCusCasDetail.Columns[2].HeaderText = "TL";
            grdCusCasDetail.Columns[2].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[3].HeaderText = "€";
            grdCusCasDetail.Columns[3].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[4].HeaderText = "$";
            grdCusCasDetail.Columns[4].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[5].HeaderText = "Ödeme Tipi";
            grdCusCasDetail.Columns[6].HeaderText = "Kayıt Tarihi";
           

            grdCusCasDetail.Columns[0].Visible = false;
            grdCusCasDetail.Columns[1].Visible = false;
        }

        private void btnQueryStock_Click_1(object sender, EventArgs e)
        {
            string yr, m, d, yr2, m2, d2;
            string yr3, m3, d3;

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

            yr = dateTimePicker1.Value.Year.ToString();
            m = dateTimePicker1.Value.Month.ToString();
            d = dateTimePicker1.Value.Day.ToString();
            yr2 = dateTimePicker2.Value.Year.ToString();
            m2 = dateTimePicker2.Value.Month.ToString();
            d2 = dateTimePicker2.Value.Day.ToString();

            SqlClass sqlCon = new SqlClass();
   
            sqlCon.ConnectSql();
            String sql = "SELECT cl.*,cl.ID AS CL_ID FROM customer_list CL Where CL.DATE BETWEEN '" + yr + "-" + m + "-" + d + "' AND " + "'" + yr2 + "-" + m2 + "-" + d2 + "'";

            DataTable dt = new DataTable();


            adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            //baglanti.Open();
            adapter.Fill(dt);
            customer_list.DataSource = dt;
            baglanti.Close();

            customer_list.Columns[0].HeaderText = "ID";
            customer_list.Columns[1].HeaderText = "Adı";
            customer_list.Columns[2].HeaderText = "Soyadı";
            customer_list.Columns[3].HeaderText = "Firma Adı";
            customer_list.Columns[4].HeaderText = "Telefon Numarası";
            customer_list.Columns[5].HeaderText = "Adres";
            customer_list.Columns[6].HeaderText = "Kayıt Tarihi";
            customer_list.Columns[7].HeaderText = "IDW";

            customer_list.Columns[0].Visible = false;
            customer_list.Columns[7].Visible = false;
            customer_list.Columns[6].Visible = false;
            customer_list.AllowUserToAddRows = false;


            String lvCL_ID;
            //dataSet = null;
            try
            {
                lvCL_ID = customer_list.CurrentRow.Cells["ID"].Value.ToString();
            }
            catch
            {
                lvCL_ID = "0";
            }


            String sqlTotal3 = "SELECT * FROM customer_case CC where CC.CL_ID = " + "'" + lvCL_ID + "'" + " and DATE BETWEEN '"
                                + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";

            DataTable dtTotal3 = new DataTable();
            dataSet = new DataSet();

            adapter = new MySqlDataAdapter();
            MySqlCommand commandTotal3 = new MySqlCommand();

            commandTotal3.CommandText = sqlTotal3;
            commandTotal3.Connection = baglanti;
            adapter.SelectCommand = commandTotal3;
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            adapter.Fill(dtTotal3);
            adapter.Fill(dataSet, "customer_case");
            grdCusCasDetail.DataSource = dataSet.Tables[0];//dtTotal3;
            baglanti.Close();


            grdCusCasDetail.Columns[0].HeaderText = "ID";
            grdCusCasDetail.Columns[1].HeaderText = "CL_ID";
            grdCusCasDetail.Columns[2].HeaderText = "TL";
            grdCusCasDetail.Columns[2].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[3].HeaderText = "€";
            grdCusCasDetail.Columns[3].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[4].HeaderText = "$";
            grdCusCasDetail.Columns[4].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[5].HeaderText = "Ödeme Tipi";
            grdCusCasDetail.Columns[6].HeaderText = "Kayıt Tarihi";


           /* grdCusCasDetail.Columns[0].HeaderText = "CL_ID";
            grdCusCasDetail.Columns[1].HeaderText = "TL";
            grdCusCasDetail.Columns[1].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[2].HeaderText = "€";
            grdCusCasDetail.Columns[2].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[3].HeaderText = "$";
            grdCusCasDetail.Columns[3].DefaultCellStyle.Format = "N";
            grdCusCasDetail.Columns[4].HeaderText = "Ödeme Tipi";
            grdCusCasDetail.Columns[5].HeaderText = "ID";*/

            grdCusCasDetail.Columns[0].Visible = false;
            grdCusCasDetail.Columns[5].Visible = false;
           
        }
    }
}
