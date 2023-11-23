using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class OficinaConfiguration : IEntityTypeConfiguration<Oficina>
{
    public void Configure(EntityTypeBuilder<Oficina> builder)
    {

        builder.ToTable("oficina");
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
        .HasColumnName("codigo")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.Property(o => o.Ciudad)
        .HasColumnName("ciudad")
        .HasColumnType("varchar")
        .HasMaxLength(30)
        .IsRequired();

        builder.Property(o => o.Pais)
        .HasColumnName("pais")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(o => o.Region)
        .HasColumnName("region")
        .HasColumnType("varchar")
        .HasMaxLength(50);

        builder.Property(o => o.CodigoPostal)
        .HasColumnName("codigoPostal")
        .HasColumnType("varchar")
        .HasMaxLength(10)
        .IsRequired();

        builder.Property(o => o.Telefono)
        .HasColumnName("telefono")
        .HasColumnType("varchar")
        .HasMaxLength(20)
        .IsRequired();

        builder.Property(p => p.LineaDireccion1)
        .HasColumnName("lineaDireccion1")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.LineaDireccion2)
        .HasColumnName("lineaDireccion2")
        .HasColumnType("varchar")
        .HasMaxLength(50);
    }
}