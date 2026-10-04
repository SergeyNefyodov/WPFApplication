using Autodesk.Revit.DB;

namespace WPFApplication.ModelVisualization
{
    public class DisplayStyleDto(DisplayStyle style, string name)
    {
        public DisplayStyle Style { get; } = style;
        public string Name { get; } = name;
    }
}
