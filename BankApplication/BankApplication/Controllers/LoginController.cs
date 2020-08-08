using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankApplication;
using BankApplication.BankAppServiceReference;
using System.Web.UI;

namespace BankApplication.Controllers
{
    public class LoginController : Controller
    {
        BankAppServiceReference.BankWebServiceSoapClient service = new BankAppServiceReference.BankWebServiceSoapClient();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Musteri musteri)
        {
            int id = service.Login(musteri.TcNumarasi, musteri.Sifre);

            if (id != -1)
            {
                Session["MusteriID"] = id;
                return RedirectToAction("UpdateCustomer","Customer");//RedirectToAction("home",index)
            }
            else
                
            return View();
            
        }
   
    }
}