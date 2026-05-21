using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WPFApplication.DataExchangeExample
{
    [Transaction(TransactionMode.Manual)]
    public class WindowCommand : IExternalCommand
    {
        private readonly WindowService _windowService = new WindowService();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitAPI.Initialize(commandData);

            var dto = _windowService.GetSelectedWindow();
            if (dto == null)
            {
                TaskDialog.Show("Selection Error", "Select only 1 window and try again");
                return Result.Cancelled;
            }

            new WebView2BrowserWindow(dto, _windowService).ShowDialog();

            return Result.Succeeded;
        }
    }
}
