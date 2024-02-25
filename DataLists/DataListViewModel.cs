using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFApplication.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WPFApplication.DataLists
{
    public partial class DataListViewModel : ObservableObject
    { 
        public DataListViewModel()     
        {            
            Levels = new FilteredElementCollector(RevitAPI.Document).
                OfClass(typeof(Level)).
                Cast<Level>().
                Select(level=> new LevelDescriptor(level)).
                ToList();
        } 
        
        public List<LevelDescriptor> Levels { get; set; } = new List<LevelDescriptor>();

        [RelayCommand]
        private void RecordParameters()
        {
            using (var transaction = new Transaction(RevitAPI.Document, "Запись значения параметра"))
            {
                transaction.Start();
                foreach (var level in Levels)
                {
                    var parameter = level.Level.get_Parameter(BuiltInParameter.IFC_GUID);
                    if (level.IsChecked == false)
                    {
                        parameter.Set(string.Empty);
                    }
                    else
                    {
                        parameter.Set("Selected");
                    }
                }
                transaction.Commit();
            }
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }        
    }
}
