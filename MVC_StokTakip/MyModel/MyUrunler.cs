using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokTakip.MyModel
{
    public class MyUrunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyUrunler()
        {
            //this.Satislar = new HashSet<Satislar>();
            //this.Sepet = new HashSet<Sepet>();
            this.MarkaListesi = new List<SelectListItem>();
            MarkaListesi.Insert(0, new SelectListItem { Text = "Önce Kategori seçilmelidir", Value = "" });
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Kategori alanı boş geçilemez")]
        public Nullable<int> KategoriID { get; set; }
        [Required(ErrorMessage = "Marka alanı boş geçilemez")]
        public Nullable<int> MarkaID { get; set; }
        [Required(ErrorMessage = "Urun Adı alanı boş geçilemez")]
        public string UrunAdi { get; set; }
        [Required(ErrorMessage = "Barkod No alanı boş geçilemez")]
        public string BarkodNo { get; set; }
        [Required(ErrorMessage = "AlısFiyatı alanı boş geçilemez")]
        public Nullable<decimal> AlisFiyati { get; set; }
        [Required(ErrorMessage = "Satıs Fiyatı alanı boş geçilemez")]
        public Nullable<decimal> SatisFiyati { get; set; }
        [Required(ErrorMessage = "Kdv alanı boş geçilemez")]
        public Nullable<int> KDV { get; set; }
        [Required(ErrorMessage = "Birim alanı boş geçilemez")]
        public Nullable<int> BirimID { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Tarih alanı boş geçilemez")]
        public Nullable<System.DateTime> Tarih { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez")]
        public string Aciklama { get; set; }
        public Nullable<decimal> Miktarı { get; set; }

        //public virtual Birimler Birimler { get; set; }
     
        public virtual MyMarkalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       // public virtual ICollection<Satislar> Satislar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Sepet> Sepet { get; set; }
        public virtual MyKategoriler Kategoriler { get; set; }
        public List<SelectListItem> KategoriListesi { get; set; }
        public List<SelectListItem> MarkaListesi { get; set; }
        //public List<SelectListItem> BirimListesi { get; set; }
    }
}
