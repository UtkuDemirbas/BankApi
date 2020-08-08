using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApiBusinessLayer.Models
{
    public  class HGS
    {
        public int HgsID { get; set; }
        public string HgsNumarasi { get; set; }
        public decimal Bakiye { get; set; }
        public int MusteriID { get; set; }
    }
}
