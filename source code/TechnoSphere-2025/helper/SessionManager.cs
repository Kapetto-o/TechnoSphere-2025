using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoSphere_2025.helper
{
    internal class SessionManager
    {
        public static string CurrentUsername { get; set; } = string.Empty;

        public static Guid? RememberToken { get; set; }
    }
}
