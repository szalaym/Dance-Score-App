using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dance_score_backend.Data;
using dance_score_backend.Models;
using System.Security.Cryptography;
using System.Text;

namespace dance_score_backend.Controllers
{
    public class BirokController : ApiController
    {
        private readonly DanceScoreDbContext db = new DanceScoreDbContext();

        /// <summary>
        /// Visszaadja az összes bíró adatait.
        /// </summary>
        /// <returns>A bírók listája JSON formátumban.</returns>
        [HttpGet]
        public IHttpActionResult GetBirok()
        {
            try
            {
                var birok = db.Birok.Select(b => new
                {
                    b.Id,
                    b.Nev,
                    b.Email,
                    b.Telefon,
                    b.Jelszo
                }).ToList();

                return Ok(birok);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "Nincs belső kivétel.";
                return BadRequest($"Hiba történt a bírók lekérdezése közben: {ex.Message}. Belső kivétel: {innerExceptionMessage}");
            }
        }

        /// <summary>
        /// Regisztrál egy új bírót.
        /// </summary>
        /// <param name="dto">A bíró regisztrációs adatai.</param>
        /// <returns>A regisztrált bíró azonosítója, vagy hibaüzenet.</returns>
        [HttpPost]
        public IHttpActionResult BiroRegisztralas([FromBody] BiroRegisztracioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(dto.Nev))
            {
                return BadRequest("A név megadása kötelező!");
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("Az email megadása kötelező!");
            }
            if (string.IsNullOrWhiteSpace(dto.Telefon))
            {
                return BadRequest("A telefon megadása kötelező!");
            }
            if (string.IsNullOrWhiteSpace(dto.Jelszo))
            {
                return BadRequest("A jelszó megadása kötelező!");
            }
            if (string.IsNullOrWhiteSpace(dto.TitkosKulcs))
            {
                return BadRequest("A titkos kulcs megadása kötelező!");
            }

            // Ellenőrizzük, hogy az email már létezik-e
            if (db.Birok.Any(b => b.Email.ToLower() == dto.Email.ToLower()))
            {
                return BadRequest("Ez az email cím már regisztrálva van!");
            }

            var biro = new Biro
            {
                Nev = dto.Nev,
                Email = dto.Email,
                Telefon = dto.Telefon,
                Jelszo = JelszoTitkositas(dto.Jelszo),
                TitkosKulcs = JelszoTitkositas(dto.TitkosKulcs)
            };

            try
            {
                db.Birok.Add(biro);
                db.SaveChanges();
                return Ok(biro.Id);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "Nincs belső kivétel.";
                return BadRequest($"Hiba történt a bíró regisztrálása közben: {ex.Message}. Belső kivétel: {innerExceptionMessage}");
            }
        }

        private string JelszoTitkositas(string jelszo)
        {
            if (string.IsNullOrWhiteSpace(jelszo))
            {
                throw new ArgumentException("A jelszó nem lehet üres!");
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(jelszo));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
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

    public class BiroRegisztracioDto
    {
        public string Nev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Jelszo { get; set; }
        public string TitkosKulcs { get; set; }
    }
}