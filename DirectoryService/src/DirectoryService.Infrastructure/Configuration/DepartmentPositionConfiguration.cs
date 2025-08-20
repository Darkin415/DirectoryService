using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.PositionVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentPositionConfiguration: IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_position", "department");

        builder.HasKey(d => new  { d.PositionId, d.DepartmentId });

        builder.Property(d => d.PositionId)
            .HasColumnName("position_id")
            .HasConversion(
                value => value.Value,
                value => PositionId.Create(value).Value);;

        builder.Property(d => d.DepartmentId)
            .HasColumnName("department_id")
            .HasConversion(
                value => value.Value,
                value => DepartmentId.Create(value).Value);;

        builder.HasOne(d => d.Position)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(d => d.PositionId);

        builder.HasOne(d => d.Department)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(d => d.DepartmentId);
    }
}