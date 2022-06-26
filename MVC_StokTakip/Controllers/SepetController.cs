﻿using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var model = db.Sepet.Where(x => x.KullaniciID == kullanici.ıd).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciID == kullanici.ıd);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmuyor";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciID == kid.KullaniciID).Sum(x => x.ToplamFiyati);
                        ViewBag.Tutar = "Toplam Tutar=" + Tutar + " TL";
                    }

                }
                return View(model);

            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var u = db.Urunler.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.ıd && x.UrunID == id);
                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyati = u.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                          KullaniciID = model.ıd,
                          UrunID = u.ID,
                           Miktari=1,
                          AlisFiyatı=u.SatisFiyati,
                         ToplamFiyati=u.SatisFiyati,
                         Tarih=DateTime.Now,
                         Saat=DateTime.Now

                    };
                    db.Entry(s).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
               


            }
            return HttpNotFound();
        }

        public ActionResult TotalCount(int? count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
                count = db.Sepet.Where(x => x.KullaniciID == model.ıd).Count();
                ViewBag.Count = count;
                if (count == 0)
                {
                    ViewBag.Count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }

        public ActionResult Arttir(int id)
        {
            var model = db.Sepet.Find(id);
            model.Miktari++;
            model.ToplamFiyati = model.AlisFiyatı * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Azalt(int id)
        {
            var model = db.Sepet.Find(id);
            if (model.Miktari == 1)
            {
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            model.Miktari--;
            model.ToplamFiyati = model.AlisFiyatı * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void DinamikMiktar(int id,decimal miktari)
        {
            var model = db.Sepet.Find(id);
            model.Miktari = miktari;
            model.ToplamFiyati = model.AlisFiyatı * model.Miktari;
            db.SaveChanges();
        }

        public ActionResult Sil(int id)
        {
            var model = db.Sepet.Find(id);
            db.Sepet.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi.Equals(kullaniciadi));
                var sil = db.Sepet.Where(x => x.KullaniciID.Equals(model.ıd));
                db.Sepet.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
           
        }

        [HttpPost]
        public ActionResult SeciliSil(FormCollection form)
        {
            string[] seciliid = form["secim_id"].Split(new char[] { ',' });
            foreach(string id in seciliid)
            {
                Sepet model = db.Sepet.Find(int.Parse(id));
                db.Sepet.Remove(model);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
   
        [HttpPost]
        public ActionResult SeciliSatinAl(List<Sepet> data=null)
        {
            string[] ids = data.Select(x => x.ID.ToString()).ToArray();
            decimal total=0;
            foreach(var item in ids)
            {
                var model = db.Sepet.Find(int.Parse(item));
                if (int.Parse(item) != 0)
                {
                    total += model.ToplamFiyati;
                }
                
            }
            ViewBag.Total = total.ToString("0.00") + "TL";
            return View(data);
        }

        [HttpPost]
        public ActionResult SeciliSatinAl2(int[] ids)
        {
            var model = db.Sepet.Where(x => ids.Contains(x.ID)).ToList();
            int row = 0;
            foreach (var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciID = model[row].KullaniciID,
                    UrunID = model[row].UrunID,
                    SepetID = model[row].ID,
                    BarkodNo = model[row].Urunler.BarkodNo,
                    BirimFiyati = model[row].AlisFiyatı,
                    Miktari = model[row].ToplamFiyati,
                    KDV = model[row].Urunler.KDV,
                    BirimID = model[row].Urunler.BirimID,
                    Tarih = DateTime.Now,
                    Saat = DateTime.Now

                };
                db.Satislar.Add(satis);
                row++;
            }
            foreach (var item in model)
            {
                var urun = db.Urunler.FirstOrDefault(x => x.ID == item.UrunID);
                if (urun != null)
                {
                    urun.Miktarı = urun.Miktarı - item.Miktari;

                }
            }



            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}