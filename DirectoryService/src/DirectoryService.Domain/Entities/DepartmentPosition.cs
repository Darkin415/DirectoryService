using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.PositionVO;

namespace DirectoryService.Domain.Entities;

public class DepartmentPosition
{
    public DepartmentPosition() { }
    
    private readonly List<DepartmentPosition> _departmentPositions = [];

    private DepartmentPosition(PositionId positionId, DepartmentId departmentId)
    {
        PositionId = positionId;
        DepartmentId = departmentId;
    }

    public DepartmentId DepartmentId { get; private set; }
    public PositionId PositionId { get; private set; }

    public Department Department { get; private set; }
    public Position Position { get; private set; }
    
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    public static Result<DepartmentPosition, Error> Create(PositionId positionId, DepartmentId departmentId)
    {
        if (positionId.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("PositionId");
        if (departmentId.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("DepartmentId");

        return new DepartmentPosition(positionId, departmentId);
    }
}