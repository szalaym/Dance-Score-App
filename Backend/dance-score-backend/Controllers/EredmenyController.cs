using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dance_score_backend.Data;
using dance_score_backend.Models;

namespace dance_score_backend.Controllers
{
    public class EredmenyController : ApiController
    {
        private readonly DanceScoreDbContext _context;

        public EredmenyController()
        {
            _context = new DanceScoreDbContext();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] EredmenyDto eredmenyDto)
        {
            try
            {
                if (eredmenyDto == null)
                {
                    return BadRequest("Érvénytelen kérés: az eredmény adatai hiányoznak.");
                }

                // Ellenőrizzük, hogy a NevezesId és BiroId létezik-e
                var nevezes = _context.Nevezesek.Find(eredmenyDto.NevezesId);
                if (nevezes == null)
                {
                    return BadRequest($"Nem található nevezés a megadott NevezesId-vel: {eredmenyDto.NevezesId}");
                }

                var biro = _context.Birok.Find(eredmenyDto.BiroId);
                if (biro == null)
                {
                    return BadRequest($"Nem található bíró a megadott BiroId-vel: {eredmenyDto.BiroId}");
                }

                // Ellenőrizzük, hogy a bíró már pontozta-e ezt a nevezést
                var existingEredmeny = _context.Eredmenyek
                    .FirstOrDefault(e => e.NevezesId == eredmenyDto.NevezesId && e.BiroId == eredmenyDto.BiroId);
                if (existingEredmeny != null)
                {
                    return BadRequest("Ez a bíró már pontozta ezt a nevezést!");
                }

                // Új eredmény létrehozása
                var eredmeny = new Eredmeny
                {
                    NevezesId = eredmenyDto.NevezesId,
                    BiroId = eredmenyDto.BiroId,
                    Pontszam = eredmenyDto.Pontszam,
                    Rogzitve = eredmenyDto.Rogzitve ?? DateTime.Now // Ha a Rogzitve null, akkor az aktuális időt használjuk
                };

                _context.Eredmenyek.Add(eredmeny);
                _context.SaveChanges();

                return Ok("Pontozás sikeresen rögzítve!");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class EredmenyDto
    {
        public int NevezesId { get; set; }
        public int BiroId { get; set; }
        public int Pontszam { get; set; }
        public DateTime? Rogzitve { get; set; }
    }
}