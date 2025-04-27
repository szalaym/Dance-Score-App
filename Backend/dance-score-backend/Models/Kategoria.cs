using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Kategoria
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }

        public virtual ICollection<VersenyKategoria> VersenyKategoriak { get; set; }

        public Kategoria()
        {
            VersenyKategoriak = new List<VersenyKategoria>();
        }
    }
}