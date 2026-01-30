using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Features.Contact.Requests;

namespace Minipris.Features.Contact;

public static class ContactEndpoints
{
    public static void MapContactEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/kontakt-meg", SubmitContact)
            .Accepts<ContactRequest>("application/json")
            .WithName("SubmitContactRequest")
            .WithDescription("Send en kontaktforespørsel for å motta forsikringstilbud");
    }

    private static async Task<Accepted> SubmitContact(
        [FromBody] ContactRequest request,
        [FromServices] ContactService contactService)
    {
        await contactService.SubmitContactRequestAsync(request);
        return TypedResults.Accepted(string.Empty);
    }
}
