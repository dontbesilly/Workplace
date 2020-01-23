using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MihaZupan;
using Telegram.Bot;

namespace Workplace1c
{
    class Telega
    {
        private readonly IEnumerable<Base> bases;
        private readonly TelegramSetting setting;
        private readonly Server server;
        private readonly TelegramBotClient bot;

        public bool IsReceiving => bot.IsReceiving;

        public Telega(IEnumerable<Base> bases, TelegramSetting setting)
        {
            this.setting = setting;
            this.bases = bases;
            this.server = new Server(setting.ServerPath, setting.ServerAdminUserName, setting.ServerAdminPass);

            HttpToSocks5Proxy proxy = Constants.proxy;
            proxy.ResolveHostnamesLocally = true;

            bot = new TelegramBotClient(setting.CommandBot.Token, proxy);
            bot.OnMessage += OnMessage;
        }

        public void Start()
        {
            bot.StartReceiving();
        }

        public void Stop()
        {
            bot.StopReceiving();
        }

        private void OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string MsgTxt = e.Message.Text;
            int chatId = e.Message.From.Id;

            if (MsgTxt is null) return;

            try
            {
                var b = bases.FirstOrDefault(x => x.Telegram == MsgTxt);
                if (b is null) return;
                ThreadPool.QueueUserWorkItem(obj => { UpdateBase(b, chatId); });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        #region old

        //private void OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        //{
        //    string MsgTxt = e.Message.Text;

        //    int chatId = e.Message.From.Id;

        //    if (!Constants.checkChatId(chatId))
        //    {
        //        SendMessage($"Пользователь {chatId} хочет добавиться", TelegramBots.BitFinanceCommand, Constants.ilyaChat);
        //        return;
        //    }

        //    if (MsgTxt is null) return;

        //    if (MsgTxt.ToLower().Contains("kick"))
        //    {
        //        Kick(MsgTxt, chatId);
        //        return;
        //    }

        //    TelegramBots bot;
        //    if (MsgTxt.ToLower().Contains("my"))
        //    {
        //        bot = TelegramBots.BitFinanceCommand;
        //    }
        //    else
        //    {
        //        chatId = 0;
        //        bot = TelegramBots.BitFinanceTeam;
        //    }

        //    try
        //    {
        //        var b = bases.Where(x => x.Telegram == MsgTxt).FirstOrDefault();
        //        if (b is null) return;
        //        ThreadPool.QueueUserWorkItem(delegate { UpdateBase(b, bot, chatId); });
        //    }
        //    catch { }
        //}

        //private void Kick(string msg, int chatId)
        //{
        //    try
        //    {
        //        string[] arr = msg.Split(' ');
        //        string baseName = arr[1];
        //        server.ClearSessions(baseName, 10);
        //        SendMessage($"Пользователи успешно выпнуты.", TelegramBots.BitFinanceCommand, chatId);
        //    }
        //    catch
        //    {
        //        string text = "Ошибка. Проверьте правильность написания базы. Регистр Важен!";
        //        SendMessage(text, TelegramBots.BitFinanceCommand, chatId);
        //    }
        //}

        private async void UpdateBase(Base b, int chatId)
        {
            SendMessage($"{b.Title} обновляется", chatId);
            if (b.IsServer)
            {
                try
                {
                    var spl = b.Folder.Split('\u005c');
                    string baseRef = spl[1];
                    server.ClearSessions(baseRef, 15000);
                }
                catch (System.Exception err)
                {
                    SendMessage("Не смог подключиться к серверу и не выбил пользователей" + err, chatId);
                    SendMessage($"{b.Title} обновляется", chatId);
                }
            }
            try
            {
                Operation op = new Operation(b, setting.Platform.FullPath);
                await op.UpdateRepository();
                SendMessage($"{b.Title} обновлена", chatId);
            }
            catch
            {
                SendMessage("Неведомая ошибка", chatId);
            }
        }

        #endregion

        public int SendMessage(string message, int chatId)
        {
            //TelegramDescription descr = new TelegramDescription(bot);

            var proxy = Constants.proxy;

            proxy.ResolveHostnamesLocally = true;

            TelegramBotClient Bot = new TelegramBotClient(setting.CommandBot.Token, proxy);
            var msg = Bot.SendTextMessageAsync(chatId, message);

            return msg.Id;
        }
    }
}