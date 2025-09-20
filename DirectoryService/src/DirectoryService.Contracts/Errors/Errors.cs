namespace DirectoryService.Contracts.Errors;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }
        
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("record.already.exist", $"{label} already exist");
        }
        
        public static Error ItsChild(string? name = null)
        {
            return Error.Validation("element.is.child", $" Element is child");
        }

        public static Error NameNotFound(string name)
        {
            return Error.Validation("record.name.notfound", "Record not found");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found {forId}");
        }

        public static Error LocationNotFound(string? locationName = null)
        {
            return Error.NotFound("locations.not.found", "Locations not found");
        }
    }
}