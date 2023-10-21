using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFApplication
{
    public class ViewModel
    {
        public ViewModel(Reference reference)
        {
            CollectParameters(reference);
        }               

        public string Prefix { get; set; }
        public string StartValue { get; set; }

        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public Parameter SelectedParameter { get; set; }

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
        public void Numerate()
        {
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
                            Reference reference = RevitAPI.UiDocument.Selection.PickObject(ObjectType.Element, $"Выберите элемент {i}");
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
    }
}
