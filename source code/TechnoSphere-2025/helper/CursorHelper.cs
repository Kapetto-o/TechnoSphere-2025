using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TechnoSphere_2025.helper
{
    public static class CursorHelper
    {
        private static Cursor _arrowCursor = null!;
        private static Cursor _handCursor = null!;
        private static Cursor _textCursor = null!;

        public static void Initialize(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentException("Имя сборки не может быть пустым", nameof(assemblyName));

            _arrowCursor = LoadCursor($"/{assemblyName};component/assets/icons/cursors/arrow.cur");
            _handCursor = LoadCursor($"/{assemblyName};component/assets/icons/cursors/hand.cur");
            _textCursor = LoadCursor($"/{assemblyName};component/assets/icons/cursors/text.cur");

            EventManager.RegisterClassHandler(
                typeof(Control),
                UIElement.PreviewMouseMoveEvent,
                new MouseEventHandler(OnControlPreviewMouseMove),
                handledEventsToo: true);

            EventManager.RegisterClassHandler(
                typeof(Hyperlink),
                UIElement.PreviewMouseMoveEvent,
                new MouseEventHandler(OnHyperlinkPreviewMouseMove),
                handledEventsToo: true);
        }

        private static Cursor LoadCursor(string packUri)
        {
            var uri = new Uri(packUri, UriKind.Relative);
            var info = Application.GetResourceStream(uri);
            if (info == null)
                throw new FileNotFoundException($"Ресурс не найден: {packUri}");
            return new Cursor(info.Stream);
        }

        private static void OnControlPreviewMouseMove(object? sender, MouseEventArgs e)
        {
            var original = e.OriginalSource as DependencyObject;

            if (FindParent<TextBox>(original) is not null || FindParent<PasswordBox>(original) is not null)
                Mouse.OverrideCursor = _textCursor;
            else if (FindParent<Button>(original) is not null)
                Mouse.OverrideCursor = _handCursor;
            else
                Mouse.OverrideCursor = _arrowCursor;
        }

        private static void OnHyperlinkPreviewMouseMove(object? sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = _handCursor;
        }

        private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T found)
                    return found;

                child = (child is Visual || child is Visual3D)
                    ? VisualTreeHelper.GetParent(child)
                    : LogicalTreeHelper.GetParent(child);
            }
            return null;
        }
    }
}