using System.Text.Json;
using DirectoryService.Domain.Constants;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class LocationConfiguration  : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations", "department");
        
        builder.HasKey(l => l.Id);
        
        builder.Property(d => d.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => LocationId.Create(value).Value);
        
        builder.ComplexProperty(l => l.Name, lp =>
        {
            lp.Property(lp => lp.Value)
                .HasColumnName("location_name")
                .IsRequired()
                .HasMaxLength(Constants.SOMETHING_MAX_LENGTH);
        });

        builder.ComplexProperty(l => l.TimeZone, lp =>
        {
            lp.Property(lp => lp.Value)
                .HasColumnName("location_timezone");
        });
        
        builder.Property(x => x.Addresses)
            .HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                valueDb => JsonSerializer.Deserialize<IReadOnlyList<Address>>(valueDb, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<Address>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnName("addresses")
            .HasColumnType("jsonb");
        
        builder.Property(l => l.IsActive)
            .IsRequired()
            .HasColumnName("is_active");
        
        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(l => l.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }
}