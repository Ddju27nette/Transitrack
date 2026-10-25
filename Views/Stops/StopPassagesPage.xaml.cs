using TransiTrack.ViewModels;

namespace TransiTrack.Views.Stops;

public partial class StopPassagesPage : ContentPage
{
    public StopPassagesPage(StopPassagesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}