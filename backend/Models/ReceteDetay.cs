using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class ReceteDetay
    {
        [Key]
        public int recete_detay_id { get; set; }
        
        public int randevu_id { get; set; }
        public int ilac_id { get; set; }
        public string kullanim_sekli { get; set; }
        public int adet { get; set; }

        // İlişkiler
        [ForeignKey("ilac_id")]
        public virtual Ilac Ilac { get; set; }
        
        [ForeignKey("randevu_id")]
        public virtual Randevu Randevu { get; set; }
    }
}