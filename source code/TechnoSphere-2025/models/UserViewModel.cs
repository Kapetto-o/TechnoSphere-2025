using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TechnoSphere_2025.managers;

namespace TechnoSphere_2025.models
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private bool _isActive;
        public int UserID { get; }
        public string Username { get; }
        public string Email { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public ICommand ToggleBlockCommand { get; }
        public string? Phone { get; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                try
                {
                    UsersRepository.SetActive(UserID, _isActive);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Не удалось изменить статус:\n{ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public UserViewModel(UserData d)
        {
            UserID = d.UserID;
            Username = d.Username;
            Email = d.Email;
            FirstName = d.FirstName;
            LastName = d.LastName;
            Phone = d.Phone;
            _isActive = d.IsActive;

            ToggleBlockCommand = new RelayCommand(_ => IsActive = !IsActive);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string n)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
