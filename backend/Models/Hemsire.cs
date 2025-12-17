using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Hemsire
    {
        [Key]
        public int hemsire_id { get; set; }
        public int bolum_id { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string telefon { get; set; }
        public string diploma_no { get; set; }
    }
}