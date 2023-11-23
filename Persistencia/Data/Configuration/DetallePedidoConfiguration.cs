using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
{
    public void Configure(EntityTypeBuilder<DetallePedido> builder)
    {

        builder.ToTable("detallePedido");

        builder.HasKey(d => new {d.CodigoPedido, d.CodigoProducto});

        builder.HasOne(d => d.Pedido)
        .WithMany(d => d.DetallePedidos)
        .HasForeignKey(d => d.CodigoPedido);

        builder.HasOne(d => d.Producto)
        .WithMany(d => d.DetallePedidos)
        .HasForeignKey(d => d.CodigoProducto);

        builder.Property(d => d.Cantidad)
        .HasColumnName("cantidad")
        .HasColumnType("int")
        .HasMaxLength(11)
        .IsRequired();

        builder.Property(d => d.PrecioUnidad)
        .HasColumnName("precioUnidad")
        .HasColumnType("decimal(15,2)")
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(d => d.NumeroLinea)
        .HasColumnName("numeroLinea")
        .HasColumnType("smallint")
        .HasMaxLength(6)
        .IsRequired();
    }
}