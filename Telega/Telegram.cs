using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MihaZupan;
using Telegram.Bot;

namespace Workplace1c
{
    class Telega
    {
        private readonly string token;
        private readonly IEnumerable<Base> bases;
        private readonly string platform;
        private readonly Server server = new Server(Constants.serverRef, Constants.adminUser, Constants.adminPass);

        public List<int> ListId { get; set; } = Constants.ListId;

        public Telega(string token, IEnumerable<Base> bases, string platform)
        {
            this.token = token;
            this.bases = bases;
            this.platform = platform;
        }

        public void Start()
        {
            HttpToSocks5Proxy proxy = Constants.proxy;
            proxy.ResolveHostnamesLocally = true;

            TelegramBotClient Bot = new TelegramBotClient(token, proxy);

            Bot.OnMessage += OnMessage;

            Bot.StartReceiving();
        }

        private void OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string MsgTxt = e.Message.Text;

            int chatId = e.Message.From.Id;

            if (!Constants.checkChatId(chatId))
            {
                SendMessage($"Пользователь {chatId} хочет добавиться", TelegramBots.BitFinanceCommand, Constants.ilyaChat);
                return;
            }

            if (MsgTxt is null) return;

            if (MsgTxt.ToLower().Contains("kick"))
            {
                Kick(MsgTxt, chatId);
                return;
            }

            TelegramBots bot;
            if (MsgTxt.ToLower().Contains("my"))
            {
                bot = TelegramBots.BitFinanceCommand;
            }
            else
            {
                chatId = 0;
                bot = TelegramBots.BitFinanceTeam;
            }

            try
            {
                var b = bases.Where(x => x.Telegram == MsgTxt).FirstOrDefault();
                if (b is null) return;
                ThreadPool.QueueUserWorkItem(delegate { UpdateBase(b, bot, chatId); });
            }
            catch { }
        }

        private void Kick(string msg, int chatId)
        {
            try
            {
                string[] arr = msg.Split(' ');
                string baseName = arr[1];
                server.ClearSessions(baseName, 10);
                SendMessage($"Пользователи успешно выпнуты.", TelegramBots.BitFinanceCommand, chatId);
            }
            catch
            {
                string text = "Ошибка. Проверьте правильность написания базы. Регистр Важен!";
                SendMessage(text, TelegramBots.BitFinanceCommand, chatId);
            }
        }

        private async void UpdateBase(Base b, TelegramBots bot, int chatId)
        {
            SendMessage($"{b.Title} обновляется", bot, chatId);
            if (b.IsServer)
            {
                try
                {
                    var spl = b.Folder.Split('\u005c');
                    string serverRef = spl[0];
                    string baseRef = spl[1];
                    server.ClearSessions(baseRef, 15000);
                }
                catch (System.Exception err)
                {
                    SendMessage("Не смог подключиться к серверу и не выбил пользователей" + err, TelegramBots.BitFinanceCommand, chatId);
                    SendMessage($"{b.Title} обновляется", bot, chatId);
                }
            }
            try
            {
                Operation op = new Operation(b, platform);
                await op.UpdateRepository();
                SendMessage($"{b.Title} обновлена", bot, chatId);
            }
            catch
            {
                SendMessage("Неведомая ошибка", bot, chatId);
            }
        }

        public static int SendMessage(string message, TelegramBots bot, int chatId = 0)
        {
            TelegramDescription descr = new TelegramDescription(bot);

            int thisChatId = chatId != 0 ? chatId : descr.ChatId;

            var proxy = Constants.proxy;

            proxy.ResolveHostnamesLocally = true;

            TelegramBotClient Bot = new TelegramBotClient(descr.Token, proxy);
            var msg = Bot.SendTextMessageAsync(thisChatId, message);

            return msg.Id;
        }
    }
}