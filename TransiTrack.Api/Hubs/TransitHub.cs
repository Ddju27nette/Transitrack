using Microsoft.AspNetCore.SignalR;
using TransiTrack.Api.Models;
using TransiTrack.Api.Services;

namespace TransiTrack.Api.Hubs;

public class TransitHub : Hub
{
    private readonly FleetStateService _fleetService;

    public TransitHub(FleetStateService fleetService)
    {
        _fleetService = fleetService;
    }

    // Le chauffeur diffuse sa nouvelle position GPS
    public async Task UpdatePosition(VehicleTelemetry telemetry)
    {
        _fleetService.UpdateVehicle(telemetry);
        await Clients.Others.SendAsync("VehiclePositionReceived", telemetry);
    }

    // Le chauffeur termine sa course
    public async Task StopTrip(string registration)
    {
        _fleetService.RemoveVehicle(registration);
        await Clients.Others.SendAsync("VehicleRemoved", registration);
    }

    // Un usager (sans compte) envoie un signalement d'incident
    public async Task SendIncident(TrafficIncident incident)
    {
        _fleetService.AddIncident(incident);
        await Clients.All.SendAsync("IncidentAlertReceived", incident);
    }
}