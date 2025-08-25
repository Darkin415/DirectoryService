using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class Address : ValueObject
{
    public Address()
    {
        
    }
    public Address(string country, string city, string street, string building, int roomNumber)
    {
        Country = country;
        City = city;
        Street = street;
        Building = building;
        RoomNumber = roomNumber;
    }

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string Building { get; }
    public int RoomNumber { get; }

    public static Result<Address, Error> Create(string country, string city, string street, string building, int roomNumber)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Errors.General.ValueIsInvalid("Country");

        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsInvalid("City");

        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid("Street");

        if (string.IsNullOrWhiteSpace(building))
            return Errors.General.ValueIsInvalid("Building");

        if (roomNumber <= 0)
            return Errors.General.ValueIsInvalid("RoomNumber");

        return new Address(country, city, street, building, roomNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return Building;
        yield return RoomNumber;
    }
}