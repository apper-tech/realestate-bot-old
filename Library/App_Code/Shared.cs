using Library;
using Library.Akaratak.DataService;
using Library.App_Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

//using TelegramBot.localhost;
namespace Telegram_Bot.Library
{
    public partial class Bot
    {

        #region Main 
        public static string _Lang_List(Options_Handler handler, out ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Lang","en")),
                    new KeyboardButton(_Get_Command_Value("Lang","ar"))
                },
                new KeyboardButton[]
                {
                     new KeyboardButton(_Get_Command_Value("Cancel",handler.Lang))
                }
            };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Lang", handler.Lang);
        }
        public static string _Command_List( Options_Handler handler, out Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardButtons)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup.OneTimeKeyboard = true;
              if (!_Check_Logged_In(handler))
            replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Lang_Select",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Search",handler.Lang))
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Insert",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Update",handler.Lang))
                }
            };
            else
                replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                           {
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Lang_Select",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Search",handler.Lang))
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(_Get_Command_Value("Manage",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Insert",handler.Lang)),
                    new KeyboardButton(_Get_Command_Value("Update",handler.Lang))
                }
                           };
            keyboardButtons = replyKeyboardMarkup;
            return _Get_Command_Value("Welcome", handler.Lang);
        }
        #endregion
        #region Command_Code     
        private static string _Get_Command_Value(string cmd, string lang)
        {
            
            return Command.ResourceManager.GetString(cmd, new CultureInfo(lang));
        }
        private static string _Get_Command_Value(string file, string cmd, string lang)
        {
            switch (file)
            {
                case "Command":
                    return Command.ResourceManager.GetString(cmd, new CultureInfo(lang));
                case "Category":
                    return Property_Category.ResourceManager.GetString(cmd, new CultureInfo(lang));
                case "Type":
                    return Property_Type.ResourceManager.GetString(cmd, new CultureInfo(lang));
            }
              return Command.ResourceManager.GetString(cmd, new CultureInfo(lang));
        }
        private static string _Get_Command(string cmd, string lang)
        {
            var resourceSet = Command.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                string resource = entry.Value.ToString();
                if (resourceKey == cmd)
                    return resourceKey;
                else if (resource == cmd)
                    return resourceKey;
            }
            return "";
        }
        private static List<string> _Get_Command_List(string file, string lang)
        {
            List<string> list = new List<string>();
            ResourceSet resourceSet = null;
            switch (file)
            {
                case "Command":
                    resourceSet = Command.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
                case "Category":
                    resourceSet = Property_Category.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
                case "Type":
                    resourceSet =Property_Type.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
            }

            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                string resource = entry.Value.ToString();
                list.Add(resourceKey);
            }
            return list;
        }
        private static string _Get_Command(string file, string cmd, string lang)
        {
            ResourceSet resourceSet = null;
            switch (file)
            {
                case "Command":
                    resourceSet =Command.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
                case "Category":
                    resourceSet =Property_Category.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
                case "Type":
                    resourceSet =Property_Type.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(lang), true, true);
                    break;
            }
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                string resource = entry.Value.ToString();
                if (resource == cmd)
                {
                    return resourceKey;
                }
            }
            return "";
        }
        private static string _Generate_Random_String(int length)
        {
            Random random = new Random();
            return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                               select s[random.Next(s.Length)]).ToArray());
        }
        public static string _Get_New_Image_Name()
        {
            string text = _Generate_Random_String(15);
            for (int i = 0; i < image_names.Count; i++)
            {
                if (text == image_names[i])
                {
                    text = _Generate_Random_String(15);
                    i = 0;
                }
            }
            image_names.Add(text);
            return text;
        }
        private static void _Insert_Init_Names_List()
        {
            if (image_names.Count == 0)
            {
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
                 var path=AppDomain.CurrentDomain.BaseDirectory + "Property_Images\\";
                FileInfo[] files = new DirectoryInfo(path).GetFiles();
                List<string> list = new List<string>();
                FileInfo[] array = files;
                foreach (FileInfo fileInfo in array)
                {
                    list.Add(fileInfo.Name);
                }
                image_names= list;
            }
        }
        #endregion 
        
    }
    
}