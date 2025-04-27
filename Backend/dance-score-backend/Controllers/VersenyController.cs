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
    public class VersenyController : ApiController
    {
        private readonly DanceScoreDbContext db = new DanceScoreDbContext();

        /// <summary>
        /// Visszaadja az összes versenyt a kategóriáikkal együtt.
        /// </summary>
        /// <returns>A versenyek listája JSON formátumban.</returns>
        [HttpGet]
        public IHttpActionResult GetVersenyek()
        {
            try
            {
                var versenyek = db.Versenyek
                    .Select(v => new
                    {
                        v.Id,
                        v.Nev,
                        v.Idopont,
                        Kategoriak = v.VersenyKategoriak.Select(vk => vk.Kategoria.Nev).ToList()
                    })
                    .ToList();

                return Ok(versenyek);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "Nincs belső kivétel.";
                return BadRequest($"Hiba történt a versenyek lekérdezése közben: {ex.Message}. Belső kivétel: {innerExceptionMessage}");
            }
        }

        /// <summary>
        /// Létrehoz egy új versenyt a megadott kategóriákkal.
        /// </summary>
        /// <param name="dto">A verseny adatai.</param>
        /// <returns>Sikeres létrehozás esetén üzenet, különben hibaüzenet.</returns>
        [HttpPost]
        public IHttpActionResult CreateVerseny([FromBody] VersenyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(dto.Nev))
            {
                return BadRequest("A verseny neve kötelező!");
            }

            if (dto.Idopont == default(DateTime))
            {
                return BadRequest("Az időpont megadása kötelező!");
            }

            if (db.Versenyek.Any(v => v.Nev.ToLower() == dto.Nev.ToLower()))
            {
                return BadRequest("Ez a verseny neve már létezik!");
            }

            if (dto.Kategoriak == null || !dto.Kategoriak.Any())
            {
                return BadRequest("Legalább egy kategória megadása kötelező!");
            }

            var verseny = new Verseny
            {
                Nev = dto.Nev,
                Idopont = dto.Idopont
            };

            try
            {
                db.Versenyek.Add(verseny);
                db.SaveChanges(); // Először mentjük a versenyt, hogy legyen Id-je

                foreach (var kategoriaNev in dto.Kategoriak)
                {
                    if (string.IsNullOrWhiteSpace(kategoriaNev))
                    {
                        continue; // Üres kategórianeveket kihagyjuk
                    }

                    var kategoria = db.Kategoriak.FirstOrDefault(k => k.Nev.ToLower() == kategoriaNev.ToLower());
                    if (kategoria == null)
                    {
                        kategoria = new Kategoria { Nev = kategoriaNev };
                        db.Kategoriak.Add(kategoria);
                        db.SaveChanges();
                    }

                    var versenyKategoria = new VersenyKategoria
                    {
                        VersenyId = verseny.Id,
                        KategoriaId = kategoria.Id
                    };

                    db.VersenyKategoriak.Add(versenyKategoria);
                }

                db.SaveChanges();
                return Ok("Verseny sikeresen hozzáadva!");
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "Nincs belső kivétel.";
                return BadRequest($"Hiba történt a verseny hozzáadása közben: {ex.Message}. Belső kivétel: {innerExceptionMessage}");
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

    public class VersenyDto
    {
        public string Nev { get; set; }
        public DateTime Idopont { get; set; }
        public string[] Kategoriak { get; set; }
    }
}