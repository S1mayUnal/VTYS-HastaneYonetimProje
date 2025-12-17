using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class HastaBakim
    {
        [Key]
        public int bakim_id { get; set; }

        public int yatis_id { get; set; }
        public int hemsire_id { get; set; }
        
        public string gorev_tanimi { get; set; }

        // --- İlişkiler (Navigation Properties) ---
        
        [ForeignKey("yatis_id")]
        public virtual Yatis Yatis { get; set; }

        [ForeignKey("hemsire_id")]
        public virtual Hemsire Hemsire { get; set; }
    }
}