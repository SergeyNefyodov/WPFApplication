namespace WPFApplication.ModelVisualization.Abstractions
{
    public interface IViewDisplayStyleService
    {
        IReadOnlyList<string> GetDisplayStyles();
        string GetDisplayStyle();
        bool CanChangeDisplayStyle();
        void SetDisplayStyle(string displayStyle);
    }
}
