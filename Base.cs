using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c
{
    public class Base : INotifyPropertyChanged
    {
        private string title = "", folder = "", user = "", password = "", repositoryPath = "", repositoryUser = "", repositoryPass = "", telegram = "";
        private bool isServer = false, isRepository = false;

        public int Id { get; set; }
        public string Title { get => title;
            set { title = value; OnPropertyChanged("Title"); } }
        public string Folder { get { return folder; } set { folder = value; OnPropertyChanged("Folder"); } }
        public string User { get { return user; } set { user = value; OnPropertyChanged("User"); } }
        public string Password { get { return password; } set { password = value; OnPropertyChanged("Password"); } }
        public string RepositoryPath { get { return repositoryPath; } set { repositoryPath = value; OnPropertyChanged("RepositoryPath"); } }
        public string RepositoryUser { get { return repositoryUser; } set { repositoryUser = value; OnPropertyChanged("RepositoryUser"); } }
        public string RepositoryPass { get { return repositoryPass; } set { repositoryPass = value; OnPropertyChanged("RepositoryPass"); } }
        public bool IsServer { get { return isServer; } set { isServer = value; OnPropertyChanged("IsServer"); } }
        public bool IsRepository { get { return isRepository; } set { isRepository = value; OnPropertyChanged("IsRepository"); } }
        public string Telegram { get { return telegram; } set { telegram = value; OnPropertyChanged("Telegram"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
