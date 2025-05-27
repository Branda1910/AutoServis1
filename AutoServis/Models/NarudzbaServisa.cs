using System;
using System.ComponentModel.DataAnnotations;

namespace AutoServis.Models
{
    public class NarudzbaServisa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmailKorisnika { get; set; } = string.Empty;

        [Required]
        public string OpisProblema { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PredlozeniDatum { get; set; }

        public DateTime? PotvrdjeniDatum { get; set; }

        [Required]
        public string Status { get; set; } = "Čeka potvrdu";

        public string VrstaUsluge { get; set; } = "";

        public decimal Cijena { get; set; } = 0;

        [DataType(DataType.DateTime)]
        public DateTime? DatumNarudzbe { get; set; } // ✅ nullable radi sigurnosti s postojećim redovima
    }
}