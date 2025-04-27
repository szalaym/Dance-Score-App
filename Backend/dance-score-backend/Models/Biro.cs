using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Biro
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Jelszo { get; set; }
        public string TitkosKulcs { get; set; }

        public virtual ICollection<Nevezes> Nevezesek { get; set; }
        public virtual ICollection<Eredmeny> Eredmenyek { get; set; }

        public Biro()
        {
            Nevezesek = new List<Nevezes>();
            Eredmenyek = new List<Eredmeny>();
        }
    }
}