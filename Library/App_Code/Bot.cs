using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using System.Linq;
using Library.Akaratak.DataService;
//using TelegramBot.localhost;

namespace Telegram_Bot.Library
{
    public partial class Bot
    {
        public static TelegramBotClient bot;
        private static List<Options_Handler> chat_options = new List<Options_Handler>();
        private static ReplyKeyboardMarkup commands = null;
        private static InlineKeyboardMarkup url_commands = null;
        private static Options_Handler settings = null;
        private static readonly string Token_GET = "---------------------";
        private static List<string> image_names = new List<string>();
        private static string replay = null;
        public static Task<Message> Send_Message(string text, Chat chat, ReplyKeyboardMarkup markup)
        {
            Debug.WriteLine("CHAT ID:" + chat.Id);
            Debug.WriteLine("CHAT Text:" + text);
            Debug.WriteLine(markup != null ? "CHAT markup:" + markup.ToString() : "");
            bot.SendChatActionAsync(chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            return bot.SendTextMessageAsync(chat.Id, text, replyMarkup: markup);
        }
        public static Task<Message> Send_Message(string text, Chat chat, InlineKeyboardMarkup markup, int debug)
        {
            Debug.WriteLine("CHAT ID:" + chat.Id);
            Debug.WriteLine("CHAT Text:" + text);
            Debug.WriteLine(markup != null ? "CHAT markup:" + markup.ToString() : "");
            bot.SendChatActionAsync(chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            return bot.SendTextMessageAsync(chat.Id, text, replyMarkup: markup);
        }
        public static Task<Message> Send_Photo(string caption, Chat chat, Uri filename)
        {
            Debug.WriteLine("CHAT ID:" + chat.Id);
            Debug.WriteLine("CHAT Text:" + caption);
            Debug.WriteLine("CHAT Image:" + filename.ToString());
            bot.SendChatActionAsync(chat.Id, Telegram.Bot.Types.Enums.ChatAction.UploadPhoto);
            System.IO.Stream stream = System.IO.File.OpenRead(filename.AbsolutePath);
            return bot.SendPhotoAsync(chat.Id, stream);
        }
        public static Task<Message> Send_Url(string text, Chat chat, InlineKeyboardMarkup markup)
        {

            bot.SendChatActionAsync(chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
            return bot.SendTextMessageAsync(chat.Id, _Get_Command_Value("Search_Result", settings.Lang), replyMarkup: markup);
        }
        public static void Bot_OnMessage(Update e, TelegramBotClient service_bot)
        {
            Chat chat = e.Message.Chat;
            bot = service_bot;
            //text handling
            if (e.Message.Type == MessageType.Text)
            {
                string message = e.Message.Text;
                settings = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (settings == null)
                {
                    settings = new Options_Handler { Id = chat.Id, Lang = "en" };
                    chat_options.Add(settings);
                }//else
                 //    settings.Read_Settings();

                replay = _Get_Command_Value("Welcome", settings.Lang);
                switch (message)
                {
                    case @"/start":

                        replay = _Command_List(settings, out commands);
                        Send_Message(replay, chat, commands);
                        break;
                    case @"/cancel":
                        settings.ExpectedEntry = "";
                        replay = _Command_List(settings, out commands);
                        Send_Message(replay, chat, commands);
                        break;
                    case @"/add_user":
                        _Add_User(new Options_Handler { Id = chat.Id, Email = "teset@test.com", Password = "QWQ#1!Dxs", FirstName = chat.FirstName, LastName = chat.LastName });
                        break;
                    case "/cal":
                        settings = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                        settings.Url_ext = 0.ToString();
                        var country_buttons = _Country_List_Filters(settings);
                        Send_Message(replay, chat, country_buttons, 0);
                        replay = _Country_List(settings, out commands);
                        Send_Message(replay, chat, commands);
                        break;
                    case @"/display_param":
                        //chat_options.Clear();
                        settings = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                        if (settings != null)
                            Send_Message(
                             "Id: " + settings.Id +
                             " , Cat= " + settings.Cat +
                             " , Type= " + settings.Type +
                              " , Gar= " + settings.Garage +
                              " , grd= " + settings.Garden
                             , chat, commands);
                        break;
                    default:
                        string command_en, command_ar;
                        _Get_Input_Commands(message, out command_en, out command_ar);
                        if (!(
                        _Lang(command_en, command_ar, chat)   ||
                        _Search(command_en, command_ar, chat) ||
                        _Other(command_en, command_ar, chat)  ||
                        _Login(command_en, command_ar, chat)  ||
                        _Insert(command_en, command_ar, chat) ||
                        _Update(command_en, command_ar, chat) ||
                        _Check_Entry(settings, message, command_en, command_ar, chat)
                                                                                      ))
                        {
                            replay = _Command_List(settings, out commands);
                            settings.ExpectedEntry = "";
                            Send_Message(replay, chat, commands);
                        }
                        break;
                }
            }
            //location handling
            if (e.Message.Type == MessageType.Location)
            {
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option == null)
                {
                    user_option = new Options_Handler { Id = chat.Id };
                    chat_options.Add(user_option);
                }
                _Check_Insert_Value_Location(user_option, e.Message.Location.Latitude.ToString(), e.Message.Location.Longitude.ToString());
                if (user_option.ExpectedEntry == "Insert_Location")
                {
                    user_option.ExpectedEntry = "Insert_Photo";
                    replay = _Get_Command_Value("Insert_Photo", settings.Lang);
                    Send_Message(replay, chat, null);
                }
                if (user_option.ExpectedEntry == "Update_Location")
                {
                    user_option.ExpectedEntry = "Update_Select_Option";
                    replay = _Update_Confirm_List(settings, out commands);
                    Send_Message(replay, chat, commands);
                }
            }
            //photo handling
            if (e.Message.Type == MessageType.Photo)
            {
                var photo = e.Message.Photo[e.Message.Photo.Length - 1];

                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option == null)
                {
                    user_option = new Options_Handler { Id = chat.Id };
                    chat_options.Add(user_option);
                }
                _Check_Insert_Value_Photo(user_option, photo);
                if (user_option.ExpectedEntry == "Insert_Photo")
                {
                    replay = _Insert_Confirm_List(user_option, out commands);
                    Send_Message(replay, chat, commands);
                }
                if (user_option.ExpectedEntry == "Update_Photo")
                {
                    user_option.ExpectedEntry = "Update_Select_Option";
                    replay = _Update_Confirm_List(settings, out commands);
                    Send_Message(replay, chat, commands);
                }
            }
            //callback query
            if (e.Type == UpdateType.CallbackQuery)
            {
                if (settings == null)
                {
                    settings = new Options_Handler { Id = e.Message.Chat.Id, Date = DateTime.Now };
                    chat_options.Add(settings);
                }
                if (e.CallbackQuery.Data.Contains("calender-day"))
                {
                    if (e.CallbackQuery.Data != "calender-day-")
                    {
                        string day = e.CallbackQuery.Data.Replace("calender-day-", "");
                        settings = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                        if (settings == null)
                        {
                            settings = new Options_Handler { Id = chat.Id, Date = DateTime.Now };
                            chat_options.Add(settings);
                        }
                        // day = settings.Date.Year + "-" + settings.Date.Month + "-" + day;
                        settings.Expire_Date = new DateTime(settings.Date.Year, settings.Date.Month, int.Parse(day));
                        bot.DeleteMessageAsync(chat, e.CallbackQuery.Message.MessageId);
                        settings.ExpectedEntry = "Insert_Floor_Num";
                        replay = _Get_Command_Value("Insert_Floor", settings.Lang);
                        Send_Message(replay, chat, null);
                    }
                }
                settings = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (e.CallbackQuery.Data == "next-month")
                {
                    bot.DeleteMessageAsync(chat, e.CallbackQuery.Message.MessageId);
                    settings.Date = settings.Date.AddMonths(1);
                    var calender = Calender.Create_Calender(null, settings.Date.Year, settings.Date.Month);
                    Send_Message(replay, chat, calender, 0);

                }
                if (e.CallbackQuery.Data == "previous-month")
                {
                    bot.DeleteMessageAsync(chat, e.CallbackQuery.Message.MessageId);
                    settings.Date = settings.Date.Month < DateTime.Now.Month ? settings.Date : settings.Date.AddMonths(-1);
                    var calender = Calender.Create_Calender(null, settings.Date.Year, settings.Date.Month);
                    Send_Message(replay, chat, calender, 0);

                }
                if (e.CallbackQuery.Data == "previous-country")
                {
                    bot.DeleteMessageAsync(chat, e.CallbackQuery.Message.MessageId);
                    if (!string.IsNullOrEmpty(settings.Url_ext))
                        settings.Url_ext = (int.Parse(settings.Url_ext) - 100).ToString();
                    else
                        settings.Url_ext = 0.ToString();
                    var country_buttons = _Country_List_Filters(settings);
                    Send_Message(replay, chat, country_buttons, 0);
                    replay = _Country_List(settings, out commands);
                    Send_Message(replay, chat, commands);

                }
                if (e.CallbackQuery.Data == "next-country")
                {
                    bot.DeleteMessageAsync(chat, e.CallbackQuery.Message.MessageId);
                    if (!string.IsNullOrEmpty(settings.Url_ext))
                        settings.Url_ext = (int.Parse(settings.Url_ext) < 0 ? 0 : int.Parse(settings.Url_ext) + 100).ToString();
                    else
                        settings.Url_ext = 0.ToString();
                    var country_buttons = _Country_List_Filters(settings);
                    Send_Message(replay, chat, country_buttons, 0);
                    replay = _Country_List(settings, out commands);
                    Send_Message(replay, chat, commands);

                }
            }
        }
    }
}