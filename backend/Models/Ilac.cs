using System.ComponentModel.DataAnnotations;

namespace HastaneProje.Models
{
    public class Ilac
    {
        [Key]
        public int ilac_id { get; set; }
        public string ilac_adi { get; set; }
        public string barkod { get; set; }
        public int stok_adedi { get; set; }
    }
}