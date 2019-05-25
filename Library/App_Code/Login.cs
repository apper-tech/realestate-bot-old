using Library.Akaratak.DataService;
using System;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

//using TelegramBot.localhost;

namespace Telegram_Bot.Library
{
    public partial class Bot
    {
        public static string _Login_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Login_SignUp",handler.Lang))
                },
                new KeyboardButton[]
                {
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Login_SignUp_Start", handler.Lang);
        }
        public static bool _Check_Logged_In(Options_Handler handler)
        {
           
           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            //if (System.Diagnostics.Debugger.IsAttached)
            //    return true;
            return telegram_Handler._User_Exist(handler, Token_GET);
        }
        public static void _Add_User(Options_Handler handler)
        {
           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            string result = telegram_Handler._Add_User(handler, Token_GET);

        }
        public static bool _Check_Input_Signup(Options_Handler handler, string message, string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            switch (handler.ExpectedEntry)
            {
                case "Signup_Email":
                    handler.ExpectedEntry = "Signup_Phone";
                    replay = _Get_Command_Value("Login_SignUp_Phone", settings.Lang);
                    var user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        user_option.Email = message;
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Email = message, Id = chat.Id });
                    }
                    Send_Message(replay, chat, null); hit = true;
                    break;
                case "Signup_Phone":
                    handler.ExpectedEntry = "Signup_Password";
                    replay = _Get_Command_Value("Login_SignUp_Password", settings.Lang);
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        user_option.Phone = message;
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Phone = message, Id = chat.Id });
                    }
                    Send_Message(replay, chat, null); hit = true;
                    break;
                case "Signup_Password":
                    handler.ExpectedEntry = "Signup_Password_Confirm";
                    replay = _Get_Command_Value("Login_SignUp_Password_Confirm", settings.Lang);
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        user_option.Password = message;
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Password = message, Id = chat.Id });
                    }
                    Send_Message(replay, chat, null); hit = true;
                    break;
                case "Signup_Password_Confirm":
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        // if (string.IsNullOrEmpty(user_option.Oldpassword))
                        user_option.Oldpassword = message;
                        if (user_option.Oldpassword == user_option.Password)
                        {
                           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
                            string result = telegram_Handler._Add_User(handler, Token_GET);
                            if (string.IsNullOrEmpty(result))
                            {
                                replay = _Get_Command_Value("Login_SignUp_Success", handler.Lang);
                                Send_Message(replay, chat, commands); hit = true;
                                user_option.ExpectedEntry = "Insert_Cat";
                                replay = _Get_Command_Value("Insert_Property_Cat", settings.Lang);
                                Send_Message(replay, chat, null); hit = true;
                            }
                            else
                            {
                                user_option.ExpectedEntry = "Signup_Email";
                                replay = result;
                                Send_Message(replay, chat, null);
                                replay = _Get_Command_Value("Login_SignUp_Email", settings.Lang);
                                Send_Message(replay, chat, null); hit = true;
                            }
                        }
                        else
                        {
                            replay = _Get_Command_Value("Login_SignUp_Password_Mismatch", settings.Lang);
                            Send_Message(replay, chat, null); hit = true;
                        }
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Oldpassword = message, Id = chat.Id });
                    }
                    //Send_Message(replay, chat, null); hit = true;
                    break;
            }
            return hit;
        }
        public static void _Check_Input_Password(Options_Handler handler, string message, string command_en, string command_ar, Chat chat, bool old)
        {
            if (old)
            {
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                user_option.ExpectedEntry = "Password_New";
                replay = _Get_Command_Value("Manage_New_Password", settings.Lang);
                
                if (user_option != null)
                {
                    user_option.Oldpassword = message;
                }
                else
                {
                    chat_options.Add(new Options_Handler { Oldpassword = message, Id = chat.Id });
                }
                Send_Message(replay, chat, null);
                return;
            }
            if (string.IsNullOrEmpty(command_en) && string.IsNullOrEmpty(command_ar))
            {
                var user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                if (user_option != null)
                {
                    user_option.Password = message;
                }

               Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
                string result = telegram_Handler._Update_User(handler);
                if (string.IsNullOrEmpty(result))
                {
                    _Account_List(settings, out commands);
                    user_option.ExpectedEntry = "";
                    replay = _Get_Command_Value("Manage_Password_Success", handler.Lang);
                    Send_Message(replay, chat, commands);
                }
                else
                {
                    replay = result;
                    Send_Message(replay, chat, null);
                    replay = _Get_Command_Value("Manage_New_Password", settings.Lang);
                    Send_Message(replay, chat, null);
                }
            }
        }
        public static void _Check_Input_Email(Options_Handler handler, string message, string command_en, string command_ar, Chat chat)
        {
            handler.Email = message;
            if (string.IsNullOrEmpty(command_en) && string.IsNullOrEmpty(command_ar))
            {
               Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
                string result = telegram_Handler._Update_User(handler);
                if (string.IsNullOrEmpty(result))
                {
                    _Account_List(settings, out commands);
                    handler.ExpectedEntry = "";
                    replay = _Get_Command_Value("Manage_Email_Success", handler.Lang);
                    Send_Message(replay, chat, commands);
                }
                else
                {
                    replay = result;
                    Send_Message(replay, chat, null);
                    replay = _Get_Command("Manage_New_Email", settings.Lang);
                    Send_Message(replay, chat, null);
                }
            }
        }
        public static string _Account_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Manage_Update_Password",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Manage_Update_Email",handler.Lang))
                },
                new KeyboardButton[]
                {
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Manage", handler.Lang);
        }
    }
}