using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechnoSphere_2025
{
    /// <summary>
    /// Логика взаимодействия для HomePageCustomer.xaml
    /// </summary>
        public partial class HomePageCustomer : Page
        {
            public HomePageCustomer()
            {
                InitializeComponent();

                this.Loaded += HomePageCustomer_Loaded;
            }

            private void HomePageCustomer_Loaded(object sender, RoutedEventArgs e)
            {
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.PreviewMouseLeftButtonDown += Window_PreviewMouseLeftButtonDown;
                }
            }

            private void LogoSetting_Click(object sender, RoutedEventArgs e)
            {
                LogoSettingPopup.IsOpen = !LogoSettingPopup.IsOpen;
                e.Handled = true;
            }

            private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (!LogoSettingPopup.IsOpen)
                    return;

                var clicked = e.OriginalSource as DependencyObject;

                if (!IsDescendantOf(clicked, LogoSetting) &&
                    !IsDescendantOf(clicked, LogoSettingPopup.Child))
                {
                    LogoSettingPopup.IsOpen = false;
                }
            }

            private bool IsDescendantOf(DependencyObject source, DependencyObject target)
            {
                while (source != null)
                {
                    if (source == target)
                        return true;
                    source = VisualTreeHelper.GetParent(source);
                }
                return false;
            }

            private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
            {
                LogoSettingPopup.IsOpen = false;
            }

            private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
            {
                LogoSettingPopup.IsOpen = false;
            }
        }
    }
