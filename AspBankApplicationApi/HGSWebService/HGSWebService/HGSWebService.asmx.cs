using Database;
using HGSWebService.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HGSWebService
{
    /// <summary>
    /// Summary description for HGSWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HGSWebService : System.Web.Services.WebService
    {

        private SqlConnection db;

        [WebMethod]
        public void HgsForSale(int MusteriID)
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

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }

        [WebMethod]

        public void UpdateBalance(decimal money, string hgsNumarasi)
        {
            using (db = Connection.Connect())
            {
                string query = "update Hgsler set bakiye+=@bakiye where hgsNumarasi=@hgsNumarasi";

                SqlCommand cmd = new SqlCommand(query, db);
                cmd.Parameters.AddWithValue("@bakiye", money);
                cmd.Parameters.AddWithValue("@hgsNumarasi", hgsNumarasi);

                cmd.ExecuteNonQuery();
                db.Close();
            }

        }
        [WebMethod]

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

        [WebMethod]

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
        
        [WebMethod]

        public void AddHgsNotification(string hgsNumarasi,string aciklama)
        {
            using (db = Connection.Connect())
            {
                string query = "insert into Hgs_Kurum values(@hgsNumarasi,@aciklama)";

                SqlCommand cmd = new SqlCommand(query, db);

                cmd.Parameters.AddWithValue("@hgsNumarasi", hgsNumarasi);
                cmd.Parameters.AddWithValue("@aciklama", aciklama);

                cmd.ExecuteNonQuery();
                db.Close();
            }
        }



        private int CreateNumber()
        {
            Random random = new Random();

            int sayi = random.Next(100000, 1000000);

            return sayi;
        }
    }
}
