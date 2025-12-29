using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ChineseCalendar.Services
{
    internal class DateConverterService
    {
        Dictionary<int, String> monthNameDict = new Dictionary<int, String>();

        public DateConverterService()
        {
            // Initialises the dictionary matching number to month name
            String[] monthArray = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < monthArray.Length; i++) {
                monthNameDict[i] = monthArray[i].ToString();
            }
        }

        /// <summary>
        /// Uses the month index to return the shortened month name.
        /// </summary>
        /// <param name="monthNum">The month number.</param>
        /// <returns></returns>
        public String MonthToString(int monthNum)
        {
            while (monthNum > 12)
            {
                monthNum = monthNum - 12;
            }
            while (monthNum <= 0)
            {
                monthNum = monthNum + 12;
            }
            return monthNameDict[monthNum-1].Substring(0, 3);
        }

        /// <summary>
        /// Uses the shortened month name to return the month order index.
        /// </summary>
        /// <param name="monthName">The month name, represented in its 3-character code.</param>
        /// <returns></returns>
        public int MonthToInt(String monthName)
        {
            foreach (int monthInt in monthNameDict.Keys)
            {
                if (monthName.Equals(monthNameDict[monthInt].Substring(0, 3))) {
                    return monthInt;
                }
            }
            return 0;
        }

        /// <summary>
        /// Converts the month number for the months into the chinese characters.
        /// </summary>
        /// <param name="monthNum">The month index, indicating the month order.</param>
        /// <returns></returns>
        public String MonthToChinese(int monthNum)
        {
            String translatedName = "";
            String monthString = monthNum.ToString();
            if (monthString.Length == 1)
            {
                switch (monthString[0])
                {
                    case '1':
                        translatedName += "一";
                        break;
                    case '2':
                        translatedName += "二";
                        break;
                    case '3':
                        translatedName += "三";
                        break;
                    case '4':
                        translatedName += "四";
                        break;
                    case '5':
                        translatedName += "五";
                        break;
                    case '6':
                        translatedName += "六";
                        break;
                    case '7':
                        translatedName += "七";
                        break;
                    case '8':
                        translatedName += "八";
                        break;
                    case '9':
                        translatedName += "九";
                        break;
                    default:
                        return "Error";
                }
            }
            else
            {
                switch (monthString[1])
                {
                    case '0':
                        translatedName += "十";
                        break;
                    case '1':
                        translatedName += "十一";
                        break;
                    case '2':
                        translatedName += "十二";
                        break;
                    default:
                        return "Error!";
                }
            }
            translatedName += "月";
            return translatedName;
        }

        /// <summary>
        /// Converts the Chinese Month name to its corresponding month order
        /// </summary>
        /// <param name="chinese">The chinese characters</param>
        /// <returns>The month order</returns>
        public int ChineseToInt(String chinese)
        {
            int translatedWord = 0;
            String numbers = chinese.Substring(0, chinese.Length - 1);
            if (numbers.Length == 1)
            {
                switch (numbers[0])
                {
                    case '一':
                        translatedWord = 1;
                        break;
                    case '二':
                        translatedWord = 2;
                        break;
                    case '三':
                        translatedWord = 3;
                        break;
                    case '四':
                        translatedWord = 4;
                        break;
                    case '五':
                        translatedWord = 5;
                        break;
                    case '六':
                        translatedWord = 6;
                        break;
                    case '七':
                        translatedWord = 7;
                        break;
                    case '八':
                        translatedWord = 8;
                        break;
                    case '九':
                        translatedWord = 9;
                        break;
                    case '十':
                        translatedWord = 10;
                        break;
                }
            }
            else if (chinese.Length == 2)
            {
                switch(numbers[1])
                {
                    case '一':
                        translatedWord = 11;
                        break;
                    case '二':
                        translatedWord = 12;
                        break;
                }
            }
            return translatedWord;
        }
    }
}
