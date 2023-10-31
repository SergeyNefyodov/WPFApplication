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

namespace WPFApplication
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ISelectionFilter _filter;
        private string _prefix = string.Empty;
        private string _startValue = string.Empty;
        private Parameter _selectedParameter;
        public ViewModel(Reference reference)
        {
            NumerateCommand = new RelayCommand(Numerate, CanNumerate);
            _filter = new SelectionFilter(RevitAPI.Document.GetElement(reference));
            CollectParameters(reference);
        }               

        public RelayCommand NumerateCommand { get; set; }

        public string Prefix 
        {
            get => _prefix;

            set
            {
                _prefix = value;
                OnPropertyChanged();
            }
        }
        public string StartValue
        {
            get => _startValue;

            set
            {
                _startValue = value;
                OnPropertyChanged();
            }
        }

        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public Parameter SelectedParameter
        {
            get => _selectedParameter;

            set
            {
                _selectedParameter = value;
                OnPropertyChanged();
            }
        }
        private void CollectParameters(Reference reference)
        {
            var element = RevitAPI.Document.GetElement(reference);
            var parameterSet = element.Parameters;
            foreach (var paramObj in parameterSet)
            {
                var parameter = (Parameter)paramObj;
                if (!parameter.IsReadOnly && parameter.StorageType != StorageType.ElementId)
                {
                    Parameters.Add(parameter);
                }
            }
        }
        public void Numerate(object param)
        {
            RaiseCloseRequest();
            int i = 1;
            int.TryParse(StartValue, out i);
            string parameterName = SelectedParameter.Definition.Name;
            using (TransactionGroup group = new TransactionGroup(RevitAPI.Document, "Нумерация элементов"))
            {
                group.Start();

                while (true)
                {
                    try
                    {
                        using (Transaction t = new Transaction(RevitAPI.Document, "Нумерация элементов"))
                        {
                            t.Start();
                            //Reference reference = RevitAPI.UiDocument.Selection.PickObject(ObjectType.Element, _filter, $"Выберите элемент {i}");
                            Reference reference = RevitAPI.UiDocument.Selection.PickObject(ObjectType.Edge, _filter, $"Выберите элемент {i}");
                            Parameter parameter = RevitAPI.Document.GetElement(reference).LookupParameter(parameterName);
                            if (parameter != null)
                            {
                                parameter.Set(Prefix + i.ToString());
                                i++;
                                t.Commit();
                            }
                            else
                            {
                                TaskDialog.Show("Ошибка", $"У элемента {reference.ElementId} нет параметра {parameterName})");
                                t.Commit();
                                group.Assimilate();
                                break;
                            }
                        }
                    }
                    catch 
                    {                        
                        group.Assimilate();
                        break;
                    }
                }
            }
        }

        private bool CanNumerate(object param)
        {
            return int.TryParse(StartValue, out _) && SelectedParameter != null;
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
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
