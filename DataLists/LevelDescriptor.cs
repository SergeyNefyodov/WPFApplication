using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace WPFApplication.DataLists
{
    public partial class LevelDescriptor : ObservableObject
    {
        [ObservableProperty] private bool _isChecked;
        public LevelDescriptor(Level level)
        {
            Level = level;
        }        
        public Level Level { get; set; }
    }
}
