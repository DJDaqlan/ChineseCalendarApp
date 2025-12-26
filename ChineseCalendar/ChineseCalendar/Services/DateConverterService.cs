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
            String[] monthArray = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < monthArray.Length; i++) {
                monthNameDict[i] = monthArray[i].ToString();
            }
            
        }

        public String MonthToString(int monthNum)
        {
            return monthNameDict[monthNum-1].Substring(0, 3);
        }

        public int MonthToInt(String monthName)
        {
            foreach (int monthInt in monthNameDict.Keys)
            {
                if (monthName.Equals(monthNameDict[monthInt].Substring(0, 3))) {
                    return monthInt+1;
                }
            }
            return 0;
        }

        public String MonthToChinese(String english)
        {
            String translatedName = "";
            int monthInt = this.MonthToInt(english);
            String monthString = monthInt.ToString();
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
                switch (monthInt.ToString()[1])
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
            translatedName += "月\n" + english;
            return translatedName;
        }
    }
}
