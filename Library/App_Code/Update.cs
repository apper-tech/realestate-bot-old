using Library.Akaratak.DataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
//using System.Web.Configuration;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

//using TelegramBot.localhost;

namespace Telegram_Bot.Library
{
    public partial class Bot
    {
        public static string _Update_Confirm_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Done_Yes",handler.Lang)),
                     new KeyboardButton(_Get_Command_Value("Update_Done_No",handler.Lang))
                }, new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Update_More_Data", handler.Lang);
        }
        public static bool _Check_Input_Update(Options_Handler handler, string message, string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            var user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
            switch (user_option.ExpectedEntry)
            {
                case "Update_Select_Proprty":
                    user_option.ExpectedEntry = "Update_Select_Option";
                    string property_data_trim = message.Remove(0, 4).TrimEnd(')').TrimStart('(');
                    int i = property_data_trim.IndexOf(" @ ");
                    string address = property_data_trim.Substring(0, i);
                    string date = property_data_trim.Substring(i + 3);
                    user_option = new Telegram_HandlerClient()._Get_Property_Value(user_option, address, date);
                    chat_options.Remove(chat_options.Where(x => x.Id == user_option.Id).FirstOrDefault());
                    chat_options.Add(user_option);
                    replay = _Update_Option_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Update_Cat"://cat
                    user_option.ExpectedEntry = "Update_Typ";
                    _Category_List(settings, out commands);
                    replay = _Get_Command_Value("Search_Select_Cat", settings.Lang);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Update_Typ":
                    string cat = _Category_List_Check(command_en, command_ar);

                    if (user_option != null)
                        user_option.Cat = cat;
                    user_option.ExpectedEntry = "Update_Typ_Chk";
                    replay = _Get_Command_Value("Search_Select_Typ", settings.Lang);
                    _Type_List(settings, cat, out commands, true);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Update_Typ_Chk":
                    user_option.ExpectedEntry = "Update_Confirm";
                    string typ = _Type_List_Check(command_en, command_ar);

                    if (user_option != null)
                        user_option.Type = typ;
                    replay = _Update_Confirm_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Update_Address":
                   hit= _Check_Update_Value_Int(user_option, message, chat, "Update_Address", "Update_Address_Invalid", true);
                    break;
                case "Update_Sale_Price":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Sale_Price", "Update_Sale_Price_Invalid", true);
                    break;
                case "Update_Rent_Price":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Rent_Price", "Update_Rent_Price_Invalid", true);
                    break;
                case "Update_Other_Details":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Other_Details", "Update_Other_Details_Invalid", true);
                    break;
                case "Update_Size":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Size", "Update_Size_Invalid", true);
                    break;
                case "Update_Num_Bed":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Num_Bed", "Update_Num_Bed_Invalid", true);
                    break;
                case "Update_Num_Bath":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Num_Bath", "Update_Num_Bath_Invalid", true);
                    break;
                case "Update_Floors":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Floors", "Update_Floors_Invalid", true);
                    break;
                case "Update_Zip_Code":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Zip_Code", "Update_Zip_Code_Invalid", true);
                    break;
                case "Update_Country":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_Country", "Update_Country_Invalid",false);
                    user_option.ExpectedEntry = "Update_City";
                    replay = _City_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Update_City":
                    hit = _Check_Update_Value_Int(user_option, message, chat, "Update_City", "Update_City_Invalid", true);
                    break;
            }
            return hit;
        }
        public static string _Update_Option_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Type",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Address",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Country_City",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Sale_Price",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Rent_Price",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Photo",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Location",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Other_Details",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Size",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Zip_Code",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Num_Bed",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Num_Bath",handler.Lang))
                },
                 new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Update_Floors",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return "🏠";
        }
        public static string _Update_Proprty_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            var proprty_list = telegram_Handler._Get_Property_List(handler);
            //apply filters
            KeyboardButton[][] keyboard = new KeyboardButton[10][];
            if(proprty_list.Length>0)
            for (int i = 0; i < 10; i++)
            {
                keyboard[i] = new KeyboardButton[] { new KeyboardButton("#" + (i + 1) + ".(" + proprty_list[i].Address + " @ " + proprty_list[i].Date_Added + ")") };
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = keyboard;
            keyboardButtons = replyKeyboardMarkup;
            return "📑";
        }
        public static bool _Check_Update_Value_Int(Options_Handler user_option, string message, Chat chat, string resource, string resource_invaled, bool defenet)
        {
            user_option = chat_options.Where(x => x.Id == user_option.Id).FirstOrDefault();
            try
            {
                if (resource_invaled.Contains("Update_Floor_Num"))
                    user_option.Floor = int.Parse(message);
                if (resource_invaled.Contains("Update_Num_Bed"))
                    user_option.Num_Bedrooms = int.Parse(message);
                if (resource_invaled.Contains("Update_Num_Bath"))
                    user_option.Num_Bathrooms = int.Parse(message);
                if (resource_invaled.Contains("Update_Floors"))
                    user_option.Num_Floors = int.Parse(message);
                if (resource_invaled.Contains("Update_Size"))
                    user_option.Property_Size = int.Parse(message);
                if (resource_invaled.Contains("Update_Zip_Code"))
                    user_option.Zip_Code = message;
                if (resource_invaled.Contains("Update_Garage"))
                {
                    if (_Get_Command(message, settings.Lang) != "Search_Select_Gar_Yes" && _Get_Command(message, settings.Lang) != "Search_Select_Gar_No")
                        throw new NullReferenceException();
                    user_option.Has_Garage = _Get_Command(message, settings.Lang) == "Search_Select_Gar_Yes" ? true : false;
                }
                if (resource_invaled.Contains("Update_Garden"))
                {
                    if (_Get_Command(message, settings.Lang) != "Search_Select_Grd_Yes" && _Get_Command(message, settings.Lang) != "Search_Select_Grd_No")
                        throw new NullReferenceException();
                    user_option.Has_Garden = _Get_Command(message, settings.Lang) == "Search_Select_Grd_Yes" ? true : false;
                }
                if (resource_invaled.Contains("Update_Other_Details"))
                    user_option.Other_Details = message;
                if (resource_invaled.Contains("Update_Sale_Price"))
                    user_option.Sale_Price = int.Parse(message);
                if (resource_invaled.Contains("Update_Rent_Price"))
                    user_option.Rent_Price = int.Parse(message);
                if (resource_invaled.Contains("Update_Address"))
                    user_option.Address = message;
                if (resource_invaled.Contains("Update_Location"))
                {
                    //https://www.google.com/maps/@33.2742896,36.1859014,16.75z
                    string loc_temp = message;
                    loc_temp = loc_temp.Replace("https://www.google.com/maps/@", "");
                    string lat = loc_temp.Split(',')[0];
                    string lng = loc_temp.Split(',')[1];
                    _Check_Insert_Value_Location(user_option, lat, lng);
                }
                if (resource_invaled.Contains("Update_Country"))
                {
                    _Check_Insert_Value_Country(user_option, message);
                }
                if (resource_invaled.Contains("Update_City"))
                {
                    _Check_Insert_Value_City(user_option, message);
                }
                if (defenet)
                {
                    user_option.ExpectedEntry = "Update_Select_Option";
                    replay = _Update_Confirm_List(settings, out commands);
                    Send_Message(replay, chat, commands);
                }
                return true;
            }
            catch (Exception)
            {
                replay = _Get_Command_Value(resource_invaled, settings.Lang);
                Send_Message(replay, chat, null); return true;
            }

        }
    }
}