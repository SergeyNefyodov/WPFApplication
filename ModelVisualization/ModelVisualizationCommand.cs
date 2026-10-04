using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WPFApplication.ModelVisualization
{
    [Transaction(TransactionMode.Manual)]
    public class ModelVisualizationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitAPI.Initialize(commandData);

            var displayStyleService = new ViewDisplayStyleService();
            if (!displayStyleService.SupportsDisplayStyle())
            {
                TaskDialog.Show("Визуальный стиль", "Активный вид не поддерживает визуальные стили");
                return Result.Cancelled;
            }

            using var viewModel = new ModelVisualizationViewModel(new ModelVisualizationService(), displayStyleService);
            new ModelVisualizationView(viewModel).ShowDialog();

            return Result.Succeeded;
        }
    }
}
