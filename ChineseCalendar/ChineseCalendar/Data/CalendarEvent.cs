using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseCalendar.Data
{
    public class CalendarEvent
    {
        public String Title { get; set; }
        public LunarDate Date { get; set; }
        public bool RepeatYear { get; set; }
        public bool RepeatMonth { get; set; }
        public bool RepeatDay { get; set; }
    }
}
