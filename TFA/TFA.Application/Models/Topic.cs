namespace TFA.Application.Models;

public class Topic
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public string Author { get; set; } = string.Empty;
}