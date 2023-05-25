using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;
using Update = Telegram.Bot.Types.Update;

namespace Bot
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("6239769324:AAE8BbkP2v0d4dyNbx45UEkvBOuBmNdSs7g");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/test")
                   //await botClient.SendTextMessageAsync(561418543,$"{}")
                return;

                {
                    SendInline(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken);
                    return;
                }
            }
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                string codeOfButton = update.CallbackQuery.Data;
                if (codeOfButton == "post")
                {
                    Console.WriteLine("Нажата Кнопка 1");
                  
                    string telegramMessage = "Вы нажали Кнопку 1";
                    await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
                if (codeOfButton == "12")
                {
                    Console.WriteLine("Нажата Кнопка 2");
                    string telegramMessage = "Вы нажали Кнопку 2";
                    // await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                    InlineKeyboardMarkup inlineKeyBoard = new InlineKeyboardMarkup(
                        new[]
                        {
                            // first row
                            new[]
                            {
                                // first button in row
                                InlineKeyboardButton.WithCallbackData(text: "Кнопка 3", callbackData: "post3"),
                                // second button in row
                                InlineKeyboardButton.WithCallbackData(text: "Кнопка 4", callbackData: "post4"),
                            },

                        });

                    // await botClient.EditMessageCaptionAsync(chatId: update.CallbackQuery.Message.Chat.Id, caption: telegramMessage, messageId: update.CallbackQuery.Message.MessageId);
                    await bot.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }

        public static async void SendInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                // keyboard
                new[]
                {
                    // first row
                    new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithCallbackData(text: "Кнопка 1", callbackData: "post"),
                        // second button in row
                        InlineKeyboardButton.WithCallbackData(text: "Кнопка 2", callbackData: "12"),
                    },
                    // second row
                    new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithUrl(text: "Ссылка", url: "https://google.com"),
                        InlineKeyboardButton.WithCallbackData("CallbackData кнопка")
                    },

                });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "за что мне это??",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}