using HastaneProje.Data;
using HastaneProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace HastaneProje.Controllers
{
    [Route("hastane")]
    [ApiController]
    public class HastaneController : ControllerBase
    {
        private readonly HastaneContext _context;

        public HastaneController(HastaneContext context)
        {
            _context = context;
        }

        // 1. DOKTORLARI GETİR (Bölüm Adıyla Birlikte)
        // 1. DOKTORLARI GETİR (Rehber için Telefon ve Email EKLENDİ)
        [HttpGet("doktorlar")]
        public async Task<ActionResult<IEnumerable<object>>> GetDoktorlar()
        {
            var doktorlar = await _context.Doktor
                .Include(d => d.Bolum)
                .Select(d => new 
                {
                    d.doktor_id,
                    d.ad,
                    d.soyad,
                    d.telefon, // YENİ EKLENDİ
                    d.email,   // YENİ EKLENDİ
                    Uzmanlik = d.Bolum.bolum_adi, // Frontend'de 'uzmanlik' olarak gelecek
                    d.bolum_id
                })
                .ToListAsync();

            return Ok(doktorlar);
        }

        // YENİ: Yatan Hastaları Getir (Doktor Paneli İçin)
        // YENİ: Yatan Hastaları ve Sorumlu Hemşireleri Getir
[HttpGet("yatan-hastalar")]
public async Task<ActionResult<IEnumerable<object>>> GetYatanHastalar()
{
    var yatanlar = await _context.Yatis
        .Include(y => y.Hasta)
        .Include(y => y.Oda)
        .Where(y => y.cikis_tarihi == null)
        .Select(y => new 
        {
            y.yatis_id,
            y.giris_tarihi,
            y.cikis_tarihi,
            HastaAdSoyad = y.Hasta.ad + " " + y.Hasta.soyad,
            OdaNo = y.Oda.oda_no,
            
            // Hemşireleri çeken kısım
            Hemsireler = _context.Hemsire
                .Where(h => _context.HastaBakim.Any(hb => hb.hemsire_id == h.hemsire_id && hb.yatis_id == y.yatis_id))
                .Select(h => h.ad + " " + h.soyad)
                .ToList()
        })
        .OrderByDescending(y => y.giris_tarihi)
        .ToListAsync();

    return Ok(yatanlar);
}

        // 2. HASTALARI GETİR
        [HttpGet("hastalar")]
        public async Task<ActionResult<IEnumerable<Hasta>>> GetHastalar()
        {
            return await _context.Hasta.ToListAsync();
        }

        // 3. RANDEVULARI GETİR (Reçete Detayları ile)
        [HttpGet("randevular")]
        public async Task<ActionResult<IEnumerable<object>>> GetRandevular()
        {
            var randevular = await _context.Randevu
                .Include(r => r.Hasta)
                .Include(r => r.Doktor)
                .Include(r => r.Doktor.Bolum) // Doktorun bölümüne erişmek için
                .Include(r => r.ReceteDetays) // Reçete detaylarını çek
                    .ThenInclude(rd => rd.Ilac) // Her detayın ilaç ismini çek
                .Select(r => new 
                {
                    r.randevu_id,
                    r.randevu_tarihi,
                    r.randevu_saati,
                    r.randevu_durumu,
                    r.hasta_id,
                    r.doktor_id,
                    r.muayene_tarihi,
                    r.tani,
                    
                    HastaAdSoyad = r.Hasta.ad + " " + r.Hasta.soyad,
                    DoktorAdSoyad = r.Doktor.ad + " " + r.Doktor.soyad,
                    Bolum = r.Doktor.Bolum.bolum_adi, // Yeni yapı
                    
                    // Frontend'de reçeteyi liste olarak göstereceğiz
                    Ilaclar = r.ReceteDetays.Select(rd => new {
                        IlacAdi = rd.Ilac.ilac_adi,
                        Kullanim = rd.kullanim_sekli,
                        Adet = rd.adet
                    }).ToList()
                })
                .ToListAsync();

            return Ok(randevular);
        }

        // 4. FATURALARI GETİR (YENİ)
        [HttpGet("faturalar/{hastaId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetFaturalar(int hastaId)
        {
            var faturalar = await _context.Fatura
                .Where(f => f.hasta_id == hastaId)
                .Select(f => new 
                {
                    f.fatura_id,
                    f.tutar,
                    f.indirim_orani,
                    f.son_odeme_tutari,
                    f.fatura_tarihi,
                    f.odeme_durumu,
                    f.odeme_yontemi,
                    // Faturanın kaynağı (Randevu mu Yatış mı?)
                    Kaynak = f.randevu_id != null ? "Muayene Ücreti" : "Yatış Ücreti"
                })
                .OrderByDescending(f => f.fatura_tarihi)
                .ToListAsync();

            return Ok(faturalar);
        }

        // YENİ: HASTANIN TAHLİL SONUÇLARINI GETİR
        [HttpGet("tahliller/{hastaId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTahliller(int hastaId)
        {
            // RandevuTahlil tablosu üzerinden sorgu yapıyoruz
            // Randevu -> Tahlil ilişkisini kullanarak verileri çekiyoruz
            var tahliller = await _context.Set<RandevuTahlil>() // DbSet<RandevuTahlil> tanımlı değilse Set<> kullanabiliriz
                .Include(rt => rt.Randevu) // Randevu tarihine erişmek için
                .Include(rt => rt.Tahlil)  // Tahlil adına erişmek için
                .Where(rt => rt.Randevu.hasta_id == hastaId)
                .Select(rt => new 
                {
                    id = rt.id,
                    tarih = rt.Randevu.randevu_tarihi, // Tahlil tarihi olarak randevu tarihini alıyoruz
                    tahlilAdi = rt.Tahlil.tahlil_adi,
                    sonuc = rt.sonuc
                })
                .OrderByDescending(t => t.tarih)
                .ToListAsync();

            return Ok(tahliller);
        }

        // 5. NÖBETÇİLERİ GETİR (YENİ - Randevu alırken göstermek için)
        [HttpGet("nobetler")]
        public async Task<ActionResult<IEnumerable<object>>> GetNobetler()
        {
            var nobetler = await _context.Nobet
                .Include(n => n.Doktor)
                .Where(n => n.baslangic_zaman >= DateTime.Today) // Geçmiş nöbetlere gerek yok
                .Select(n => new 
                {
                    n.doktor_id,
                    baslangic = n.baslangic_zaman,
                    bitis = n.bitis_zaman,
                    tur = n.nobet_turu
                })
                .ToListAsync();

            return Ok(nobetler);
        }

        // 6. LOGIN İŞLEMİ
        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap([FromBody] LoginModel model)
        {
            // sp_GirisYap prosedüründe bir değişiklik yapmadıysak aynı kalabilir
            // Ancak Entity Framework Core ile Raw SQL çalıştırırken parametre kullanımına dikkat
            var result = await _context.Database
                .SqlQueryRaw<LoginResult>("EXEC sp_GirisYap @kullanici_adi={0}, @sifre={1}, @rol={2}", 
                    model.KullaniciAdi, model.Sifre, model.Rol)
                .ToListAsync();

            if (result.Count > 0)
            {
                return Ok(result.First());
            }
            
            return Unauthorized("Kullanıcı adı veya şifre hatalı.");
        }

        // 7. RANDEVU OLUŞTUR
        [HttpPost("randevu-olustur")]
        public IActionResult RandevuOlustur([FromBody] RandevuEkleDto model)
        {
            if (model == null) return BadRequest("Geçersiz veri.");

            try
            {
                var yeniRandevu = new Randevu 
                {
                    doktor_id = model.DoktorId,
                    hasta_id = model.HastaId,
                    randevu_tarihi = model.RandevuTarihi,
                    randevu_durumu = "Aktif",
                    randevu_saati = new TimeSpan(9, 0, 0) // Varsayılan saat
                };

                _context.Randevu.Add(yeniRandevu);
                _context.SaveChanges();

                return Ok(new { message = "Randevu başarıyla oluşturuldu.", id = yeniRandevu.randevu_id });
            }
            catch (Exception ex)
            {
                var hataMesaji = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { error = "Veritabanı hatası: " + hataMesaji });
            }
        }

        // 1. İLAÇLARI GETİR (Dropdown için)
        [HttpGet("ilaclar")]
        public async Task<ActionResult<IEnumerable<object>>> GetIlaclar()
        {
            var ilaclar = await _context.Ilac
                .Select(i => new 
                {
                    i.ilac_id,
                    i.ilac_adi,
                    i.stok_adedi
                })
                .ToListAsync();

            return Ok(ilaclar);
        }

        // 2. MUAYENE YAP (Tanı Gir ve Durumu Güncelle)
        [HttpPost("muayene-yap")]
        public async Task<IActionResult> MuayeneYap([FromBody] MuayeneTamamlaDto model)
        {
            var randevu = await _context.Randevu.FindAsync(model.randevu_id);
            if (randevu == null) return NotFound("Randevu bulunamadı.");

            // Bilgileri güncelle
            randevu.tani = model.tani;
            randevu.muayene_tarihi = DateTime.Now;
            randevu.randevu_durumu = "Tamamlandi"; // Durumu güncelle ki 'Geçmiş' sekmesine düşsün

            await _context.SaveChangesAsync();
            return Ok(new { message = "Muayene tamamlandı." });
        }

        // 3. REÇETE EKLE (İlaç Yaz)
        [HttpPost("recete-ekle")]
        public async Task<IActionResult> ReceteEkle([FromBody] ReceteEkleDto model)
        {
            try 
            {
                var yeniReceteDetay = new ReceteDetay
                {
                    randevu_id = model.randevu_id,
                    ilac_id = model.ilac_id,
                    kullanim_sekli = model.kullanim_sekli,
                    adet = model.adet
                };

                _context.ReceteDetay.Add(yeniReceteDetay);
                
                // İlaç stoktan düş (Opsiyonel ama mantıklı)
                var ilac = await _context.Ilac.FindAsync(model.ilac_id);
                if(ilac != null && ilac.stok_adedi > 0)
                {
                    ilac.stok_adedi -= model.adet;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "İlaç reçeteye eklendi." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Reçete eklenirken hata: " + ex.Message);
            }
        }
    }

    // DTOs
    public class LoginModel {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; }
    }

    public class LoginResult {
        public int kullanici_id { get; set; }
        public string rol { get; set; }
        public int? hasta_id { get; set; }   // Yeni eklendi (Null olabilir)
        public int? doktor_id { get; set; } 
        public string ad { get; set; }
        public string soyad { get; set; }
    }

    public class RandevuEkleDto {
        // Klinik ID kaldırıldı, çünkü artık Doktor -> Bolum ilişkisi var
        [JsonPropertyName("doktor_id")] public int DoktorId { get; set; }
        [JsonPropertyName("hasta_id")] public int HastaId { get; set; }
        [JsonPropertyName("randevu_tarihi")] public DateTime RandevuTarihi { get; set; }
    }

    // Frontend'den gelen verileri karşılamak için modeller
    public class MuayeneTamamlaDto
    {
        public int randevu_id { get; set; }
        public string tani { get; set; }
    }

    public class ReceteEkleDto
    {
        public int randevu_id { get; set; }
        public int ilac_id { get; set; }
        public string kullanim_sekli { get; set; }
        public int adet { get; set; }
    }
}