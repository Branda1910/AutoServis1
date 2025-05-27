using Microsoft.EntityFrameworkCore;

namespace AutoServis.Models
{
    public class BazaContext : DbContext
    {
        public BazaContext(DbContextOptions<BazaContext> options) : base(options) { }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<NarudzbaServisa> NarudzbeServisa { get; set; }
    }
}
