﻿namespace TFA.Application.UseCases.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title); 