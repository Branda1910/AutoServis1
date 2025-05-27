using System.ComponentModel.DataAnnotations;

namespace AutoServis.Models
{
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Lozinka { get; set; }

        public bool Aktiviran { get; set; }
    }
}
