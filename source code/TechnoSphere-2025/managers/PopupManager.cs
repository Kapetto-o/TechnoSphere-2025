using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace TechnoSphere_2025.managers
{
    public class PopupManager : IDisposable
    {
        private readonly Window _window;
        private readonly List<(Popup popup, UIElement owner)> _items = new List<(Popup, UIElement)>();

        public PopupManager(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _window.PreviewMouseLeftButtonDown += Window_PreviewMouseLeftButtonDown;
            _window.Deactivated += Window_Deactivated;
        }

        public void Register(Popup popup, UIElement owner)
        {
            if (popup == null) throw new ArgumentNullException(nameof(popup));
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            _items.Add((popup, owner));
        }

        public void Toggle(Popup popup, double? width = null)
        {
            if (popup == null) throw new ArgumentNullException(nameof(popup));

            if (width.HasValue)
                popup.Width = width.Value;

            popup.IsOpen = !popup.IsOpen;
        }

        private void Window_PreviewMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as DependencyObject;

            foreach (var (popup, owner) in _items)
            {
                if (!popup.IsOpen)
                    continue;

                if (!IsDescendantOf(clicked, owner) &&
                    !IsDescendantOf(clicked, popup.Child))
                {
                    popup.IsOpen = false;
                }
            }
        }

        private void Window_Deactivated(object? sender, EventArgs e)
        {
            foreach (var (popup, _) in _items)
            {
                popup.IsOpen = false;
            }
        }

        private bool IsDescendantOf(DependencyObject? source, DependencyObject target)
        {
            if (source == null)
                return false;

            while (source != null)
            {
                if (source == target)
                    return true;
                source = VisualTreeHelper.GetParent(source);
            }
            return false;
        }

        public void Unregister()
        {
            _window.PreviewMouseLeftButtonDown -= Window_PreviewMouseLeftButtonDown;
            _window.Deactivated -= Window_Deactivated;
            _items.Clear();
        }

        public void Dispose() => Unregister();
    }
}