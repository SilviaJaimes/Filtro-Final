using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {

        builder.ToTable("pago");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
        .HasColumnName("idTransaccion")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

        builder.HasOne(d => d.Cliente)
        .WithMany(d => d.Pagos)
        .HasForeignKey(d => d.CodigoCliente);

        builder.Property(p => p.FormaPago)
        .HasColumnName("formaPago")
        .HasColumnType("varchar")
        .HasMaxLength(40)
        .IsRequired();

        builder.Property(p => p.FechaPago)
        .HasColumnName("fechaPago")
        .HasColumnType("date")
        .IsRequired();

        builder.Property(p => p.Total)
        .HasColumnName("total")
        .HasColumnType("decimal(15,2)")
        .HasMaxLength(100)
        .IsRequired();
    }
}