using Autodesk.Revit.UI;
using System.Windows;
using WPFApplication.ModelVisualization.Abstractions;

namespace WPFApplication.ModelVisualization
{
    public class ModelVisualizationService : IViewPreviewService
    {
        public FrameworkElement CreatePreview()
        {
            return new PreviewControl(RevitAPI.Document, RevitAPI.Document.ActiveView.Id);
        }

        public void ReleasePreview(FrameworkElement preview)
        {
            (preview as PreviewControl)?.Dispose();
        }
    }
}
