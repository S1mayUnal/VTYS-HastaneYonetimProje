using System.ComponentModel.DataAnnotations;


namespace HastaneProje.Models
{
    public class Bolum
    {
        [Key]
        public int bolum_id { get; set; }
        public string bolum_adi { get; set; }
        public int kat_no { get; set; }
    }
}