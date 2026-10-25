using TransiTrack.Models;

namespace TransiTrack.Views.Stops;

[QueryProperty(nameof(Stop), "Stop")]
public partial class StopMapPage : ContentPage
{
    private BusStop _stop = new();
    public BusStop Stop
    {
        get => _stop;
        set
        {
            _stop = value;
            OnPropertyChanged();
        }
    }

    public Command ViewPassagesCommand { get; }

    public StopMapPage()
    {
        InitializeComponent();
        ViewPassagesCommand = new Command(async () =>
        {
            var navParams = new Dictionary<string, object> { { "Stop", Stop } };
            await Shell.Current.GoToAsync(nameof(StopPassagesPage), navParams);
        });
        BindingContext = this;
    }
}