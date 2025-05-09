namespace TechnoSphere_2025.managers
{
    internal class SessionManager
    {
        public static string CurrentUsername { get; set; } = string.Empty;
        public static byte CurrentUserRole { get; set; }
        public static Guid? RememberToken { get; set; }
    }
}
