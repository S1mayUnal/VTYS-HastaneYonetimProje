using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Yatis
    {
        [Key]
        public int yatis_id { get; set; }
        
        public int hasta_id { get; set; }
        public int oda_id { get; set; }
        
        public DateTime giris_tarihi { get; set; }
        public DateTime? cikis_tarihi { get; set; } // Null olabilir

        // İlişkiler (Controller'da Include yapmak için şart)
        [ForeignKey("hasta_id")]
        public virtual Hasta Hasta { get; set; }

        [ForeignKey("oda_id")]
        public virtual Oda Oda { get; set; }
    }
}