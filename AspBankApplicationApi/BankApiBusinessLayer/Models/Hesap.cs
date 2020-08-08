using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspBankApplicationApi.Models
{
    public class Hesap
    {
        public int HesapID { get; set; }
        public string HesapNo { get; set; }
        public decimal Bakiye { get; set; }
        public bool IsOpen { get; set; }
        public int MusteriID { get; set; }
    }
}