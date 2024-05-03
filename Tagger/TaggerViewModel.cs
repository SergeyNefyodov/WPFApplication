using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Tagger
{
    public partial class TaggerViewModel : ObservableObject
    {        
        [ObservableProperty] private CategoryMapper _activeMapper;
        [ObservableProperty] private List<Element> _tagsList;
        [ObservableProperty] private Element _activeTag;
        [ObservableProperty] private double _verticalOffset;
        [ObservableProperty] private double _horizontalOffset;
        [ObservableProperty] private bool _addLeader;

        public List<CategoryMapper> Mappers { get; set; } = new List<CategoryMapper>()
        {
            new CategoryMapper()
            {
                CategoryName = "Окна",
                Category = BuiltInCategory.OST_Windows,
                TagCategory = BuiltInCategory.OST_WindowTags
            },
            new CategoryMapper()
            {
                CategoryName = "Двери",
                Category = BuiltInCategory.OST_Doors,
                TagCategory = BuiltInCategory.OST_DoorTags
            }
        };

        partial void OnActiveMapperChanged(CategoryMapper value)
        {
            ActiveTag = null;
            TagsList = new FilteredElementCollector(RevitAPI.Document).
                OfCategory(value.TagCategory).
                WhereElementIsElementType().
                ToList();
        }

        [RelayCommand]
        private void MarkAll()
        {
            if (ActiveMapper is null || ActiveTag is null) return;
            var view = RevitAPI.Document.ActiveView;
            var elements = new FilteredElementCollector(RevitAPI.Document, view.Id).
                OfCategory(ActiveMapper.Category).
                WhereElementIsNotElementType();
            using var transaction = new Transaction(RevitAPI.Document, "Tag all");
            transaction.Start();
            foreach (var element in elements)
            {
                var reference = new Reference(element);
                var locationPoint = (LocationPoint)element.Location;
                if (locationPoint is null) continue;
                var point = locationPoint.Point;
                var tagPoint = new XYZ(point.X+HorizontalOffset * view.Scale/ 304.8,
                    point.Y+VerticalOffset * view.Scale / 304.8,
                    point.Z);
                var tag = IndependentTag.Create(RevitAPI.Document,
                    view.Id,
                    reference, AddLeader,
                    TagMode.TM_ADDBY_CATEGORY,
                    TagOrientation.Horizontal,
                    tagPoint
                    );

                tag.LeaderEndCondition = LeaderEndCondition.Free;
                tag.TagHeadPosition = tagPoint;
            }
            transaction.Commit();
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
