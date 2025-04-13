using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Weather.Core.API; 

namespace Weather
{
    class Program
    {
        static async Task Main()
        {
            string telegramToken = "";
            var botClient = new TelegramBotClient(telegramToken);

            var weatherService = new WeatherService();

            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                async (bot, update, token) => await HandleUpdate(bot, update, weatherService),
                HandleError,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"🤖 Бот {me.Username} запущен...");
            Console.ReadLine();

            cts.Cancel();
        }

        static async Task HandleUpdate(ITelegramBotClient botClient, Update update, IWeatherService weatherService)
        {
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text!;
            var weatherInfo = await weatherService.GetWeather(messageText);
            await botClient.SendTextMessageAsync(chatId, weatherInfo);
        }

        static Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Ошибка бота: {exception.Message}");
            return Task.CompletedTask;
        }

    } 
} 
