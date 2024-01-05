using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipesApi.Models;

public partial class DbRecipiesContext : DbContext
{
    public DbRecipiesContext()
    {
    }

    public DbRecipiesContext(DbContextOptions<DbRecipiesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacione> Calificaciones { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<Preparacion> Preparacions { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacione>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("PK__Califica__40E4A75106488F7F");

            entity.Property(e => e.Idreceta).HasColumnName("IDReceta");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdrecetaNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.Idreceta)
                .HasConstraintName("FK__Calificac__IDRec__46E78A0C");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.Idusuario)
                .HasConstraintName("FK__Calificac__IDUsu__45F365D3");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.IdFavorito).HasName("PK__Favorito__39DCEE504EC573C1");

            entity.Property(e => e.IdFavorito).ValueGeneratedNever();
            entity.Property(e => e.Idreceta).HasColumnName("IDReceta");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdrecetaNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.Idreceta)
                .HasConstraintName("FK__Favoritos__IDRec__4316F928");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.Idusuario)
                .HasConstraintName("FK__Favoritos__IDUsu__4222D4EF");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.Idingrediente).HasName("PK__Ingredie__2E73012F203DB406");

            entity.Property(e => e.Idingrediente).HasColumnName("IDIngrediente");
            entity.Property(e => e.Idreceta).HasColumnName("IDReceta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdrecetaNavigation).WithMany(p => p.Ingredientes)
                .HasForeignKey(d => d.Idreceta)
                .HasConstraintName("FK__Ingredien__IDRec__3C69FB99");
        });

        modelBuilder.Entity<Preparacion>(entity =>
        {
            entity.HasKey(e => e.Idpaso).HasName("PK__Preparac__8A5C9C5F40709CA4");

            entity.ToTable("Preparacion");

            entity.Property(e => e.Idpaso).HasColumnName("IDPaso");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Idreceta).HasColumnName("IDReceta");

            entity.HasOne(d => d.IdrecetaNavigation).WithMany(p => p.Preparacions)
                .HasForeignKey(d => d.Idreceta)
                .HasConstraintName("FK__Preparaci__IDRec__3F466844");
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.Idreceta).HasName("PK__Recetas__91B4C6BC8D7AC740");

            entity.Property(e => e.Idreceta).HasColumnName("IDReceta");
            entity.Property(e => e.Descripción)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.Idusuario)
                .HasConstraintName("FK__Recetas__IDUsuar__398D8EEE");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__Usuarios__52311169DA64322C");

            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
