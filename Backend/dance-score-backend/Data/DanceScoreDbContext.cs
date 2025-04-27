using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dance_score_backend.Models;
using System.Data.Entity;

namespace dance_score_backend.Data
{
    public class DanceScoreDbContext : DbContext
    {
        public DanceScoreDbContext() : base("name=DanceScoreDbContext")
        {
        }

        public DbSet<Biro> Birok { get; set; }
        public DbSet<Csapat> Csapatok { get; set; }
        public DbSet<Csapattag> Csapattagok { get; set; }
        public DbSet<Verseny> Versenyek { get; set; }
        public DbSet<Kategoria> Kategoriak { get; set; }
        public DbSet<VersenyKategoria> VersenyKategoriak { get; set; }
        public DbSet<Nevezes> Nevezesek { get; set; }
        public DbSet<Eredmeny> Eredmenyek { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Biro>().ToTable("Birok");
            modelBuilder.Entity<Csapat>().ToTable("Csapatok");
            modelBuilder.Entity<Csapattag>().ToTable("Csapattagok");
            modelBuilder.Entity<Verseny>().ToTable("Versenyek");
            modelBuilder.Entity<Kategoria>().ToTable("Kategoriak");
            modelBuilder.Entity<VersenyKategoria>().ToTable("VersenyKategoriak");
            modelBuilder.Entity<Nevezes>().ToTable("Nevezesek");
            modelBuilder.Entity<Eredmeny>().ToTable("Eredmenyek");

            modelBuilder.Entity<VersenyKategoria>()
                .HasKey(vk => new { vk.VersenyId, vk.KategoriaId });

            modelBuilder.Entity<VersenyKategoria>()
                .HasRequired(vk => vk.Verseny)
                .WithMany(v => v.VersenyKategoriak)
                .HasForeignKey(vk => vk.VersenyId);

            modelBuilder.Entity<VersenyKategoria>()
                .HasRequired(vk => vk.Kategoria)
                .WithMany(k => k.VersenyKategoriak)
                .HasForeignKey(vk => vk.KategoriaId);

            modelBuilder.Entity<Nevezes>()
                .HasOptional(n => n.Biro)
                .WithMany(b => b.Nevezesek)
                .HasForeignKey(n => n.BiroId);

            modelBuilder.Entity<Nevezes>()
                .HasRequired(n => n.Verseny)
                .WithMany(v => v.Nevezesek)
                .HasForeignKey(n => n.VersenyId);

            modelBuilder.Entity<Nevezes>()
                .HasRequired(n => n.Csapat)
                .WithMany(c => c.Nevezesek)
                .HasForeignKey(n => n.CsapatId);

            modelBuilder.Entity<Nevezes>()
                .HasOptional(n => n.Kategoria) // Kapcsolat a Kategoria entitással
                .WithMany()
                .HasForeignKey(n => n.KategoriaId);

            modelBuilder.Entity<Csapattag>()
                .HasRequired(ct => ct.Csapat)
                .WithMany(c => c.Csapattagok)
                .HasForeignKey(ct => ct.CsapatId);

            modelBuilder.Entity<Eredmeny>()
                .HasRequired(e => e.Nevezes)
                .WithMany(n => n.Eredmenyek)
                .HasForeignKey(e => e.NevezesId);

            modelBuilder.Entity<Eredmeny>()
                .HasRequired(e => e.Biro)
                .WithMany(b => b.Eredmenyek)
                .HasForeignKey(e => e.BiroId);
        }
    }
}