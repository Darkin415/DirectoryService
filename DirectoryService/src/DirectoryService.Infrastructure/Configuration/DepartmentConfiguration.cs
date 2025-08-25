using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments", "department");

        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => DepartmentId.Create(value).Value);
        
        builder.HasMany(d => d.DepartmentLocations)
            .WithOne(dl => dl.Department)
            .HasForeignKey(dl => dl.DepartmentId);

        builder.Property(d => d.Depth)
            .IsRequired()
            .HasColumnName("depth");
        
        builder.Property(d => d.ParentId)
            .HasColumnName("parentId")
            .HasConversion(
                value => value.Value,
                value => DepartmentId.Create(value).Value);

        builder.HasMany(d => d.DepartmentPositions)
            .WithOne(dp => dp.Department)
            .HasForeignKey(dp => dp.DepartmentId);
        
        builder.HasMany(d => d.ChildrenDepartments)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ComplexProperty(d => d.Name, nb =>
        {
            nb.Property(nb => nb.Value)
                .HasColumnName("department_name");
        });

        builder.ComplexProperty(d => d.Identifier, db =>
        {
            db.Property(d => d.Value)
                .HasColumnName("department_identifier");
        });
        
        // builder.Property(x => x.ParentId)
        //     .IsRequired(false)
        //     .HasConversion(
        //         value => value == null ? (Guid?)null : value.Value,
        //         value => value == null ? null : DepartmentId.Create((Guid)value).Value);

        builder.ComplexProperty(d => d.Path, db =>
        {
            db.Property(db => db.Value)
                .IsRequired()
                .HasColumnName("department_path");
        });
        
        builder.Property(d => d.ChildrenCount)
            .IsRequired()
            .HasColumnName("children_count");
        
        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasColumnName("is_active");
        
        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(d => d.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }
}