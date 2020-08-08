using BankApplication.BankAppServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Controllers
{
    public class CustomerController : Controller
    {
        BankAppServiceReference.BankWebServiceSoapClient service = new BankAppServiceReference.BankWebServiceSoapClient();
        public ActionResult AddCustomer()
        {
            Musteri musteri = new Musteri();
            return View(musteri);
        }
        [HttpPost]
        public ActionResult AddCustomer(Musteri musteri)
        {
            service.AddCustomer(musteri);
            return RedirectToAction("Login", "Login");
        }
        [HttpGet]
        public ActionResult UpdateCustomer()
        {
            Musteri musteri = service.GetCustomer(Convert.ToInt32(Session["MusteriID"]));
            return View(musteri);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(Musteri mus)
        {
            mus.MusteriID = Convert.ToInt32(Session["MusteriID"]);
            service.UpdateCustomer(mus);
            return View();//redirecttoaction("home","ındex")
        }
        [HttpGet]
        public ActionResult OpenAccount()
        {
            Hesap hesap=service.OpenAccount(4);//Convert.ToInt32(Session["MusteriID"])
            return View(hesap);
        }
        [HttpPost]
        public ActionResult OpenAccount(Hesap hes)
        {
            //return RedirectToAction("Home","Index")
            return View();
        }
        [HttpGet]
        public ActionResult ListAccount()
        {

            return View(service.ListAccounts(14));//Convert.ToInt32(Session["MusteriID"])
        }
        [HttpGet]
        public ActionResult CloseAccount(string HesapNo)
        {
            service.CloseAccount(Convert.ToInt32(Session["MusteriID"]), HesapNo);
            return RedirectToAction("ListAccount","Customer");
        }
    }
}