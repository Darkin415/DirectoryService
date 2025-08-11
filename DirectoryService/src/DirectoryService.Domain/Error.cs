namespace DirectoryService.Domain;

public class Error
{
    private Error(string message)
    {
        Message = message;
    }
    
    public string Message {get; private set;}
    
    public static Error Create(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Сообщение об ошибке не может быть пустым");
        }

        return new Error(message);
    }
}