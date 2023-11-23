using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {

        builder.ToTable("pedido");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("codigo")
        .HasColumnType("int")
        .HasMaxLength(11)
        .IsRequired();

        builder.Property(p => p.FechaPedido)
        .HasColumnName("fechaPedido")
        .HasColumnType("date")
        .IsRequired();

        builder.Property(p => p.FechaEsperada)
        .HasColumnName("fechaEsperada")
        .HasColumnType("date")
        .IsRequired();

        builder.Property(p => p.FechaEntrega)
        .HasColumnName("fechaEntrega")
        .HasColumnType("date")
        .IsRequired(false);

        builder.Property(p => p.Estado)
        .HasColumnName("estado")
        .HasColumnType("varchar")
        .HasMaxLength(15)
        .IsRequired();

        builder.Property(p => p.Comentario)
        .HasColumnName("comentario")
        .HasColumnType("text")
        .HasMaxLength(250);

        builder.HasOne(d => d.Cliente)
        .WithMany(d => d.Pedidos)
        .HasForeignKey(d => d.CodigoCliente);
    }
}