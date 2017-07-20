using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class AddForm : Form
    {

        String bag;
        MySqlConnection baglanti;

        MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
        //  String sql = "SELECT * FROM customer_list "; // +DataListesi;
        //if (DataGridList.Name == "dtgridSalesList")
        //    sql = sql + " ORDER BY  SALE_CODE";
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand command = new MySqlCommand();
        MySqlCommandBuilder cmdb;
        
        String lgvCustumer;
        String PaymentType;

        String userid;
        String stocklist_id;

        //Bunu buraya ekledik tüm işlemler bunun üzerinden yürüyecek 
        List<char> tcPricesValue;
        String txMoney;
        public AddForm()
        {


            InitializeComponent();
            /*MySqlConnectionStringBuilder bag = new MySqlConnectionStringBuilder();
            bag.Server = "185.33.234.64";
            bag.UserID = "root";
            bag.Password = ";
            bag.Database = "case_follow";
            MySqlConnection baglanti = new MySqlConnection(bag.ToString());
             baglanti.Open();
             MessageBox.Show("Bağlantı Sağlandı …");
           
           * bag.Server = “uzakServer”; Örnek : 127.0.0.1 , 195.175.39.39 gibi Direk IP verebilirsiniz .
             bag.UserID = “veritabanıKullanıcıAdı”;
             bag.Password = “veritabaniSifre”;
             bag.Database = “veritabaniAdi”;
             MySqlConnection baglanti = new MySqlConnection(bag.ToString());
             baglanti.Open();
             messageBox.show(“Bağlantı Sağlandı …”);*/

            build.Server = "127.0.0.1";////	localhost
            build.UserID = "root";
            build.Password = "12345678";//"12345678";
            build.Database = "case_follow";
            //  build.Port = 3306;

            bag = build.ToString();
            baglanti = new MySqlConnection(bag);

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
            }
            catch
            {
                MessageBox.Show("Data bağlantısını kontrol ediniz.", "Hata Mesajı");
             }
            if (baglanti.State == ConnectionState.Open)
            {
                userid = "0";
                stocklist_id = "0";

                tcPricesValue = new System.Collections.Generic.List<char>();
                tcPrices.RightToLeft = RightToLeft.Yes;
                tbPrice.RightToLeft = RightToLeft.Yes;
                tbPricePay.RightToLeft = RightToLeft.Yes;
                txMoney = "";
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddStock_Click(object sender, EventArgs e)//private void button1_Click(object sender, EventArgs e)
        {
            String CustomName;
            Double lvD = 0; Double lvT = 0; Double lvE = 0;

            string yr3, m3, d3;

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;
       
            if ((tbName.Text.Trim() == "") || (tbCode.Text.Trim() == "") || (tbPieces.Text.Trim() == "") || ((tbCustomer.Text.Trim() == "") && (tbCustomerOto.Text.Trim() == "")) ||
                (tcPrices.Text.Trim() == "") || (cbCinsi.Text.Trim() == "") || (cbODEMECINSI.Text.Trim() == ""))
            {
                MessageBox.Show("Boş alan brakmayınız.", "Bilgilendirme Mesajı");

            }
            else
            {
                if (tbCustomer.Text.Trim() != "")
                   CustomName = tbCustomer.Text;
                else CustomName = tbCustomerOto.Text;

               SqlClass sqlConn = new SqlClass();

                sqlConn.AddStock(tbName.Text, tbCode.Text, Convert.ToInt32(tbPieces.Text), CustomName, Convert.ToDouble(tcPrices.Text), cbCinsi.Text, cbODEMECINSI.Text);

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                String sqlNewID = "select ID from stock_list order by id desc limit 0, 1";
                MySqlCommand komutID = new MySqlCommand(sqlNewID, baglanti);
                String LastID = komutID.ExecuteScalar().ToString();
                baglanti.Close();

                tcPrices.Text = tcPrices.Text.ToString().Replace(',', '.'); 

                if (String.Equals(cbCinsi.Text, "$"))
                { lvD = Convert.ToDouble(tcPrices.Text); }
                if (String.Equals(cbCinsi.Text, "TL"))
                { lvT = Convert.ToDouble(tcPrices.Text); }
                if (String.Equals(cbCinsi.Text, "€"))
                { lvE = Convert.ToDouble(tcPrices.Text); }

                String textt;

                try
                {
                    baglanti.Open();
                    textt = "INSERT INTO stock_list_case(CL_ID, TL, DOLAR, EURO, ODEMETIPI,PIECES, KISI_ADI, CODE,SL_ID) VALUES (" + Convert.ToInt64(userid) +
                             "," + lvT + " ," + lvD + " ," + lvE + " ," + "'" + cbODEMECINSI.Text + "'" + " ," + Convert.ToInt64(tbPieces.Text) + " ," + "'" +
                             CustomName + "'" + " ," + "'" + tbCode.Text + "'" + "," + Convert.ToInt32(LastID) + ")";

                    string sql2 = textt;

                    MySqlCommand komut2 = new MySqlCommand(sql2, baglanti);

                    komut2.ExecuteNonQuery();//

                    baglanti.Close();
                }
                catch (IOException ee)
                {    // Extract some information from this exception, and then   
                    // throw it to the parent method.  
                    MessageBox.Show("Bir hata oluştu kaydı tekrar yapınız!", "Bilgilendirme Mesajı");

                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    textt = "DELETE * FROM  stock_list WHERE ID = " + LastID;

                    string sqlCustom = textt;

                    MySqlCommand komutCustom = new MySqlCommand(sqlCustom, baglanti);

                    komutCustom.ExecuteNonQuery();//

                    baglanti.Close();

                   
                }  
                try{

                if (String.Equals(cbCinsi.Text, "$"))
                { lvD = Convert.ToDouble(tcPrices.Text) * Convert.ToInt32(tbPieces.Text); }
                if (String.Equals(cbCinsi.Text, "TL"))
                { lvT = Convert.ToDouble(tcPrices.Text) * Convert.ToInt32(tbPieces.Text); }
                if (String.Equals(cbCinsi.Text, "€"))
                { lvE = Convert.ToDouble(tcPrices.Text) * Convert.ToInt32(tbPieces.Text); }

                baglanti.Open();
                String sqlNewCase = "SELECT COUNT(*) FROM customer_case CC WHERE CC.CL_ID =" + Convert.ToInt64(userid) + " AND CC.ODEMETIPI = " + "'" + cbODEMECINSI.Text + "'" +
                     "AND DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";
                MySqlCommand komutCase = new MySqlCommand(sqlNewCase, baglanti);
                String Case = komutCase.ExecuteScalar().ToString();
                if (Case != "0")
                {

                    textt = "UPDATE customer_case SET TL = TL + " + lvT + ",EURO = EURO + " + lvE + ",DOLAR = DOLAR + " + lvD + " WHERE CL_ID = " + Convert.ToInt64(userid) +
                             " and ODEMETIPI = " + "'" + cbODEMECINSI.Text + "'";

                    string sqlUpdate = textt;

                    MySqlCommand komutUpdate = new MySqlCommand(sqlUpdate, baglanti);

                    komutUpdate.ExecuteNonQuery();//

                    baglanti.Close();

                }
                else
                {

                    textt = "INSERT INTO CUSTOMER_CASE (CL_ID, TL,DOLAR, EURO, ODEMETIPI) VALUES (" + Convert.ToInt64(userid) +
                             "," + lvT + " ," + lvD + " ," + lvE + " ," + "'" + cbODEMECINSI.Text + "'" + ")";

                    string sqlCustom = textt;

                    MySqlCommand komutCustom = new MySqlCommand(sqlCustom, baglanti);

                    komutCustom.ExecuteNonQuery();//

                    baglanti.Close();
                }
            }
            catch (IOException ee)
            {    // Extract some information from this exception, and then   
                // throw it to the parent method.  

                MessageBox.Show("Bir hata oluştu kaydı tekrar yapınız!", "Bilgilendirme Mesajı");

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                textt = "DELETE * FROM  stock_list WHERE ID = " + LastID;

                string sqlCustom = textt;

                MySqlCommand komutCustom = new MySqlCommand(sqlCustom, baglanti);

                komutCustom.ExecuteNonQuery();//

                baglanti.Close();
  
            }  

                tbName.Text = "";
                tbCode.Text = "";
                tbPieces.Text = "";
                tbCustomer.Text = "";
                tbCustomerOto.Text = "";
                tcPrices.Text = "";
                cbCinsi.Text = "";
                lblSave.Visible = true;

                FullCombobax();
            
            }
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string yr3, m3, d3;

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

           // String id;
            Double lvD = 0; Double lvT = 0; Double lvE = 0;
            //id = cbIDtutar.Text;
            if ((cbStockCode.Text == "") || (tbSalePiece.Text == "") || (tbPrice.Text == "") || (cbType.Text == "") || (cbSalePayType.Text == "") || ((tbCustumerManuel.Text == "") && (cbCustomerOto.Text == "")))
            {
                MessageBox.Show("Boş alan brakmayınız.", "Bilgilendirme Mesajı");

            }
            else
            {
                lgvCustumer = "";
                if (cbCustomerOto.Text.Trim() != "")
                {
                    lgvCustumer = cbCustomerOto.Text;
                //    id = cbID.GetItemText(1).ToString();
                }
                else
                    lgvCustumer = tbCustumerManuel.Text;

                SqlClass sqlConn = new SqlClass();

               // sqlConn.AddSale(cbStockCode.Text, Convert.ToInt32(tbSalePiece.Text),
                //    Convert.ToInt32(tbPrice.Text), cbType.Text, cbSalePayType.Text, lgvCustumer);
                tbPrice.Text = tbPrice.Text.ToString().Replace(',', '.'); 
                if (String.Equals(cbSalePayType.Text, "$"))
                { lvD = Convert.ToDouble(tbPrice.Text); }
                if (String.Equals(cbSalePayType.Text,"TL"))
                { lvT = Convert.ToDouble(tbPrice.Text); }
                if (String.Equals(cbSalePayType.Text,"€"))
                { lvE = Convert.ToDouble(tbPrice.Text); }
                       
                
                String bag;
                String textt, textinsert;


                baglanti.Open();//	ID	SALE_CODE SATILAN URUN KODU	PRICE FIYATI	PAY_CARNEL ODEME TIPI	ODEMECINSI PIECES 

             
               textinsert = "INSERT INTO daily_sale_case(CL_ID, SL_ID, TL, DOLAR, EURO, PIECES, ODEMETIPI) VALUES (" + Convert.ToInt32(userid) + ","+ Convert.ToInt32(stocklist_id) +
                            "," + lvT + " ," + lvD + " ," + lvE + " ," + Convert.ToInt32(tbSalePiece.Text) + " ," + "'" + cbType.Text + "'" + ")";
                string sqlinsert = textinsert;

                MySqlCommand komutinsert = new MySqlCommand(sqlinsert, baglanti);

                komutinsert.ExecuteNonQuery();
                
                baglanti.Close();


                if (String.Equals(cbSalePayType.Text, "$"))
                { lvD = Convert.ToDouble(tbPrice.Text) * Convert.ToInt32(tbSalePiece.Text); }
                if (String.Equals(cbSalePayType.Text, "TL"))
                { lvT = Convert.ToDouble(tbPrice.Text) * Convert.ToInt32(tbSalePiece.Text); }
                if (String.Equals(cbSalePayType.Text, "€"))
                { lvE = Convert.ToDouble(tbPrice.Text) * Convert.ToInt32(tbSalePiece.Text); }

                baglanti.Open();
                String sqlNewCase = "SELECT COUNT(*) FROM customer_case CC WHERE CC.CL_ID =" + Convert.ToInt64(userid) + " AND CC.ODEMETIPI = " + "'" + cbType.Text + "'" +
                    "AND DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";
                MySqlCommand komutCase = new MySqlCommand(sqlNewCase, baglanti);
                String Case = komutCase.ExecuteScalar().ToString();
                if (Case != "0")
                {

                    textt = "UPDATE customer_case SET TL = TL + " + lvT + ",EURO = EURO + " + lvE + ",DOLAR = DOLAR + " + lvD + " WHERE CL_ID = " + Convert.ToInt64(userid) + " and ODEMETIPI = " + "'" + cbType.Text + "'";

                    string sqlUpdate = textt;

                    MySqlCommand komutUpdate = new MySqlCommand(sqlUpdate, baglanti);

                    komutUpdate.ExecuteNonQuery();//

                    baglanti.Close();

                }
                else
                {

                    textt = "INSERT INTO CUSTOMER_CASE (CL_ID, TL,DOLAR, EURO, ODEMETIPI) VALUES (" + Convert.ToInt64(userid) +
                             "," + lvT + " ," + lvD + " ," + lvE + " ," + "'" + cbType.Text + "'" + ")";

                    string sqlCustom = textt;

                    MySqlCommand komutCustom = new MySqlCommand(sqlCustom, baglanti);

                    komutCustom.ExecuteNonQuery();//

                    baglanti.Close();
                }




           
                cbStockCode.Text = "";
                tbSalePiece.Text = "";
                tbPrice.Text = "";
                cbType.Text = "";
                cbCinsi.Text = "";
                tbCustumerManuel.Text = "";
                cbCustomerOto.Text = "";

                lblSave2.Visible = true;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            string yr3, m3, d3;

            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;

           // String id;
            Double lvD = 0; Double lvT = 0; Double lvE = 0;
           // id = cbIDtutar1.Text;
            if ((tbPricePay.Text == "") || (cb2Cinsi.Text == "") || (cb2Type.Text == "") || ((cbGivePay.Checked == false) && (cbTakePay.Checked == false)) || ((cbCustumerOto2.Text == "") && (tbCustumerManuel2.Text == "")))
            {
                MessageBox.Show("Boş alan brakmayınız.", "Bilgilendirme Mesajı");

            }
            else
            {
                if (cbTakePay.Checked == true)
                    PaymentType = cbTakePay.Text;
                else if (cbGivePay.Checked == true)
                    PaymentType = cbGivePay.Text;
                lgvCustumer = "";
                if (cbCustumerOto2.Text.Trim() != "")
                {   
                    lgvCustumer = cbCustumerOto2.Text;
                }
                else
                    lgvCustumer = tbCustumerManuel2.Text;

                //SqlClass sqlConn = new SqlClass();
                //sqlConn.AddPayment(PaymentType, Convert.ToInt32(tbPricePay.Text), cb2Cinsi.Text, cb2Type.Text, lgvCustumer);//


                ////

                tbPricePay.Text = tbPricePay.Text.ToString().Replace(',', '.'); 

                if (cb2Cinsi.Text.Trim() == "$")//if (String.Equals(cb2Cinsi.Text, "$"))
                { lvD = Convert.ToDouble(tbPricePay.Text); }
                if (cb2Cinsi.Text.Trim() == "TL")
                { lvT = Convert.ToDouble(tbPricePay.Text); }
                if (cb2Cinsi.Text.Trim() == "€")
                { lvE = Convert.ToDouble(tbPricePay.Text); }


                String bag;
                String textt, textinsert2;

                baglanti.Open();//	ID	SALE_CODE SATILAN URUN KODU	PRICE FIYATI	PAY_CARNEL ODEME TIPI	ODEMECINSI PIECES 


                string sql = "INSERT INTO payments (PAY_TYPE, PAY_CARNEL, TL, EURO, DOLAR, CL_ID, CUSTOMER) VALUES ('" + PaymentType + "','" +
                          cb2Type.Text + "','" + lvT + "','" + lvE + "','" + lvD + "','" + Convert.ToInt64(userid) + "','" + lgvCustumer + "')";
               // textinsert2 = textt = "INSERT INTO payments_case(CL_ID, TL, DOLAR, EURO, ODEMETIPI ) VALUES (" + Convert.ToInt32(userid) + "," + lvT + " ," + lvD + " ," + lvE + " ," + "'" + cbType.Text + "'" + ")";
                string sqlinsert2 = sql;

                MySqlCommand komutinsert2 = new MySqlCommand(sqlinsert2, baglanti);

                komutinsert2.ExecuteNonQuery();//
                

                baglanti.Close();

                if (String.Equals(cb2Cinsi.Text, "$"))
                { lvD = Convert.ToDouble(tbPricePay.Text); }
                if (String.Equals(cb2Cinsi.Text, "TL"))
                { lvT = Convert.ToDouble(tbPricePay.Text); }
                if (String.Equals(cb2Cinsi.Text, "€"))
                { lvE = Convert.ToDouble(tbPricePay.Text); }

                if (cbTakePay.Checked)
                {
                    lvT = lvT *1;
                    lvE = lvE *1;
                    lvD = lvD *1;
                }
                else
                {
                    lvT = lvT * -1;
                    lvE = lvE * -1;
                    lvD = lvD * -1;
                }


                baglanti.Open();
                String sqlNewCase = "SELECT COUNT(*) FROM customer_case CC WHERE CC.CL_ID =" + Convert.ToInt64(userid) + " AND CC.ODEMETIPI = " + "'" + cb2Type.Text + "'" +
                    "AND DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'";
                MySqlCommand komutCase = new MySqlCommand(sqlNewCase, baglanti);
                String Case = komutCase.ExecuteScalar().ToString();
                if (Case != "0")
                {

                    textt = "UPDATE customer_case SET TL = TL + " + lvT + ",EURO = EURO + " + lvE + ",DOLAR = DOLAR + " + lvD + " WHERE CL_ID = " + Convert.ToInt64(userid) +
                        " and ODEMETIPI = " + "'" + cb2Type.Text + "'";

                    string sqlUpdate = textt;

                    MySqlCommand komutUpdate = new MySqlCommand(sqlUpdate, baglanti);

                    komutUpdate.ExecuteNonQuery();//

                    baglanti.Close();

                }
                else
                {

                    textt = "INSERT INTO CUSTOMER_CASE (CL_ID, TL,DOLAR, EURO, ODEMETIPI) VALUES (" + Convert.ToInt64(userid) +
                             "," + lvT + " ," + lvD + " ," + lvE + " ," + "'" + cb2Type.Text + "'" + ")";

                    string sqlCustom = textt;

                    MySqlCommand komutCustom = new MySqlCommand(sqlCustom, baglanti);

                    komutCustom.ExecuteNonQuery();//

                    baglanti.Close();
                }



                cbGivePay.Checked = false;
                cbTakePay.Checked = false;
                tbPricePay.Text = "";
                cb2Cinsi.Text = "";
                cb2Type.Text = "";
                cbCustumerOto2.Text = "";
                tbCustumerManuel2.Text = "";



                lblSave3.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmStockList frmStckList = new frmStockList();
            frmStckList.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmSalesList frmSaleList = new frmSalesList();
            frmSaleList.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmPaymentList frmPayList = new frmPaymentList();
            frmPayList.Show();
        }

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            lblSave.Visible = false;

        }

        private void cbStockCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void tbPricePay_TextChanged(object sender, EventArgs e)
        {
            lblSave3.Visible = false;
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
           
            FullCombobax();
        }
        public void FullCombobax()
        {//customer_list
            //
            String bag;
       //     MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();

            DataSet dSet = new DataSet();

            DataGridView stock_list = new DataGridView();
            stock_list.Name = "stock_list";
            DataGridView customer_list = new DataGridView();
            customer_list.Name = "customer_list";
            SqlClass sqlCon = new SqlClass();

            SqlClass connect = new SqlClass();
            connect.ConnectSql();
            String sql2 = "SELECT CONCAT(SL.NAME, ' (', SL.CODE,')') AS NAME, SL.* FROM stock_list SL ";
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
            try
            {
                if(baglanti.State == ConnectionState.Closed)
                  baglanti.Open();
            }
            catch
            {
                MessageBox.Show("Bağlantı sağlanamıyor.","Hata Mesajı");
            }
            if (baglanti.State == ConnectionState.Open)
            {
                adapter2.Fill(dtable2);
                adapter2.Fill(dSet);
                stock_list.DataSource = dtable2;
                baglanti.Close();

                String sql = "SELECT CONCAT(NAME, ' ', SURNAME) AS NAME, ID FROM customer_list ";
                DataTable dtable = new DataTable();

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();

                command.CommandText = sql;
                command.Connection = baglanti;
                adapter.SelectCommand = command;
                dSet.Clear();
                baglanti.Open();
                adapter.Fill(dtable);
                adapter.Fill(dSet);
                customer_list.DataSource = dtable;
                baglanti.Close();

                String sql3 = "SELECT CONCAT(NAME, ' ', SURNAME) AS NAME, ID FROM customer_list ";
                DataTable dtable3 = new DataTable();

                MySqlDataAdapter adapter3 = new MySqlDataAdapter();
                MySqlCommand command3 = new MySqlCommand();

                command3.CommandText = sql3;
                command3.Connection = baglanti;
                adapter3.SelectCommand = command3;
                dSet.Clear();
                baglanti.Open();
                adapter3.Fill(dtable3);
                adapter3.Fill(dSet);
                customer_list.DataSource = dtable3;
                baglanti.Close();
                //cbCustomerOto212.DataSource = customer_list.DataSource;



                String sql4 = "SELECT CONCAT(NAME, ' ', SURNAME) AS NAME, ID FROM customer_list ";
                DataTable dtable4 = new DataTable();

                MySqlDataAdapter adapter4 = new MySqlDataAdapter();
                MySqlCommand command4 = new MySqlCommand();

                command4.CommandText = sql4;
                command4.Connection = baglanti;
                adapter4.SelectCommand = command4;
                dSet.Clear();
                baglanti.Open();
                adapter4.Fill(dtable4);
                adapter4.Fill(dSet);
                customer_list.DataSource = dtable4;
                baglanti.Close();
            }

        }

        private void tbPieces_TextChanged(object sender, EventArgs e)
        {
            
        }
      

        private void tbPieces_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
           /* e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.';
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }*/
            if (char.IsDigit(e.KeyChar))
            {

                // txtboxtaki valuenin  değerinin bizim listimize aktarılması 
                SetValue(2);
                // listimizdeki valuenin textboxa aktarılması 
                SetTextbox(2);
            }
            else
            {
                e.Handled = true;
            } 
        }

        private void tbSalePiece_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbPricePay_KeyPress(object sender, KeyPressEventArgs e)
        {
           /* e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.';
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }*/

            if (char.IsDigit(e.KeyChar))
            {

                // txtboxtaki valuenin  değerinin bizim listimize aktarılması 
                SetValue(3);
                // listimizdeki valuenin textboxa aktarılması 
                SetTextbox(3);
            }
            else
            {
                e.Handled = true;
            } 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmGrid grid = new frmGrid();

            string yr3, m3, d3;
            Double ToplamGelirTL,ToplamGelirDOLAR,ToplamGelirEURO;
            Double ToplamGiderTL, ToplamGiderDOLAR, ToplamGiderEURO;
            Double GenelToplamTL, GenelToplamDOLAR, GenelToplamEURO;
          
            yr3 = DateTime.Now.Year.ToString();
            m3 = DateTime.Now.Month.ToString();
            d3 = DateTime.Now.Day.ToString();

            int d4 = Convert.ToInt32(d3) + 1;
            if (baglanti.State == ConnectionState.Closed)
            baglanti.Open();
/*
            String sql0 = "SELECT SUM(p.TL) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                     yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql1 = "SELECT SUM(p.DOLAR) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                            yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql2 = "SELECT SUM(p.EURO) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                            yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";*/


            String sql3 = "SELECT SUM(p.TL) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql4 = "SELECT SUM(p.DOLAR) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                            yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql5 = "SELECT SUM(p.EURO) FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME' AND ( DATE BETWEEN '" +
                            yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql6 = "SELECT SUM(SC.TL*SC.PIECES) FROM stock_list_case SC  WHERE ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql7 = "SELECT SUM(SC.DOLAR*SC.PIECES) FROM stock_list_case SC  WHERE ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql8 = "SELECT SUM(SC.EURO*SC.PIECES) FROM stock_list_case SC  WHERE ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";

            String sql10 = "SELECT SUM(DC.TL) FROM daily_sale_case DC  WHERE ( DATE BETWEEN '" +
                       yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql11 = "SELECT SUM(DC.DOLAR) FROM daily_sale_case DC  WHERE ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql12 = "SELECT SUM(DC.EURO) FROM daily_sale_case DC  WHERE ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";

            String sql13 = "SELECT SUM(p.TL) FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME' AND ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql14 = "SELECT SUM(p.TL) FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME' AND ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";
            String sql15 = "SELECT SUM(p.TL) FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME' AND ( DATE BETWEEN '" +
                        yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")";

            MySqlCommand komut3 = new MySqlCommand(sql3, baglanti);
            String SonucTL = komut3.ExecuteScalar().ToString();
            MySqlCommand komut4 = new MySqlCommand(sql4, baglanti);
            String SonucDOLAR = komut4.ExecuteScalar().ToString();
            MySqlCommand komut5 = new MySqlCommand(sql5, baglanti);
            String SonucEURO = komut5.ExecuteScalar().ToString();
            MySqlCommand komut6 = new MySqlCommand(sql6, baglanti);
            String SonucSKTL = komut6.ExecuteScalar().ToString();
            MySqlCommand komut7 = new MySqlCommand(sql7, baglanti);
            String SonucSKDOLAR = komut7.ExecuteScalar().ToString();
            MySqlCommand komut8 = new MySqlCommand(sql8, baglanti);
            String SonucSKEURO = komut8.ExecuteScalar().ToString();

            MySqlCommand komut10 = new MySqlCommand(sql10, baglanti);
            String SonucGulukSaTL = komut10.ExecuteScalar().ToString();
            MySqlCommand komut11 = new MySqlCommand(sql11, baglanti);
            String SonucGulukSaDOLAR = komut11.ExecuteScalar().ToString();
            MySqlCommand komut12 = new MySqlCommand(sql12, baglanti);
            String SonucGulukSaEURO = komut12.ExecuteScalar().ToString();
            MySqlCommand komut13 = new MySqlCommand(sql13, baglanti);
            String SonucAlinanOdemeTL = komut13.ExecuteScalar().ToString();
            MySqlCommand komut14 = new MySqlCommand(sql14, baglanti);
            String SonucAlinanOdemeDOLAR = komut14.ExecuteScalar().ToString();
            MySqlCommand komut15 = new MySqlCommand(sql15, baglanti);
            String SonucAlinanOdemeEURO = komut15.ExecuteScalar().ToString();

           // komut3.ExecuteNonQuery();

            baglanti.Close();

            if (SonucGulukSaTL.Trim() == "") SonucGulukSaTL = "0"; if (SonucGulukSaDOLAR.Trim() == "") SonucGulukSaDOLAR = "0";
            if (SonucGulukSaEURO.Trim() == "") SonucGulukSaEURO = "0"; if (SonucAlinanOdemeTL.Trim() == "") SonucAlinanOdemeTL = "0";
            if (SonucAlinanOdemeDOLAR.Trim() == "") SonucAlinanOdemeDOLAR = "0"; if (SonucAlinanOdemeEURO.Trim() == "") SonucAlinanOdemeEURO = "0"; 

            ToplamGelirTL = Convert.ToDouble(SonucGulukSaTL) + Convert.ToDouble(SonucAlinanOdemeTL);
            ToplamGelirDOLAR = Convert.ToDouble(SonucGulukSaDOLAR) + Convert.ToDouble(SonucAlinanOdemeDOLAR);
            ToplamGelirEURO = Convert.ToDouble(SonucGulukSaEURO) + Convert.ToDouble(SonucAlinanOdemeEURO);


            if (SonucTL.Trim() == "") SonucTL = "0"; if (SonucSKTL.Trim() == "") SonucSKTL = "0";
            if (SonucSKDOLAR.Trim() == "") SonucSKDOLAR = "0"; if (SonucDOLAR.Trim() == "") SonucDOLAR = "0";
            if (SonucEURO.Trim() == "") SonucEURO = "0"; if (SonucSKEURO.Trim() == "") SonucSKEURO = "0"; 

            ToplamGiderTL = Convert.ToDouble(SonucTL) + Convert.ToDouble(SonucSKTL);
            ToplamGiderDOLAR = Convert.ToDouble(SonucDOLAR) + Convert.ToDouble(SonucSKDOLAR);
            ToplamGiderEURO = Convert.ToDouble(SonucEURO) + Convert.ToDouble(SonucSKEURO);

            GenelToplamTL = ToplamGelirTL - ToplamGiderTL;
            GenelToplamDOLAR = ToplamGelirDOLAR - ToplamGiderDOLAR;
            GenelToplamEURO = ToplamGelirEURO - ToplamGiderEURO;


            String sql = "SELECT sl.NAME  FROM stock_list  sl"; 

            command.CommandText = sql;
            command.Connection = baglanti;
            adapter.SelectCommand = command;
            

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata.DataSource = dt;

            baglanti.Close();
            
            /////////////////////
    
            
            string dosyakayityolu;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
               dosyakayityolu =  saveFileDialog1.FileName.ToString() + ".pdf";
               int strsayisi = grid.lgvgriddata.Columns.Count;
               
            //iTextSharp.text.Document raporum = new iTextSharp.text.Document();
               Document raporum = new Document(iTextSharp.text.PageSize.A4,10,10,10,35);//iTextSharp.text.PageSize.A4.Rotate()
                //(iTextSharp.text.PageSize.LETTER, 10,10,10,35);

            raporum.AddCreationDate();
            raporum.AddAuthor("ae-robotic");//yazar
          //  raporum.AddHeader("Başlık", "Pdf uygulaması oluştur");
         //   raporum.AddTitle("Test pdf");
                
           

            PdfWriter.GetInstance(raporum, new FileStream(dosyakayityolu, FileMode.OpenOrCreate));

            PdfPTable table = new PdfPTable(2); //+
            PdfPTable table_1 = new PdfPTable(1); //+ table5 i kapsar
            PdfPTable table_2 = new PdfPTable(1); //+ table6 yı kapsar

            PdfPTable table2 = new PdfPTable(5);  //+ KASA DEVİR
            PdfPTable table2_1 = new PdfPTable(4);  //+ Giderler
            PdfPTable table3 = new PdfPTable(2);  //+ table2 ve table2_1 yi kapsayan tablo
            PdfPTable table4 = new PdfPTable(4);  //+
            
            PdfPTable table5 = new PdfPTable(4); //+ 
            PdfPTable table5_1 = new PdfPTable(4); //+ table5 in altındaki tabloar
            PdfPTable table5_2 = new PdfPTable(4); //+ 
            PdfPTable table5_3 = new PdfPTable(4); // kullanılmıyor

            PdfPTable table6 = new PdfPTable(6);  //+ SATIN ALINAN STOKLAR
           
                #region  font ayarları
            //BaseFont arial = BaseFont.CreateFont("C:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            table.HorizontalAlignment = 1;
            table.PaddingTop = 5f;
            table.WidthPercentage = 100;
            table.DefaultCell.BorderWidth = 0;

            table_1.HorizontalAlignment = 1;
            table_1.PaddingTop = 10f;
            table_1.WidthPercentage = 100;
            table_1.DefaultCell.BorderWidth = 0;

            table_2.HorizontalAlignment = 1;
            table_2.PaddingTop = 10f;
            table_2.WidthPercentage = 100;
            table_2.DefaultCell.BorderWidth = 0;

            table2.HorizontalAlignment = 0;//0=Left, 1=Centre, 2=Right
            table2.PaddingTop = 5f;
            table2.SetWidths(new int[5] { 50, 10, 10, 10, 5 }); 

            table2_1.HorizontalAlignment = 2;//0=Left, 1=Centre, 2=Right
            table2_1.PaddingTop = 5f;
            table2_1.SetWidths(new int[4] { 50, 10, 10, 10 }); 

            table3.HorizontalAlignment = 1;
            table3.PaddingTop = 10f;
            table3.WidthPercentage = 100;
            table3.DefaultCell.BorderWidthRight = 2;
            table3.DefaultCell.BorderWidthLeft= 0;
            table3.DefaultCell.BorderWidthBottom = 1;
            table3.DefaultCell.BorderWidthTop = 1;
                // table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    

            table4.HorizontalAlignment = 1;
            table4.PaddingTop = 5f;
            table4.WidthPercentage = 100;
            table4.SetWidths(new int[4] { 50, 20, 20, 20 }); 

            table5.HorizontalAlignment = 2;
            table5.PaddingTop = 5f;
            table5.SetWidths(new int[4] { 40, 10, 10, 10 });

            table5_1.HorizontalAlignment = 2;
            table5_1.PaddingTop = 5f;
            table5_1.SetWidths(new int[4] { 40, 10, 10, 10 });

            table5_2.HorizontalAlignment = 2;
            table5_2.PaddingTop = 5f;
            table5_2.SetWidths(new int[4] { 40, 10, 10, 10 });

            table5_3.HorizontalAlignment = 2;
            table5_3.PaddingTop = 5f;
            table5_3.SetWidths(new int[4] { 40, 10, 10, 10 }); 

            table6.HorizontalAlignment = 1;
            table6.PaddingTop = 5f;
            table6.WidthPercentage = 100;
            table6.SetWidths(new int[6] { 50, 10, 10, 10, 10, 15 }); 

            BaseFont arial = BaseFont.CreateFont("C:\\windows\\fonts\\Corbel.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            // en ust reklam yazı ADEM OLGUNER
            iTextSharp.text.Font font = new iTextSharp.text.Font(arial, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontBuyukYazi = new iTextSharp.text.Font(arial, 12, iTextSharp.text.Font.NORMAL); 
            // tablonun içeriğinin oldugu yermakalenin özellikleri  ve detaylar
            iTextSharp.text.Font font2 = new iTextSharp.text.Font(arial, 10, iTextSharp.text.Font.NORMAL); 
            // tablo baslıkları hangi alanlar   baslik makaleid gibi a
            iTextSharp.text.Font font3 = new iTextSharp.text.Font(arial, 10, iTextSharp.text.Font.BOLD,BaseColor.BLUE);
            iTextSharp.text.Font font4 = new iTextSharp.text.Font(arial, 8, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font font5 = new iTextSharp.text.Font(arial, 8, iTextSharp.text.Font.NORMAL); 

            #endregion
            #region tablo ana başlık firma adı
            string frmadi = " ";
            PdfPCell cell = new PdfPCell(new Phrase(frmadi, font2));
            cell.Colspan = 2;
            cell.PaddingBottom = 3f;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.BorderWidth = 0;
            table.AddCell(cell);

            string frmadi_1 = " ";
            PdfPCell cell_1 = new PdfPCell(new Phrase(frmadi_1, font2));
            cell_1.Colspan = 2;
            cell_1.PaddingBottom = 3f;
            cell_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell_1.BorderWidth = 0;
            table_1.AddCell(cell_1);

            string frmadi_2 = "  ";
            PdfPCell cell_2 = new PdfPCell(new Phrase(frmadi_2, font2));
            cell_2.Colspan = 2;
            cell_2.PaddingBottom = 3f;
            cell_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell_2.BorderWidth = 0;
            table_2.AddCell(cell_2);
            #endregion
            #region tablo başlıkları için
       
            /*PdfPCell cell2 = new PdfPCell(new Phrase("ÜRÜN ADI", font3));
            PdfPCell cell3 = new PdfPCell(new Phrase("ADET", font3));
      
            cell2.Colspan = 1;
            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell2.BackgroundColor = BaseColor.BLUE;
            cell3.Colspan = 1;
            cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
  
            table.AddCell(cell2);
            table.AddCell(cell3);*/
      
     
            #endregion

            
            #region tablo2 ana başlık kasa adı
            string PdfAdi = "KASA DEVİR";
            PdfPCell cellT2 = new PdfPCell(new Phrase(PdfAdi, font));
            cellT2.Colspan =5;
            cellT2.PaddingBottom = 5f;
            cellT2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
           // cellT2.BorderColor = BaseColor.BLUE;
           // cellT2.AddElement(table);
            cellT2.BorderWidth = 0;
             
            table2.AddCell(cellT2);
            #endregion

            #region tablo2_1 ana başlık kasa adı
            string PdfAdi_1 = "Giderler";
            PdfPCell cellT2_1 = new PdfPCell(new Phrase(PdfAdi_1, font));
            cellT2_1.Colspan = 5;
            cellT2_1.PaddingBottom = 5f;
            cellT2_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            //cellT2_1.BorderColor = BaseColor.BLUE;
            // cellT2.AddElement(table);
            cellT2_1.BorderWidth = 0;

            table2_1.AddCell(cellT2_1);
            #endregion

            #region tablo3 ana başlık kasa adı
            string PdfAdi3 = DateTime.Now.ToLongDateString(); //"KASA DEVİR";
            PdfPCell cellT3 = new PdfPCell(new Phrase(PdfAdi3, font));
            cellT3.Colspan = 2;
            cellT3.PaddingBottom = 5f;
            cellT3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT3.BorderColor = BaseColor.BLUE;
          //  cellT3.AddElement(table);

           // cellT3.Border = 0;
            cellT3.BorderWidth = 0;


            table3.AddCell(cellT3);
          #endregion
            #region tablo4 ana başlık kasa adı
            string PdfAdi4 = " ";
            PdfPCell cellT4 = new PdfPCell(new Phrase(PdfAdi4, font2));
            cellT4.Colspan = 4;
            cellT4.PaddingBottom = 3f; // tablo alan genişliği
            cellT4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT4.BorderWidth = 0;
            

            table4.AddCell(cellT4);
            #endregion

            #region tablo5 ana başlık kasa adı
            string PdfAdi5 = "VERESİYE SATIŞLAR";
            PdfPCell cellT5 = new PdfPCell(new Phrase(PdfAdi5, font));
            cellT5.Colspan = 4;
            cellT5.PaddingBottom = 5f;
            cellT5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT5.BorderColor = BaseColor.BLUE;
            //  cellT3.AddElement(table);

            table5.AddCell(cellT5);
            #endregion

            #region tablo5_1 ana başlık kasa adı
            string PdfAdi5_1 = "VERESİYE TAHSİLATLAR";
            PdfPCell cellT5_1 = new PdfPCell(new Phrase(PdfAdi5_1, font));
            cellT5_1.Colspan = 4;
            cellT5_1.PaddingBottom = 5f;
            cellT5_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT5_1.BorderColor = BaseColor.BLUE;
            //  cellT3.AddElement(table);

            table5_1.AddCell(cellT5_1);
            #endregion

            #region tablo5_2 ana başlık kasa adı
            string PdfAdi5_2 = "GİDERLER";
            PdfPCell cellT5_2 = new PdfPCell(new Phrase(PdfAdi5_2, font));
            cellT5_2.Colspan = 4;
            cellT5_2.PaddingBottom = 5f;
            cellT5_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT5_2.BorderColor = BaseColor.BLUE;
            //  cellT3.AddElement(table);

            table5_2.AddCell(cellT5_2);
            #endregion

            #region tablo6 ana başlık kasa adı
            string PdfAdi6 = " SATIN ALINAN STOKLAR ";
            PdfPCell cellT6 = new PdfPCell(new Phrase(PdfAdi6, font));
            cellT6.Colspan = 6;
            cellT6.PaddingBottom = 5f;
            cellT6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT2.BackgroundColor = BaseColor.RED;
            cellT6.BorderColor = BaseColor.BLUE;
            //  cellT3.AddElement(table);

            table6.AddCell(cellT6);
            #endregion


            #region tablo2 başlıkları için  

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT11 = new PdfPCell(new Phrase("Genel", font3));
            PdfPCell cellT21 = new PdfPCell(new Phrase("TL", font3));
            PdfPCell cellT22 = new PdfPCell(new Phrase("$", font3));
            PdfPCell cellT23 = new PdfPCell(new Phrase("€", font3));
            PdfPCell cellT24 = new PdfPCell(new Phrase(" ", font3));
          
            //  cell1.Colspan = 1;
            //  cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //  cell1.BackgroundColor = BaseColor.YELLOW;
            cellT11.Colspan = 1;
            cellT11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
          //  cellT11.BackgroundColor = BaseColor.BLUE;
            cellT11.BorderColor = BaseColor.BLUE;
            cellT21.Colspan = 1;
            cellT21.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
          //  cellT21.BackgroundColor = BaseColor.BLUE;
            cellT21.BorderColor = BaseColor.BLUE;
            cellT22.Colspan = 1;
            cellT22.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT22.BackgroundColor = BaseColor.BLUE;
            cellT22.BorderColor = BaseColor.BLUE;
            cellT23.Colspan = 1;
            cellT23.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
          //  cellT23.BackgroundColor = BaseColor.BLUE;
            cellT23.BorderColor = BaseColor.BLUE;

            cellT24.Colspan = 1;
            cellT24.HorizontalAlignment = 1;
            cellT24.BorderWidth = 0;
           

            table2.AddCell(cellT11);
            table2.AddCell(cellT21);
            table2.AddCell(cellT22);
            table2.AddCell(cellT23);
            table2.AddCell(cellT24);

            #endregion

            #region tablo2_1 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT24_1 = new PdfPCell(new Phrase("  ", font3));
            PdfPCell cellT11_1 = new PdfPCell(new Phrase(" Gider ", font3));
            PdfPCell cellT21_1 = new PdfPCell(new Phrase("TL", font3));
            PdfPCell cellT22_1 = new PdfPCell(new Phrase("$", font3));
            PdfPCell cellT23_1 = new PdfPCell(new Phrase("€", font3));
           
            //  cell1.Colspan = 1;
            //  cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //  cell1.BackgroundColor = BaseColor.YELLOW;
            cellT11_1.Colspan = 1;
            cellT11_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT11_1.BorderColor= BaseColor.BLUE;
            cellT21_1.Colspan = 1;
            //cellT11_1.BorderWidth = 0;

            cellT21_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT21_1.BorderColor = BaseColor.BLUE;
            cellT22_1.Colspan = 1;
            cellT22_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT22_1.BorderColor = BaseColor.BLUE;
            cellT23_1.Colspan = 1;
            cellT23_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT23_1.BorderColor = BaseColor.BLUE;

            //cellT24_1.BorderWidth = 0;
            cellT24_1.HorizontalAlignment = 1; 
            cellT24_1.Colspan = 1;

           // table2_1.AddCell(cellT24_1);
            table2_1.AddCell(cellT11_1);
            table2_1.AddCell(cellT21_1);
            table2_1.AddCell(cellT22_1);
            table2_1.AddCell(cellT23_1);
           

            #endregion


            #region tablo3 başlıkları için
/*
            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT31 = new PdfPCell(new Phrase("Genel", font3));
            PdfPCell cellT32 = new PdfPCell(new Phrase("TL", font3));

            cellT31.Colspan =1;
            cellT31.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT31.BackgroundColor = BaseColor.BLUE;
             

            cellT32.Colspan = 9;
            cellT32.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT32.BackgroundColor = BaseColor.BLUE;

            table3.AddCell(cellT31);
            table3.AddCell(cellT32);*/

            #endregion

            #region tablo4 başlıkları için
/*
            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT41 = new PdfPCell(new Phrase("ÜRÜN ADI", font4));
            PdfPCell cellT42 = new PdfPCell(new Phrase("TL", font4));
            PdfPCell cellT43 = new PdfPCell(new Phrase("$", font4));
            PdfPCell cellT44 = new PdfPCell(new Phrase("€", font4));

            cellT41.Colspan = 1;
            cellT41.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT41.BackgroundColor = BaseColor.BLUE;
            cellT41.PaddingTop = 5f;
            cellT42.Colspan = 1;
            cellT42.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT42.BackgroundColor = BaseColor.BLUE;
            cellT42.PaddingTop = 5f;
            cellT43.Colspan = 1;
            cellT43.PaddingTop = 5f;
            cellT43.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT43.BackgroundColor = BaseColor.BLUE;
            cellT44.Colspan = 1;
            cellT44.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellT44.BackgroundColor = BaseColor.BLUE;
            cellT44.PaddingTop = 5f;

            table4.AddCell(cellT41);
            table4.AddCell(cellT42);
            table4.AddCell(cellT43);
            table4.AddCell(cellT44);*/

            #endregion

            #region tablo5 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT51 = new PdfPCell(new Phrase("ÜRÜN ADI", font2));
            PdfPCell cellT52 = new PdfPCell(new Phrase("TL", font2));
            PdfPCell cellT53 = new PdfPCell(new Phrase("$", font2));
            PdfPCell cellT54 = new PdfPCell(new Phrase("€", font2));

            cellT51.Colspan = 1;
            cellT51.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT51.BackgroundColor = BaseColor.BLUE;
            cellT52.Colspan = 1;
            cellT52.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT52.BackgroundColor = BaseColor.BLUE;
            cellT53.Colspan = 1;
            cellT53.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT53.BackgroundColor = BaseColor.BLUE;
            cellT54.Colspan = 1;
            cellT54.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT54.BackgroundColor = BaseColor.BLUE;

            table5.AddCell(cellT51);
            table5.AddCell(cellT52);
            table5.AddCell(cellT53);
            table5.AddCell(cellT54);

            #endregion

            #region tablo5_1 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT51_1 = new PdfPCell(new Phrase("Müşteri Adı", font2));
            PdfPCell cellT52_1 = new PdfPCell(new Phrase("TL", font2));
            PdfPCell cellT53_1 = new PdfPCell(new Phrase("$", font2));
            PdfPCell cellT54_1 = new PdfPCell(new Phrase("€", font2));

            cellT51_1.Colspan = 1;
            cellT51_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            // cellT51.BackgroundColor = BaseColor.BLUE;
            cellT52_1.Colspan = 1;
            cellT52_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT52.BackgroundColor = BaseColor.BLUE;
            cellT53_1.Colspan = 1;
            cellT53_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT53.BackgroundColor = BaseColor.BLUE;
            cellT54_1.Colspan = 1;
            cellT54_1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT54.BackgroundColor = BaseColor.BLUE;

            table5_1.AddCell(cellT51_1);
            table5_1.AddCell(cellT52_1);
            table5_1.AddCell(cellT53_1);
            table5_1.AddCell(cellT54_1);

            #endregion

            #region tablo5_2 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT51_2 = new PdfPCell(new Phrase("Ödeme Yapılan", font2));
            PdfPCell cellT52_2 = new PdfPCell(new Phrase("TL", font2));
            PdfPCell cellT53_2 = new PdfPCell(new Phrase("$", font2));
            PdfPCell cellT54_2 = new PdfPCell(new Phrase("€", font2));

            cellT51_2.Colspan = 1;
            cellT51_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            // cellT51.BackgroundColor = BaseColor.BLUE;
            cellT52_2.Colspan = 1;
            cellT52_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT52.BackgroundColor = BaseColor.BLUE;
            cellT53_2.Colspan = 1;
            cellT53_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT53.BackgroundColor = BaseColor.BLUE;
            cellT54_2.Colspan = 1;
            cellT54_2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT54.BackgroundColor = BaseColor.BLUE;

            table5_2.AddCell(cellT51_2);
            table5_2.AddCell(cellT52_2);
            table5_2.AddCell(cellT53_2);
            table5_2.AddCell(cellT54_2);

            #endregion

            #region tablo5_3 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT51_3 = new PdfPCell(new Phrase("Alınan Ürünler", font2));
            PdfPCell cellT52_3 = new PdfPCell(new Phrase("TL", font2));
            PdfPCell cellT53_3 = new PdfPCell(new Phrase("$", font2));
            PdfPCell cellT54_3 = new PdfPCell(new Phrase("€", font2));

            cellT51_3.Colspan = 1;
            cellT51_3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            // cellT51.BackgroundColor = BaseColor.BLUE;
            cellT52_3.Colspan = 1;
            cellT52_3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT52_3.= BaseColor.BLUE;
            cellT53_3.Colspan = 1;
            cellT53_3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT53.BackgroundColor = BaseColor.BLUE;
            cellT54_3.Colspan = 1;
            cellT54_3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT54.BackgroundColor = BaseColor.BLUE;

            table5_3.AddCell(cellT51_3);
            table5_3.AddCell(cellT52_3);
            table5_3.AddCell(cellT53_3);
            table5_3.AddCell(cellT54_3);

            #endregion

            #region tablo6 başlıkları için

            //PdfPCell cell1 = new PdfPCell(new Phrase("ID", font3));
            PdfPCell cellT61 = new PdfPCell(new Phrase("ÜRÜN ADI", font2));
            PdfPCell cellT62 = new PdfPCell(new Phrase("Adet", font2));
            PdfPCell cellT63 = new PdfPCell(new Phrase("TL", font2));
            PdfPCell cellT64 = new PdfPCell(new Phrase("$", font2));
            PdfPCell cellT65 = new PdfPCell(new Phrase("€", font2));
            PdfPCell cellT66 = new PdfPCell(new Phrase("Toplam", font2));

            cellT61.Colspan = 1;
            cellT61.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT61.BorderColor = BaseColor.BLUE;
            cellT62.Colspan = 1;
            cellT62.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT62.BorderColor = BaseColor.BLUE;
            cellT63.Colspan = 1;
            cellT63.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT63.BorderColor = BaseColor.BLUE;
            cellT64.Colspan = 1;
            cellT64.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT64.BorderColor = BaseColor.BLUE;

            cellT65.Colspan = 1;
            cellT65.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cellT65.BorderColor = BaseColor.BLUE;

            cellT66.Colspan = 1;
            cellT66.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
           // cellT66.BorderColor = BaseColor.BLUE;

            table6.AddCell(cellT61);
            table6.AddCell(cellT62);
            table6.AddCell(cellT63);
            table6.AddCell(cellT64);
            table6.AddCell(cellT65);
            table6.AddCell(cellT66);

            #endregion


            if (raporum.IsOpen() == false)
            { raporum.Open(); }

            Paragraph eklenecekmetin = new Paragraph("paragraf");
          //  raporum.AddHeader("HEADER",null);

 
           //raporum.Add(eklenecekmetin);

            int SatirNumarası = 0;
            int SutunNumarası = 0;
            string yazi;

     #region tablo4 başlıkları için
            PdfPCell cellT4_1 = new PdfPCell();
            String yaziPDF;
            yaziPDF = "";
            for (int i = 0; i < 1;i++)
            {

                SutunNumarası = 0;
                for (int k = 0; k < 4; k++ )
                {

                    if (SutunNumarası < 4)
                    {
                        if (k == 0) yaziPDF = "GÜNLÜK GENEL TOPLAM(DEVİR)=";
                        if (k == 1) yaziPDF = GenelToplamTL.ToString() + "  --tl";
                        if (k == 2) yaziPDF = GenelToplamDOLAR.ToString() +"  --$";
                        if (k == 3) yaziPDF = GenelToplamEURO.ToString() +"  --€";

                        yazi = yaziPDF;
                        if (yazi.Trim() == "")
                            yazi = "0";
                        cellT4_1 = new PdfPCell(new Phrase(yazi.TrimEnd('2').ToString().TrimEnd('.').ToString(), fontBuyukYazi));
                        cellT4_1.Colspan = 1;
                        cellT4_1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellT4_1.BorderWidth = 2;
                        cellT4_1.BorderColor = BaseColor.BLUE;
                        cellT4_1.PaddingTop = 5f;
                        cellT4_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cellT4_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;


                        table4.AddCell(cellT4_1);

                    }
                }

            }
     #endregion

            #region tablo2 veri doldurulması

            /////////////////
            PdfPCell celldT2_1 = new PdfPCell();

            if (baglanti.State == ConnectionState.Closed)
            baglanti.Open();
            if (baglanti.State == ConnectionState.Open)
              {
                  baglanti.Close();
                  baglanti.Open();

              }

            String sqlT2 = "SELECT CC.ODEMETIPI,SUM(CC.TL) AS TL, SUM(CC.EURO) AS EURO, SUM(CC.DOLAR) AS DOLAR FROM customer_case cc WHERE " +
                "     (cc.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                " GROUP BY CC.ODEMETIPI";
                           
            dt.Clear();
            command.CommandText = sqlT2;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

           
            adapter.Fill(dt);
            grid.lgvgriddata.DataSource = dt;

            //VERESİYE SATIS TABLOSU DOLDUR
            baglanti.Close();
            int strsayisi2;
            strsayisi2 = grid.lgvgriddata.Columns.Count;
            
            /////////////////////
            
            SatirNumarası = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata.Columns)
                {

                    if (SutunNumarası < strsayisi2 +1 )
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (SutunNumarası + 1 == strsayisi2)
                            yazi = " ";
                        else if (grid.lgvgriddata.Rows[SatirNumarası].Cells[SutunNumarası+1].Value == null)
                            yazi = "0";
                        else     //frmSList.lgvdtgridSalesList.Rows[2].Cells[3].Value.ToString();
                            yazi = Satir.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası+1].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        if ((yazi.Trim() == "") && (SutunNumarası + 1 != strsayisi2))
                            yazi = "0";
                        if ((yazi == "0") && (SatirNumarası == 3))
                            yazi = ""; 
                        //eklenecekmetin = new Paragraph(yazi);
                        //if (SutunNumarası != 0)
                          celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd('2').ToString().TrimEnd('.').ToString(), font2));
                        //else celldT2_1 = new PdfPCell(new Phrase(" ", font2));
                       // celldT2_1.AddElement(eklenecekmetin);
                          if ((SutunNumarası == 4) || (SatirNumarası == 3))
                          celldT2_1.BorderWidth = 0;
                        else celldT2_1.BorderWidth = 1;

                        celldT2_1.BorderColor = BaseColor.BLUE;

                    
                        table2.AddCell(celldT2_1);
                     

                    }
                
                    if ((SutunNumarası + 1 == strsayisi2) && (SatirNumarası==3))
                    {
                        String yaziPdf;
                        yaziPdf = "";
                        for (int j = 0; j < 2; j++)
                        {
                            for (int t = 0; t < 5; t++)
                            {
                                
                            
                                //else celldT2_1 = new PdfPCell(new Phrase(" ", font2));
                                // celldT2_1.AddElement(eklenecekmetin);
                                if (t == 4)
                                {
                                    yaziPdf = " ";
                                }
                                else if ((j == 0) && (t == 0)) yaziPdf = "GÜNLÜK TOPLAM GELİR = ";
                                else if ((j == 0) && (t == 1)) yaziPdf = ToplamGelirTL.ToString() + "/tl";
                                else if ((j == 0) && (t == 2)) yaziPdf = ToplamGelirDOLAR.ToString() + "/$";
                                else if ((j == 0) && (t == 3)) yaziPdf = ToplamGelirEURO.ToString() + "/€";
                                else if ((j == 1) && (t == 0)) yaziPdf = "GÜNLÜK TOPLAM GİDER = ";
                                else if ((j == 1) && (t == 1)) yaziPdf = ToplamGiderTL.ToString() + "/tl" ;
                                else if ((j == 1) && (t == 2)) yaziPdf = ToplamGiderDOLAR.ToString() + "/$" ;
                                else if ((j == 1) && (t == 3)) yaziPdf = ToplamGiderEURO.ToString() + "/€";

                             

                                yazi = yaziPdf;
                                celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd('2').ToString().TrimEnd('.').ToString(), font2));

                                if (t == 4)
                                {
                                    celldT2_1.BorderWidth = 0;
                                }
                                else celldT2_1.BorderWidth = 1;

                                celldT2_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                celldT2_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                 celldT2_1.BorderColor = BaseColor.BLUE;

                                table2.AddCell(celldT2_1);
                            }
                        }
                     }
                    


                    
                    SutunNumarası = SutunNumarası + 1;
                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;
                SatirNumarası = SatirNumarası + 1;

            }


