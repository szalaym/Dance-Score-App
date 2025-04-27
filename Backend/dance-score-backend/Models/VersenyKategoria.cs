using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    public class VersenyKategoria
    {
        [Key]
        public int VersenyId { get; set; }
        [Key]
        public int KategoriaId { get; set; }

        public virtual Verseny Verseny { get; set; }
        public virtual Kategoria Kategoria { get; set; }
    }
}