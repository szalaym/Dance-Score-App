using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Csapat
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }
        public int Letszam { get; set; }
        public string ZeneLink { get; set; }

        public virtual ICollection<Csapattag> Csapattagok { get; set; }
        public virtual ICollection<Nevezes> Nevezesek { get; set; }

        public Csapat()
        {
            Csapattagok = new List<Csapattag>();
            Nevezesek = new List<Nevezes>();
        }
    }
}