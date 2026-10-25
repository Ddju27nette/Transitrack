using TransiTrack.ViewModels;

namespace TransiTrack.Views.Driver;

public partial class DriverPage : ContentPage
{
    public DriverPage(DriverViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}