using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {

        builder.ToTable("empleado");
        builder.HasKey(e => e.Id);

        builder.Property(o => o.Id)
        .HasColumnName("id")
        .HasColumnType("int")
        .HasMaxLength(11)
        .IsRequired();

        builder.Property(e => e.Nombre)
        .HasColumnName("nombre")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.Apellido1)
        .HasColumnName("apellido1")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.Apellido2)
        .HasColumnName("apellido2")
        .HasColumnType("varchar")
        .HasMaxLength(50);

        builder.Property(e => e.Extension)
        .HasColumnName("extension")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.Property(e => e.Email)
        .HasColumnName("email")
        .HasColumnType("varchar")
        .HasMaxLength(100)
        .IsRequired();

        builder.HasOne(e => e.Oficina)
        .WithMany(e => e.Empleados)
        .HasForeignKey(e => e.CodigoOficina)
        .IsRequired();

        builder.HasOne(e => e.Jefe)
        .WithMany(e => e.Empleados)
        .HasForeignKey(e => e.CodigoJefe)
        .IsRequired(false);

        builder.Property(e => e.Puesto)
        .HasColumnName("puesto")
        .HasColumnType("varchar")
        .HasMaxLength(50);
    }
}