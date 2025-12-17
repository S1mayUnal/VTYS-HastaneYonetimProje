using System.Text.Json.Serialization;

namespace HastaneProje.Models
{
    public class RandevuEkleDto
    {
        // Vue'dan "klinik_id" olarak geliyor, C# tarafında eşleştiriyoruz
        [JsonPropertyName("klinik_id")]
        public int KlinikId { get; set; }

        [JsonPropertyName("doktor_id")]
        public int DoktorId { get; set; }

        [JsonPropertyName("hasta_id")]
        public int HastaId { get; set; }

        [JsonPropertyName("randevu_tarihi")]
        public DateTime RandevuTarihi { get; set; }

        [JsonPropertyName("durum")]
        public string Durum { get; set; }
    }
}