﻿namespace AppLevelAuthorization.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    IEnumerable<string> Roles { get; }
}