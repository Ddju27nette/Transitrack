using TransiTrack.Api.Hubs;
using TransiTrack.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<FleetStateService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapHub<TransitHub>("/hubs/transit");
app.MapGet("/api/vehicles", (FleetStateService service) => service.GetAllVehicles());
app.MapGet("/api/incidents", (FleetStateService service) => service.GetIncidents());

app.Run();