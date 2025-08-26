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
            .HasColumnName("location_id")
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

        builder.ComplexProperty(l => l.Address, lp =>
        {
            lp.Property(b => b.Country)
                .IsRequired()
                .HasColumnName("location_country");
                
            lp.Property(b => b.Building)
                .IsRequired()
                .HasColumnName("location_building");
            lp.Property(b => b.City)
                .IsRequired()
                .HasColumnName("location_city");
            lp.Property(b => b.RoomNumber)
                .IsRequired()
                .HasColumnName("location_room_number");
            lp.Property(b => b.Street)
                .IsRequired()
                .HasColumnName("location_street");
        });
        
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