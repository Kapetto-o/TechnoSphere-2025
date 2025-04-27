using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

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
        Dispatcher.InvokeAsync(() =>
        {
            double leftOffset = 0;
            double rightOffset = 70;

            if (e.Content is not AuthorizationPage and not RegistrationPage)
            {
                leftOffset = 155;
                if (MainFrame.Content is Page page)
                {
                    var userBtn = page.FindName("UserAccount") as FrameworkElement;
                    if (userBtn != null)
                    {
                        userBtn.UpdateLayout();
                        rightOffset = userBtn.ActualWidth + 92;
                    }
                    else
                    {
                        rightOffset = 70;
                    }
                }
            }
            WindowControl_Moving.Margin = new Thickness(leftOffset, 0, rightOffset, 0);
        }, DispatcherPriority.Loaded);
    }
}