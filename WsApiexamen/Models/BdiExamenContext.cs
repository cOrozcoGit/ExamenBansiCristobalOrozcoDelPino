using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WsApiexamen.Models;

public partial class BdiExamenContext : DbContext
{
    public BdiExamenContext()
    {
    }

    public BdiExamenContext(DbContextOptions<BdiExamenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Examen> TblExamen { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS;database=BdiExamen;trusted_connection=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Examen>(entity =>
        {
            entity.HasKey(e => e.IdExamen);

            entity.ToTable("tblExamen");

            entity.Property(e => e.IdExamen).HasColumnName("idExamen");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
