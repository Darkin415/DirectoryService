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
        
        builder.HasMany(d => d.Locations)
            .WithMany(l => l.Departments)
            .UsingEntity(
                "departments_locations",
                r => r
                    .HasOne(typeof(Location))
                    .WithMany()
                    .HasForeignKey("location_id"),
                l => l
                    .HasOne(typeof(Department))
                    .WithMany()
                    .HasForeignKey("department_id"),
                
                j => j.HasKey("location_id", "department_id"));
        
        builder.HasMany(d => d.Positions)
            .WithMany(p => p.Departments)
            .UsingEntity(
                "departments_position",
                r => r
                    .HasOne(typeof(Position))
                    .WithMany()
                    .HasForeignKey("position_id"),
                l => l.HasOne(typeof(Department))
                    .WithMany()
                    .HasForeignKey("department_id"),
                
                j => j.HasKey("position_id", "department_id"));
        
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

        builder.Property(d => d.Identifier)
            .HasColumnName("department_identifier");
        
        builder.Property(x => x.ParentId)
            .IsRequired(false)
            .HasConversion(
                value => value == null ? (Guid?)null : value.Value,
                value => value == null ? null : DepartmentId.Create((Guid)value).Value);

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