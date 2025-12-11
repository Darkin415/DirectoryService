using DirectoryService.Domain.Constants;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.PositionVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions", "department");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => PositionId.Create(value).Value);
        
        builder.ComplexProperty(p => p.Name, pb =>
        {
            pb.Property(pb => pb.Value)
                .IsRequired()
                .HasMaxLength(Constants.SOMETHING_MAX_LENGTH)
                .HasColumnName("position_name");
        });

        builder.ComplexProperty(p => p.Description, pb =>
        {
            pb.Property(pb => pb.Value)
                .IsRequired()
                .HasColumnName("position_description")
                .HasMaxLength(Constants.SOMETHING_MAX_LENGTH)
                .IsRequired(false);
        });

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
        
    }
}