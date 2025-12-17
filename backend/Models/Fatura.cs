using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Fatura
    {
        [Key]
        public int fatura_id { get; set; }
        public int hasta_id { get; set; }
        
        public int? randevu_id { get; set; } // Null olabilir
        public int? yatis_id { get; set; }   // Null olabilir
        
        public decimal tutar { get; set; }
        public decimal indirim_orani { get; set; }
        
        // SQL'de hesaplanan alan (Computed Column) olduğu için
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public decimal? son_odeme_tutari { get; set; }
        
        public DateTime fatura_tarihi { get; set; }
        public string odeme_durumu { get; set; }
        public string odeme_yontemi { get; set; }
    }
}