#endregion
    #region tablo2_1 veri doldurulması

            /////////////////
            PdfPCell celldT2_1_1 = new PdfPCell();
            command = new MySqlCommand();
            baglanti = new MySqlConnection(bag);
            DataGridView dataGridv = new DataGridView();
            grid = new frmGrid();
            baglanti.Close();

            String yazilcakDeger;
            yazilcakDeger = "";

            SatirNumarası = 0;
            for (int i = 0; i < 4; i++ )
            {

                SutunNumarası = 0;
                for (int j = 0; j < 4; j++ )
                {

                    if (SutunNumarası < strsayisi2 - 1)
                    {

                        if (SatirNumarası == 0 && SutunNumarası == 0)
                        {

                            yazilcakDeger = "YAPILAN ÖDEME : ";
                        }
                        else if (SatirNumarası == 0 && SutunNumarası == 1)
                        {
                            yazilcakDeger = SonucTL;//Satir.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası+1].Value.ToString();

                        }
                        else if (SatirNumarası == 0 && SutunNumarası == 2)
                        {
                            yazilcakDeger = SonucDOLAR;
                        }
                        else if (SatirNumarası == 0 && SutunNumarası == 3) //(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        {
                            yazilcakDeger = SonucEURO;

                        }
                        else if (SatirNumarası == 1 && SutunNumarası == 0)
                        {
                            yazilcakDeger = "ÜRÜN ALMA ÜCRETİ : ";
                        }
                        else if (SatirNumarası == 1 && SutunNumarası == 1)
                        {
                            yazilcakDeger = SonucSKTL;
                        }
                        else if (SatirNumarası == 1 && SutunNumarası == 2)
                        {
                            yazilcakDeger = SonucSKDOLAR;
                        }
                        else if (SatirNumarası == 1 && SutunNumarası == 3)
                        {
                            yazilcakDeger = SonucSKEURO;
                        }
                        else if (SatirNumarası == 3 && SutunNumarası == 0)
                        {
                            yazilcakDeger = "TOLAM = ";
                        }
                        else if (SatirNumarası == 3 && SutunNumarası == 1)
                        {
                            yazilcakDeger = ToplamGiderTL.ToString() + "/tl" ;
                                                          
                        }
                        else if (SatirNumarası == 3 && SutunNumarası == 2)
                        {
                            yazilcakDeger = ToplamGiderDOLAR.ToString() + "/$";
                        }
                        else if (SatirNumarası == 3 && SutunNumarası == 3)
                        {
                            yazilcakDeger = ToplamGiderEURO.ToString() + "/€";
                        }

                        yazi = yazilcakDeger;

                        if (yazi.Trim() == "")
                            yazi = "0";
                        if (SatirNumarası == 2)
                        {     
                            yazi = " ";
                        }

                        celldT2_1_1 = new PdfPCell(new Phrase(yazi.TrimEnd('.').ToString().TrimEnd('.').ToString(), font2));
                        celldT2_1_1.BorderColor = BaseColor.BLUE;
                        celldT2_1_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT2_1_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        if (SatirNumarası == 2) celldT2_1_1.BorderWidth = 0;

                        table2_1.AddCell(celldT2_1_1);

                    }
                    SutunNumarası = SutunNumarası + 1;
                }
                SatirNumarası = SatirNumarası + 1;

            }


            #endregion

  #region tablo3 veri doldurulması
            SatirNumarası = 0;
            for (int i = 1; i < 3; i++)
            {

                SutunNumarası = 0;
                for (int k = 1; k < 3; k++)
                {
                    SutunNumarası = SutunNumarası + 1;
                    if (SutunNumarası < 8)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if ((SatirNumarası == 0) && (SutunNumarası == 1))
                        {
                           table3.AddCell(table2);   // yazi = "Günlüş Satış";
                        }
                        else if ((SatirNumarası == 0) && (SutunNumarası == 2))
                        {
                            table3.AddCell(table2_1);//  yazi = "Veresiye Tahsilat";
                          //  table3.AddCell(table2_1);//  yazi = "Veresiye Tahsilat";

                        }
                        else if ((SatirNumarası == 1) && (SutunNumarası == 1))
                        {
                           // table3.AddCell(table4);
                        }
                    }

                
                }

                SatirNumarası = SatirNumarası + 1;

            }

  #endregion
            #region tablo5 nin satırlarının oldurulduğu  durum

            PdfPCell celldT5_1 = new PdfPCell();
            String pdfYazdir2;
            int strsayisi5, strsayisi5_2, strsayisirows;

            String sqlT5 = "SELECT CONCAT(sl.NAME, ' ', CONCAT('(', sl.CODE, ')')) AS SNAME," +
                           "     dsc.TL, dsc.DOLAR, dsc.EURO" +  // dsc.PIECES,
                           " FROM" +
                           "     daily_sale_case dsc" +
                           " JOIN stock_list sl ON" +
                           "     DSC.SL_ID = SL.ID" +
                           " WHERE" +
                           "     (dsc.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                           "     AND (dsc.ODEMETIPI = 'Veresiye')" +
                           "ORDER BY dsc.DATE";
            dt.Clear();
            command.CommandText = sqlT5;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata2.DataSource = dt;
            baglanti.Close();
            strsayisi5 = grid.lgvgriddata2.Columns.Count;
            strsayisirows = grid.lgvgriddata2.Rows.Count;

                pdfYazdir2 = "";
            SatirNumarası = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata2.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata2.Columns)
                {

                    if (SutunNumarası < strsayisi5-2)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (grid.lgvgriddata2.Rows[SatirNumarası].Cells[SutunNumarası+2].Value == null)
                            pdfYazdir2 = " ";
                        else if (SutunNumarası == 0) pdfYazdir2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 5].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        else if (SutunNumarası == 1) pdfYazdir2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 2) pdfYazdir2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 2].Value.ToString();
                        else if (SutunNumarası == 3) pdfYazdir2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası].Value.ToString();

                        yazi = pdfYazdir2;

                        celldT5_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                        celldT5_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1.BorderColor = BaseColor.BLUE;

                        table5.AddCell(celldT5_1);

                    }
