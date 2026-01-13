using Microsoft.EntityFrameworkCore;
using HastaneProje.Models;

namespace HastaneProje.Data
{
    public class HastaneContext : DbContext
    {
        public HastaneContext(DbContextOptions<HastaneContext> options) : base(options) { }

        public DbSet<Doktor> Doktor { get; set; }
        public DbSet<Hasta> Hasta { get; set; }
        public DbSet<Randevu> Randevu { get; set; }
        public DbSet<Bolum> Bolum { get; set; }
        public DbSet<Ilac> Ilac { get; set; }
        public DbSet<ReceteDetay> ReceteDetay { get; set; }
        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<Nobet> Nobet { get; set; }
        public DbSet<Hemsire> Hemsire { get; set; }

        public DbSet<Oda> Oda { get; set; }
        public DbSet<Yatis> Yatis { get; set; }

        public DbSet<Tahlil> Tahlil { get; set; }
        public DbSet<RandevuTahlil> RandevuTahlil { get; set; }
        
        public DbSet<HastaBakim> HastaBakim { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doktor>().ToTable("Doktor");
            modelBuilder.Entity<Hasta>().ToTable("Hasta");
            modelBuilder.Entity<Randevu>().ToTable("Randevu");
        }
    }
}