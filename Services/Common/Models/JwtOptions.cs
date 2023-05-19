﻿namespace Shared.Models;

public class JwtOptions
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public int ExpiryMinutes { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public string ValidAudience { get; set; }
}