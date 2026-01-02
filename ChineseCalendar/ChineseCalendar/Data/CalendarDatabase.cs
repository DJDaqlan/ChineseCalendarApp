using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseCalendar.Data
{
    public class CalendarDatabase
    {
        public int DatabaseId { get; set; }
        public String CalendarType { get; set; }
        public List<CalendarEvent> Events { get; set; }
    }
}
