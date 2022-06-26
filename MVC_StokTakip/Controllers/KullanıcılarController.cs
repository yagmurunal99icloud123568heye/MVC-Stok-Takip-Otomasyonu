using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace MVC_StokTakip.Controllers
{

    [AllowAnonymous]
    public class KullanıcılarController : Controller
    {
        // GET: Kullanıcılar
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        [HttpGet]
       
            
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanicilar k)
        {
            var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(k.KullaniciAdi,false);
                return RedirectToAction("Index","Urunler");

            }
            ViewBag.hata = "Kullanıcı adı veya şifre yanlış";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("Login");
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar k)
        {
            var model = db.Kullanicilar.Where(x => x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                Guid guid = new Guid();
                model.Sifre = guid.ToString().Substring(0, 8);
                db.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                //client.UseDefaultCredentials = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("testyagmur123@gmail.com", "reset password");
               
                mail.To.Add(model.Email);
              
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre değiştirme İsteği";
                mail.Body += "Merhaba" + model.AdiSoyadi + "<br/> Kullanıcı adınız=" + model.KullaniciAdi + "<br/> Şifreniz:" + model.Sifre;
                NetworkCredential net = new NetworkCredential("testyagmur123@gmail.com", "karakarga06");
                client.Credentials = net;
                client.Send(mail);
                 db.SaveChanges();
                 return RedirectToAction("Login");
                
                
                   
                }

            ViewBag.hata = "Böyle bi email adresi yoktur";
            return View();
           

        }
           

        

        [HttpGet]
        public ActionResult Kaydol()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kaydol(Kullanicilar k)
        {
            if (!ModelState.IsValid) return View();
           
            db.Entry(k).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            
            return RedirectToAction("Login");
        }

        public ActionResult Guncelle()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return View(new Kullanicilar());
                }
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Guncelle(Kullanicilar k)
        {
            //var kullanici = db.Kullanicilar.Find(k.ıd);
            db.Entry(k).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            //FormsAuthentication.SignOut();
            //FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);

            return RedirectToAction("Login","Kullanıcılar");

        }
    }
}