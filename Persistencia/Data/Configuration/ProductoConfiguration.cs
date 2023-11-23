using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {

        builder.ToTable("producto");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("codigo")
        .HasColumnType("varchar")
        .HasMaxLength(15)
        .IsRequired();

        builder.Property(p => p.Nombre)
        .HasColumnName("nombre")
        .HasColumnType("varchar")
        .HasMaxLength(70)
        .IsRequired();

        builder.HasOne(d => d.GamaProducto)
        .WithMany(d => d.Productos)
        .HasForeignKey(d => d.Gama);

        builder.Property(p => p.Dimensiones)
        .HasColumnName("dimensiones")
        .HasColumnType("varchar")
        .HasMaxLength(25);

        builder.Property(p => p.Proveedor)
        .HasColumnName("proveedor")
        .HasColumnType("varchar")
        .HasMaxLength(50);

        builder.Property(p => p.Descripcion)
        .HasColumnName("descripcion")
        .HasColumnType("text")
        .HasMaxLength(350);

        builder.Property(p => p.CantidadStock)
        .HasColumnName("cantidadStock")
        .HasColumnType("smallint")
        .HasMaxLength(6)
        .IsRequired();

        builder.Property(p => p.PrecioVenta)
        .HasColumnName("precioVenta")
        .HasColumnType("decimal(15,2)")
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(p => p.PrecioProveedor)
        .HasColumnName("precioProveedor")
        .HasColumnType("decimal(15,2)")
        .HasMaxLength(100);
    }
}