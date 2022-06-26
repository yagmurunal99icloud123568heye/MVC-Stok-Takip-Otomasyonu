using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using MVC_StokTakip.MyModel;

namespace MVC_StokTakip.Controllers
{
    [Authorize(Roles ="A,U")]
    public class BirimlerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Birimler
        
        public ActionResult Index()
        {
            return View(db.Birimler.ToList());
        }
        [HttpGet]
        public ActionResult Ekle()
        {
            return View("Kaydet");
        }
        [HttpPost]
        public ActionResult Kaydet(Birimler b)
        {
            if(b.ID==0)
            {
                db.Birimler.Add(b);
            }
            else
            {
                db.Entry(b).State = System.Data.Entity.EntityState.Modified;
            }
           
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult GuncelleBilgiGetir(Birimler b)
        {
            //var model = db.Birimler.Find(b.ID);

            //if (model == null) return HttpNotFound();
            //return View("Kaydet",model);
            var model = db.Birimler.Find(b.ID);

            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Guncelle(Birimler b)
        {
            db.Entry(b).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SilBilgiGetir(Birimler b)
        {
            var model = db.Birimler.Find(b.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Birimler b)
        {
            db.Entry(b).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}