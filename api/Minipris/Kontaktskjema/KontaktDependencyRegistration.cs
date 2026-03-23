using Minipris.Kontaktskjema.Services;

namespace Minipris.Kontaktskjema;

public static class KontaktDependencyRegistration
{
    public static void RegisterKontaktDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<KontaktService>();
    }
}
