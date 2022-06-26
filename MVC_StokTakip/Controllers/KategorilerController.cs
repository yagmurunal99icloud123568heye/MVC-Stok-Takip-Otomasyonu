using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A,U2")]
    public class KategorilerController : Controller
    {
        // GET: Kategoriler
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {

            return View(db.Kategoriler.ToList());
        }
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Ekle2(Kategoriler p) {
           // if (!ModelState.IsValid == false) return View("Ekle"); 
            db.Kategoriler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GuncelleBilgiGetir(Kategoriler p)
        {
            var model = db.Kategoriler.Find(p.ID);
           
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Guncelle(Kategoriler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(Kategoriler p)
        {
            var model = db.Kategoriler.Find(p.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Kategoriler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    
        public ActionResult Markalar(int id)
        {
            var model = db.Markalar.Where(x => x.Kategoriler.ID == id).ToList();
            var kategori = db.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.viewkategori = kategori;
            return View(model);
        }
        public ActionResult Urunler(int id)
        {
            var model = db.Urunler.Where(x => x.Kategoriler.ID == id).ToList();
            var kategori = db.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.viewkategori = kategori;
            return View(model);
        }
    }



    }