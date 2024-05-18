using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication
{
    public class Application : IExternalApplication
    {
        static AddInId AddInId = new AddInId(new Guid("F9E2EC33-E932-43F9-B3BA-16617DED5026"));
        private readonly string assemblyPath = Assembly.GetExecutingAssembly().Location;        

        public Result OnStartup(UIControlledApplication application)
        {
            SetupPanel(application);
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void SetupPanel(UIControlledApplication application)
        {
            string tabName = "WPFApplication";
            application.CreateRibbonTab(tabName);
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "WPFApplication");
            AddPushButton(ribbonPanel, "Button 1", assemblyPath, "WPFApplication.DataListsCommand");
            AddPushButton(ribbonPanel, "Button 2", assemblyPath, "WPFApplication.SharedParameterCommand");
            AddPushButton(ribbonPanel, "Button 3", assemblyPath, "WPFApplication.Tagger.TaggerCommand");
            AddPushButton(ribbonPanel, "Web browser", assemblyPath, "WPFApplication.WebBrowserExample.WebBrowserCommand");
        }

        private PushButton AddPushButton(RibbonPanel ribbonPanel, string buttonName, string path, string linkToCommand)
        {
            var buttonData = new PushButtonData(buttonName, buttonName, path, linkToCommand);
            var button = ribbonPanel.AddItem(buttonData) as PushButton;
            return button;
        }
    }
}
