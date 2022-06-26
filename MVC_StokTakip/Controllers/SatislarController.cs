﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A,U")]
    public class SatislarController : Controller
    {
        // GET: Satislar
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            var model = db.Satislar.ToList();

            return View(model);
        }
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.ID == id);
            return View(model);

        }
        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.ID == id);
                    var urun = db.Urunler.FirstOrDefault(x => x.ID == model.UrunID);
                    urun.Miktarı = urun.Miktarı - model.Miktari;
                    var satis = new Satislar
                    {
                        KullaniciID = model.KullaniciID,
                        UrunID = model.UrunID,
                        SepetID = model.ID,
                        BarkodNo = model.Urunler.BarkodNo,
                        BirimFiyati = model.AlisFiyatı,
                        Miktari = model.Miktari,
                        ToplamFiyati = model.ToplamFiyati,
                        KDV = model.Urunler.KDV,
                        BirimID = model.Urunler.BirimID,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    db.Sepet.Remove(model);
                    db.Satislar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın alma işlemi başarılı bir şekilde gerçekleşmiştir";
                }

            }
            catch (Exception)
            {
                ViewBag.islem = "Satın alma işlemi başarısız";
            }

            return View("islem");




        }

        public ActionResult HepsiniSatinAl(decimal? Tutar)
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
    
        public ActionResult HepsiniSatinal2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == username);
            var model = db.Sepet.Where(x => x.KullaniciID == kullanici.ıd).ToList();
            int row = 0;
            foreach(var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciID=model[row].KullaniciID,
                    UrunID=model[row].UrunID,
                    SepetID=model[row].ID,
                    BarkodNo=model[row].Urunler.BarkodNo,
                    BirimFiyati=model[row].AlisFiyatı,
                    Miktari=model[row].ToplamFiyati,
                    KDV=model[row].Urunler.KDV,
                    BirimID=model[row].Urunler.BirimID,
                    Tarih=DateTime.Now,
                    Saat=DateTime.Now

                };
                db.Satislar.Add(satis);
                row++;
            }
            foreach(var item in model)
            {
                var urun = db.Urunler.FirstOrDefault(x => x.ID == item.UrunID);
                if (urun != null)
                {
                    urun.Miktarı = urun.Miktarı - item.Miktari;

                }
            }



            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
    }

        
    }
