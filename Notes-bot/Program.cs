using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Notes_bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Notes_bot;

class Program
{
    static void Main(string[] args)
    {
        using var cts = new CancellationTokenSource();
        var botClient = new TelegramBotClient("7957079084:AAEySj8z-W70Q-viYfWUga_DGIpWL2OlyiA",
            cancellationToken: cts.Token);
        botClient.StartReceiving(Update, Error);
        Console.ReadLine();
    }
    static async Task Error(ITelegramBotClient arg1, Exception arg2, HandleErrorSource arg3, CancellationToken arg4)
    {
        throw new NotImplementedException();
    }

    static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandlerMessages(botClient, update);
                break;
            
            case UpdateType.CallbackQuery:
                await HandlerQuery(botClient, update);
                break;
        }
    }

    static async Task HandlerQuery(ITelegramBotClient botClient, Update update)
    {
        await botClient.AnswerCallbackQuery(update.CallbackQuery.Id,  "✏️ Напишите заметку:  ");
        return;
    }

    static async Task HandlerMessages(ITelegramBotClient botClient, Update update)
    {
        var message = update.Message;
        if (update.Type != UpdateType.Message || message.Text == null)
        {
            await botClient.SendMessage(message.Chat.Id, "Напишите текст!");
            return;
        }
        var text = message.Text;
        var username = message.From.Username ?? message.From.Id.ToString();

        switch (text?.ToLower())
        {
            case "/start":
                await botClient.SendMessage(message.Chat.Id,
                    $"Добро пожаловать, {message.From.FirstName} выбери что хочешь сделать: ",
                    replyMarkup:
                    new KeyboardButton[] { "Создать заметку", "Просмотреть заметки" });
                return;

            case "создать заметку":
                await botClient.SendMessage(message.Chat.Id, "Напишите заметку: ");
                return;

            case "просмотреть заметки":
                var notes = NoteService.GetNotes(message.Chat.Id);

                if (notes.Count == 0)
                {
                    await botClient.SendMessage(message.Chat.Id, "У вас нет заметок!");
                    return;
                }

                var txtNote = "";
                for (int i = 0; i < notes.Count; i++)
                {
                    txtNote += $"{i + 1}.  {notes[i].Title}\n";
                }

                await botClient.SendMessage(message.Chat.Id, $"Ваши заметки:\n{txtNote}",
                    replyMarkup:
                    new InlineKeyboardButton[] {"Удалить заметку"});

        return;
            
            default:
                NoteService.CreateNote(message.Chat.Id, text, username);
                await botClient.SendMessage(message.Chat.Id, "✅ Заметка сохранена!");
                return;
        }
    }
}