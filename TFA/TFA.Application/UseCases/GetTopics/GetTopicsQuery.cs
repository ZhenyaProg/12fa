﻿namespace TFA.Application.UseCases.GetTopics;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take);