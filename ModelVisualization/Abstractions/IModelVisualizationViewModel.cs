using System.ComponentModel;
using System.Windows;

namespace WPFApplication.ModelVisualization.Abstractions
{
    public interface IModelVisualizationViewModel : INotifyPropertyChanged
    {
        IReadOnlyList<string> DisplayStyles { get; }
        string SelectedDisplayStyle { get; set; }
        bool IsDisplayStyleEditable { get; }
        FrameworkElement Preview { get; }
    }
}
