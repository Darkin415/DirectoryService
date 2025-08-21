using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class Address : ValueObject
{
    private Address(string country, string city, string street, string building, int roomNumber)
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
            return Error.Create("Country не может быть пустым");

        if (string.IsNullOrWhiteSpace(city))
            return Error.Create("City не может быть пустым");

        if (string.IsNullOrWhiteSpace(street))
            return Error.Create("Street не может быть пустым");

        if (string.IsNullOrWhiteSpace(building))
            return Error.Create("Building не может быть пустым");

        if (roomNumber <= 0)
            return Error.Create("RoomNumber должен быть больше 0");

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