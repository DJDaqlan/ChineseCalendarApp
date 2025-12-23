using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
    }
}
