using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class RandevuTahlil
    {
        [Key]
        public int id { get; set; }
        public int randevu_id { get; set; }
        public int tahlil_id { get; set; }
        public string sonuc { get; set; }

        [ForeignKey("randevu_id")]
        public virtual Randevu Randevu { get; set; }

        [ForeignKey("tahlil_id")]
        public virtual Tahlil Tahlil { get; set; }
    }
}