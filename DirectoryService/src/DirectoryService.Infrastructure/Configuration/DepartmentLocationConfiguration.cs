using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentLocationConfiguration: IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_location", "department");

        builder.HasKey(d => new  { d.LocationId, d.DepartmentId });

        builder.Property(d => d.LocationId)
            .HasColumnName("location_id")
            .HasConversion(
                value => value.Value,
                value => LocationId.Create(value).Value);

        builder.Property(d => d.DepartmentId)
            .HasColumnName("department_id")
            .HasConversion(
                value => value.Value,
                value => DepartmentId.Create(value).Value);

        builder.HasOne(d => d.Location)
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey(d => d.LocationId);

        builder.HasOne(d => d.Department)
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey(d => d.DepartmentId);
    }
}