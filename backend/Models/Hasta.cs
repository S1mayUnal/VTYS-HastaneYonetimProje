using System.ComponentModel.DataAnnotations;

namespace HastaneProje.Models
{
    public class Hasta
    {
        [Key]
        public int hasta_id { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string tc_no { get; set; }
        public DateTime? dogum_tarihi { get; set; }
        public string cinsiyet { get; set; }
        public string telefon_no { get; set; } // SQL'de rename işlemi yaptığın için
    }
}