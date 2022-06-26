using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokTakip.Models.Entity;
using Excel = Microsoft.Office.Interop.Excel;

namespace MVC_StokTakip.Controllers
{
    [Authorize]
    public class UrunlerController : Controller
    {
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        // GET: Urunler
        public ActionResult Index(string ara)
        {
            var model = db.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList();
            }
            return View("Index",model);
        }
        [HttpGet]
        public ActionResult Ekle()
        {
            var model = new Urunler();
            Yenile(model);

                                
            return View(model);
        }
        private void Yenile(Urunler model)
        {
            List<Kategoriler> kategorilist = db.Kategoriler.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in kategorilist
                                     select new SelectListItem
                                     {
                                         Text = x.Kategori,
                                         Value = x.ID.ToString()
                                     }).ToList();
            List<Birimler> birimlist = db.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimlist
                                  select new SelectListItem
                                  {
                                      Text = x.Birim,
                                      Value = x.ID.ToString()
                                  }
                                ).ToList();
        }
        [HttpPost]
        public ActionResult Ekle(Urunler m)
        {
            if (!ModelState.IsValid)
            {
                var model = new Urunler();
                Yenile(model);
                return View(model);
            }
            db.Entry(m).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult MiktarEkle(int id)
        {
            var model = db.Urunler.Find(id);
            return View();

        }
        [HttpPost]
        public ActionResult MiktarEkle(Urunler p)
        {
            var model = db.Urunler.Find(p.ID);
            model.Miktarı = model.Miktarı + p.Miktarı;
            db.SaveChanges();
            return RedirectToAction("Index2");
        }
        [HttpPost]
        public JsonResult GetMarka(int id2)
        {
            var model = new Urunler();
            List<Markalar> markaliste = db.Markalar.Where(x => x.KategoriID == id2).OrderBy(x => x.Marka).ToList();
            model.MarkaListesi = (from x in markaliste
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()

                                  }).ToList();
            model.MarkaListesi.Insert(0, new SelectListItem { Text = "SEÇİNİZ", Value = "" });
            return Json(model.MarkaListesi,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetir(int id)
        {
            var model = db.Urunler.Find(id);
            Yenile(model);
            List<Markalar> markalist = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            model.MarkaListesi = (from x in markalist
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()
                                  }).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Guncelle(Urunler p)
        {
            if (!ModelState.IsValid)
            {
                var model = db.Urunler.Find(p.ID);
                Yenile(model);
                List<Markalar> markalist = db.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
                model.MarkaListesi = (from x in markalist
                                      select new SelectListItem
                                      {
                                          Text = x.Marka,
                                          Value = x.ID.ToString()
                                      }).ToList();
                return View(model);
            }
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Sil(int id)
        //{
        //    var model = db.Urunler.FirstOrDefault(x => x.ID == id);
        //    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult SilBilgiGetir(Urunler p)
        {
            var model = db.Urunler.Find(p.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }
        public ActionResult Sil(Urunler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Index2(string ara)
        {
            var model = db.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList();
            }
            return View("Index2", model);
        }
        public JsonResult ExcelExport()
        {
            try
            {
                var liste = db.Urunler.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workboook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workboook.ActiveSheet;
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Ürün Adı";
                worksheet.Cells[1, 3] = "Barkod No";
                worksheet.Cells[1, 4] = "Fiyatı";
                worksheet.Cells[1, 5] = "Miktarı";
                worksheet.Cells[1, 6] = "Tarih";
                int row = 2;
                foreach(var item in liste)
                {
                    worksheet.Cells[row, 1] =item.ID;
                    worksheet.Cells[row, 2] = item.UrunAdi;
                    worksheet.Cells[row, 3] = item.BarkodNo;
                    worksheet.Cells[row, 4] = item.SatisFiyati;
                    worksheet.Cells[row, 5] = item.Miktarı;
                    worksheet.Cells[row, 6] = item.Tarih;
                    worksheet.Cells[row, 2].ColumnWidth = 15;
                    worksheet.Cells[row, 4].ColumnWidth = 15;
                    worksheet.Cells[row, 6].ColumnWidth = 15;
                    row++;
                }
                var heading = worksheet.get_Range("A1", "F1");
                heading.Font.Bold = true;
                heading.Font.Size = 13;
                heading.Font.Color = System.Drawing.Color.Red;
                var sum = db.Urunler.Sum(x => x.SatisFiyati);
                var range_sum = worksheet.get_Range("D" + row);
                range_sum.Value2 = "Total" + sum;
                range_sum.Font.Bold = true;
                var range_Fiyat = worksheet.get_Range("D2","D"+row);
                range_Fiyat.NumberFormat = "#,###,###.00";

                var range_Tarih = worksheet.get_Range("F2", "F" + row);
                range_Tarih.NumberFormat = "dd.MM.yyyy";





                workboook.SaveAs("C:\\ProgramData\\Microsoft Visual Studio\\test.xlsx");
                workboook.Close();
                application.Quit();

             

                ViewBag.mesaj = "İşlem başarılı";
            }
            catch(Exception ex)
            {
                ViewBag.mesaj = ex.Message;
            }
            return Json(ViewBag.mesaj, JsonRequestBehavior.AllowGet);

        }


    }
 
}