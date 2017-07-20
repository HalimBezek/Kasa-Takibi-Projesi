using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;


namespace WindowsFormsApplication12
{
    public class SqlClass
    {
        MySqlConnection baglanti;
        String DataListesi;
        DateTimePicker dtp;
        String OTipi;
       
        public void ConnectSql()
        {

            String bag;
            MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

            build.Server = "127.0.0.1";//	localhost
            build.UserID = "root";
            build.Password = "12345678";
            build.Database = "case_follow";
            build.Port = 3306;


            bag = build.ToString();
            baglanti = new MySqlConnection(bag);

        }
    

        public void ListData(DataGridView DataGridList, DateTime dtStart, DateTime dtFinish)
        {
            if (DataGridList.Name == "dtgridSalesList")
                DataListesi = "daily_sale";
            else if (DataGridList.Name == "dtgrdStockList")
                DataListesi = "stock_list";
            else if (DataGridList.Name == "dtgridPaymentList")
                DataListesi = "payments";
            else if (DataGridList.Name == "customer_list1")
                DataListesi = "customer_list";
            else DataListesi = DataGridList.Name;

            ConnectSql();
            String sql = "SELECT  *  FROM  " + DataListesi;
            //if (DataGridList.Name == "dtgridSalesList")
            //    sql = sql + " ORDER BY  SALE_CODE";
            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            DataGridList.DataSource = dt;
            baglanti.Close();

        }

        public void AddStock(String Name, String Code, int Pieces,String Customer,double Price, String Cinsi, String Tipi)
        {
            string yr,m, d , t;
            DateTime  DTIME;
            yr = DateTime.Now.Date.Year.ToString();
            m = DateTime.Now.Date.Month.ToString();
            d = DateTime.Now.Date.Day.ToString();
            DTIME =DateTime.Now.Date;
            t = yr + "-" + m + "-" + d;

            ConnectSql();
            baglanti.Open();//	ID	NAME STOK ADI	CODE STOK KODU	PIECE KAÇ ADET	CUSTOMER KIM ALDI	BRAND URUN MARKASI

            string sql = "INSERT INTO stock_list (NAME, CODE) VALUES ('" + Name + "','" + Code + "')";


            MySqlCommand komut = new MySqlCommand(sql, baglanti);


            komut.ExecuteNonQuery(); 
            baglanti.Close();


        }
        public void AddPerson(String Name, String Surname, String Firm_name, String Tel_number, String Adress)
        {
            string yr, m, d, t;
            DateTime DTIME;
            yr = DateTime.Now.Date.Year.ToString();
            m = DateTime.Now.Date.Month.ToString();
            d = DateTime.Now.Date.Day.ToString();
            DTIME = DateTime.Now.Date;
            t = yr + "-" + m + "-" + d;

            ConnectSql();
            baglanti.Open();
                            
            string sql = "INSERT INTO customer_list (NAME, SURNAME, FIRM_NAME, TEL_NO, ADRESS) VALUES ('" + Name + "','" + Surname +
                                                "','" + Firm_name + "','" + Tel_number + "','" + Adress + "')";

            MySqlCommand komut = new MySqlCommand(sql, baglanti);

            komut.ExecuteNonQuery();
            baglanti.Close();
            
        }
        //cbStockCode.Text, Convert.ToInt32(tbSalePiece.Text), Convert.ToInt32(tbPrice.Text), Convert.ToInt32(cbType.Text), cbCinsi.Text
        public void AddSale(String StockCode, int SalePiece, int Price, String Type, String Cinsi, String Custumer)
        {
            ConnectSql();
            baglanti.Open();

            string sql = "INSERT INTO daily_sale (SALE_CODE, PRICE, PAY_CARNEL, ODEMECINSI, PIECES,CUSTOMER) VALUES ('" + StockCode + "','" + Price +
                                               "','" + Type + "','" + Cinsi + "','" + SalePiece + "', '" + Custumer + "') ";// + Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-mm-dd")) + "')";

        //    Convert(DateTime, DateTime.Now.Date.ToString("yyyy-mm-dd"), 102);
            MySqlCommand komut = new MySqlCommand(sql, baglanti);


            komut.ExecuteNonQuery();
    
            baglanti.Close();


        }

        public void AddPayment(String PaymentType, int PricePay, String Cinsi, String Type, String Custumer) //cus_id
        {
           /* ConnectSql();
            baglanti.Open();//ID	PAY_TYPE 1:ALINAN 2:YAPILAN ODEMELER	PAY_PRICE	PAY_CARNEL ODEME TIPI	ODEMECINSI ODEME CINSI 

            string sql = "INSERT INTO payments (PAY_TYPE, PAY_PRICE, PAY_CARNEL, ODEMECINSI,CUSTOMER) VALUES ('" + PaymentType + "','" + PricePay +
                                               "','" + Type + "','" + Cinsi + "', '" + Custumer + "')";


            MySqlCommand komut = new MySqlCommand(sql, baglanti);


            komut.ExecuteNonQuery();
            baglanti.Close();*/


        }

        public void CustumerCase(String PaymentType, int PricePay, String Cinsi, String Type, String Custumer) //cus_id
        {
            DataTable dtable = new DataTable();
            ConnectSql();
            string sql = "SELECT " +
                            "  CONCAT(CL.NAME, ' ' , CL.SURNAME) AS CL_NAME, " + 
                            " SUM((DSC.EURO) *(DSC.PIECES)) AS CPEURO," +
                            " SUM((DSC.DOLAR) *(DSC.PIECES)) AS CPDOLAR," +
                            "  SUM((DSC.TL) *(DSC.PIECES)) AS CPTL," +
                            "   DSC.ODEMETIPI " + 
                            "FROM      customer_list CL " +
                            "JOIN" +
                            "daily_sale_case DSC" +
                            "ON" +
                            "   CL.ID = DSC.CL_ID" +
                            "GROUP BY" + 
                            "   DSC.CL_ID," +
                            "  DSC.ODEMETIPI" ;


             DataTable dt = new DataTable();

             MySqlDataAdapter adapter = new MySqlDataAdapter();
             MySqlCommand command = new MySqlCommand();

             command.CommandText = sql;
             command.Connection = baglanti;
             adapter.SelectCommand = command;

             baglanti.Open();
             adapter.Fill(dt);
             dtable = dt;
             baglanti.Close();


        }

    }
}
