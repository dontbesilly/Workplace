﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Workplace1c.Views;

namespace Workplace1c.VewModels
{
    class TelegramViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;

        public ObservableCollection<TelegramBot> TelegramBots { get; set; }
        public ObservableCollection<Platform> Platforms { get; set; }
        public Visibility BotParamsCardVisibility { get; set; } = Visibility.Hidden;

        public TelegramViewModel(WorkplaceContext db)
        {
            TelegramBots = db.GetTelegramBotsLocal();
            Platforms = db.GetPlatformsLocal();
            this.db = db;
            telegramSetting = db.TelegramSetting;
        }

        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettingsCommandExecuted);
        public ICommand AddBotCommand => new RelayCommand(AddBotCommandExecuted);
        public ICommand DeleteBotCommand => new RelayCommand(DeleteBotCommandExecuted);
        public ICommand EditBotCommand => new RelayCommand(EditBotCommandExecuted);
        public ICommand AddChatIdCommand => new RelayCommand(AddChatIdCommandExecuted);
        public ICommand DeleteChatIdCommand => new RelayCommand(DeleteChatIdCommandExecuted);

        private TelegramBot selectedTelegramBot;
        public TelegramBot SelectedTelegramBot
        {
            get => selectedTelegramBot;
            set
            {
                selectedTelegramBot = value;
                BotParamsCardVisibility = selectedTelegramBot is null ? Visibility.Hidden : Visibility.Visible;
                OnPropertyChanged(nameof(SelectedTelegramBot));
            }
        }

        private TelegramSetting telegramSetting;
        public TelegramSetting TelegramSetting
        {
            get => telegramSetting;
            set
            {
                telegramSetting = value;
                OnPropertyChanged(nameof(TelegramSetting));
            }
        }

        private ChatId currentChatId;
        public ChatId CurrentChatId
        {
            get => currentChatId;
            set
            {
                currentChatId = value;
                OnPropertyChanged(nameof(CurrentChatId));
            }
        }

        private void AddChatIdCommandExecuted(object obj)
        {
            var chatId = new ChatId
            {
                Chat = 12345
            };
            db.AddEntity(chatId);
            TelegramSetting.ApprovedChatIds.Add(chatId);
        }

        private void DeleteChatIdCommandExecuted(object obj)
        {
            if (CurrentChatId is null) return;
            db.RemoveEntity(CurrentChatId);
        }

        private async void AddBotCommandExecuted(object obj)
        {
            var bot = new TelegramBot();
            var view = new TelegramBotView
            {
                DataContext = bot
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result != null && (bool)result)
            {
                db.AddEntity(bot);
            }
        }

        private void DeleteBotCommandExecuted(object obj)
        {
            if (selectedTelegramBot is null) return;
            db.RemoveEntity(selectedTelegramBot);
        }

        private async void EditBotCommandExecuted(object obj)
        {
            if (selectedTelegramBot is null) return;

            var view = new TelegramBotView
            {
                DataContext = selectedTelegramBot
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result != null && (bool)result)
            {
                db.UpdateEntity(selectedTelegramBot);
            }
        }

        private void SaveSettingsCommandExecuted(object obj)
        {
            if (telegramSetting is null) return;
            db.UpdateEntity(telegramSetting);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
