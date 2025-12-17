using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Doktor
{
    [Key]
    public int doktor_id { get; set; }
    public int bolum_id { get; set; } // Yeni eklendi
    public string ad { get; set; }
    public string soyad { get; set; }
    public string telefon { get; set; }
    public string email { get; set; }
    public decimal maas { get; set; }

    // İlişki Tanımı
    [ForeignKey("bolum_id")]
    public virtual Bolum Bolum { get; set; }
}
}