using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Grid = System.Windows.Controls.Grid;
using Panel = System.Windows.Controls.Panel;

namespace WPFApplication
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AddImageToTabCommand : IExternalCommand
    {
        private const string StackPanelName = "mStackPanel";
        private const string Phrase = "HELLO WORLD!";
        private const string MarkerName = "Heart";
        private const string HeartGeometry =
            "M 16,29 C 16,29 2,20 2,11 C 2,6 6,2 10.5,2 C 13.2,2 15.2,3.5 16,5.5 " +
            "C 16.8,3.5 18.8,2 21.5,2 C 26,2 30,6 30,11 C 30,20 16,29 16,29 Z";

        private static readonly SolidColorBrush HeartBrush = new(System.Windows.Media.Colors.DarkBlue);

        private static readonly List<FrameworkElement> Hearts = [];

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var ribbon = ComponentManager.Ribbon;
            if (ribbon == null)
            {
                message = "Лента Revit недоступна.";
                return Result.Failed;
            }

            if (FindFirstHeart(ribbon) != null)
            {
                RemoveHearts(ribbon);
                return Result.Succeeded;
            }

            var tab = ribbon.Tabs.FirstOrDefault();
            if (tab == null)
            {
                message = "В ленте нет ни одной вкладки.";
                return Result.Failed;
            }

            var stackPanel = FindTabStackPanel(ribbon, tab);
            if (stackPanel == null)
            {
                message = $"Элемент \"{StackPanelName}\" не найден во вкладке \"{tab.Title}\".";
                return Result.Failed;
            }

            for (int i = 0; i < Phrase.Length; i++)
            {
                var item = char.IsWhiteSpace(Phrase[i])
                    ? CreateGap()
                    : CreateHeart(Phrase[i]);

                stackPanel.Children.Insert(i, item);
                Hearts.Add(item);
            }

            return Result.Succeeded;
        }

        private static FrameworkElement FindFirstHeart(DependencyObject ribbon)
        {
            var tracked = Hearts.FirstOrDefault(h => VisualTreeHelper.GetParent(h) != null);
            if (tracked != null) return tracked;

            return GetDescendants<FrameworkElement>(ribbon).FirstOrDefault(e => e.Name == MarkerName);
        }

        private static void RemoveHearts(DependencyObject ribbon)
        {
            var all = Hearts
                .Concat(GetDescendants<FrameworkElement>(ribbon).Where(e => e.Name == MarkerName))
                .Distinct()
                .ToList();

            foreach (var heart in all)
            {
                if (VisualTreeHelper.GetParent(heart) is Panel parent)
                    parent.Children.Remove(heart);
            }

            Hearts.Clear();
        }

        private static FrameworkElement CreateGap()
        {
            return new Border { Name = MarkerName, Width = 6 };
        }

        private static Grid CreateHeart(char letter)
        {
            var heart = new Grid
            {
                Name = MarkerName,
                Width = 18,
                Height = 18,
                Margin = new Thickness(4, 0, 4, 0),
                VerticalAlignment = VerticalAlignment.Center,
                ToolTip = Phrase
            };

            heart.Children.Add(new Path
            {
                Data = Geometry.Parse(HeartGeometry),
                Fill = HeartBrush,
                Stretch = Stretch.Uniform
            });

            heart.Children.Add(new TextBlock
            {
                Text = letter.ToString(),
                FontSize = 8,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 2)
            });

            return heart;
        }

        private static StackPanel FindTabStackPanel(DependencyObject ribbon, RibbonTab tab)
        {
            foreach (var element in GetDescendants<FrameworkElement>(ribbon))
            {
                if (!ReferenceEquals(element.DataContext, tab)) continue;

                var stackPanel = GetDescendants<StackPanel>(element)
                    .FirstOrDefault(e => e.Name == StackPanelName);

                if (stackPanel != null) return stackPanel;
            }

            return null;
        }

        private static IEnumerable<T> GetDescendants<T>(DependencyObject root) where T : DependencyObject
        {
            if (root == null) yield break;

            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(current);
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    if (child is T typed) yield return typed;
                    queue.Enqueue(child);
                }
            }
        }
    }
}