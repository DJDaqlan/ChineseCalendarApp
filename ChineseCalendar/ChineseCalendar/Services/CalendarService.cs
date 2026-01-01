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

        ChineseLunisolarCalendar chineseCalendar;
        DateConverterService dateConverter;
        calendarType version;

        int displayedYear;
        int displayedMonth;
        int displayedDay;
        public CalendarService(calendarType version = calendarType.Gregorian)
        {
            chineseCalendar = new ChineseLunisolarCalendar();
            dateConverter = new DateConverterService();
            this.version = version;
            if (version.Equals(calendarType.Gregorian))
            {
                displayedYear = DateTime.Today.Year;
                displayedMonth = DateTime.Today.Month;
                displayedDay = DateTime.Today.Day;
            }
            else if (version.Equals(calendarType.LunarChinese)) {
                displayedYear = chineseCalendar.GetYear(DateTime.Today);
                displayedMonth = chineseCalendar.GetMonth(DateTime.Today);
                displayedDay = chineseCalendar.GetDayOfMonth(DateTime.Today);
            }
        }

        public int GetYear() 
        { 
            return this.displayedYear; 
        }
        public void SetYear(int year) 
        { 
            this.displayedYear = year; 
        }
        public int GetMonth() 
        { 
            return this.displayedMonth; 
        }
        public int GetMonth(String chinese)
        {
            return dateConverter.ChineseToInt(chinese.Substring(0, chinese.Length - 1));
        }
        public void SetMonth(int month) 
        {
            this.displayedMonth = month;
        }

        public int GetDay()
        {
            return this.displayedDay;
        }

        public void SetDay(int day)
        {
            this.displayedDay = day;
        }

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

            if (version.Equals(calendarType.Gregorian))
            {
                int monthInt = date.Month;
                int yearInt = date.Year;
                String monthEng = dateConverter.MonthToString(monthInt);
                month = monthEng;
                year = yearInt.ToString();
            }

            if (version.Equals(calendarType.LunarChinese))
            {
                int monthInt = chineseCalendar.GetMonth(date);
                int yearInt = chineseCalendar.GetYear(date);
                int leapMonth = chineseCalendar.GetLeapMonth(yearInt);

                if (monthInt < leapMonth)
                {
                    String monthEng = dateConverter.MonthToString(monthInt+1);
                    String monthChinese = dateConverter.MonthToChinese(monthInt+1);
                    month = monthChinese + "\n" + monthEng;
                }
                else if (monthInt == leapMonth)
                {
                    String monthEng = dateConverter.MonthToString(monthInt);
                    String monthChinese = dateConverter.MonthToChinese(monthInt);
                    month = '闰' + monthChinese + "\n" + monthEng;
                }
                else // monthInt > leapMonth
                {
                    String monthEng = "";
                    String monthChinese = "";
                    if (chineseCalendar.IsLeapYear(yearInt))
                    {
                        monthEng = dateConverter.MonthToString(monthInt - 12);
                        monthChinese = dateConverter.MonthToChinese(monthInt-1);
                        
                    }
                    else
                    {
                        monthEng = dateConverter.MonthToString(monthInt);
                        monthChinese = dateConverter.MonthToChinese(monthInt);
                    }
                    month = monthChinese;
                }

                year = yearInt.ToString() + "年";
                
            }
            return (year, month, day);
        }

        public (String, String) GetDateStr(int yearInt, int monthInt)
        {
            String month = "";
            String year = "";

            if (version.Equals(calendarType.Gregorian))
            {
                String monthEng = dateConverter.MonthToString(monthInt);
                month = monthEng;
                year = yearInt.ToString();
            }

            if (version.Equals(calendarType.LunarChinese))
            {
                int leapMonth = chineseCalendar.GetLeapMonth(yearInt);

                if (monthInt < leapMonth)
                {
                    String monthEng = dateConverter.MonthToString(monthInt + 1);
                    String monthChinese = dateConverter.MonthToChinese(monthInt);
                    month = monthChinese + "\n" + monthEng;
                }
                else if (monthInt == leapMonth)
                {
                    String monthEng = dateConverter.MonthToString(monthInt);
                    String monthChinese = dateConverter.MonthToChinese(monthInt-1);
                    month = '闰' + monthChinese + "\n" + monthEng;
                }
                else // monthInt > leapMonth
                {
                    String monthEng = "";
                    String monthChinese = "";
                    if (chineseCalendar.IsLeapYear(yearInt))
                    {
                        monthEng = dateConverter.MonthToString(monthInt - 12);
                        monthChinese = dateConverter.MonthToChinese(monthInt - 1);

                    }
                    else
                    {
                        monthEng = dateConverter.MonthToString(monthInt);
                        monthChinese = dateConverter.MonthToChinese(monthInt);
                    }
                    month = monthChinese;
                }

                year = yearInt.ToString() + "年";

            }
            return (year, month);
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

        public String[] GetDayRange(int year, int month)
        {
            int daysNum = this.GetNumDays(year, month);
            String[] days = new String[daysNum];
            for (int d = 0; d < daysNum; d++)
            {
                days[d] = (d + 1).ToString();
            }
            return days;
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
                for (int m = 1; m < monthRange.Length; m++)
                {
                    if (chineseCalendar.IsLeapMonth(year, m))
                    {
                        monthRange[m-1] = "闰" + dateConverter.MonthToChinese(m-1);
                        passedLeapMonth = true;
                    }
                    else if (passedLeapMonth)
                    {
                        monthRange[m-1] = dateConverter.MonthToChinese(m-1);
                    }
                    else
                    {
                        monthRange[m-1] = dateConverter.MonthToChinese(m);
                    }
                }
                monthRange[monthRange.Length-1] = dateConverter.MonthToChinese(12);
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

        /// <summary>
        /// Changes the date values to a desired year and month
        /// </summary>
        /// <param name="year">Year to change it to.</param>
        /// <param name="month">Month to change it to</param>
        /// <returns>The DateTime represented by that specific year and month.</returns>
        public DateTime ChangeDate(int year, int month)
        {
            DateTime newDate = DateTime.Now;
            if (version.Equals(calendarType.Gregorian))
            {
                newDate = new DateTime(year, month, 1);
            }
            return newDate;
        }
    }
}
