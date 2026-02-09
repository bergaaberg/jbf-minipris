using Minipris.Bilforsikring;
using Minipris.Kontaktskjema;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.RegisterCarsDependencies();
builder.RegisterContactDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

var api = app.MapGroup("/api");
api.MapCarsEndpoints();
api.MapContactEndpoints();

app.Run();