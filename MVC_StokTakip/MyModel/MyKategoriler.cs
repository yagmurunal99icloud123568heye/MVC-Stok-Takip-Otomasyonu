using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.MyModel
{
    public class MyKategoriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyKategoriler()
        {
            this.Markalar = new HashSet<MyMarkalar>();
            //this.Sepet = new HashSet<Sepet>();
            this.Urunler = new HashSet<MyUrunler>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "kategori alanı boş geçilemez")]
        public string Kategori { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez")]
        public string Aciklama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MyMarkalar> Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
      //  public virtual ICollection<Sepet> Sepet { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MyUrunler> Urunler { get; set; }
    }
}
