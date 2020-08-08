using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApiBusinessLayer.Models
{
    public class HesapHareketler
    {
        public int HareketID { get; set; }
        public string Gonderen { get; set; }
        public string Alici { get; set; }
        public decimal Tutar { get; set; }
        public int HesapID { get; set; }
        public string Aciklama { get; set; }
    }
}
