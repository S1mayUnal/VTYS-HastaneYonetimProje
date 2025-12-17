using System.ComponentModel.DataAnnotations;

namespace HastaneProje.Models
{
    public class Tahlil
    {
        [Key]
        public int tahlil_id { get; set; }
        public string tahlil_adi { get; set; }
        public decimal ucret { get; set; }
    }
}