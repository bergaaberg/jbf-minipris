using Minipris.Kontaktskjema.Requests;

namespace Minipris.Kontaktskjema.Services;

public class KontaktService(ILogger<KontaktService> logger)
{
    public Task SendKontaktforespørselAsync(ContactRequest request)
    {
        logger.LogInformation(
            "Kontaktforespørsel mottatt for {Regnummer}. Telefon: {Telefonnummer}, E-post: {Epost}",
            request.Regnummer,
            request.Telefonnummer ?? "(ikke oppgitt)",
            request.Epost ?? "(ikke oppgitt)");

        return Task.CompletedTask;
    }
}
