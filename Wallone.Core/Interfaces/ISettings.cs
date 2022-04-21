﻿namespace Wallone.Core.Interfaces
{
    public interface ISettings
    {
        string Email { get; set; }
        string Token { get; set; }
        string Language { get; set; }
        string Theme { get; set; }
        string Host { get; set; }
    }
}