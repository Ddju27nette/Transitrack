using System.ComponentModel.DataAnnotations;

namespace TransiTrack.Api.Models;

/// <summary>Payload for opening a vehicle trip in the local demonstration backend.</summary>
public sealed record StartTripRequest
{
    [Required, StringLength(80, MinimumLength = 2)]
    public required string AgencyName { get; init; }

    [Required, StringLength(80, MinimumLength = 1)]
    public required string RouteName { get; init; }

    [Required, RegularExpression(@"^[A-Za-z0-9-]{3,20}$")]
    public required string VehiclePlate { get; init; }

    [Required, StringLength(40, MinimumLength = 2)]
    public required string CityId { get; init; }

    [Required, StringLength(100, MinimumLength = 2)]
    public required string DestinationName { get; init; }

    [Range(-90, 90)]
    public required double DestinationLatitude { get; init; }

    [Range(-180, 180)]
    public required double DestinationLongitude { get; init; }
}

/// <summary>Payload for ending an active vehicle trip.</summary>
public sealed record EndTripRequest
{
    [Required, StringLength(200, MinimumLength = 32)]
    public required string DriverSessionToken { get; init; }
}

/// <summary>Payload containing a driver's latest GPS observation.</summary>
public sealed record DriverPositionRequest
{
    [Range(-90, 90)]
    public required double Latitude { get; init; }

    [Range(-180, 180)]
    public required double Longitude { get; init; }

    [Range(0, 80)]
    public required double SpeedMetersPerSecond { get; init; }

    [Range(0, 360)]
    public required double HeadingDegrees { get; init; }

    [Range(0, 1000)]
    public required double AccuracyMeters { get; init; }

    [Range(1, long.MaxValue)]
    public required long Sequence { get; init; }

    public required DateTimeOffset RecordedAtUtc { get; init; }
}

/// <summary>Snapshot of a bus currently visible to passengers.</summary>
public sealed record VehicleResponse(
    string VehicleId,
    string VehiclePlate,
    string AgencyName,
    string RouteName,
    string CityId,
    string DestinationName,
    double Latitude,
    double Longitude,
    double SpeedMetersPerSecond,
    double HeadingDegrees,
    int EtaMinutes,
    long Sequence,
    DateTimeOffset RecordedAtUtc);

/// <summary>Credentials and identifier returned when a trip is started.</summary>
public sealed record TripStartedResponse(
    string TripId,
    string VehicleId,
    string DriverSessionToken,
    DateTimeOffset StartedAtUtc);

/// <summary>Public, non-secret view of a vehicle trip.</summary>
public sealed record TripResponse(
    string TripId,
    string VehicleId,
    string VehiclePlate,
    string AgencyName,
    string RouteName,
    string CityId,
    string DestinationName,
    bool IsActive,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? EndedAtUtc);

/// <summary>Payload for submitting a passenger incident report.</summary>
public sealed record CreateIncidentReportRequest
{
    [Required, StringLength(80, MinimumLength = 2)]
    public required string IncidentType { get; init; }

    [Required, StringLength(120, MinimumLength = 2)]
    public required string LocationDescription { get; init; }

    [Range(-90, 90)]
    public required double Latitude { get; init; }

    [Range(-180, 180)]
    public required double Longitude { get; init; }

    [StringLength(1000)]
    public string Details { get; init; } = string.Empty;
}

/// <summary>Incident report received from a passenger.</summary>
public sealed record IncidentReportResponse(
    string Id,
    string IncidentType,
    string LocationDescription,
    double Latitude,
    double Longitude,
    string Details,
    string Status,
    DateTimeOffset ReportedAtUtc);
