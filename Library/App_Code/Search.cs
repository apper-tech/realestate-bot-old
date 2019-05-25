using Library.Akaratak.DataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

//using TelegramBot.localhost;

namespace Telegram_Bot.Library
{
    public partial class Bot
    {
        public static string _Search_Option_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Search_Select_Cat",handler.Lang))
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Search_Select_Gar",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Search_Select_Grd",handler.Lang))
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Search_Done",handler.Lang))
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }

            };
            keyboardButtons = replyKeyboardMarkup;
            return "🔎";
        }
        public static string _Category_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Property_Category","Residental",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Property_Category","Commertial",handler.Lang))
                },
                  new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Property_Category","Luxury",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Property_Category","Land",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return "🏠";
        }
        public static string _Category_List_Check(string command_en, string command_ar)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(command_en))
            {
                list = _Get_Command_List("Category", "en");
                foreach (var item in list)
                {
                    if (item == command_en)
                    {
                        return command_en;
                    }
                }
            }
            if (!string.IsNullOrEmpty(command_ar))
            {
                list = _Get_Command_List("Category", "ar");
                foreach (var item in list)
                {
                    if (item == command_ar)
                    {
                        return command_ar;
                    }
                }
            }
            return "__NULL";
        }
        public static string _Type_List(Options_Handler handler, string cat, out ReplyKeyboardMarkup keyboardButtons, bool insert)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            switch (cat)
            {
                case "Residental":
                    replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                       {
                            new KeyboardButton[]
                            {
                                new KeyboardButton(_Get_Command_Value("Property_Type","Apartment",handler.Lang))
                               },
                            new KeyboardButton[]
                            {  new KeyboardButton(_Get_Command_Value("Property_Type","Commonhold",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Timeshare",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","House",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Villa",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Mansion",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(insert?"":_Get_Command_Value("Property_Type","All",handler.Lang))
                            }
                       };
                    break;
                case "Commertial":
                    replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                       {
                            new KeyboardButton[]
                            {
                                new KeyboardButton(_Get_Command_Value("Property_Type","In_Building_Shop",handler.Lang))
                               },
                            new KeyboardButton[]
                            {  new KeyboardButton(_Get_Command_Value("Property_Type","In_Mall_Shop",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Office",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Building",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Mall",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Hotel",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(insert?"":_Get_Command_Value("Property_Type","All",handler.Lang))
                            }
                       };
                    break;
                case "Luxury":
                    replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                      {
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Resort",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Hotel_Room",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Resturant_Table",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(insert?"":_Get_Command_Value("Property_Type","All",handler.Lang))
                            }
                      };
                    break;
                case "Land":
                    replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                       {
                            new KeyboardButton[]
                            {  new KeyboardButton(_Get_Command_Value("Property_Type","Residental",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Commertial",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Luxury",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Agriculture",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(_Get_Command_Value("Property_Type","Non_Licensed",handler.Lang))
                              },
                            new KeyboardButton[]
                            {   new KeyboardButton(insert?"":_Get_Command_Value("Property_Type","All",handler.Lang))
                            }
                       };
                    break;
            }
            keyboardButtons = replyKeyboardMarkup;
            return "🏘";
        }
        public static string _Type_List_Check(string command_en, string command_ar)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(command_en))
            {
                list = _Get_Command_List("Type", "en");
                foreach (var item in list)
                {
                    if (item == command_en)
                    {
                        return command_en;
                    }
                }
            }
            if (!string.IsNullOrEmpty(command_ar))
            {
                list = _Get_Command_List("Type", "ar");
                foreach (var item in list)
                {
                    if (item == command_ar)
                    {
                        return command_ar;
                    }
                }
            }
            return "__NULL";
        }
        public static string _Garage_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Command","Search_Select_Gar_Yes",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Command","Search_Select_Gar_No",handler.Lang))
                },
                  new KeyboardButton[]
                {
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return "🚗";
        }
        public static bool _Garage_List_Check(string command_en, string command_ar, out bool select)
        {
            if (command_en == "Search_Select_Gar_Yes")
            {
                select = true;
                return true;
            }
            if (command_en == "Search_Select_Gar_No")
            {
                select = false;
                return true;
            }
            if (command_ar == "Search_Select_Gar_Yes")
            {
                select = true;
                return true;
            }
            if (command_ar == "Search_Select_Gar_No")
            {
                select = false;
                return true;
            }
            select = false;
            return false;
        }
        public static string _Garden_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Command","Search_Select_Grd_Yes",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Command","Search_Select_Grd_No",handler.Lang))
                },
                  new KeyboardButton[]
                {
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return "👩‍🌾";
        }
        public static bool _Garden_List_Check(string command_en, string command_ar, out bool select)
        {
            if (command_en == "Search_Select_Grd_Yes")
            {
                select = true;
                return true;
            }
            if (command_en == "Search_Select_Grd_No")
            {
                select = false;
                return true;
            }
            if (command_ar == "Search_Select_Grd_Yes")
            {
                select = true;
                return true;
            }
            if (command_ar == "Search_Select_Grd_No")
            {
                select = false;
                return true;
            }
            select = false;
            return false;
        }
        public static string _Build_Search_Url(Options_Handler user, out InlineKeyboardMarkup commands, out Uri filename)
        {
            var url = user.Lang == null ? "https://Akaratak.com/Search" : user.Lang == "en" ? "https://Akaratak.com/Search" : "https://عقاراتك.com/بحث";
            url = !(string.IsNullOrEmpty(user.Cat)) ? url + "/" + _Get_Command_Value("Property_Category", user.Cat, user.Lang).Replace(" ", "_") : url;
            url = !(string.IsNullOrEmpty(user.Cat) && string.IsNullOrEmpty(user.Type)) ? url + "/" + _Get_Command_Value("Property_Type", user.Type, user.Lang).Replace(" ", "_") : url;

           

            var button = InlineKeyboardButton.WithUrl(_Get_Command_Value("Search_Result", user.Lang), url);

            var btnarray = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    button
                }
            };
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(btnarray);
            commands = markup;
            filename = new Uri(@"http://akaratak.com/customdesign/images/search_image.png");
            return url;
        }
    }

}