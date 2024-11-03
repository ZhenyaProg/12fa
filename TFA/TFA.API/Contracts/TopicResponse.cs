namespace TFA.API.Contracts;

public class TopicResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
}