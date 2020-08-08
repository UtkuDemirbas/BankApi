using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGSWebService.Model
{
    public class HGS
    {
        public int HgsID { get; set; }
        public string HgsNumarasi { get; set; }
        public decimal Bakiye { get; set; }
        public int MusteriID { get; set; }
    }
}