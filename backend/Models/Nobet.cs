using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Nobet
    {
        [Key]
        public int nobet_id { get; set; }
        public int bolum_id { get; set; }
        
        public int? doktor_id { get; set; }
        public int? hemsire_id { get; set; }
        
        public DateTime baslangic_zaman { get; set; }
        public DateTime bitis_zaman { get; set; }
        public string nobet_turu { get; set; }

        // İlişkiler (Controller'daki Include için gerekli)
        [ForeignKey("doktor_id")]
        public virtual Doktor Doktor { get; set; }
    }
}