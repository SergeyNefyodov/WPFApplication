using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.SharedParameterManager
{
    public partial class SharedParameterManagerViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<SharedParameterDescriptor> _parameters = [];

        public SharedParameterManagerViewModel()
        {
            var parameters = new FilteredElementCollector(RevitAPI.Document).
                OfClass(typeof(SharedParameterElement)).
                Cast<SharedParameterElement>().
                ToArray();

            foreach (var parameter in parameters)
            {
                Parameters.Add(new SharedParameterDescriptor(parameter));
            }
        }

        [RelayCommand]
        private void DeleteParameter(SharedParameterDescriptor parameter)
        {
            using var transaction = new Transaction(RevitAPI.Document, $"Delete shared parameter {parameter.Name}");
            transaction.Start();
            RevitAPI.Document.Delete(parameter.Id);
            transaction.Commit();
            Parameters.Remove(parameter);
        }
    }
}
