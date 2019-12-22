using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c
{
    public class Base : INotifyPropertyChanged
    {
        private string title = "", folder = "", user = "", password = "", repositoryPath = "", repositoryUser = "", repositoryPass = "", telegram = "";
        private bool isServer = false, isRepository = false;

        public int Id { get; set; }
        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }
        public string Folder
        {
            get => folder;
            set { folder = value; OnPropertyChanged(nameof(Folder)); }
        }
        public string User
        {
            get => user;
            set { user = value; OnPropertyChanged(nameof(User)); }
        }
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }
        public string RepositoryPath
        {
            get => repositoryPath;
            set { repositoryPath = value; OnPropertyChanged(nameof(RepositoryPath)); }
        }
        public string RepositoryUser
        {
            get => repositoryUser;
            set { repositoryUser = value; OnPropertyChanged(nameof(RepositoryUser)); }
        }
        public string RepositoryPass
        {
            get => repositoryPass;
            set { repositoryPass = value; OnPropertyChanged(nameof(RepositoryPass)); }
        }
        public bool IsServer
        {
            get => isServer;
            set { isServer = value; OnPropertyChanged(nameof(IsServer)); }
        }
        public bool IsRepository
        {
            get => isRepository;
            set { isRepository = value; OnPropertyChanged(nameof(IsRepository)); }
        }
        public string Telegram
        {
            get => telegram;
            set { telegram = value; OnPropertyChanged(nameof(Telegram)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
