using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Domain.Entities;

public class DepartmentLocation
{
    protected DepartmentLocation() { }

    public DepartmentLocation(LocationId locationId, DepartmentId  departmentId)
    {
        LocationId = locationId;
        DepartmentId = departmentId;
    }

    public DepartmentId  DepartmentId { get; private set; }
    public LocationId LocationId { get; private set; }

    public Department Department { get; private set; }
    public Location Location { get; private set; }

    public static Result<DepartmentLocation, Error> Create(LocationId locationId, DepartmentId departmentId)
    {
        if (locationId.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("LocationId");
        if (departmentId.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("DepartmentId");

        return new DepartmentLocation(locationId, departmentId);
    }
}