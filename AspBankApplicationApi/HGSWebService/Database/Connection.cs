using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Connection
    {

        private static SqlConnection db = null;

        public static SqlConnection Connect()
        {
            db = new SqlConnection("server=.; Initial Catalog=BankaUygulamasi;Integrated Security=SSPI");
            db.Open();
            return db;
        }
    }
}
