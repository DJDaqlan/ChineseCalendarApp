using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChineseCalendar.Services
{
    internal class CalendarService
    {
        public enum calendarType
        {
            Gregorian,
            LunarChinese
        }

        DateTime date;
        ChineseLunisolarCalendar chineseCalendar;
        DateConverterService dateConverter;
        calendarType version;
        public CalendarService(calendarType version = calendarType.Gregorian)
        {
            chineseCalendar = new ChineseLunisolarCalendar();
            dateConverter = new DateConverterService();
            this.version = version;
        }

        public int GetYear(DateTime date) { return date.Year; }

        /// <summary>
        /// Gets the numbers of the year, month and day by the specified calendar type defined in constructor.
        /// </summary>
        /// <param name="date">The date to get the numbers</param>
        /// <returns>A tuple of 3 integers, ordered by year, month and day.</returns>
        public (int, int, int) GetDateNum(DateTime date)
        {
            int month = 0;
            int year = 0;
            int day = 0;
            if (version.Equals(calendarType.LunarChinese))
            {
                month = chineseCalendar.GetMonth(date);
                year = chineseCalendar.GetYear(date);
                day = chineseCalendar.GetDayOfMonth(date);
                int leapMonth = chineseCalendar.GetLeapMonth(year);
                if (month >= leapMonth)
                {
                    month -= 1;
                }
            }
            else if (version.Equals(calendarType.Gregorian))
            {
                month = date.Month;
                year = date.Year;
                day = date.Day;
            }
            return (year, month, day);
        }

        /// <summary>
        /// Gets the name of the year, month and day in its respective culture.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A tuple of 3 strings, ordered by year, month and day.</returns>
        public (String, String, String) GetDateStr(DateTime date)
        {
            String month = "";
            String year = "";
            String day = "";

            (int, int, int) dateNums = this.GetDateNum(date);
            int yearInt = dateNums.Item1;
            int monthInt = dateNums.Item2;
            int dayInt = dateNums.Item3;

            String monthEng = dateConverter.MonthToString(monthInt);
            month = monthEng;
            year = yearInt.ToString();

            if (version.Equals(calendarType.LunarChinese))
            {
                int leapMonth = chineseCalendar.GetLeapMonth(yearInt);
                String monthChinese = dateConverter.MonthToChinese(monthEng);
                month = monthChinese + "\n" + monthEng;

                if (monthInt == leapMonth)
                {
                    month = '闰' + month + " (Leap)";
                }

                year = year + "年";
            }
            return (year, month, day);
        }
        
        /// <summary>
        /// Gets the number of days within a specific month, within a specific year
        /// </summary>
        /// <param name="year">The suggested year</param>
        /// <param name="month">The suggested month</param>
        /// <returns>The number of days alloted in that specific month of that specific year.</returns>
        public int GetNumDays(int year, int month)
        {
            int dayNum = 0;
            if (version.Equals(calendarType.Gregorian))
            {
                if ((month % 2 == 1 && month < 8) || (month % 2 == 0 && month > 7))
                {
                    dayNum = 31;
                }
                else
                {
                    dayNum = 30;
                }
            }
            else if (version.Equals(calendarType.LunarChinese))
            {
                dayNum = chineseCalendar.GetDaysInMonth(year, month);
            }
            return dayNum;
        }
        
        /// <summary>
        /// Gets the first weekday of the month. Since other calendars don't follow this convention, 
        /// this follows the standard Gregorian weekday order
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month</param>
        /// <returns>A DayOfWeek that signifies the first Gregorian day of the week.</returns>
        public DayOfWeek GetFirstDayOfMonth(int year, int month)
        {
            DayOfWeek dayName = DayOfWeek.Sunday;
            if (version.Equals(calendarType.Gregorian))
            {
                dayName = new DateTime(year, month, 1).DayOfWeek;
            }
            else if (version.Equals(calendarType.LunarChinese))
            {
                dayName = chineseCalendar.ToDateTime(year, month, 1, 0, 0, 0, 0).DayOfWeek;
            }
            return dayName;
        }

        /// <summary>
        /// Gets the range of months in the specific year
        /// </summary>
        /// <param name="year">The seleected year</param>
        /// <returns>String array of all available months in the selected year</returns>
        public String[] GetMonthRange(int year)
        {
            String[] monthRange = new string[1];
            List<String> months = Enumerable.Range(1, 12).Select(m => new DateTime(2025, m, 1).ToString("MMMM")).ToList();
            if (version.Equals(calendarType.Gregorian))
            {
                monthRange = months.ToArray();
            }
            else if (version.Equals(calendarType.LunarChinese))
            {
                monthRange = new string[chineseCalendar.IsLeapYear(year) ? 13 : 12];
                bool passedLeapMonth = false;
                for (int m = 0; m < monthRange.Length; m++)
                {
                    if (chineseCalendar.IsLeapMonth(year, m + 1))
                    {
                        monthRange[m] = months[m - 1] + " (Leap Month)";
                        passedLeapMonth = true;
                    }
                    else if (passedLeapMonth)
                    {
                        monthRange[m] = months[m - 1];
                    }
                    else
                    {
                        monthRange[m] = months[m];
                    }
                }
            }

            return monthRange;

        }

        /// <summary>
        /// Gets the range of years, with the selected year in the middle.
        /// </summary>
        /// <param name="year">The center year</param>
        /// <param name="range">The range of years that can be explored.</param>
        /// <returns>An int array with all the years that can be selected.</returns>
        public int[] GetYearRange(int year, int range) 
        {
            int[] yearRange = Enumerable.Range(year - (range / 2), range + 1).ToArray();
            return yearRange;
        }
    }
}
