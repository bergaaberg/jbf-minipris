using Minipris.Kontaktskjema.Requests;

namespace Minipris.Kontaktskjema;

public class ContactService(ILogger<ContactService> logger)
{
    public Task SubmitContactRequestAsync(ContactRequest request)
    {
        logger.LogInformation(
            "Contact request received for {RegNumber}. Phone: {Phone}, Email: {Email}",
            request.RegNumber,
            request.PhoneNumber ?? "(not provided)",
            request.Email ?? "(not provided)");

        return Task.CompletedTask;
    }
}