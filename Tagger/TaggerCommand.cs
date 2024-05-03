using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.SharedParameterManager;
using WPFApplication.Tagger;

namespace WPFApplication.Tagger
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class TaggerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (RevitAPI.UiApplication == null)
            {
                RevitAPI.Initialize(commandData);
            }
            var viewModel = new TaggerViewModel();
            var view = new TaggerView(viewModel);
            viewModel.CloseRequest += (s, e) => view.Close();
            view.ShowDialog();
            return Result.Succeeded;
        }
    }
}
