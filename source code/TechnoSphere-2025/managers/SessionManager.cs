namespace TechnoSphere_2025.managers
{
    internal class SessionManager
    {
        public static string CurrentUsername { get; set; } = string.Empty;

        public static Guid? RememberToken { get; set; }
    }
}
