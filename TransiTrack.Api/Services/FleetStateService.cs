using System.Collections.Concurrent;
using TransiTrack.Api.Models;

namespace TransiTrack.Api.Services;

public class FleetStateService
{
    private readonly ConcurrentDictionary<string, VehicleTelemetry> _vehicles = new();
    private readonly List<TrafficIncident> _incidents = new();

    public void UpdateVehicle(VehicleTelemetry telemetry)
    {
        _vehicles[telemetry.VehicleRegistration] = telemetry;
    }

    public void RemoveVehicle(string registration)
    {
        _vehicles.TryRemove(registration, out _);
    }

    public IEnumerable<VehicleTelemetry> GetAllVehicles() => _vehicles.Values;

    public void AddIncident(TrafficIncident incident)
    {
        lock (_incidents)
        {
            _incidents.Add(incident);
        }
    }

    public IEnumerable<TrafficIncident> GetIncidents()
    {
        lock (_incidents)
        {
            return _incidents.ToList();
        }
    }

    // Calcule la distance en kilomètres (Formule de Haversine)
    public double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double r = 6371;
        var dLat = (lat2 - lat1) * Math.PI / 180.0;
        var dLon = (lon2 - lon1) * Math.PI / 180.0;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return r * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    // Calcule l'ETA en minutes en tenant compte des ralentissements
    public int EstimateEtaMinutes(double currentLat, double currentLon, double targetLat, double targetLon, double speedKmh)
    {
        double distanceKm = CalculateDistanceKm(currentLat, currentLon, targetLat, targetLon);
        double effectiveSpeed = speedKmh > 5 ? speedKmh : 20.0;

        // Si un incident existe dans un rayon de 1 km, division par 2 de la vitesse
        bool hasJam = false;
        lock (_incidents)
        {
            hasJam = _incidents.Any(i => CalculateDistanceKm(currentLat, currentLon, i.Latitude, i.Longitude) <= 1.5);
        }

        if (hasJam)
        {
            effectiveSpeed = Math.Max(5.0, effectiveSpeed * 0.4); // Vitesse réduite
        }

        return (int)Math.Ceiling((distanceKm / effectiveSpeed) * 60);
    }
}