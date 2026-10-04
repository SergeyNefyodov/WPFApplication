using System.Windows;

namespace WPFApplication.ModelVisualization.Abstractions
{
    public interface IViewPreviewService
    {
        FrameworkElement CreatePreview();
        void ReleasePreview(FrameworkElement preview);
    }
}
