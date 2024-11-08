﻿namespace TFA.Application.Models;

public class Topic
{
    public Guid Id { get; set; }
    public Guid ForumId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
}