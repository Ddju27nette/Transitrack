# TransiTrack API

ASP.NET Core .NET 10 API and SignalR hub used by the MAUI application.

## Run locally

Set development-only keys in the current PowerShell session, then start the API:

```powershell
$env:Transit__DriverApiKey = "choose-a-local-driver-key"
$env:Transit__ModeratorApiKey = "choose-a-local-moderator-key"
dotnet run --project TransiTrack.Api --launch-profile http
```

The API listens at `http://localhost:5080`; OpenAPI is at
`/openapi/v1.json`. Requests for trips require `X-Driver-Key`. SignalR driver
updates require the one-time session token returned by `POST /api/v1/trips`.
Report validation requires `X-Moderator-Key`.

The Development environment starts one simulated Douala vehicle and publishes
updates every two seconds, so the passenger map can be exercised without a
physical bus. The in-memory data is discarded when the API process stops.

## Map keys

Windows uses the Google Maps JavaScript API inside a MAUI WebView. Set
`GOOGLE_MAPS_API_KEY` to a Google Maps Platform key with Maps JavaScript API
enabled and billing configured before launching the app. In PowerShell, set
the key for the current session before launching the MAUI app:

```powershell
$env:GOOGLE_MAPS_API_KEY = "your-windows-maps-javascript-key"
```

For Visual Studio or VS Code launches, set it as a Windows user environment
variable and restart the IDE so the app inherits it. On Android, the native
Google Maps SDK uses `GOOGLE_MAPS_ANDROID_API_KEY` when building; if unset, it
falls back to `GOOGLE_MAPS_API_KEY`. Prefer separate keys restricted to the
Maps JavaScript API and Android Maps SDK respectively. Neither key is stored in
source control.

Debug Android builds permit local HTTP traffic to `10.0.2.2:5080`; Release
builds disable cleartext traffic. Use HTTPS and a configured server URL outside
the local development environment. Google Maps displays its required branding
and attribution; the previous Microsoft Azure attribution is no longer used.

## Current prototype limits

- Fleet, trip, and report data are in memory; use persistent storage before
  deployment.
- Driver credentials use a shared development API key, not agency user
  authentication or role-based authorization.
- The driver page streams GPS while it is active in the foreground. A
  platform-specific Android foreground service and iOS background-location
  configuration are still required for reliable background tracking.
- ETA currently uses straight-line distance divided by observed speed, with a
  conservative minimum speed. A validated incident halves the estimated speed
  for vehicles within one kilometer for 30 minutes. Replace this with routed
  distance and a traffic/incident model before production.
- The Development traffic simulator is not registered in other environments.
