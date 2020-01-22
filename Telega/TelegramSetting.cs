using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c
{
    public class TelegramSetting : INotifyPropertyChanged
    {
        public ObservableCollection<ChatId> ApprovedChatIds { get; set; }

        private int adminChatId;
        private string serverAdminUserName = "", serverAdminPass = "", serverPath = "";
        private TelegramBot commandBot, chatBot;
        private Platform platform;

        public int Id { get; set; }

        public Platform Platform
        {
            get => platform;
            set { platform = value; OnPropertyChanged(nameof(Platform)); }
        }

        public TelegramBot CommandBot
        {
            get => commandBot;
            set { commandBot = value; OnPropertyChanged(nameof(CommandBot)); }
        }

        public TelegramBot ChatBot
        {
            get => chatBot;
            set { chatBot = value; OnPropertyChanged(nameof(ChatBot)); }
        }

        public int AdminChatId
        {
            get => adminChatId;
            set { adminChatId = value; OnPropertyChanged(nameof(AdminChatId)); }
        }

        public string ServerPath
        {
            get => serverPath;
            set { serverPath = value; OnPropertyChanged(nameof(ServerPath)); }
        }


        public string ServerAdminUserName
        {
            get => serverAdminUserName;
            set { serverAdminUserName = value; OnPropertyChanged(nameof(ServerAdminUserName)); }
        }

        public string ServerAdminPass
        {
            get => serverAdminPass;
            set { serverAdminPass = value; OnPropertyChanged(nameof(ServerAdminPass)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}