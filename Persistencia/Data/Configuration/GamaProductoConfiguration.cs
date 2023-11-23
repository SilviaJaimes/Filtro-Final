using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class GamaProductoConfiguration : IEntityTypeConfiguration<GamaProducto>
{
    public void Configure(EntityTypeBuilder<GamaProducto> builder)
    {

        builder.ToTable("gamaProducto");
        builder.HasKey(g => g.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("gama")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(p => p.DescripcionTexto)
        .HasColumnName("descripcionTexto")
        .HasColumnType("text")
        .HasMaxLength(350);

        builder.Property(p => p.DescripcionHtml)
        .HasColumnName("descripcionHtml")
        .HasColumnType("text")
        .HasMaxLength(350);

        builder.Property(p => p.Imagen)
        .HasColumnName("imagen")
        .HasColumnType("varchar")
        .HasMaxLength(256);
    }
}