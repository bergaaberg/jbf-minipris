namespace Minipris.Features.Cars;

public static class CarsDependencyRegistration
{
    public static void RegisterCarsDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<CarInfoService>();
        builder.Services.AddTransient<CarService>();
    }
}