#region tablo5 e eklenen devam
                    /*if ((SutunNumarası + 1 == strsayisi5) && (SatirNumarası == 3))
                    {
                        String yaziPdf;
                        yaziPdf = "";
                        for (int j = 0; j < 2; j++)
                        {
                            for (int t = 0; t < 4; t++)
                            {


                                //else celldT2_1 = new PdfPCell(new Phrase(" ", font2));
                                // celldT2_1.AddElement(eklenecekmetin);
                                if (t == 4)
                                {
                                    yaziPdf = " ";
                                }
                                else if ((j == 0) && (t == 0)) yaziPdf = "  ";
                                else if ((j == 0) && (t == 1)) yaziPdf = " ";
                                else if ((j == 0) && (t == 2)) yaziPdf = " ";
                                else if ((j == 0) && (t == 3)) yaziPdf = " ";
                                else if ((j == 1) && (t == 0)) yaziPdf = "VERESİYE TAHSİLATLAR = ";
                                else if ((j == 1) && (t == 1)) yaziPdf = " tl";
                                else if ((j == 1) && (t == 2)) yaziPdf =  " $";
                                if ((j == 1) && (t == 3)) 
                                {
                                    yaziPdf = " €";
                                    yazi = yaziPdf;
                                    celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd(':').ToString(), font2));

                                    ///
                                    PdfPCell celldT5_2 = new PdfPCell();
                                    String pdfYazdir5_2;
                                    adapter = new MySqlDataAdapter();
                                    command = new MySqlCommand();

                                    String sqlT5_2 = " SELECT P.CUSTOMER, P.TL, P.EURO, P.DOLAR FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME'" +
                                                   " AND p.PAY_CARNEL = 'Veresiye'"+
                                                   "AND" +
                                                   "     (P.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                                                   " ORDER BY P.DATE";
                                    dt.Clear();
                                    command.CommandText = sqlT5_2;
                                    command.Connection = baglanti;
                                    adapter.SelectCommand = command;

                                    baglanti.Open();
                                    adapter.Fill(dt);
                                    grid.lgvgriddata.DataSource = dt;

                                    //VERESİYE SATIS TABLOSU DOLDUR
                                    baglanti.Close();

                                    ///

                                    int SatirNumarasi2, SutunNumarasi2, strsayisi2rows;

                                    strsayisi5_2 = grid.lgvgriddata.Columns.Count;
                                    strsayisi2rows = grid.lgvgriddata.Rows.Count;





                                    pdfYazdir5_2 = "";
                                    if ((j == 0) && ((t == 0) || (t == 1) || (t == 2) || (t == 3)))
                                    {
                                        celldT2_1.BorderWidth = 0;
                                    }
                                    else celldT2_1.BorderWidth = 1;

                                    celldT2_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    celldT2_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                    celldT2_1.BorderColor = BaseColor.BLUE;

                                    table5.AddCell(celldT2_1);

                                SatirNumarasi2 = 0;
                                foreach (DataGridViewRow Satir2 in grid.lgvgriddata.Rows)
                                {

                                    SutunNumarasi2 = 0;
                                    foreach (DataGridViewColumn Sutun2 in grid.lgvgriddata.Columns)
                                    {

                                        if (SutunNumarasi2 < strsayisi2rows)
                                        {
                                            //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                                            if (grid.lgvgriddata.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value == null)
                                                pdfYazdir5_2 = " ";
                                            else if (SutunNumarasi2 == 0) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 6].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                                            else if (SutunNumarasi2 == 1) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 1].Value.ToString();
                                            else if (SutunNumarasi2 == 2) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value.ToString();
                                            else if (SutunNumarasi2 == 3) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2].Value.ToString();
                                           // else
                                            //    pdfYazdir5_2 = "str:" + SatirNumarasi2.ToString() + " stn:" + SutunNumarasi2.ToString() + Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value.ToString();
                                            yazi = pdfYazdir5_2;

                                            celldT5_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                                            celldT5_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            celldT5_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                                            table5.AddCell(celldT5_1);

                                        }
                                        
                                        SutunNumarasi2 = SutunNumarasi2 + 1;
                                    }
                                    //    if (SatirNumarası == 4)
                                    //        SatirNumarası = 1;
                                    SatirNumarasi2 = SatirNumarasi2 + 1;

                                }
                                
                                }
                                else
                               { 

                                yazi = yaziPdf;
                                celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd(':').ToString(), font2));

                                if ((j == 0) && ((t == 0) || (t == 1) || (t == 2) || (t == 3)))
                                {
                                    celldT2_1.BorderWidth = 0;
                                }
                                else celldT2_1.BorderWidth = 1;

                                celldT2_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                celldT2_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                celldT2_1.BorderColor = BaseColor.BLUE;

                                table5.AddCell(celldT2_1);
                            }
                            }
                        }*/
                    ///1. son
                        /////////////////////
                        /*
                        //String yaziPdf;
                        yaziPdf = "";
                        for (int j = 0; j < 2; j++)
                        {
                            for (int t = 0; t < 4; t++)
                            {


                                //else celldT2_1 = new PdfPCell(new Phrase(" ", font2));
                                // celldT2_1.AddElement(eklenecekmetin);
                                if (t == 4)
                                {
                                    yaziPdf = " ";
                                }
                                else if ((j == 0) && (t == 0)) yaziPdf = "  ";
                                else if ((j == 0) && (t == 1)) yaziPdf = " ";
                                else if ((j == 0) && (t == 2)) yaziPdf = " ";
                                else if ((j == 0) && (t == 3)) yaziPdf = " ";
                                else if ((j == 1) && (t == 0)) yaziPdf = "VERESİYE TAHSİLATLAR = ";
                                else if ((j == 1) && (t == 1)) yaziPdf = " tl";
                                else if ((j == 1) && (t == 2)) yaziPdf =  " $";
                                if ((j == 1) && (t == 3)) 
                                {
                                    yaziPdf = " €";
                                    yazi = yaziPdf;
                                    celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd(':').ToString(), font2));

                                    ///
                                    PdfPCell celldT5_2 = new PdfPCell();
                                    String pdfYazdir5_2;
                                    adapter = new MySqlDataAdapter();
                                    command = new MySqlCommand();

                                    String sqlT5_2 = " SELECT P.CUSTOMER, P.TL, P.EURO, P.DOLAR FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME'" +
                                                   " AND p.PAY_CARNEL = 'Veresiye'"+
                                                   "AND" +
                                                   "     (P.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                                                   " ORDER BY P.DATE";
                                    dt.Clear();
                                    command.CommandText = sqlT5_2;
                                    command.Connection = baglanti;
                                    adapter.SelectCommand = command;

                                    baglanti.Open();
                                    adapter.Fill(dt);
                                    grid.lgvgriddata.DataSource = dt;

                                    //VERESİYE SATIS TABLOSU DOLDUR
                                    baglanti.Close();

                                    ///

                                    int SatirNumarasi2, SutunNumarasi2, strsayisi2rows;

                                    strsayisi5_2 = grid.lgvgriddata.Columns.Count;
                                    strsayisi2rows = grid.lgvgriddata.Rows.Count;





                                    pdfYazdir5_2 = "";
                                    if ((j == 0) && ((t == 0) || (t == 1) || (t == 2) || (t == 3)))
                                    {
                                        celldT2_1.BorderWidth = 0;
                                    }
                                    else celldT2_1.BorderWidth = 1;

                                    celldT2_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    celldT2_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                    celldT2_1.BorderColor = BaseColor.BLUE;

                                    table5.AddCell(celldT2_1);

                                SatirNumarasi2 = 0;
                                foreach (DataGridViewRow Satir2 in grid.lgvgriddata.Rows)
                                {

                                    SutunNumarasi2 = 0;
                                    foreach (DataGridViewColumn Sutun2 in grid.lgvgriddata.Columns)
                                    {

                                        if (SutunNumarasi2 < strsayisi2rows)
                                        {
                                            //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                                            if (grid.lgvgriddata.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value == null)
                                                pdfYazdir5_2 = " ";
                                            else if (SutunNumarasi2 == 0) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 6].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                                            else if (SutunNumarasi2 == 1) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 1].Value.ToString();
                                            else if (SutunNumarasi2 == 2) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value.ToString();
                                            else if (SutunNumarasi2 == 3) pdfYazdir5_2 = Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2].Value.ToString();
                                           // else
                                            //    pdfYazdir5_2 = "str:" + SatirNumarasi2.ToString() + " stn:" + SutunNumarasi2.ToString() + Sutun2.DataGridView.Rows[SatirNumarasi2].Cells[SutunNumarasi2 + 2].Value.ToString();
                                            yazi = pdfYazdir5_2;

                                            celldT5_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                                            celldT5_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            celldT5_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                                            table5.AddCell(celldT5_1);

                                        }
                                        
                                        SutunNumarasi2 = SutunNumarasi2 + 1;
                                    }
                                    //    if (SatirNumarası == 4)
                                    //        SatirNumarası = 1;
                                    SatirNumarasi2 = SatirNumarasi2 + 1;

                                }
                                
                                }
                                else
                               { 

                                yazi = yaziPdf;
                                celldT2_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd(':').ToString(), font2));

                                if ((j == 0) && ((t == 0) || (t == 1) || (t == 2) || (t == 3)))
                                {
                                    celldT2_1.BorderWidth = 0;
                                }
                                else celldT2_1.BorderWidth = 1;

                                celldT2_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                celldT2_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                celldT2_1.BorderColor = BaseColor.BLUE;

                                table5.AddCell(celldT2_1);
                            }
                            }
                        }
                        */
                    //2.son
                        /////////////////////


                    // }
