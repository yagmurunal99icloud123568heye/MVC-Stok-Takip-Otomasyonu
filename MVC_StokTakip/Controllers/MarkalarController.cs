using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles = "A,U2")]
    public class MarkalarController : Controller
    {
        // GET: Markalar
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public ActionResult Index()
        {
            return View(db.Markalar.ToList());
        }
        [HttpGet]
        public ActionResult Ekle()
        {
            SelecteBilgiGetir()
                ;
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Markalar m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KategoriID = new SelectList(db.Kategoriler, "ID", "Kategori", m.KategoriID);
                return View();

            }
            db.Entry(m).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void SelecteBilgiGetir()
        {
            var model = new Markalar();
            List<SelectListItem> liste = new List<SelectListItem>(from x in db.Kategoriler
                                                                  select new SelectListItem
                                                                  {
                                                                      Value = x.ID.ToString(),
                                                                      Text = x.Kategori
                                                                  }
                                                               ).ToList();
            ViewBag.l = liste;
        }

      
        public ActionResult GuncelleBilgiGetir(int id)
        {
            SelecteBilgiGetir();
            var model = db.Markalar.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Guncelle(Markalar m)
        {
            if (!ModelState.IsValid)
            {
                return View("GuncelleBilgiGetir");
            }
            db.Entry(m).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(Markalar m)
        {
            var getir = db.Markalar.Find(m.ID);


            return View(getir);
        }
        public ActionResult Sil(Markalar m)
        {
            db.Entry(m).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Urunler(int id)
        {
            var model = db.Urunler.Where(x => x.Markalar.ID == id).ToList();
            var kategori = db.Markalar.Where(x => x.ID == id).Select(x => x.Kategoriler).FirstOrDefault();
            var marka = db.Markalar.Where(x => x.ID == id).Select(x => x.Marka).FirstOrDefault();
            ViewBag.k = kategori;
            ViewBag.m = marka;
            return View(model);
        }
    }
}