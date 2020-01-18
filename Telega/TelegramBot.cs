using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c
{
    public class TelegramBot : INotifyPropertyChanged
    {
        string token = "";
        int chatId = 1;

        public int Id { get; set; }

        public string Token
        {
            get => token;
            set { token = value; OnPropertyChanged(nameof(Token)); }
        }

        public int ChatId
        {
            get => chatId;
            set { chatId = value; OnPropertyChanged(nameof(ChatId)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}