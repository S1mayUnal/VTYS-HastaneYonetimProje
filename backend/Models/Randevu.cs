using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Randevu
{
    [Key]
    public int randevu_id { get; set; }
    public int hasta_id { get; set; }
    public int doktor_id { get; set; }
    public DateTime randevu_tarihi { get; set; }
    public TimeSpan randevu_saati { get; set; }
    public string randevu_durumu { get; set; }
    public DateTime? muayene_tarihi { get; set; }
    public string tani { get; set; }
    // recete stringini silmiştik, modelden de sil

    // İlişkiler
    [ForeignKey("hasta_id")]
    public virtual Hasta Hasta { get; set; }

    [ForeignKey("doktor_id")]
    public virtual Doktor Doktor { get; set; }

    // YENİ EKLENECEK OLAN KISIM:
    public virtual ICollection<ReceteDetay> ReceteDetays { get; set; }
}
}