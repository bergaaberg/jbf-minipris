using Minipris.Bilforsikring.Services;

namespace Minipris.Bilforsikring;

public static class BilforsikringDependencyRegistration
{
    public static void RegisterBilforsikringDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<BilInfoRepository>();
        builder.Services.AddTransient<BilService>();
    }
}
