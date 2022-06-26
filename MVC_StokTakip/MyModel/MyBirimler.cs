using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_StokTakip.MyModel
{
    public class MyBirimler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyBirimler()
        {
            this.Urunler = new HashSet<MyUrunler>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "birim alanı boş geçilemez")]
        public string Birim { get; set; }
        [Required(ErrorMessage = "açıklama alanı boş geçilemez")]
        public string Aciklama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MyUrunler> Urunler { get; set; }
    }
}
