using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using WPFApplication.ModelVisualization.Abstractions;

namespace WPFApplication.ModelVisualization
{
    public partial class ModelVisualizationViewModel : ObservableObject, IModelVisualizationViewModel, IDisposable
    {
        private readonly IViewPreviewService _previewService;
        private readonly IViewDisplayStyleService _displayStyleService;

        [ObservableProperty] private string _selectedDisplayStyle;
        [ObservableProperty] private FrameworkElement _preview;

        public ModelVisualizationViewModel(IViewPreviewService previewService, IViewDisplayStyleService displayStyleService)
        {
            _previewService = previewService;
            _displayStyleService = displayStyleService;

            DisplayStyles = displayStyleService.GetDisplayStyles();
            IsDisplayStyleEditable = displayStyleService.CanChangeDisplayStyle();
            SelectedDisplayStyle = displayStyleService.GetDisplayStyle();
            Preview = previewService.CreatePreview();
        }

        public IReadOnlyList<string> DisplayStyles { get; }
        public bool IsDisplayStyleEditable { get; }

        partial void OnSelectedDisplayStyleChanged(string value)
        {
            _displayStyleService.SetDisplayStyle(value);
        }

        public void Dispose()
        {
            var preview = Preview;
            Preview = null;
            _previewService.ReleasePreview(preview);
        }
    }
}