#endregion
                    if ((SutunNumarası + 1 == strsayisi5) && (SatirNumarası == 3))
                    break;
                    SutunNumarası = SutunNumarası + 1; 
                   
                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;
               
                SatirNumarası = SatirNumarası + 1;

            }


            #endregion
            #region tablo6 nin satırlarının oldurulduğu  durum
            PdfPCell celldT6_1 = new PdfPCell();
            baglanti = new MySqlConnection(bag);
            command = new MySqlCommand();
            grid.lgvgriddata2.DataSource = null;
            String pdfYazdir;

            String sqlT6=" ";
           sqlT6 = "SELECT "+
                        "    CONCAT("+
                        "        sl.NAME,"+
                        "        ' ',"+
                        "        CONCAT('(', sl.CODE, ')')"+

                        "    ) AS SNAME,"+
                        "    SLC.PIECES,"+
                        "    SLC.TL,"+
                        "    SLC.DOLAR,"+
                        "    SLC.EURO"+
                        " FROM"+
                        "    stock_list SL "+
                        "JOIN stock_list_case SLC ON "+
                           " SLC.CODE = SL.CODE " +
                        " WHERE" +
                        "  (SLC.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")"+
                        "ORDER BY SLC.DATE";
                           
            dt.Clear();
            //dt = new DataTable();
            //adapter = new MySqlDataAdapter();
            command.CommandText = sqlT6;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata2.DataSource = dt;

            //VERESİYE SATIS TABLOSU DOLDUR
            baglanti.Close();
            int strsayisi6, adet, stnsayisi6;
            Double tl, dolar, euro;
            strsayisi6 = grid.lgvgriddata2.Columns.Count;
            stnsayisi6 = grid.lgvgriddata2.Rows.Count;
            pdfYazdir = " ";
            SatirNumarası = 0;
            adet = 0;
            tl = 0; dolar = 0; euro = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata2.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata2.Columns)
                {

                    if (SutunNumarası < strsayisi6 -1)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (grid.lgvgriddata2.Rows[SatirNumarası].Cells[SutunNumarası].Value == null)
                            pdfYazdir = "0";
                        else if (SutunNumarası == 0) pdfYazdir = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 5].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        else if (SutunNumarası == 1) //adet
                        {
                            pdfYazdir = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 5].Value.ToString();
                            if (pdfYazdir.Trim() == "") pdfYazdir = "0";
                            adet = Convert.ToInt32(pdfYazdir);
                        }
                        else if (SutunNumarası == 2)//tl
                        {
                            pdfYazdir = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası ].Value.ToString();
                             tl = Convert.ToDouble(pdfYazdir); 
                        }
                        else if (SutunNumarası == 3) //dolar
                        {
                            pdfYazdir = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası +1 ].Value.ToString();
                            dolar = Convert.ToDouble(pdfYazdir);
                        }
                        else if (SutunNumarası == 4){ //euro
                            pdfYazdir = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası -1].Value.ToString();
                            euro = Convert.ToDouble(pdfYazdir);
                        }
                        else if (SutunNumarası == 5)
                        {
                            if(tl > 0)
                              pdfYazdir = (adet * tl).ToString() + "/tl";
                            else if (dolar > 0)
                                pdfYazdir = (adet * dolar).ToString()+"/$";

                            else if (euro > 0)
                                pdfYazdir = (adet * euro).ToString()+"/€";

                        }
                        yazi = pdfYazdir;

                        celldT6_1 = new PdfPCell(new Phrase(yazi.ToString(), font2));//new Phrase(yazi.TrimEnd(':').ToString().TrimEnd(':').ToString(), font2)
                        celldT6_1.BorderColor = BaseColor.BLUE;

                        table6.AddCell(celldT6_1);
                        //table.AddCell(table2);

                    }
                    SutunNumarası = SutunNumarası + 1;
                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;
                SatirNumarası = SatirNumarası + 1;

            }
            #endregion

            #region tablo5_1 nin satırlarının oldurulduğu  durum

            PdfPCell celldT5_1_1 = new PdfPCell();
            String pdfYazdir2_1;
            int strsayisi5_1, strsayisirows_1;

            String sqlT5_1 = " SELECT P.CUSTOMER, P.TL, P.EURO, P.DOLAR FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME'" +
                                                   " AND p.PAY_CARNEL = 'Veresiye'" +
                                                   "AND" +
                                                   "     (P.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                                                   " ORDER BY P.DATE";
            dt.Clear();
            command.CommandText = sqlT5_1;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata.DataSource = dt;
            baglanti.Close();

            strsayisi5_1 = grid.lgvgriddata.Columns.Count;
            strsayisirows_1 = grid.lgvgriddata.Rows.Count;

            pdfYazdir2_1 = "";
            SatirNumarası = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata.Columns)
                {

                    if (SutunNumarası < strsayisi5_1 - 2)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (grid.lgvgriddata.Rows[SatirNumarası].Cells[SutunNumarası].Value == null)
                            pdfYazdir2_1 = " ";
                        else if (SutunNumarası == 0) pdfYazdir2_1 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 7].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        else if (SutunNumarası == 1) pdfYazdir2_1 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 2) pdfYazdir2_1 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 3) pdfYazdir2_1 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                         
                      //   else
                       //     pdfYazdir2_1 = "str:" + SatirNumarası.ToString() + " stn:" + SutunNumarası.ToString()+ "deg=" + Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası].Value.ToString();
                        yazi = pdfYazdir2_1;

                        celldT5_1_1 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                        celldT5_1_1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1_1.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1_1.BorderColor = BaseColor.BLUE;
                        if (SutunNumarası <= 3)
                        table5_1.AddCell(celldT5_1_1);

                    }

                    SutunNumarası = SutunNumarası + 1;

                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;

                SatirNumarası = SatirNumarası + 1;

            }


            #endregion

            #region tablo5_2 nin satırlarının oldurulduğu  durum

            PdfPCell celldT5_1_2 = new PdfPCell();
            String pdfYazdir2_2;
            int strsayisi5_1_2, strsayisirows_2;

            String sqlT5_2 = " SELECT P.CUSTOMER, P.TL, P.EURO, P.DOLAR FROM payments p  WHERE p.PAY_TYPE = 'YAPILAN ÖDEME'" +
                                                   "AND" +
                                                   "     (P.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                                                   " ORDER BY P.DATE";
            dt.Clear();
            command.CommandText = sqlT5_2;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata.DataSource = dt;
            baglanti.Close();

            strsayisi5_1_2 = grid.lgvgriddata.Columns.Count;
            strsayisirows_2 = grid.lgvgriddata.Rows.Count;

            pdfYazdir2_2 = "";
            SatirNumarası = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata.Columns)
                {

                    if (SutunNumarası < strsayisi5_1 - 2)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (grid.lgvgriddata.Rows[SatirNumarası].Cells[SutunNumarası].Value == null)
                            pdfYazdir2_2 = " ";
                        else if (SutunNumarası == 0) pdfYazdir2_2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 7].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        else if (SutunNumarası == 1) pdfYazdir2_2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 2) pdfYazdir2_2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 3) pdfYazdir2_2 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();

                        //   else
                        //     pdfYazdir2_1 = "str:" + SatirNumarası.ToString() + " stn:" + SutunNumarası.ToString()+ "deg=" + Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası].Value.ToString();
                        yazi = pdfYazdir2_2;

                        celldT5_1_2 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                        celldT5_1_2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1_2.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1_2.BorderColor = BaseColor.BLUE;
                        if (SutunNumarası <= 3)
                            table5_2.AddCell(celldT5_1_2);

                    }

                    SutunNumarası = SutunNumarası + 1;

                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;

                SatirNumarası = SatirNumarası + 1;

            }


            #endregion

            #region tablo5_3 nin satırlarının oldurulduğu  durum //iptal
