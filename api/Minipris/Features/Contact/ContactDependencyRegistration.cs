namespace Minipris.Features.Contact;

public static class ContactDependencyRegistration
{
    public static void RegisterContactDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ContactService>();
    }
}
