using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class Csapattag
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }
        public int CsapatId { get; set; }

        public virtual Csapat Csapat { get; set; }
    }
}