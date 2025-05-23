using System.Collections.ObjectModel;

namespace TechnoSphere_2025.models
{
    public class UsersListViewModel
    {
        public ObservableCollection<UserViewModel> Users { get; }
            = new ObservableCollection<UserViewModel>();

        public UsersListViewModel()
        {
            var list = UsersRepository.GetAllUsers();
            foreach (var d in list)
                Users.Add(new UserViewModel(d));
        }
    }
}