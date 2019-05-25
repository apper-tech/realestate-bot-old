using Library.Akaratak.DataService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
//using TelegramBot.localhost;

namespace Telegram_Bot.Library
{
    public class Calender
    {
        public static InlineKeyboardMarkup Create_Calender(Options_Handler handler, int year, int month)
        {
            InlineKeyboardButton[] week1 = new InlineKeyboardButton[7];
            InlineKeyboardButton[] week2 = new InlineKeyboardButton[7];
            InlineKeyboardButton[] week3 = new InlineKeyboardButton[7];
            InlineKeyboardButton[] week4 = new InlineKeyboardButton[7];
            InlineKeyboardButton[] extra = new InlineKeyboardButton[7];
            int i = 0;
            int j = 1;
            foreach (DateTime day in EachDay(new DateTime(year, month, 1), new DateTime(year, month + 1, 1)))
            {
                if (j == 1)
                {
                    if (i < 7)
                    {
                        week1[i] = new InlineKeyboardButton { Text = day.Day + "", CallbackData = "calender-day-" + day.Day }; i++;
                    }
                    if (i == 7)
                    {
                        i = 0;
                        j++;
                        continue;
                    }
                }
                if (j == 2)
                {
                    if (i < 7)
                    {
                        week2[i] = new InlineKeyboardButton { Text = day.Day + "", CallbackData = "calender-day-" + day.Day }; i++;
                    }
                    if (i == 7)
                    {
                        i = 0;
                        j++;
                        continue;
                    }
                }
                if (j == 3)
                {
                    if (i < 7)
                    {
                        week3[i] = new InlineKeyboardButton { Text = day.Day + "", CallbackData = "calender-day-" + day.Day }; i++;
                    }
                    if (i == 7)
                    {
                        i = 0;
                        j++;
                        continue;
                    }
                }
                if (j == 4)
                {
                    if (i < 7)
                    {
                        week4[i] = new InlineKeyboardButton { Text = day.Day + "", CallbackData = "calender-day-" + day.Day }; i++;
                    }
                    if (i == 7)
                    {
                        i = 0;
                        j++;
                        continue;
                    }
                }
                if (j > 4)
                {
                    if (i < 6)
                    {
                        if (day.Day != 1)
                            extra[i] = new InlineKeyboardButton { Text = day.Day + "", CallbackData = "calender-day-" + day.Day }; i++;
                    }
                    if (i == 6)
                    {
                        break;
                    }
                }

            }
            for (int l = 0; l < extra.Length; l++)
            {
                extra[l] = extra[l] == null ? new InlineKeyboardButton { Text = " ", CallbackData = "ignore" } : extra[l];
            }
            var btnarray = new InlineKeyboardButton[][]
                  {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton{ Text=new DateTime(year, month, 1).ToString("MMM", CultureInfo.InvariantCulture)+" "+year,CallbackData="ignore"}
                },
                  new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton{Text="S" },
                    new InlineKeyboardButton{Text="U" },
                    new InlineKeyboardButton{Text="M" },
                    new InlineKeyboardButton{Text="T" },
                    new InlineKeyboardButton{Text="W" },
                    new InlineKeyboardButton{Text="R" },
                    new InlineKeyboardButton{Text="F" }
                },
                  week1,
                  week2,
                  week3,
                  week4,
                  extra
                  ,
                   new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton{Text="<",CallbackData="previous-month"},
                    new InlineKeyboardButton{Text=" ",CallbackData="ignore"},
                    new InlineKeyboardButton{Text=">",CallbackData="next-month"},
                }
                  };
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(btnarray);
            return markup;
        }
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

    }
}