using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Eredmeny
    {
        [Key]
        public int Id { get; set; }
        public int NevezesId { get; set; }
        public int BiroId { get; set; }
        public int? Pontszam { get; set; }
        public DateTime? Rogzitve { get; set; }

        public virtual Nevezes Nevezes { get; set; }
        public virtual Biro Biro { get; set; }
    }
}