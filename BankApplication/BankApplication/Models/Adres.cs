using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Adres
    {
        public int AdresID { get; set; }
        public string İl { get; set; }
        public string İlce { get; set; }
        public string Aciklama { get; set; }
        public int MusteriID { get; set; }
    }
}