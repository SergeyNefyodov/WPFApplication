using WPFApplication.ModelVisualization.Abstractions;

namespace WPFApplication.ModelVisualization
{
    public partial class ModelVisualizationView
    {
        public ModelVisualizationView(IModelVisualizationViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
