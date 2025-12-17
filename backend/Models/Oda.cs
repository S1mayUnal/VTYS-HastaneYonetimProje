using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneProje.Models
{
    public class Oda
    {
        [Key]
        public int oda_id { get; set; }
        public int bolum_id { get; set; }
        public string oda_no { get; set; }
        public int kapasite { get; set; }

        [ForeignKey("bolum_id")]
        public virtual Bolum Bolum { get; set; }
    }
}