using AspBankApplicationApi.Models;
using BankApiBusinessLayer;
using BankApiBusinessLayer.Models;
using DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace AspBankApplicationApi.Controllers
{
    public class BankController : ApiController
    {
        /*
          Update Fonksiyonları HTTPPut
          Insert Fonksiyonları HTTPPost 
          List+Get Fonksiyonları HTTPGet
          Delete Fonksiyonları HTTPDelete 
        */
        BankApiFunction baf = new BankApiFunction();

        [HttpPost]
        public JsonResult<bool> AddCustomer([FromBody]Musteri item)
        {
            return Json<bool>(baf.AddCustomer(item));
        }

        [HttpGet]
        public JsonResult<Musteri> GetCustomer(int id)
        {
            return Json<Musteri>(baf.GetCustomer(id));
        }

        [HttpPut]
        public JsonResult<bool> UpdateCustomer([FromBody]Musteri item)
        {
            return Json<bool>(baf.UpdateCustomer(item));
        }

        [HttpDelete]
        public JsonResult<bool> DeleteCustomer(int id)
        {
            return Json<bool>(baf.DeleteCustomer(id));
        }

        [HttpGet]
        public JsonResult<List<Musteri>> ListCustomers()
        {
            return Json<List<Musteri>>(baf.ListCustomers());
        }

        [HttpGet]
        public JsonResult<int> Login([FromUri]string TcNumarasi, [FromUri]string Sifre)
        {
            return Json<int>(baf.Login(TcNumarasi, Sifre));
        }
        [HttpPost]
        public JsonResult<bool> OpenAccount(int id)
        {
            return Json<bool>(baf.OpenAccount(id));
        }
        [HttpPut]
        public JsonResult<bool> CloseAccount([FromUri]int id, [FromUri]string hesapNumarasi)
        {
            return Json<bool>(baf.CloseAccount(id, hesapNumarasi));
        }
        [HttpGet]
        public JsonResult<List<Hesap>> ListAccounts(int id)
        {
            return Json<List<Hesap>>(baf.ListAccounts(id));
        }
        [HttpGet]
        public JsonResult<Hesap> GetAccount(string hesapNumarasi)
        {
            return Json<Hesap>(baf.GetAccount(hesapNumarasi));
        }
        [HttpPut]
        public JsonResult<bool> WithdrawMoney([FromUri]string hesapNumarasi, [FromUri] decimal money)
        {
            return Json<bool>(baf.WithdrawMoney(hesapNumarasi, money));
        }
        [HttpPut]
        public JsonResult<bool> ToDepositMoney([FromUri]string hesapNumarasi, [FromUri] decimal money)
        {
            return Json<bool>(baf.ToDepositMoney(hesapNumarasi, money));
        }
        [HttpPut]
        public JsonResult<bool> SendTransfer([FromUri]string sender, [FromUri]string buyer, [FromUri] decimal money, [FromUri]string type)
        {
            return Json<bool>(baf.SendTransfer(sender, buyer, money, type));
        }
        [HttpPost]
        public JsonResult<bool> AddAccountActivities([FromUri]string gonderen, [FromUri] string alici, [FromUri] decimal money, [FromUri]int hesapID, [FromUri]string aciklama)
        {
            return Json<bool>(baf.AddAccountActivities(gonderen, alici, money, hesapID, aciklama));
        }
        [HttpGet]
        public JsonResult<List<HesapHareketler>> ListAccountActivities(int hesapID)
        {
            return Json<List<HesapHareketler>>(baf.ListAccountActivities(hesapID));
        }
        [HttpPost]
        public JsonResult<bool> HgsForSale(int MusteriID)
        {
            return Json<bool>(baf.HgsForSale(MusteriID));
        }
        [HttpPut]
        public JsonResult<bool> UpdateBalance([FromUri]decimal money, [FromUri]string hgsNumarasi)
        {
            return Json<bool>(baf.UpdateBalance(money, hgsNumarasi));
        }
        [HttpGet]
        public JsonResult<List<HGS>> ListHGS(int id)
        {
            return Json<List<HGS>>(baf.ListHGS(id));
        }
        [HttpGet]
        public JsonResult<HGS> HGSGet(string hgsNumarasi)
        {
            return Json<HGS>(baf.HGSGet(hgsNumarasi));
        }
        [HttpPost]
        public JsonResult<bool> AddHgsNotification([FromUri]string hgsNumarasi,[FromUri] string aciklama)
        {
            return Json<bool>(baf.AddHgsNotification(hgsNumarasi, aciklama));
        }
    }
}



