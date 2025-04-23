using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechnoSphere_2025;

/// <summary>
/// Interaction logic for WindowMain.xaml
/// </summary>
public partial class WindowMain : Window
{
    public WindowMain()
    {
        InitializeComponent();
        MainFrame.Navigated += Page_Navigated;
        MainFrame.Navigated += MainFrame_Navigated;
        MainFrame.Navigate(new AuthorizationPage());
    }

    private void Show_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void WindowControl_Moving_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }

    private void Page_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is AuthorizationPage || e.Content is RegistrationPage)
        {
            HideButton.Style = (Style)Application.Current.FindResource("HideColor_1");
            CloseButton.Style = (Style)Application.Current.FindResource("CloseColor_1");
        }
        else
        {
            HideButton.Style = (Style)Application.Current.FindResource("HideColor_2");
            CloseButton.Style = (Style)Application.Current.FindResource("CloseColor_2");
        }
    }

    private void MainFrame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is AuthorizationPage || e.Content is RegistrationPage)
        {
            Grid.SetColumn(WindowControl_Moving, 0);
            Grid.SetColumnSpan(WindowControl_Moving, 3);
        }
        else
        {
            Grid.SetColumn(WindowControl_Moving, 1);
            Grid.SetColumnSpan(WindowControl_Moving, 1);
        }
    }
}