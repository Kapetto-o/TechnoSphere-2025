using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using TechnoSphere_2025.managers;

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
        Loaded += WindowMain_Loaded;
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
        if (e.Content is PageAuthorization || e.Content is PageRegistration)
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

    private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        if (parent == null) return null;
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T correctlyTyped)
                return correctlyTyped;
            var descendent = FindVisualChild<T>(child);
            if (descendent != null)
                return descendent;
        }
        return null;
    }

    private void MainFrame_Navigated(object sender, NavigationEventArgs e)
    {
        Dispatcher.InvokeAsync(() =>
        {
            if (MainFrame.Content is not Page page)
            {
                WindowControl_Moving.Margin = new Thickness(0, 0, 70, 0);
                return;
            }

            var header = FindVisualChild<controls.header.HeaderControlBase>(page);
            if (header == null)
            {
                WindowControl_Moving.Margin = new Thickness(155, 0, 70, 0);
                return;
            }

            var logoBtn = header.LogoButton;
            var userBtn = header.UserButton;

            UpdateDragArea(logoBtn, userBtn);

            logoBtn.SizeChanged -= HeaderButton_SizeChanged;
            logoBtn.SizeChanged += HeaderButton_SizeChanged;
            userBtn.SizeChanged -= HeaderButton_SizeChanged;
            userBtn.SizeChanged += HeaderButton_SizeChanged;

        }, DispatcherPriority.Loaded);
    }

    private void HeaderButton_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (MainFrame.Content is not Page page)
            return;

        var header = FindVisualChild<controls.header.HeaderControlBase>(page);
        if (header == null)
            return;

        UpdateDragArea(header.LogoButton, header.UserButton);
    }


    private void WindowMain_Loaded(object sender, RoutedEventArgs e)
    {
        var tok = Properties.Settings.Default.RememberToken;
        if (Guid.TryParse(tok, out var token))
        {
            var connString = ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"].ConnectionString;

            using var conn = new SqlConnection(connString);
            conn.Open();

            using var cmd = new SqlCommand(@"
            SELECT UserID, Username, Role 
              FROM Users 
             WHERE RememberToken = @t 
               AND IsActive = 1", conn);
            cmd.Parameters.AddWithValue("@t", token);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                SessionManager.CurrentUserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                SessionManager.CurrentUsername = reader.GetString(reader.GetOrdinal("Username"));
                SessionManager.CurrentUserRole = reader.GetByte(reader.GetOrdinal("Role"));

                MainFrame.Navigate(
                    SessionManager.CurrentUserRole == 1
                        ? (object)new PageHome_Admin()
                        : new PageHome_User()
                );

                ClearBackStack();
                return;
            }
        }

        MainFrame.Navigate(new PageAuthorization());
        ClearBackStack();
    }

    private void ClearBackStack()
    {
        var nav = MainFrame.NavigationService;
        while (nav.CanGoBack)
            nav.RemoveBackEntry();
    }
}