using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_final.Models;

public partial class ProyectoFinalDbContext : DbContext
{
    public ProyectoFinalDbContext()
    {
    }

    public ProyectoFinalDbContext(DbContextOptions<ProyectoFinalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cuenta> Cuentas { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Ingreso> Ingresos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JCL1K0P; Database=GestorGastosPersonales;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E515F34B32");

            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.CuentaId).HasName("PK__Cuentas__40072E812E319C24");

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.NombreCuenta)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SaldoInicial)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UsuarioId");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.GastoId).HasName("PK__Gastos__815BB0F08591589A");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_CategoriaId");

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.CuentaId)
                .HasConstraintName("FK_CuentaId");
        });

        modelBuilder.Entity<Ingreso>(entity =>
        {
            entity.HasKey(e => e.IngresoId).HasName("PK__Ingresos__DBF0909A0B0A49BA");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Ingresos)
                .HasForeignKey(d => d.CuentaId)
                .HasConstraintName("FK_Ingreso_CuentaId");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Usuarios__1788CC4C429A347A");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105340FD59392").IsUnique();

            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
