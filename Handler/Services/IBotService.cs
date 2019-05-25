using Telegram.Bot;

namespace Handler.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
