namespace Minipris.Features.Contact.Requests;

public record ContactRequest
{
    public required string RegNumber { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
}
