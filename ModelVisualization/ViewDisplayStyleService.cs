using Autodesk.Revit.DB;
using WPFApplication.ModelVisualization.Abstractions;

namespace WPFApplication.ModelVisualization
{
    public class ViewDisplayStyleService : IViewDisplayStyleService
    {
        private static readonly ElementId ModelDisplayParameterId = new(BuiltInParameter.GRAPHIC_DISPLAY_OPTIONS_MODEL);
        private static readonly ElementId GraphicsStyleParameterId = new(BuiltInParameter.MODEL_GRAPHICS_STYLE);

        private readonly IReadOnlyList<DisplayStyleDto> _displayStyles = new List<DisplayStyleDto>
        {
            new(DisplayStyle.Wireframe, "Каркас"),
            new(DisplayStyle.HLR, "Скрытие линий"),
            new(DisplayStyle.Shading, "Тонирование"),
            new(DisplayStyle.ShadingWithEdges, "Тонирование с кромками"),
            new(DisplayStyle.FlatColors, "Согласованные цвета"),
            new(DisplayStyle.Realistic, "Реалистичный"),
            new(DisplayStyle.RealisticWithEdges, "Реалистичный с кромками")
        };

        public bool SupportsDisplayStyle()
        {
            var view = RevitAPI.Document.ActiveView;
            return view != null
                   && !view.IsTemplate
                   && view.get_Parameter(BuiltInParameter.MODEL_GRAPHICS_STYLE) != null;
        }

        public IReadOnlyList<string> GetDisplayStyles()
        {
            return _displayStyles.Select(descriptor => descriptor.Name).ToList();
        }

        public string GetDisplayStyle()
        {
            var view = RevitAPI.Document.ActiveView;
            return _displayStyles.FirstOrDefault(descriptor => descriptor.Style == view.DisplayStyle)?.Name;
        }

        public bool CanChangeDisplayStyle()
        {
            var view = RevitAPI.Document.ActiveView;
            var parameter = view.get_Parameter(BuiltInParameter.MODEL_GRAPHICS_STYLE);
            if (parameter == null || parameter.IsReadOnly) return false;

            if (view.ViewTemplateId == ElementId.InvalidElementId) return true;
            if (RevitAPI.Document.GetElement(view.ViewTemplateId) is not View template) return true;

            var controlledIds = template.GetTemplateParameterIds();
            var nonControlledIds = template.GetNonControlledTemplateParameterIds();

            return !new[] { ModelDisplayParameterId, GraphicsStyleParameterId }
                .Any(id => controlledIds.Contains(id) && !nonControlledIds.Contains(id));
        }

        public void SetDisplayStyle(string displayStyle)
        {
            if (displayStyle == null || !CanChangeDisplayStyle()) return;

            var view = RevitAPI.Document.ActiveView;
            var descriptor = _displayStyles.FirstOrDefault(style => style.Name == displayStyle);
            if (descriptor == null || view.DisplayStyle == descriptor.Style) return;

            using var transaction = new Transaction(RevitAPI.Document, "Изменение визуального стиля");
            transaction.Start();
            view.DisplayStyle = descriptor.Style;
            transaction.Commit();

            RevitAPI.UiDocument.RefreshActiveView();
        }
    }
}
