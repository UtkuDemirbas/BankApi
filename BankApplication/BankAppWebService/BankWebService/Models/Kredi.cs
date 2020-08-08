using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankWebService.Models
{
    public class Kredi
    {
        public int KrediID { get; set; }
        public string KrediTipi { get; set; }
        public decimal AnaPara { get; set; }
        public decimal FaizPara { get; set; }
        public decimal ToplamBorc { get; set; }
        public int MusteriID { get; set; }
    }
}