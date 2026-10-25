using TransiTrack.ViewModels;

namespace TransiTrack.Views.Report;

public partial class ReportPage : ContentPage
{
    public ReportPage(ReportViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}