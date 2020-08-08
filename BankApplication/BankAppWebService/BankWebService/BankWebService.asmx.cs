using BankWebService.Models;
using Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BankWebService
{
    /// <summary>
    /// Summary description for BankWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BankWebService : System.Web.Services.WebService
    {
        private SqlConnection db;
        private SqlConnection con;

        [WebMethod]
        public void AddCustomer(Musteri item)
        {
            using (db = Connection.Connect())
            {
                string insert = "insert into Musteriler(tcnumarasi,adi,soyadi,tel,eposta,sifre) values(@tcNumarasi,@adi,@soyadi,@tel,@eposta,@sifre)";
                SqlCommand cmd = new SqlCommand(insert,db);

                cmd.Parameters.AddWithValue("@tcNumarasi", item.TcNumarasi);
                cmd.Parameters.AddWithValue("@adi", item.Adi);
                cmd.Parameters.AddWithValue("@soyadi", item.Soyadi);
                cmd.Parameters.AddWithValue("@tel", item.Tel);
                cmd.Parameters.AddWithValue("@eposta", item.Eposta);
                cmd.Parameters.AddWithValue("@sifre", item.Sifre);

                cmd.ExecuteNonQuery();
                db.Close();

            }
        }

        [WebMethod]

        public Musteri GetCustomer(int id)
        {
         
            using (db = Connection.Connect())
            {
                Musteri item = new Musteri();


                string get = "select * from Musteriler where musteriID=@id";
                SqlCommand cmd = new SqlCommand(get, db);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    item.MusteriID = Convert.ToInt32(dr["musteriID"].ToString());
                    item.TcNumarasi = dr["tcNumarasi"].ToString();
                    item.Adi = dr["adi"].ToString();
                    item.Soyadi = dr["soyadi"].ToString();
                    item.Tel = dr["tel"].ToString();
                    item.Eposta = dr["eposta"].ToString();
                    item.Sifre = dr["sifre"].ToString();
                }
                db.Close();
                return item;
            }
               
        }

        [WebMethod]

        public void UpdateCustomer(Musteri item)
        {
            using (db = Connection.Connect())
            {
                string update = "update Musteriler set adi=@adi,soyadi=@soyadi,tel=@tel,eposta=@eposta,sifre=@sifre where musteriID=@MusteriID";
                SqlCommand cmd = new SqlCommand(update, db);

                cmd.Parameters.AddWithValue("@adi", item.Adi);
                cmd.Parameters.AddWithValue("@soyadi", item.Soyadi);
                cmd.Parameters.AddWithValue("@tel", item.Tel);
                cmd.Parameters.AddWithValue("@eposta", item.Eposta);
                cmd.Parameters.AddWithValue("@sifre", item.Sifre);
                cmd.Parameters.AddWithValue("@MusteriID", item.MusteriID);

                cmd.ExecuteNonQuery();

                db.Close();

            }
        }

        [WebMethod]

        public void DeleteCustomer(int id)
        {
            using (db = Connection.Connect())
            {
                string delete = "delete from Musteri where musteriID=@id";

                SqlCommand cmd = new SqlCommand(delete, db);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }
        [WebMethod]
        public List<Musteri> ListCustomers()
        {
            using (db = Connection.Connect())
            {
                List<Musteri> musteriler = new List<Musteri>();
                string list = "select * from Musteriler";

                SqlCommand cmd = new SqlCommand(list, db);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Musteri musteri = new Musteri();

                    musteri.MusteriID = Convert.ToInt32(dr["musteriID"].ToString());
                    musteri.TcNumarasi = dr["tcNumarasi"].ToString();
                    musteri.Adi = dr["adi"].ToString();
                    musteri.Soyadi = dr["soyadi"].ToString();
                    musteri.Tel = dr["tel"].ToString();
                    musteri.Eposta = dr["eposta"].ToString();
                    musteri.Sifre = dr["sifre"].ToString();

                    musteriler.Add(musteri);
                }

                db.Close();
                return musteriler;
            }
        }

        [WebMethod]

        public int Login(string TcNumarasi,string Sifre)
        {
            using (db = Connection.Connect())
            {
                int id = -1;
                string get = "select musteriID from Musteriler where tcNumarasi=@TcNumarasi and sifre=@Sifre";

                SqlCommand cmd = new SqlCommand(get,db);

                cmd.Parameters.AddWithValue("@TcNumarasi", TcNumarasi);
                cmd.Parameters.AddWithValue("@Sifre", Sifre);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    id=Convert.ToInt32(dr["musteriID"].ToString());
                }
                db.Close();
                return id;   

            }
        }

        [WebMethod]
        public void OpenAccount(int id)
        {
            using (db = Connection.Connect())
            {
                Hesap hesap = new Hesap();

                string control = "select * from Hesaplar where musteriID=@id";

                SqlCommand cmd = new SqlCommand(control, db);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                string hesapNo = "";
                if (!dr.HasRows)
                {
                    hesapNo = CreateNumber().ToString()+"-1001";
                }
                else
                {
                    while (dr.Read())
                    {
                        hesapNo = dr["hesapNo"].ToString();
                    }
                    string []splitted = hesapNo.Split('-');
                    hesapNo = splitted[0]+"-"+(Convert.ToUInt32(splitted[1])+1).ToString();
                }
                db.Close();

                hesap.HesapNo = hesapNo;
                hesap.Bakiye = 0;
                hesap.IsOpen = true;
                hesap.MusteriID = id;

                con = Connection.Connect();

                string insert = "insert into Hesaplar(hesapNo,bakiye,isOpen,musteriID) values(@hesapNo,@bakiye,@isOpen,@musteriID)";
                cmd = new SqlCommand(insert, con);

                cmd.Parameters.AddWithValue("@hesapNo",hesap.HesapNo);
                cmd.Parameters.AddWithValue("@bakiye", hesap.Bakiye);
                cmd.Parameters.AddWithValue("@isOpen", hesap.IsOpen);
                cmd.Parameters.AddWithValue("@musteriID", hesap.MusteriID);

                cmd.ExecuteNonQuery();
                con.Close();

            }

        }

        [WebMethod]
        public void CloseAccount(int id,string hesapNumarasi)
        {
            using (db = Connection.Connect())
            {
                string close = "update Hesaplar set isOpen=@isOpen where musteriID=@id and hesapNo=@hesapNumarasi";

                SqlCommand cmd = new SqlCommand(close, db);

                cmd.Parameters.AddWithValue("@isOpen", false);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }

        [WebMethod]
        public List<Hesap> ListAccounts(int id)
        {
            using (db = Connection.Connect())
            {
                List<Hesap> hesaplar = new List<Hesap>();
;
                string list = "select * from Hesaplar where musteriID=@id and isOpen=@isOpen";

                SqlCommand cmd = new SqlCommand(list, db);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@isOpen", true);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                { 
                    Hesap hesap = new Hesap();

                    hesap.HesapID = Convert.ToInt32(dr["hesapID"].ToString());
                    hesap.HesapNo = dr["hesapNo"].ToString();
                    hesap.Bakiye = Convert.ToDecimal(dr["bakiye"].ToString());
                    hesap.IsOpen = Convert.ToBoolean(dr["isOpen"].ToString());
                    hesap.MusteriID = Convert.ToInt32(dr["musteriID"].ToString());

                    hesaplar.Add(hesap);
                }

                db.Close();
                return hesaplar;
            }
        }
        [WebMethod]

        public Hesap GetAccount(string hesapNumarasi)
        {
            using (db = Connection.Connect())
            {
                Hesap item = new Hesap();


                string get = "select * from Hesaplar where hesapNo=@hesapNumarasi";
                SqlCommand cmd = new SqlCommand(get, db);

                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    item.HesapID = Convert.ToInt32(dr["hesapID"].ToString());
                    item.HesapNo = dr["hesapNo"].ToString();
                    item.Bakiye = Convert.ToDecimal(dr["bakiye"].ToString());
                    item.IsOpen =Convert.ToBoolean(dr["isOpen"].ToString());
                    item.MusteriID = Convert.ToInt32(dr["musteriID"].ToString());
                }
                db.Close();
                return item;
            }
        }


        [WebMethod]
        public void WithdrawMoney(string hesapNumarasi, decimal money)
        {
            using (db = Connection.Connect())
            {
                string command = "update Hesaplar set bakiye-=@Bakiye where hesapNo=@hesapNumarasi";
                SqlCommand cmd = new SqlCommand(command, db);

                cmd.Parameters.AddWithValue("@Bakiye", money);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }

        [WebMethod]
        public void ToDepositMoney(string hesapNumarasi,decimal money)
        {
            using (db = Connection.Connect())
            {
                string command = "update Hesaplar set bakiye+=@Bakiye where hesapNo=@hesapNumarasi";
                SqlCommand cmd = new SqlCommand(command,db);

                cmd.Parameters.AddWithValue("@Bakiye", money);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }

        [WebMethod]
        public void SendTransfer(string sender,string buyer,decimal money,string type)
        {
            using (db = Connection.Connect())
            {
                WithdrawMoney(sender, money);
                if (type == "transfer")
                    WithdrawMoney(sender, (decimal)1.50);
                ToDepositMoney(buyer, money);
            }
        }
        [WebMethod]

        public void AddAccountActivities(string gonderen,string alici,decimal money,int hesapID,string aciklama)
        {
            using (db = Connection.Connect())
            {
                string query = "insert into HesapHareketleri values(@gonderen,@alici,@tutar,@hesapID,@aciklama)";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@gonderen", gonderen);
                cmd.Parameters.AddWithValue("@alici", alici);
                cmd.Parameters.AddWithValue("@tutar", money);
                cmd.Parameters.AddWithValue("@hesapID", hesapID);
                cmd.Parameters.AddWithValue("@aciklama", aciklama);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }
        [WebMethod]

        public List<HesapHareketler> ListAccountActivities(int hesapID)
        {
            List<HesapHareketler> hesapHareketleri = new List<HesapHareketler>();
            using (db = Connection.Connect())
            {
                string query = "select * from HesapHareketleri where hesapID=@hesapID";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@hesapID", hesapID);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    HesapHareketler hareketler = new HesapHareketler();
                    hareketler.HareketID = Convert.ToInt32(dr["hareketID"].ToString());
                    hareketler.Gonderen = dr["gonderen"].ToString();
                    hareketler.Alici = dr["alici"].ToString();
                    hareketler.Tutar = Convert.ToDecimal(dr["tutar"].ToString());
                    hareketler.HesapID = Convert.ToInt32(dr["hesapID"].ToString());
                    hareketler.Aciklama = dr["aciklama"].ToString();

                    hesapHareketleri.Add(hareketler);
                }

                db.Close();
                return hesapHareketleri;
            }
        }

        [WebMethod]
        public void GetCredit()
        {

        }
        private int CreateNumber()
        {
            Random random = new Random();

            int sayi = random.Next(1000000,10000000);

            return sayi;
        }


    }
}
