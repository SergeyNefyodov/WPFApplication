using Autodesk.Revit.DB;
using System.Linq;

namespace WPFApplication.DataExchangeExample
{
    public class WindowService
    {
        public WindowDto GetSelectedWindow()
        {
            var windows = RevitAPI.UiDocument.Selection.GetElementIds()
                .Select(RevitAPI.Document.GetElement)
                .OfType<FamilyInstance>()
                .Where(fi => fi.Category?.Id.IntegerValue == (int)BuiltInCategory.OST_Windows)
                .ToList();

            if (windows.Count != 1)
                return null;

            var window = windows[0];
            var heightParam = window?.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM);

            var heightMm = heightParam != null
                ? UnitUtils.ConvertFromInternalUnits(heightParam.AsDouble(), UnitTypeId.Millimeters)
                : 0;

            return new WindowDto
            {
                Id = window.Id.IntegerValue,
                Name = window.Name,
                Height = heightMm
            };
        }

        public void UpdateWindowHeight(WindowDto dto)
        {
            var element = RevitAPI.Document.GetElement(new ElementId(dto.Id)) as FamilyInstance;
            var heightParam = element?.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM);

            if (heightParam == null || heightParam.IsReadOnly) return;

            using var transaction = new Transaction(RevitAPI.Document, "Update Window Sill Height");
            transaction.Start();
            heightParam.Set(UnitUtils.ConvertToInternalUnits(dto.Height, UnitTypeId.Millimeters));
            transaction.Commit();
        }
    }
}
