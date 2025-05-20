using System.Windows;
using TechnoSphere_2025.helper;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemeManager.Initialize();
            LocalizationManager.Initialize();
            CursorHelper.Initialize("TechnoSphere_2025");
        }
    }
}