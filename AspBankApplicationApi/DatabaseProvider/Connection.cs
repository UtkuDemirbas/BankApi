using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider
{
    public class Connection
    {
        private static SqlConnection db = null;
        public static SqlConnection Connect()
        {             
              db = new SqlConnection("Server=DESKTOP-EM3U44N;Database=BankaUygulamasi;User Id=utku;Password =10021998ud; ");
              db.Open();
            return db;
        }

    }
}
