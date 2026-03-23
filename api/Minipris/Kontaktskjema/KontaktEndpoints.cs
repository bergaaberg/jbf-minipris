using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Kontaktskjema.Requests;
using Minipris.Kontaktskjema.Services;

namespace Minipris.Kontaktskjema;

public static class KontaktEndpoints
{
    public static void MapKontaktEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/kontakt-meg", SendKontaktforespørsel)
            .Accepts<ContactRequest>("application/json")
            .WithName("SendKontaktforespørsel")
            .WithDescription("Send en kontaktforespørsel for å motta forsikringstilbud");
    }

    private static async Task<Accepted> SendKontaktforespørsel(
        [FromBody] ContactRequest request,
        [FromServices] KontaktService kontaktService)
    {
        await kontaktService.SendKontaktforespørselAsync(request);
        return TypedResults.Accepted(string.Empty);
    }
}
