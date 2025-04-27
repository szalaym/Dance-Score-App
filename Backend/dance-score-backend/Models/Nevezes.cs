using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Nevezes
    {
        [Key]
        public int Id { get; set; }
        public int? BiroId { get; set; }
        public int VersenyId { get; set; }
        public int CsapatId { get; set; }
        public int? KategoriaId { get; set; } // Új mező
        public DateTime? Datum { get; set; }

        public virtual Biro Biro { get; set; }
        public virtual Verseny Verseny { get; set; }
        public virtual Csapat Csapat { get; set; }
        public virtual Kategoria Kategoria { get; set; } // Kapcsolat a Kategoria entitással
        public virtual ICollection<Eredmeny> Eredmenyek { get; set; }

        public Nevezes()
        {
            Eredmenyek = new List<Eredmeny>();
        }
    }
}