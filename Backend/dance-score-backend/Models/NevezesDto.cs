using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dance_score_backend.Models
{
    [DataContract]
    public class NevezesDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Verseny { get; set; }

        [DataMember]
        public DateTime VersenyIdopont { get; set; }

        [DataMember]
        public string Kategoria { get; set; }

        [DataMember]
        public int CsapatId { get; set; }

        [DataMember]
        public string CsapatNev { get; set; }

        [DataMember]
        public int? Pontszam { get; set; } // int helyett int?, hogy kezelje a null értéket

        [DataMember]
        public DateTime? Rogzitve { get; set; }

        [DataMember]
        public int PontozasokSzama { get; set; }

        [DataMember]
        public string PontozasHiany { get; set; }
    }
}