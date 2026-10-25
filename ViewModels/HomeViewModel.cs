using TransiTrack.Models;

namespace TransiTrack.ViewModels;

public class HomeMapViewModel : BaseViewModel
{
    private VehiclePosition? _selectedVehicle;
    private bool _hasSelection;
    private int _estimatedMinutes;

    public VehiclePosition? SelectedVehicle
    {
        get => _selectedVehicle;
        set
        {
            _selectedVehicle = value;
            HasSelection = value != null;
            if (value != null)
            {
                // Estimation approximative d'arrivée selon la vitesse
                EstimatedMinutes = value.SpeedKmh > 5 ? (int)Math.Max(3, 40 / value.SpeedKmh * 10) : 15;
            }
            OnPropertyChanged();
        }
    }

    public bool HasSelection
    {
        get => _hasSelection;
        set { _hasSelection = value; OnPropertyChanged(); }
    }

    public int EstimatedMinutes
    {
        get => _estimatedMinutes;
        set { _estimatedMinutes = value; OnPropertyChanged(); }
    }
}