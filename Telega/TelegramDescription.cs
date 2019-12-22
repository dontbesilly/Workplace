using Workplace1c;

namespace telega
{
    enum TelegramBots
    {
        BitFinanceTeam,
        BitFinanceCommand
    }

    struct TelegramDescription
    {
        string token;
        int chatId;

        public string Token
        {
            get { return token; }
        }
        public int ChatId
        {
            get { return chatId; }
        }

        public TelegramDescription(TelegramBots bot)
        {
            token = "";
            chatId = 0;

            if (bot == TelegramBots.BitFinanceTeam)
            {
                token = Constants.BitfinanceToken;
                chatId = Constants.BitfinanceChat;
            }
            else if (bot == TelegramBots.BitFinanceCommand)
            {
                token = Constants.BitfinanceCommandToken;
                chatId = Constants.BitfinanceCommandChat;
            }
        }
    }
}