/*
            PdfPCell celldT5_1_3 = new PdfPCell();
            String pdfYazdir2_3;
            int strsayisi5_1_3, strsayisirows_3;

            String sqlT5_3 = " SELECT P.CUSTOMER, P.TL, P.EURO, P.DOLAR FROM payments p  WHERE p.PAY_TYPE = 'ALINAN ÖDEME'" +
                                                   " AND p.PAY_CARNEL = 'Veresiye'" +
                                                   "AND" +
                                                   "     (P.DATE BETWEEN '" + yr3 + "-" + m3 + "-" + d3 + "' AND " + "'" + yr3 + "-" + m3 + "-" + d4 + "'" + ")" +
                                                   " ORDER BY P.DATE";
            dt.Clear();
            command.CommandText = sqlT5_3;
            command.Connection = baglanti;
            adapter.SelectCommand = command;

            baglanti.Open();
            adapter.Fill(dt);
            grid.lgvgriddata.DataSource = dt;
            baglanti.Close();

            strsayisi5_1_3 = grid.lgvgriddata.Columns.Count;
            strsayisirows_3 = grid.lgvgriddata.Rows.Count;

            pdfYazdir2_3 = "";
            SatirNumarası = 0;
            foreach (DataGridViewRow Satir in grid.lgvgriddata.Rows)
            {

                SutunNumarası = 0;
                foreach (DataGridViewColumn Sutun in grid.lgvgriddata.Columns)
                {

                    if (SutunNumarası < strsayisi5_1_3 - 2)
                    {
                        //myvalue =dgvList.Rows[rowindex].Cells[columnindex].Value.ToString();
                        if (grid.lgvgriddata.Rows[SatirNumarası].Cells[SutunNumarası].Value == null)
                            pdfYazdir2_3 = " ";
                        else if (SutunNumarası == 0) pdfYazdir2_3 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 7].Value.ToString();//(Satir.DataGridView.Columns[Sutun.DataGridView.Columns[Sutun.Index].Name].Name);
                        else if (SutunNumarası == 1) pdfYazdir2_3 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 2) pdfYazdir2_3 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();
                        else if (SutunNumarası == 3) pdfYazdir2_3 = Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası + 1].Value.ToString();

                        //   else
                        //     pdfYazdir2_1 = "str:" + SatirNumarası.ToString() + " stn:" + SutunNumarası.ToString()+ "deg=" + Sutun.DataGridView.Rows[SatirNumarası].Cells[SutunNumarası].Value.ToString();
                        yazi = pdfYazdir2_3;

                        celldT5_1_3 = new PdfPCell(new Phrase(yazi.TrimEnd(':').ToString().TrimEnd('.').ToString(), font2));
                        celldT5_1_3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        celldT5_1_3.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        if (SutunNumarası <= 3)
                            table5_3.AddCell(celldT5_1_3);

                    }

                    SutunNumarası = SutunNumarası + 1;

                }
                //    if (SatirNumarası == 4)
                //        SatirNumarası = 1;

                SatirNumarası = SatirNumarası + 1;

            }
                */

            #endregion//iptal


            #region tablo veri doldurulması

            for (int i = 0; i < 3; i++)
            {


                for (int k = 0; k < 2; k++)
                {
                    if ((i == 0) && (k == 0))  //veresiye satışlar
                    {                   
                        table_1.AddCell(table5);
                    }
                    else if ((i == 0) && (k == 1))  // satın alınan stoklar
                    {

                        table_2.AddCell(table6);

                    }
                    else if ((i == 1) && (k == 0))
                    {
                        table_1.AddCell(table5_1);
                    }
                    else if ((i == 2) && (k == 0))
                    {
                        table_1.AddCell(table5_2);
                    }
                   
                }

            }
