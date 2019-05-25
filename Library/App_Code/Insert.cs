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
        public static bool _Check_Input_Insert(Options_Handler handler, string message, string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            var user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
            switch (user_option.ExpectedEntry)
            {
                case "Insert_Cat":
                    user_option.ExpectedEntry = "Insert_Typ";
                    string cat = _Category_List_Check(command_en, command_ar);
                    replay = _Get_Command_Value("Search_Select_Typ", settings.Lang);
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        //command_en
                        user_option.Cat = cat;
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Cat = cat, Id = chat.Id });
                    }
                    _Type_List(settings, cat, out commands, true);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Insert_Typ":
                    user_option.ExpectedEntry = "Insert_Exp";
                    replay = _Get_Command_Value("Search_Select_Exp", settings.Lang);
                    string typ = _Type_List_Check(command_en, command_ar);
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option != null)
                    {
                        user_option.Type = typ;
                    }
                    else
                    {
                        chat_options.Add(new Options_Handler { Type = typ, Id = chat.Id });
                    }
                    if (user_option.Date.Year < DateTime.Now.Year - 1)
                        user_option.Date = DateTime.Now;
                    var calender = Calender.Create_Calender(handler, DateTime.Now.Year, DateTime.Now.Month + 3);
                    Send_Message(replay, chat, calender, 0); hit = true;
                    break;
                case "Insert_Exp":
                    user_option.ExpectedEntry = "Insert_Floor_Num";
                    replay = _Get_Command_Value("Insert_Floor", settings.Lang);
                    user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
                    if (user_option.Date == DateTime.Now)
                        user_option.Date = DateTime.Now.AddMonths(3);
                    Send_Message(replay, chat, null); hit = true;
                    break;
                case "Insert_Floor_Num":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Num_Bed", "Insert_Floor_Invalid");
                    break;
                case "Insert_Num_Bed":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Num_Bath", "Insert_Num_Bed_Invalid");
                    break;
                case "Insert_Num_Bath":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Floors", "Insert_Num_Bath_Invalid");
                    break;
                case "Insert_Floors":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Size", "Insert_Floors_Invalid");
                    break;
                case "Insert_Size":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Zip_Code", "Insert_Size_Invalid");
                    break;
                case "Insert_Zip_Code":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Country", "Insert_Zip_Code_Invalid");
                    if (!string.IsNullOrEmpty(settings.Url_ext))
                        settings.Url_ext = (int.Parse(settings.Url_ext) - 100).ToString();
                    else
                        settings.Url_ext = 0.ToString();
                    var country_buttons = _Country_List_Filters(settings);
                    Send_Message(replay, chat, country_buttons, 0);
                    replay = _Country_List(settings, out commands);
                    Send_Message(replay, chat, commands);
                    break;
                case "Insert_Country":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_City", "Insert_Country_Invalid");
                    replay = _City_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Insert_City":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Garage", "Insert_City_Invalid");
                    replay = _Garage_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Insert_Garage":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Garden", "Insert_Garage_Invalid");
                    replay = _Garden_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                    break;
                case "Insert_Garden":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Other_Details", "Insert_Garden_Invalid");
                    break;
                case "Insert_Other_Details":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Sale_Price", "Insert_Other_Details_Invalid");
                    break;
                case "Insert_Sale_Price":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Rent_Price", "Insert_Rent_Price_Invalid");
                    break;
                case "Insert_Rent_Price":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Address", "Insert_Rent_Price_Invalid");
                    break;
                case "Insert_Address":
                    hit = _Check_Insert_Value_Int(user_option, handler, message, chat, "Insert_Location", "Insert_Addess_Invalid");
                    break;

            }
            return hit;
        }
        public static bool _Check_Insert_Value_Int(Options_Handler user_option, Options_Handler handler, string message, Chat chat, string resource, string resource_invaled)
        {
            user_option = chat_options.Where(x => x.Id == handler.Id).FirstOrDefault();
            try
            {
                if (resource_invaled.Contains("Insert_Floor_Num"))
                    user_option.Floor = int.Parse(message);
                if (resource_invaled.Contains("Insert_Num_Bed"))
                    user_option.Num_Bedrooms = int.Parse(message);
                if (resource_invaled.Contains("Insert_Num_Bath"))
                    user_option.Num_Bathrooms = int.Parse(message);
                if (resource_invaled.Contains("Insert_Floors"))
                    user_option.Num_Floors = int.Parse(message);
                if (resource_invaled.Contains("Insert_Size"))
                    user_option.Property_Size = int.Parse(message);
                if (resource_invaled.Contains("Insert_Zip_Code"))
                    user_option.Zip_Code = message;
                if (resource_invaled.Contains("Insert_Garage"))
                {
                    if (_Get_Command(message, settings.Lang) != "Search_Select_Gar_Yes" && _Get_Command(message, settings.Lang) != "Search_Select_Gar_No")
                        throw new NullReferenceException();
                    user_option.Has_Garage = _Get_Command(message, settings.Lang) == "Search_Select_Gar_Yes" ? true : false;
                }
                if (resource_invaled.Contains("Insert_Garden"))
                {
                    if (_Get_Command(message, settings.Lang) != "Search_Select_Grd_Yes" && _Get_Command(message, settings.Lang) != "Search_Select_Grd_No")
                        throw new NullReferenceException();
                    user_option.Has_Garden = _Get_Command(message, settings.Lang) == "Search_Select_Grd_Yes" ? true : false;
                }
                if (resource_invaled.Contains("Insert_Other_Details"))
                    user_option.Other_Details = message;
                if (resource_invaled.Contains("Insert_Sale_Price"))
                    user_option.Sale_Price = int.Parse(message);
                if (resource_invaled.Contains("Insert_Rent_Price"))
                    user_option.Rent_Price = int.Parse(message);
                if (resource_invaled.Contains("Insert_Addess"))
                    user_option.Address = message;
                if (resource_invaled.Contains("Insert_Location"))
                {
                    //https://www.google.com/maps/@33.2742896,36.1859014,16.75z
                    string loc_temp = message;
                    loc_temp = loc_temp.Replace("https://www.google.com/maps/@", "");
                    string lat = loc_temp.Split(',')[0];
                    string lng = loc_temp.Split(',')[1];
                    _Check_Insert_Value_Location(handler, lat, lng);
                }
                if(resource_invaled.Contains("Insert_Country"))
                {
                    _Check_Insert_Value_Country(handler, message);
                }
                if (resource_invaled.Contains("Insert_City"))
                {
                    _Check_Insert_Value_City(handler, message);
                }
                user_option.ExpectedEntry = resource;
                replay = _Get_Command_Value(resource, settings.Lang);
                Send_Message(replay, chat, null); return true;
            }
            catch (Exception)
            {
                replay = _Get_Command_Value(resource_invaled, settings.Lang);
                Send_Message(replay, chat, null); return true;
            }
        }
        public static void _Check_Insert_Value_Location(Options_Handler user_option, string Latitude, string Longitude)
        {
            if (!string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude + ""))
            {
                user_option.Location = Latitude + "," + Longitude + "," + "16";
            }
        }
        public static void _Check_Insert_Value_Country(Options_Handler user_option, string country)
        {
           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            var country_list = telegram_Handler._Get_Country_List();
            var ct = country.Substring(0,country.IndexOf('/'));
            foreach (var item in country_list)
            {
                if (item.Country_Name==ct)
                    user_option.Country_ID = item.Country_ID;
            }
            if (user_option.Country_ID == 0)
                user_option.Country_ID = 1;
        }
        public static void _Check_Insert_Value_City(Options_Handler user_option, string city)
        {
           Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            var city_list = telegram_Handler._Get_City_List((int)user_option.Country_ID);
            var ct = city.Substring(0, city.IndexOf('/'));
            foreach (var item in city_list)
            {
                if(item.City_Name==ct)
                {
                    user_option.City_ID = item.City_ID;
                }
            }
            if (user_option.City_ID == 0)
                user_option.City_ID = 1;
        }
        public static void _Check_Insert_Value_Photo(Options_Handler user_option, PhotoSize photo)
        {
            string name = "";
            if (!string.IsNullOrEmpty(photo.FileId))
            {
                name = _Get_New_Image_Name() + ".jpg";
                string localFilePath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + name;
                try
                {
                    using (var fileStream = System.IO.File.OpenWrite(localFilePath))
                    {
                        Telegram.Bot.Types.File fileInfo = bot.GetInfoAndDownloadFileAsync(
                          fileId: photo.FileId,
                          destination: fileStream
                        ).Result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error downloading: " + ex.Message);
                }
            }
            if (string.IsNullOrEmpty(user_option.Property_Photo))
                user_option.Property_Photo = name + ",";
            else
            {
                //extract names
                List<string> photos = user_option.Property_Photo.Split(',').ToList();
                if (photos.Count < 4)
                {
                    photos.Add(name);
                    string ptemp = "";
                    foreach (var item in photos)
                    {
                        ptemp += item + ",";
                    }
                    user_option.Property_Photo = ptemp;
                }
                else
                {
                    foreach (var item in photos)
                    {
                        if (item != name)
                        {
                            user_option.Property_Photo = user_option.Property_Photo.Replace(item, name);
                            break;
                        }
                    }
                }
            }
        }
        public static string _Country_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            int start = int.Parse(handler.Url_ext);
            Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            var country_list = telegram_Handler._Get_Country_List();
            int k = start>0?start:0;
            KeyboardButton[][] keyboard = new KeyboardButton[100][];
            for (int i = 0; i < 100; i++)
                if ( k < country_list.Length)
                { keyboard[i] = new KeyboardButton[] { new KeyboardButton(country_list[k].Country_Name + "/" + country_list[k].Country_Native_Name) }; k++; } else break;
            keyboard = keyboard.Where(x => x!=null&& !string.IsNullOrEmpty(x[0].Text)).ToArray(); ;
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = keyboard;
            keyboardButtons = replyKeyboardMarkup;
            return "🌎";
        }
        public static InlineKeyboardMarkup _Country_List_Filters(Options_Handler handler)
        {
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
              {
                  new InlineKeyboardButton[]
                  {
                    new InlineKeyboardButton{ Text="<",CallbackData="previous-country"},
                    new InlineKeyboardButton{ Text=" ",CallbackData="ignore"},
                    new InlineKeyboardButton{ Text=">",CallbackData="next-country"},
                  }
              });
            return markup;
        }
        public static string _City_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
            var city_list = telegram_Handler._Get_City_List((int)handler.Country_ID);
            KeyboardButton[][] keyboard = new KeyboardButton[city_list.Length][];
            for (int i = 0; i <100; i++)
            {
                if(i<city_list.Length)
                keyboard[i] = new KeyboardButton[] { new KeyboardButton(city_list[i].City_Name + "/" + city_list[i].City_Native_Name) };
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            keyboard = keyboard.Where(x => x != null && !string.IsNullOrEmpty(x[0].Text)).ToArray();
            replyKeyboardMarkup.Keyboard = keyboard;
            keyboardButtons = replyKeyboardMarkup;
            return "🗺";
        }
        public static string _Insert_Confirm_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Insert_Done_Yes",handler.Lang)),
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }                
            };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Insert_Done", handler.Lang);
        }
      
    }
}