using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Verseny
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }
        public DateTime Idopont { get; set; }

        public virtual ICollection<VersenyKategoria> VersenyKategoriak { get; set; }
        public virtual ICollection<Nevezes> Nevezesek { get; set; }

        public Verseny()
        {
            VersenyKategoriak = new List<VersenyKategoria>();
            Nevezesek = new List<Nevezes>();
        }
    }
}