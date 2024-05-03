using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Tagger
{
    public class CategoryMapper
    {
        public string CategoryName { get; set; }
        public BuiltInCategory Category { get; set; }
        public BuiltInCategory TagCategory { get; set; }
    }
}
