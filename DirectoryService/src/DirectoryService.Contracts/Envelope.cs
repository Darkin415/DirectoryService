using DirectoryService.Contracts.Errors;

namespace DirectoryService.Contracts;

public class Envelope
{
    public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);
    private Envelope(object? result, ErrorList errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }
    public object? Result { get; }
    
    public ErrorList? Errors { get; }
    
    public string? ErrorMessage { get; }
    
    public DateTime TimeGenerated { get; }

    public static Envelope Ok(object? result = null) => new(result, null);
    
    public static Envelope Error(ErrorList errors) => new(null, errors);
    
}