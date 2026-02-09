namespace Minipris.Bilforsikring;

public static class CarsDependencyRegistration
{
    public static void RegisterCarsDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<CarInfoRepository>();
        builder.Services.AddTransient<CarService>();
    }
}