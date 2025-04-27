using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dance_score_backend.Data;
using dance_score_backend.Models;
using System.IO;

namespace dance_score_backend.Controllers
{
    public class CsapatController : ApiController
    {
        private readonly DanceScoreDbContext db = new DanceScoreDbContext();

        /// <summary>
        /// Regisztrál egy új csapatot, és opcionálisan nevezést hoz létre egy versenyhez.
        /// </summary>
        /// <param name="dto">A csapat regisztrációs adatai.</param>
        /// <returns>A regisztrált csapat azonosítója, vagy hibaüzenet.</returns>
        [HttpPost]
        public IHttpActionResult CsapatRegisztralas([FromBody] CsapatRegisztracioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(dto.CsapatNev))
            {
                return BadRequest("A csapat neve kötelező!");
            }

            if (dto.Letszam <= 0)
            {
                return BadRequest("A létszámnak nagyobbnak kell lennie, mint 0!");
            }

            if (dto.Tagok == null || dto.Tagok.Length != dto.Letszam)
            {
                return BadRequest("A tagok száma nem egyezik a létszámmal!");
            }

            // Ellenőrizzük, hogy a csapat neve már létezik-e
            if (db.Csapatok.Any(c => c.Nev.ToLower() == dto.CsapatNev.ToLower()))
            {
                return BadRequest("Ez a csapatnév már létezik!");
            }

            var csapat = new Csapat
            {
                Nev = dto.CsapatNev,
                Letszam = dto.Letszam,
                ZeneLink = dto.ZeneLink,
                Csapattagok = new List<Csapattag>()
            };

            foreach (var tag in dto.Tagok)
            {
                if (string.IsNullOrWhiteSpace(tag))
                {
                    return BadRequest("Minden csapattag neve kötelező!");
                }
                csapat.Csapattagok.Add(new Csapattag { Nev = tag });
            }

            try
            {
                db.Csapatok.Add(csapat);
                db.SaveChanges();

                // Ha van Verseny és Kategoria, hozzunk létre egy Nevezes rekordot
                if (!string.IsNullOrWhiteSpace(dto.Verseny) && !string.IsNullOrWhiteSpace(dto.Kategoria))
                {
                    var verseny = db.Versenyek.FirstOrDefault(v => v.Nev.ToLower() == dto.Verseny.ToLower());
                    if (verseny == null)
                    {
                        return BadRequest("A megadott verseny nem létezik!");
                    }

                    var kategoria = db.Kategoriak.FirstOrDefault(k => k.Nev.ToLower() == dto.Kategoria.ToLower());
                    if (kategoria == null)
                    {
                        return BadRequest("A megadott kategória nem létezik!");
                    }

                    // Ellenőrizzük, hogy a versenyhez tartozik-e a kategória
                    if (!db.VersenyKategoriak.Any(vk => vk.VersenyId == verseny.Id && vk.KategoriaId == kategoria.Id))
                    {
                        return BadRequest("A megadott kategória nem tartozik a versenyhez!");
                    }

                    // Hozzunk létre egy nevezést
                    var nevezes = new Nevezes
                    {
                        VersenyId = verseny.Id,
                        CsapatId = csapat.Id,
                        KategoriaId = kategoria.Id, // Itt állítjuk be a KategoriaId-t
                        Datum = DateTime.Now
                    };

                    db.Nevezesek.Add(nevezes);
                    db.SaveChanges();
                }

                return Ok(csapat.Id);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "Nincs belső kivétel.";
                return BadRequest($"Hiba történt a csapat regisztrálása közben: {ex.Message}. Belső kivétel: {innerExceptionMessage}");
            }
        }

        /// <summary>
        /// Felszabadítja az adatbázis kapcsolatot.
        /// </summary>
        /// <param name="disposing">Igaz, ha a felszabadítás explicit módon történik.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class CsapatRegisztracioDto
    {
        public string CsapatNev { get; set; }
        public int Letszam { get; set; }
        public string Kategoria { get; set; }
        public string Verseny { get; set; }
        public string ZeneLink { get; set; }
        public string[] Tagok { get; set; }
    }
}