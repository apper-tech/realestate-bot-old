using Library.Akaratak.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;

//using TelegramBot.localhost;
namespace Telegram_Bot.Library
{
    public partial class Bot
    {
        private static void _Get_Input_Commands(string message, out string command_en, out string command_ar)
        {
            string c_en = _Get_Command(message, "en");
            string c_ar = _Get_Command(message, "ar");
            if (string.IsNullOrEmpty(c_en))
                c_en = _Get_Command("Category", message, "en");
            if (string.IsNullOrEmpty(c_en))
                c_en = _Get_Command("Type", message, "en");
            if (string.IsNullOrEmpty(c_ar))
                c_ar = _Get_Command("Category", message, "ar");
            if (string.IsNullOrEmpty(c_ar))
                c_ar = _Get_Command("Type", message, "ar");
            command_en = c_en;
            command_ar = c_ar;
        }
        public static bool _Check_Entry(Options_Handler handler, string message, string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            if (message.Contains("www.google.com/maps") || handler.ExpectedEntry == "Insert_Location")
            {
                hit = _Check_Insert_Value_Int(handler, handler, message, chat, "Insert_Photo", "Insert_Location_Invalid");
            }
            else
            {
                switch (handler.ExpectedEntry)
                {
                    case "Password_Old":
                        _Check_Input_Password(handler, message, command_en, command_ar, chat, true);
                        hit = true;
                        break;
                    case "Password_New":
                        _Check_Input_Password(handler, message, command_en, command_ar, chat, false);
                        hit = true;
                        break;
                    case "Email":
                        _Check_Input_Email(handler, message, command_en, command_ar, chat);
                        hit = true;
                        break;
                    case "Cancel":
                        replay = _Command_List(settings, out commands);
                        handler.ExpectedEntry = "";
                        Send_Message(replay, chat, commands);
                        break;
                    default:
                        hit = (_Check_Input_Signup(handler, message, command_en, command_ar, chat) ||
                               _Check_Input_Insert(handler, message, command_en, command_ar, chat)||
                               _Check_Input_Update(handler, message, command_en, command_ar, chat));
                        break;
                }
            }
            return hit;
        }
        #region Insert
        public static bool _Insert(string command_en, string command_ar, Chat chat)
        {
           var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
            bool hit = false;
            if (command_ar == "Insert" || command_en == "Insert")
            {
                
                if (_Check_Logged_In(settings))
                {
                    user_option.ExpectedEntry = "Insert_Cat";
                    _Category_List(settings, out commands);
                    replay = _Get_Command_Value("Insert_Property_Cat", settings.Lang);
                    Send_Message(replay, chat, commands); hit = true;
                }
                else
                {
                    replay = _Login_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                }
            }
            else if (command_ar == "Manage_Update_Password" || command_en == "Manage_Update_Password")
            {
                user_option.ExpectedEntry = "Password_Old";
                replay = _Get_Command_Value("Manage_Old_Password", settings.Lang);
                Send_Message(replay, chat, null); hit = true;
            }
            else if (command_ar == "Manage_Update_Email" || command_en == "Manage_Update_Email")
            {
                user_option.ExpectedEntry = "Email";
                replay = _Get_Command_Value("Manage_New_Email", settings.Lang);
                Send_Message(replay, chat, null); hit = true;
            }
            else if (command_ar == "Insert_Done_Yes" || command_en == "Insert_Done_Yes")
            {
                if (settings != null)
                {
                    user_option.ExpectedEntry = "";
                    Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
                    var result = telegram_Handler._Add_Property(settings, Token_GET);
                    if (string.IsNullOrEmpty(result))
                    {
                        replay = _Get_Command_Value("Insert_Done_Confirm", settings.Lang);
                        Send_Message(replay, chat, null);
                        replay = _Command_List(settings, out commands);
                        Send_Message(replay, chat, commands);
                    }
                    else
                    {
                        replay = result;
                        Send_Message(replay, chat, null);
                    }
                }
                hit = true;
            }
            return hit;
        }
        #endregion
        #region Update
        public static bool _Update(string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            if (command_ar == "Update" || command_en == "Update")
            {
                if (_Check_Logged_In(settings))
                {
                    settings.ExpectedEntry = "Update_Select_Proprty";
                    replay = _Update_Proprty_List(settings, out commands);
                    //filters
                    Send_Message(replay, chat, commands); hit = true;
                }
                else
                {
                    replay = _Login_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                }
            }
            else if (command_ar == "Update_Type" || command_en == "Update_Type")
            {
                    settings.ExpectedEntry = "Update_Cat";
                    replay = _Category_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Address" || command_en == "Update_Address")
            {
                settings.ExpectedEntry = "Update_Address";
                replay = _Get_Command_Value("Update_Address_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Sale_Price" || command_en == "Update_Sale_Price")
            {
                settings.ExpectedEntry = "Update_Sale_Price";
                replay = _Get_Command_Value("Update_Sale_Price_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Rent_Price" || command_en == "Update_Rent_Price")
            {
                settings.ExpectedEntry = "Update_Rent_Price";
                replay = _Get_Command_Value("Update_Rent_Price_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Other_Details" || command_en == "Update_Other_Details")
            {
                settings.ExpectedEntry = "Update_Other_Details";
                replay = _Get_Command_Value("Update_Other_Details_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Size" || command_en == "Update_Size")
            {
                settings.ExpectedEntry = "Update_Size";
                replay = _Get_Command_Value("Update_Size_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Num_Bed" || command_en == "Update_Num_Bed")
            {
                settings.ExpectedEntry = "Update_Num_Bed";
                replay = _Get_Command_Value("Update_Num_Bed_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Num_Bath" || command_en == "Update_Num_Bath")
            {
                settings.ExpectedEntry = "Update_Num_Bath";
                replay = _Get_Command_Value("Update_Num_Bath_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Floors" || command_en == "Update_Floors")
            {
                settings.ExpectedEntry = "Update_Floors";
                replay = _Get_Command_Value("Update_Floors_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Zip_Code" || command_en == "Update_Zip_Code")
            {
                settings.ExpectedEntry = "Update_Zip_Code";
                replay = _Get_Command_Value("Update_Zip_Code_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Zip_Code" || command_en == "Update_Zip_Code")
            {
                settings.ExpectedEntry = "Update_Zip_Code";
                replay = _Get_Command_Value("Update_Zip_Code_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Location" || command_en == "Update_Location")
            {
                settings.ExpectedEntry = "Update_Location";
                replay = _Get_Command_Value("Update_Location_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Photo" || command_en == "Update_Photo")
            {
                settings.ExpectedEntry = "Update_Photo";
                replay = _Get_Command_Value("Update_Photo_Select", settings.Lang);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Country_City" || command_en == "Update_Country_City")
            {
                settings.ExpectedEntry = "Update_Country";
                if (!string.IsNullOrEmpty(settings.Url_ext))
                    settings.Url_ext = (int.Parse(settings.Url_ext) - 100).ToString();
                else
                    settings.Url_ext = 0.ToString();
                var country_buttons = _Country_List_Filters(settings);
                Send_Message(replay, chat, country_buttons, 0);
                replay = _Country_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Done_Yes" || command_en == "Update_Done_Yes")
            {                    
                replay = _Update_Option_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Update_Done_No" || command_en == "Update_Done_No")
            {
                settings.ExpectedEntry = "";
                hit = true;
                Telegram_HandlerClient telegram_Handler = new Telegram_HandlerClient();
                var result = telegram_Handler._Add_Property(settings, Token_GET);
                if (string.IsNullOrEmpty(result))
                {
                    replay = _Get_Command_Value("Update_Done_Confirm", settings.Lang);
                    Send_Message(replay, chat, null);
                    replay = _Command_List(settings, out commands);
                    Send_Message(replay, chat, commands);
                }
                else
                {
                    replay = result;
                    Send_Message(replay, chat, null);
                    replay = _Update_Option_List(settings, out commands);
                    Send_Message(replay, chat, commands); hit = true;
                }
            }
            return hit;
        }
        #endregion
        #region Others
        public static bool _Other(string command_en, string command_ar, Chat chat)
        {
            bool hit = false;

            if (command_ar == "Cancel" || command_en == "Cancel")
            {
                replay = _Command_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            return hit;

        }
        #endregion
        #region Lang
        public static bool _Lang(string command_en, string command_ar, Chat chat)
        {
            bool hit = false;

            if (command_ar == "Lang_Select" || command_en == "Lang_Select")
            {
                replay = _Lang_List(settings, out commands);
                Send_Message(replay, chat, commands);
                hit = true;
            }
            else if (command_ar == "Lang")
            {
                Send_Message(_Get_Command_Value("Lang_Selected", "ar"), chat, commands);
                settings.Lang = "ar";
                //store settings
                replay = _Command_List(settings, out commands);
                Send_Message(replay, chat, commands);
                hit = true;
            }
            else if (command_en == "Lang")
            {
                Send_Message(_Get_Command_Value("Lang_Selected", "en"), chat, commands);
                settings.Lang = "en";
                //store settings
                replay = _Command_List(settings, out commands);
                Send_Message(replay, chat, commands);
                hit = true;
            }

            return hit;
        }
        #endregion
        #region Login
        public static bool _Login(string command_en, string command_ar, Chat chat)
        {
            bool hit = false;
            
            if (command_ar == "Manage" || command_en == "Manage")
            {
                replay = _Account_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Manage_Update_Password" || command_en == "Manage_Update_Password")
            {
                settings.ExpectedEntry = "Password_Old";
                replay = _Get_Command_Value("Manage_Old_Password", settings.Lang);
                Send_Message(replay, chat, null); hit = true;
            }
            else if (command_ar == "Manage_Update_Email" || command_en == "Manage_Update_Email")
            {
                settings.ExpectedEntry = "Email";
                replay = _Get_Command_Value("Manage_New_Email", settings.Lang);
                Send_Message(replay, chat, null); hit = true;
            }
            else if (command_ar == "Login_SignUp" || command_en == "Login_SignUp")
            {
                settings.ExpectedEntry = "Signup_Email";
                replay = _Get_Command_Value("Login_SignUp_Email", settings.Lang);
                Send_Message(replay, chat, null); hit = true;
            }

           
            return hit;
        }
        #endregion
        #region Search
        public static bool _Search(string command_en, string command_ar, Chat chat)
        {

            bool select = false;
            bool hit = false;
            //display search list
            if (command_ar == "Search" || command_en == "Search")
            {
                replay = _Search_Option_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //display category list
            else if (command_ar == "Search_Select_Cat" || command_en == "Search_Select_Cat")
            {
                replay = _Category_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //display type list
            else if (command_en == _Category_List_Check(command_en, command_ar) || command_ar == _Category_List_Check(command_en, command_ar))
            {
                string cat = _Category_List_Check(command_en, command_ar);
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option != null)
                {
                    user_option.Cat = cat;
                }
                else
                {
                    chat_options.Add(new Options_Handler { Cat = cat, Id = chat.Id });
                }
                replay = _Type_List(settings, cat, out commands, false);
                Send_Message(replay, chat, commands); hit = true;
            }
            //select type and back to list
            else if (command_en == _Type_List_Check(command_en, command_ar) || command_ar == _Type_List_Check(command_en, command_ar))
            {
                string typ = _Type_List_Check(command_en, command_ar);
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option != null)
                {
                    user_option.Type = typ;
                }
                else
                {
                    chat_options.Add(new Options_Handler { Type = typ, Id = chat.Id });
                }
                replay = _Search_Option_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //select has garage
            else if (command_ar == "Search_Select_Gar" || command_en == "Search_Select_Gar")
            {
                replay = _Garage_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //select has garden
            else if (command_ar == "Search_Select_Grd" || command_en == "Search_Select_Grd")
            {
                replay = _Garden_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //check and display for garage
            else if (_Garage_List_Check(command_en, command_ar, out select))
            {
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option != null)
                {
                    user_option.Garage = select;
                }
                else
                {
                    chat_options.Add(new Options_Handler { Garage = select, Id = chat.Id });
                }
                replay = _Search_Option_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            //check and display for garden
            else if (_Garden_List_Check(command_en, command_ar, out select))
            {
                var user_option = chat_options.Where(x => x.Id == chat.Id).FirstOrDefault();
                if (user_option != null)
                {
                    user_option.Garden = select;
                }
                else
                {
                    chat_options.Add(new Options_Handler { Garden = select, Id = chat.Id });
                }
                replay = _Search_Option_List(settings, out commands);
                Send_Message(replay, chat, commands); hit = true;
            }
            else if (command_ar == "Search_Done" || command_en == "Search_Done")
            {
                Uri fileInfo = null;
                replay = _Build_Search_Url(settings, out url_commands, out fileInfo);
                // Send_Photo(replay, chat, fileInfo);
                Send_Url(replay, chat, url_commands);
            }
            return hit;
        }
        #endregion
    }
}