namespace TransiTrack.Services;

public static class TransitApiConfiguration
{
    public static Uri BaseUri
    {
        get
        {
#if ANDROID
            return new Uri("http://10.0.2.2:5080/");
#else
            return new Uri("http://localhost:5080/");
#endif
        }
    }

    public static Uri FleetHubUri => new(BaseUri, "hubs/fleet");
}
