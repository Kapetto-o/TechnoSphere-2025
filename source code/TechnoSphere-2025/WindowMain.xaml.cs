using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using TechnoSphere_2025.helper;

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
        MainFrame.Navigate(new PageCommon_Authorization());
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
        if (e.Content is PageCommon_Authorization || e.Content is PageCommon_Registration)
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

    private void UpdateDragArea(FrameworkElement logoBtn, FrameworkElement userBtn)
    {
        logoBtn.UpdateLayout();
        userBtn.UpdateLayout();

        double leftOffset = logoBtn.ActualWidth + 15;
        double rightOffset = userBtn.ActualWidth + 92;

        WindowControl_Moving.Margin =
            new Thickness(leftOffset, 0, rightOffset, 0);
    }

    private void MainFrame_Navigated(object sender, NavigationEventArgs e)
    {
        Dispatcher.InvokeAsync(() =>
        {
            if (e.Content is PageCommon_Authorization ||
                e.Content is PageCommon_Registration ||
                MainFrame.Content is not Page page)
            {
                WindowControl_Moving.Margin = new Thickness(0, 0, 70, 0);
                return;
            }

            var header = page.FindName("PageHeader") as helper.HeaderControl;
            if (header == null)
            {
                WindowControl_Moving.Margin = new Thickness(155, 0, 70, 0);
                return;
            }

            var logoBtn = header.LogoButton;
            var userBtn = header.UserButton;

            UpdateDragArea(logoBtn, userBtn);

            logoBtn.SizeChanged -= HeaderButton_SizeChanged;
            userBtn.SizeChanged -= HeaderButton_SizeChanged;

            logoBtn.SizeChanged += HeaderButton_SizeChanged;
            userBtn.SizeChanged += HeaderButton_SizeChanged;

        }, DispatcherPriority.Loaded);
    }

    private void HeaderButton_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (MainFrame.Content is not Page page) return;
        var header = page.FindName("PageHeader") as helper.HeaderControl;
        if (header == null) return;

        UpdateDragArea(header.LogoButton, header.UserButton);
    }
}