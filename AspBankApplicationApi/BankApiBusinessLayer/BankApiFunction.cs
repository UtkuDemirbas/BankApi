using AspBankApplicationApi.Models;
using BankApiBusinessLayer.Models;
using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApiBusinessLayer
{
   
    public class BankApiFunction
    {
        private SqlConnection db;
        private SqlConnection con;

        public bool AddCustomer(Musteri item)
        {
            using (db = Connection.Connect())
            {
                string insert = "insert into Musteriler(tcnumarasi,adi,soyadi,tel,eposta,sifre) values(@tcNumarasi,@adi,@soyadi,@tel,@eposta,@sifre)";
                SqlCommand cmd = new SqlCommand(insert, db);

                cmd.Parameters.AddWithValue("@tcNumarasi", item.TcNumarasi);
                cmd.Parameters.AddWithValue("@adi", item.Adi);
                cmd.Parameters.AddWithValue("@soyadi", item.Soyadi);
                cmd.Parameters.AddWithValue("@tel", item.Tel);
                cmd.Parameters.AddWithValue("@eposta", item.Eposta);
                cmd.Parameters.AddWithValue("@sifre", item.Sifre);

                int etkilenensatirsayisi=cmd.ExecuteNonQuery();
                db.Close();
                if (etkilenensatirsayisi>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

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

        public bool UpdateCustomer(Musteri item)
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

                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                db.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteCustomer(int id)
        {
            using (db = Connection.Connect())
            {
                string delete = "delete from Musteriler where musteriID=@id";

                SqlCommand cmd = new SqlCommand(delete, db);

                cmd.Parameters.AddWithValue("@id", id);

                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                db.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

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

        public int Login(string TcNumarasi, string Sifre)
        {
            using (db = Connection.Connect())
            {
                int id = -1;
                string get = "select musteriID from Musteriler where tcNumarasi=@TcNumarasi and sifre=@Sifre";

                SqlCommand cmd = new SqlCommand(get, db);

                cmd.Parameters.AddWithValue("@TcNumarasi", TcNumarasi);
                cmd.Parameters.AddWithValue("@Sifre", Sifre);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    id = Convert.ToInt32(dr["musteriID"].ToString());
                }
                db.Close();
                return id;

            }
        }

        public bool OpenAccount(int id)
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
                    hesapNo = CreateNumber().ToString() + "-1001";
                }
                else
                {
                    while (dr.Read())
                    {
                        hesapNo = dr["hesapNo"].ToString();
                    }
                    string[] splitted = hesapNo.Split('-');
                    hesapNo = splitted[0] + "-" + (Convert.ToUInt32(splitted[1]) + 1).ToString();
                }
                db.Close();

                hesap.HesapNo = hesapNo;
                hesap.Bakiye = 0;
                hesap.IsOpen = true;
                hesap.MusteriID = id;

                con = Connection.Connect();

                string insert = "insert into Hesaplar(hesapNo,bakiye,isOpen,musteriID) values(@hesapNo,@bakiye,@isOpen,@musteriID)";
                cmd = new SqlCommand(insert, con);

                cmd.Parameters.AddWithValue("@hesapNo", hesap.HesapNo);
                cmd.Parameters.AddWithValue("@bakiye", hesap.Bakiye);
                cmd.Parameters.AddWithValue("@isOpen", hesap.IsOpen);
                cmd.Parameters.AddWithValue("@musteriID", hesap.MusteriID);

                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                con.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool CloseAccount(int id, string hesapNumarasi)
        {
            using (con = Connection.Connect())
            {
                string close = "update Hesaplar set isOpen=@isOpen where musteriID=@id and hesapNo=@hesapNumarasi";

                SqlCommand cmd = new SqlCommand(close, con);

                cmd.Parameters.AddWithValue("@isOpen", false);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);
               
                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                con.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

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
                    item.IsOpen = Convert.ToBoolean(dr["isOpen"].ToString());
                    item.MusteriID = Convert.ToInt32(dr["musteriID"].ToString());
                }
                db.Close();
                return item;
            }
        }

        public bool WithdrawMoney(string hesapNumarasi, decimal money)
        {
            using (con = Connection.Connect())
            {
                string command = "update Hesaplar set bakiye-=@Bakiye where hesapNo=@hesapNumarasi";
                SqlCommand cmd = new SqlCommand(command, con);

                cmd.Parameters.AddWithValue("@Bakiye", money);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);

                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                con.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ToDepositMoney(string hesapNumarasi, decimal money)
        {
            using (con = Connection.Connect())
            {
                string command = "update Hesaplar set bakiye+=@Bakiye where hesapNo=@hesapNumarasi";
                SqlCommand cmd = new SqlCommand(command, con);

                cmd.Parameters.AddWithValue("@Bakiye", money);
                cmd.Parameters.AddWithValue("@hesapNumarasi", hesapNumarasi);


                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                con.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SendTransfer(string sender, string buyer, decimal money, string type)
        {
            using (db = Connection.Connect())
            {
                bool With = false;
                bool Deposit = false;              
                With=WithdrawMoney(sender, money);
                if (type == "transfer")
                WithdrawMoney(sender, (decimal)1.50);
                Deposit=ToDepositMoney(buyer, money);
                if ((Deposit==true&&With==true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddAccountActivities(string gonderen, string alici, decimal money, int hesapID, string aciklama)
        {
            using (con = Connection.Connect())
            {
                string query = "insert into HesapHareketleri values(@gonderen,@alici,@tutar,@hesapID,@aciklama)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@gonderen", gonderen);
                cmd.Parameters.AddWithValue("@alici", alici);
                cmd.Parameters.AddWithValue("@tutar", money);
                cmd.Parameters.AddWithValue("@hesapID", hesapID);
                cmd.Parameters.AddWithValue("@aciklama", aciklama);

                int etkilenensatirsayisi = cmd.ExecuteNonQuery();
                con.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

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

        public void GetCredit()
        {

        }

        private int CreateNumber()
        {
            Random random = new Random();

            int sayi = random.Next(1000000, 10000000);

            return sayi;
        }

        public bool HgsForSale(int MusteriID)
        {
            using (db = Connection.Connect())
            {
                HGS hgs = new HGS();
                string query = "insert into Hgsler(hgsNumarasi,bakiye,musteriID) values(@hgsNumarasi,@bakiye,@musteriID)";
                SqlCommand cmd = new SqlCommand(query, db);

                hgs.HgsNumarasi = CreateNumber().ToString();
                hgs.Bakiye = 10;
                hgs.MusteriID = MusteriID;

                cmd.Parameters.AddWithValue("@hgsNumarasi", hgs.HgsNumarasi);
                cmd.Parameters.AddWithValue("@bakiye", hgs.Bakiye);
                cmd.Parameters.AddWithValue("@musteriID", hgs.MusteriID);

                int etkilenensatirsayisi=cmd.ExecuteNonQuery();
                db.Close();
                if(etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateBalance(decimal money, string hgsNumarasi)
        {
            using (db = Connection.Connect())
            {
                string query = "update Hgsler set bakiye+=@bakiye where hgsNumarasi=@hgsNumarasi";

                SqlCommand cmd = new SqlCommand(query, db);
                cmd.Parameters.AddWithValue("@bakiye", money);
                cmd.Parameters.AddWithValue("@hgsNumarasi", hgsNumarasi);

                 int etkilenensatirsayisi=cmd.ExecuteNonQuery();
                db.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public List<HGS> ListHGS(int id)
        {
            using (db = Connection.Connect())
            {
                List<HGS> hgsler = new List<HGS>();

                string query = "select hgsNumarasi,bakiye from Hgsler where musteriID=@MusteriID";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@MusteriID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    HGS hgs = new HGS();
                    hgs.HgsNumarasi = dr["hgsNumarasi"].ToString();
                    hgs.Bakiye = Convert.ToDecimal(dr["bakiye"].ToString());

                    hgsler.Add(hgs);
                }
                return hgsler;
            }
        }

        public HGS HGSGet(string hgsNumarasi)
        {
            using (db = Connection.Connect())
            {
                HGS hgs = new HGS();

                string query = "select hgsNumarasi,bakiye from Hgsler where hgsNumarasi=@hgsNumarasi";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@hgsNumarasi", hgsNumarasi);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {

                    hgs.HgsNumarasi = dr["hgsNumarasi"].ToString();
                    hgs.Bakiye = Convert.ToDecimal(dr["bakiye"].ToString());


                }
                return hgs;
            }
        }

        public bool AddHgsNotification(string hgsNumarasi, string aciklama)
        {
            using (db = Connection.Connect())
            {
                string query = "insert into Hgs_Kurum values(@hgsNumarasi,@aciklama)";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@hgsNumarasi", hgsNumarasi);
                cmd.Parameters.AddWithValue("@aciklama", aciklama);

                int etkilenensatirsayisi=cmd.ExecuteNonQuery();
                db.Close();
                if (etkilenensatirsayisi > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