#endregion

            //table2.DeleteRow(1);

            table.AddCell(table_1);
            table.AddCell(table_2);
            raporum.Add(table3);
            raporum.Add(table4);
            raporum.Add(table);

            
            raporum.Close();
            MessageBox.Show("Fatura pdf i oluşturuldu");
            }
            else
            { MessageBox.Show("Lütfen kayıt yolu secin"); }
            

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AddPerson frmPerson = new AddPerson();
            frmPerson.Show();
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void tcPrices_KeyPress(object sender, KeyPressEventArgs e)
        {/*
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.';
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }*/

            if (char.IsDigit(e.KeyChar))
            {

                // txtboxtaki valuenin  değerinin bizim listimize aktarılması 
                SetValue(1);
                // listimizdeki valuenin textboxa aktarılması 
                SetTextbox(1);
            }
            else
            {
                e.Handled = true;
            } 
   
            //float num = float.Parse(textBox1.Text);
          //  string stringValue = num.ToString().Replace(',', '.');
        }

        public void SetValue(int a)
        {
            // adım adım listeye aktarım 

            tcPricesValue.Clear();
            if (a == 1)
            {
                for (int i = 0; i < tcPrices.Text.Length; i++)
                {
                    tcPricesValue.Add(tcPrices.Text[i]);
                }
            }
            if (a == 2)
            {
                for (int i = 0; i < tbPrice.Text.Length; i++)
                {
                    tcPricesValue.Add(tbPrice.Text[i]);
                }
            }
            if (a == 3)
            {
                for (int i = 0; i < tbPricePay.Text.Length; i++)
                {
                    tcPricesValue.Add(tbPricePay.Text[i]);
                }
            }
        }
        public void SetTextbox(int a)
        {
            // boş string tanımladık 
            string s = string.Empty;
            //eğer  bizim listimizdeki sayı sayısı 2 den büyük ise  sonran 2. karakterin önüne virgül atma şeysi 
            if (tcPricesValue.Count > 2)
            {
                // önce listimizdeki tüm virgülleri kaldırdık 
                tcPricesValue.Remove(',');
                // listimizin sondan 3. karakterine gene virgül koyduk 
                tcPricesValue.Insert(tcPricesValue.Count - 1, ',');
            }


            //mod 3 e  göre  değer  0 ise . koyalım dedik ama  virgülü de unutmadık bu yuzden 5 den büyükse eleman sayısı nokta  koyduk 
            if (tcPricesValue.Count >= 6)
            {
                // tüm . ları temizliyorum. 
                RemovePoints();
                //kaç tane Point koyacağız 
                int pointCount = 1 + (tcPricesValue.Count - 6) / 3;

                for (int i = 1; i <= pointCount; i++)
                {
                    int pointposition = ((tcPricesValue.Count - 4) - (i * 1)) - ((i - 1) * 3);
                    tcPricesValue.Insert(pointposition, '.');
                }
            }

            //tcPricesValue listimizdeki değrleri string yapıyorum ve textboxa atıyorum. 
            for (int i = 0; i < tcPricesValue.Count; i++)
            {
                s += tcPricesValue[i];
            }
            if (a == 1)
            {
                tcPrices.Text = s;
                //burada cursorun hep en sağda kalmasını sağlıyorum 
                tcPrices.SelectionStart = tcPrices.Text.Length;
            }
            if (a == 2)
            {
                tbPrice.Text = s;
                //burada cursorun hep en sağda kalmasını sağlıyorum 
                tbPrice.SelectionStart = tbPrice.Text.Length;
            }
            if (a == 3)
            {
                tbPricePay.Text = s;
                //burada cursorun hep en sağda kalmasını sağlıyorum 
                tbPricePay.SelectionStart = tbPricePay.Text.Length;
            }
        } 

        private void tbCustumerManuel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
            && !char.IsSeparator(e.KeyChar);
        }

        private void tbCustumerManuel2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
            && !char.IsSeparator(e.KeyChar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmCusumerCase frmCase = new frmCusumerCase();
            frmCase.Show();
        }

        private void tbCustumerManuel_TextChanged(object sender, EventArgs e)
        {
            if (tbCustumerManuel.Text.Trim() != "")
            {
                cbCustomerOto.Enabled = false;
            }
            else cbCustomerOto.Enabled = true;
        }

        private void tbCustumerManuel2_TextChanged(object sender, EventArgs e)
        {
            if (tbCustumerManuel2.Text.Trim() != "")
            {
                cbCustumerOto2.Enabled = false;
            }
            else cbCustumerOto2.Enabled = true;
        }

        private void cbCustomerOto_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbCustumerOto2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbTakePay_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTakePay.Checked == true)
            {
                cbGivePay.Checked = false;
                cbGivePay.Enabled = false;
            }
            else cbGivePay.Enabled = true;
        }

        private void cbGivePay_CheckedChanged(object sender, EventArgs e)
        {

            if (cbGivePay.Checked == true)
            {
                cbTakePay.Checked = false;
                cbTakePay.Enabled = false;
            }
            else cbTakePay.Enabled = true;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            if (tbCustomer.Text.Trim() != "")
            {
                tbCustomerOto.Enabled = false;
            }
            else tbCustomerOto.Enabled = true;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            stocklist_id = "0";
            frmListe frmListe = new frmListe();
            if (frmListe.ShowDialog() == DialogResult.OK)
            {
                cbStockCode.Text = frmListe.dtgrdList.CurrentRow.Cells["NAME"].Value.ToString() + " ( "+ frmListe.dtgrdList.CurrentRow.Cells["CODE"].Value.ToString() + " )";    //tbName.Text;
                stocklist_id = frmListe.dtgrdList.CurrentRow.Cells["ID"].Value.ToString();
            }
          
        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            lblSave2.Visible = false;
        }

        private void buttonEdit1_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            userid = "0";
            frmKisiListesi kl = new frmKisiListesi();
            if (kl.ShowDialog() == DialogResult.OK)
            {
                cbCustomerOto.Text = kl.dtgrdList.CurrentRow.Cells["NAME"].Value.ToString() + " " + kl.dtgrdList.CurrentRow.Cells["SURNAME"].Value.ToString();    //tbName.Text;
                userid = kl.dtgrdList.CurrentRow.Cells["ID"].Value.ToString();
            }
        }

        private void cbCustomerOto_TextChanged_1(object sender, EventArgs e)
        {

            if (cbCustomerOto.Text.Trim() != "")
            {
                tbCustumerManuel.Enabled = false;
            }
            else tbCustumerManuel.Enabled = true;

        }

        private void buttonEdit1_ButtonClick_2(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            userid = "0";
            frmKisiListesi kl = new frmKisiListesi();
            if (kl.ShowDialog() == DialogResult.OK)
            {
                tbCustomerOto.Text = kl.dtgrdList.CurrentRow.Cells["NAME"].Value.ToString() + " " + kl.dtgrdList.CurrentRow.Cells["SURNAME"].Value.ToString();    //tbName.Text;
                userid = kl.dtgrdList.CurrentRow.Cells["ID"].Value.ToString();
            }
        }

        private void buttonEdit1_TextChanged(object sender, EventArgs e)
        {

            if (tbCustomerOto.Text.Trim() != "")
            {
                tbCustomer.Enabled = false;
            }
            else tbCustomer.Enabled = true;
        }

        private void cbCustumerOto2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            userid = "0";
            frmKisiListesi kl = new frmKisiListesi();
            if (kl.ShowDialog() == DialogResult.OK)
            {
                cbCustumerOto2.Text = kl.dtgrdList.CurrentRow.Cells["NAME"].Value.ToString() + " " + kl.dtgrdList.CurrentRow.Cells["SURNAME"].Value.ToString();    //tbName.Text;
                userid = kl.dtgrdList.CurrentRow.Cells["ID"].Value.ToString();
            }
        }

        private void cbCustumerOto2_TextChanged_1(object sender, EventArgs e)
        {
            if (cbCustumerOto2.Text.Trim() != "")
            {
                tbCustumerManuel2.Enabled = false;
            }
            else tbCustumerManuel2.Enabled = true;
        }

        private void cbCustumerOto2_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void tcPrices_KeyUp(object sender, KeyEventArgs e)
        {
            //eğer silme veya backspaceye basılırsa textboxu sıfırlıyoruz. 
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                tcPricesValue.Clear();
                tcPrices.Text = "";
            } 
        }
        private void RemovePoints()
        {
            for (int i = 0; i < tcPricesValue.Count; i++)
            {
                if (tcPricesValue[i] == '.')
                    tcPricesValue.RemoveAt(i);
            }

        }

        private void tbPrice_KeyUp(object sender, KeyEventArgs e)
        {
            //eğer silme veya backspaceye basılırsa textboxu sıfırlıyoruz. 
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                tcPricesValue.Clear();
                tbPrice.Text = "";
            } 
        }

        private void tbPricePay_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                tcPricesValue.Clear();
                tbPricePay.Text = "";
            } 
        }

        private void AddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        } 
   

    }
}
