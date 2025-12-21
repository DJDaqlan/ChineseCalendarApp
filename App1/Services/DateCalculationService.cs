using System.Globalization;

public class CalendarCalculator
{
    public DateTime ConvertWesternToChinese(DateTime date)
    {
        ChineseLunisolarCalendar chineseDate = new ChineseLunisolarCalendar();
        int era = chineseDate.GetEra(DateTime.Now);
        int hour = chineseDate.GetHour(DateTime.Now);
        int month = chineseDate.GetMonth(DateTime.Now);
        int year = chineseDate.GetYear(DateTime.Now);
        int day = chineseDate.GetDayOfMonth(DateTime.Now);
        DateTime convertedDate = chineseDate.ToDateTime(year, month, day, hour, 0, 0, 0);
        return convertedDate;
    }
}