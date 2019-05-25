using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Handler.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}
