using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Reflection.PortableExecutable;
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

    private void MainFrame_Navigated(object sender, NavigationEventArgs e)
    {
        Dispatcher.InvokeAsync(() =>
        {
            if (e.Content is PageAuthorization ||
                e.Content is PageRegistration ||
                MainFrame.Content is not Page page)
            {
                WindowControl_Moving.Margin = new Thickness(0, 0, 70, 0);
                return;
            }

            var headerObj = page.FindName("PageHeader");

            FrameworkElement? logoBtn = null, userBtn = null;

            if (headerObj is helper.HeaderControl_User userHeader)
            {
                logoBtn = userHeader.LogoButton;
                userBtn = userHeader.UserButton;
            }
            else if (headerObj is helper.HeaderControl_Admin adminHeader)
            {
                logoBtn = adminHeader.LogoButton;
                userBtn = adminHeader.UserButton;
            }

            if (logoBtn == null || userBtn == null)
            {
                WindowControl_Moving.Margin = new Thickness(155, 0, 70, 0);
                return;
            }

            UpdateDragArea(logoBtn, userBtn);

            logoBtn.SizeChanged -= HeaderButton_SizeChanged;
            userBtn.SizeChanged -= HeaderButton_SizeChanged;
            logoBtn.SizeChanged += HeaderButton_SizeChanged;
            userBtn.SizeChanged += HeaderButton_SizeChanged;

        }, DispatcherPriority.Loaded);
    }

    private void HeaderButton_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (MainFrame.Content is not Page page)
            return;

        var headerObj = page.FindName("PageHeader");
        FrameworkElement? logoBtn = null, userBtn = null;

        if (headerObj is helper.HeaderControl_User userHeader)
        {
            logoBtn = userHeader.LogoButton;
            userBtn = userHeader.UserButton;
        }
        else if (headerObj is helper.HeaderControl_Admin adminHeader)
        {
            logoBtn = adminHeader.LogoButton;
            userBtn = adminHeader.UserButton;
        }
        else
        {
            return;
        }

        UpdateDragArea(logoBtn, userBtn);
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
                    SELECT Username, Role FROM Users 
                     WHERE RememberToken = @t AND IsActive = 1", conn);
            cmd.Parameters.AddWithValue("@t", token);
            var roleObj = cmd.ExecuteScalar();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                SessionManager.CurrentUsername = reader.GetString(reader.GetOrdinal("Username"));
                byte role = (byte)reader["Role"];

                MainFrame.Navigate(role == 1
                    ? (object)new PageHome_Admin()
                    : new PageHome_User());

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