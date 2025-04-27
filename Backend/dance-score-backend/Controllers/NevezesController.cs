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
    public class NevezesController : ApiController
    {
        private readonly DanceScoreDbContext _context;

        public NevezesController()
        {
            _context = new DanceScoreDbContext();
        }

        [HttpGet]
        public IHttpActionResult GetNevezesek(string verseny = null, string kategoria = null)
        {
            try
            {
                var nevezesekQuery = from n in _context.Nevezesek
                                     join v in _context.Versenyek on n.VersenyId equals v.Id
                                     join c in _context.Csapatok on n.CsapatId equals c.Id
                                     join k in _context.Kategoriak on n.KategoriaId equals k.Id into kJoin
                                     from k in kJoin.DefaultIfEmpty()
                                     select new
                                     {
                                         NevezesId = n.Id,
                                         VersenyNev = v.Nev,
                                         VersenyIdopont = v.Idopont,
                                         KategoriaNev = k != null ? k.Nev : null,
                                         CsapatId = c.Id,
                                         CsapatNev = c.Nev,
                                         Eredmenyek = _context.Eredmenyek.Where(e => e.NevezesId == n.Id).ToList()
                                     };

                if (!string.IsNullOrEmpty(verseny))
                {
                    nevezesekQuery = nevezesekQuery.Where(n => n.VersenyNev == verseny);
                }

                if (!string.IsNullOrEmpty(kategoria))
                {
                    nevezesekQuery = nevezesekQuery.Where(n => n.KategoriaNev == kategoria);
                }

                var nevezesek = nevezesekQuery.ToList().Select(n => new NevezesDto
                {
                    Id = n.NevezesId,
                    Verseny = n.VersenyNev,
                    VersenyIdopont = n.VersenyIdopont,
                    Kategoria = n.KategoriaNev ?? "Nincs kategória",
                    CsapatId = n.CsapatId,
                    CsapatNev = n.CsapatNev,
                    Pontszam = n.Eredmenyek.Any() ? (int?)n.Eredmenyek.Sum(e => e.Pontszam ?? 0) : null, // Átlag helyett összeg
                    Rogzitve = n.Eredmenyek.Any() ? n.Eredmenyek.Max(e => e.Rogzitve) : null,
                    PontozasokSzama = n.Eredmenyek.Count,
                    PontozasHiany = n.Eredmenyek.Count < 5 ? $"Hiányzik {5 - n.Eredmenyek.Count} pontozás" : "Teljes"
                }).ToList();

                return Ok(nevezesek);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}