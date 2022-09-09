using System.ComponentModel.DataAnnotations;

namespace FaksPrijave.Models
{
    #region snippet1

    public class Fakultet //Mozda za ovo iskopirat ID ko iz AspNetUser
    {
        public int FakultetId { get; set; }
        public string? FakultetIme { get; set; }
        public int? SlobodnoMjesta { get; set; }
    }

    public class Prijava
    {
        public int PrijavaId { get; set; }
        public string? OwnerID { get; set; }
        public int FakultetId { get; set; }
        public int? BodoviPrijemni { get; set; }
        public int? BodoviSrednja { get; set; }
        public int? BodoviNatjecanja { get; set; }
        public int? UkupnoBodova { get; set; }
    }
    #endregion
}
