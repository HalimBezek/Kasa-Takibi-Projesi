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
    public partial class frmListe : Form
    {
        String bag;
        MySqlConnection baglanti;

        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
        public frmListe()
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

        private void Liste_Load(object sender, EventArgs e)
        {
            SqlClass sqlCon = new SqlClass();

            String bag;
            //     MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

            DataSet dSet = new DataSet();


            //build.Server = "localhost";
            //build.UserID = "root";
            //build.Password = "12345678";
            //build.Database = "case_follow";
            //build.Port = 3306;
            //

            DataGridView stock_list = new DataGridView();
            stock_list.Name = "stock_list";

            //SqlClass sqlCon = new SqlClass();

            SqlClass connect = new SqlClass();
            connect.ConnectSql();
            String sql2 = "SELECT SL.NAME, SL.CODE, SL.ID FROM stock_list SL "; //, SL.*
            //if (DataGridList.Name == "dtgridSalesList")
            //  sql = sql + " ORDER BY  SALE_CODE";
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
            stock_list.DataSource = dtable2;
            dtgrdList.DataSource = dtable2;
            baglanti.Close();
          // sqlCon.ListData(dtgrdStockList, DateTime.Now, DateTime.Now);


            dtgrdList.Columns[0].HeaderText = "Adı";
            dtgrdList.Columns[1].HeaderText = "Kodu";
            dtgrdList.Columns[2].HeaderText = "ID";

            dtgrdList.Columns[2].Visible = false;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            SqlClass sqlCon = new SqlClass();

            String bag;
            //     MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

            DataSet dSet = new DataSet();


            //build.Server = "localhost";
            //build.UserID = "root";
            //build.Password = "12345678";
            //build.Database = "case_follow";
            //build.Port = 3306;
            //

            DataGridView stock_list = new DataGridView();
            stock_list.Name = "stock_list";

            //SqlClass sqlCon = new SqlClass();

            SqlClass connect = new SqlClass();
            connect.ConnectSql();
            String sql2 = "SELECT SL.NAME, SL.CODE FROM stock_list SL WHERE SL.NAME LIKE '"+ tbName.Text + "%'"; //, SL.*
            //if (DataGridList.Name == "dtgridSalesList")
            //  sql = sql + " ORDER BY  SALE_CODE";
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
            stock_list.DataSource = dtable2;
            dtgrdList.DataSource = dtable2;
            baglanti.Close();
            // sqlCon.ListData(dtgrdStockList, DateTime.Now, DateTime.Now);


            dtgrdList.Columns[0].HeaderText = "Adı";
            dtgrdList.Columns[1].HeaderText = "Kodu";
            // dtgrdList.Columns[1].HeaderText = "Adı";

            //dtgrdStockList.Columns[0].Visible = false;
        }

        private void dtgrdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            this.DialogResult = DialogResult.OK;
          //  a =  dtgrdList. Rows[SatirNumarası].Cells[SutunNumarası].Value.ToString()  //GetFocusedRowCellValue //Columns[1].Index.ToString();
            
        }

    
    }
}
