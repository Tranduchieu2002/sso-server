using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Errors;

public abstract class ApiError
{
    [BindProperty(Name = "error")] public string Error { get; set; } = string.Empty; // Machine-readable code

    [BindProperty(Name = "errorDescription")]
    public string ErrorDescription { get; set; } = string.Empty; // Human-readable description

    [BindProperty(Name = "errorUri")] public string? ErrorUri { get; set; } // Optional documentation link

    [BindProperty(Name = "state")] public string? State { get; set; } // Optional state for context

    [BindProperty(Name = "details")] public object? Details { get; set; } // Additional error context (optional)
}