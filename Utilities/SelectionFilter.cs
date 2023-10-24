using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Utilities
{
    internal class SelectionFilter : ISelectionFilter
    {
        private ElementId categoryId;
        private XYZ point;
        public SelectionFilter(Element element)
        {
            categoryId = element.Category.Id;
            if (element.Location is LocationPoint point0) point = point0.Point;
        }
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Id == categoryId) return true;
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position) // for more information follow me on telegram @https://t.me/revitAPIDevelopment
        {
            if (position.DistanceTo(point) < 1000 / 304.8) return true;
            return false;
        }
    }
}
