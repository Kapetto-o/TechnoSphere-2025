using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TechnoSphere_2025.controls;
using TechnoSphere_2025.models;

namespace TechnoSphere_2025.modules.customer
{
    public partial class Window_Comparison : Window
    {
        private ComparisonTabViewModel? _currentTabVm;

        public Window_Comparison()
        {
            InitializeComponent();

            if (DataContext is ComparisonViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                if (vm.ActiveTab != null)
                {
                    SetupTab(vm.ActiveTab);
                    BuildDataGridColumnsAndRows(vm.ActiveTab);
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not ComparisonViewModel vm) return;
            if (e.PropertyName == nameof(ComparisonViewModel.ActiveTab))
            {

                var newTab = vm.ActiveTab;
                if (newTab != null)
                {
                    SetupTab(newTab);
                    BuildDataGridColumnsAndRows(newTab);
                }
                else
                {
                    CompareDataGrid.ItemsSource = null;
                    CompareDataGrid.Columns.Clear();
                    _currentTabVm = null;
                }
            }
        }

        private void CurrentTab_NotifyRowsChanged(object? sender, EventArgs e)
        {
            if (sender is ComparisonTabViewModel tab)
            {
                BuildDataGridColumnsAndRows(tab);
            }
        }

        private void SetupTab(ComparisonTabViewModel tab)
        {
            if (_currentTabVm != null)
                _currentTabVm.NotifyRowsChanged -= CurrentTab_NotifyRowsChanged;

            _currentTabVm = tab;

            _currentTabVm.NotifyRowsChanged += CurrentTab_NotifyRowsChanged;
        }

        private void BuildDataGridColumnsAndRows(ComparisonTabViewModel tab)
        {
            CompareDataGrid.Columns.Clear();

            var colSpecName = new DataGridTextColumn
            {
                Binding = new Binding(nameof(ComparisonRow.DisplayName)),
                IsReadOnly = true,
                Width = new DataGridLength(200)
            };
            CompareDataGrid.Columns.Add(colSpecName);

            foreach (var prodVm in tab.Products)
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Width = new DataGridLength(250)
                };

                var headerFactory = new FrameworkElementFactory(typeof(ProductCard_Panel));
                headerFactory.SetValue(FrameworkElement.WidthProperty, 250.0);
                headerFactory.SetBinding(
                    FrameworkElement.DataContextProperty,
                    new Binding() { Source = prodVm }
                );
                templateColumn.HeaderTemplate = new DataTemplate { VisualTree = headerFactory };

                var cellFactory = new FrameworkElementFactory(typeof(TextBlock));
                var cellBinding = new Binding($"[{prodVm.ProductID}]");
                cellFactory.SetBinding(TextBlock.TextProperty, cellBinding);
                cellFactory.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
                cellFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(4));
                templateColumn.CellTemplate = new DataTemplate { VisualTree = cellFactory };

                CompareDataGrid.Columns.Add(templateColumn);
            }

            CompareDataGrid.ItemsSource = tab.Rows;
        }

        private void CompareDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OuterScroll.ScrollToVerticalOffset(OuterScroll.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
