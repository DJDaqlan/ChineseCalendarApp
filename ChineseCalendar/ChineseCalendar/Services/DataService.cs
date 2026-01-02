using ChineseCalendar.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace ChineseCalendar.Services
{
    internal class DataService
    {
        private static int id = 0;
        String fileName;
        String filePath;
        String missingEventWarning = "No event found!\nCreate one";
        String missingDatabaseWarning = "No database found!\nCreate one";
        public DataService(String fileName, String fileDir)
        {
            this.fileName = fileName;
            Directory.CreateDirectory(fileDir);
            this.filePath = Path.Combine(fileDir, fileName);
        }

        /// <summary>
        /// Creates a database with the skeleton structure.
        /// </summary>
        /// <param name="type">The calendar type.</param>
        public void CreateDatabase(CalendarService.calendarType type = CalendarService.calendarType.LunarChinese)
        {
            if (!File.Exists(filePath))
            {
                CalendarDatabase db = new CalendarDatabase
                {
                    DatabaseId = id,
                    CalendarType = type.ToString(),
                    Events = new List<CalendarEvent>()
                };

                WriteTo(filePath, db);
                MessageBox.Show(this.fileName + " created at " + filePath);

                id += 1;
            }
        }

        /// <summary>
        /// Writes the database to the specified file path into a JSON format.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        /// <param name="database">The Calendar Database to save/write.</param>
        public void WriteTo(String filePath, CalendarDatabase database)
        {
            JsonSerializerOptions dbOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            String json = JsonSerializer.Serialize(database, dbOptions);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Reads from the specified file path from the JSON format.
        /// </summary>
        /// <param name="filePath">File path to read from.</param>
        /// <returns>The database.</returns>
        public CalendarDatabase ReadFrom(String filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            String json = File.ReadAllText(filePath);
            CalendarDatabase db = JsonSerializer.Deserialize<CalendarDatabase>(json);
            return db;
        }

        /// <summary>
        /// Adds an event to the calendar database
        /// </summary>
        /// <param name="calendarEvent">The event to add to the database.</param>
        /// <param name="type">The calendar type.</param>
        public void AddEntry(CalendarEvent calendarEvent, CalendarService.calendarType type)
        {
            CalendarDatabase db;

            if (!File.Exists(filePath))
            {
                CreateDatabase();
            }
            db = ReadFrom(filePath);
            if (db != null)
            {
                if (SearchEntry(db, calendarEvent.Title) == null)
                {
                    db.Events.Add(calendarEvent);
                    WriteTo(filePath, db);
                }
                else
                {
                    MessageBox.Show("Event already exists!\nEdit the entry instead");
                }
            }
        }

        /// <summary>
        /// Searches for the specific event entry, designated by the target title
        /// </summary>
        /// <param name="database">The database to search within.</param>
        /// <param name="targetTitle">The event title to search for.</param>
        /// <returns>The Calendar event, or null to signify no event found</returns>
        public CalendarEvent SearchEntry(CalendarDatabase database, String targetTitle)
        {
            if (!File.Exists(this.filePath) || database == null)
            {
                return null;
            }
            List<CalendarEvent> events = database.Events;
            foreach (CalendarEvent calendarEvent in events)
            {
                if (calendarEvent.Title.Equals(targetTitle))
                {
                    return calendarEvent;
                }
            }
            return null;
        }

        /// <summary>
        /// Edits/replaces the event with a newer event.
        /// </summary>
        /// <param name="oldEventTitle">The old event to remove</param>
        /// <param name="newEvent">The new event to replace the old event with.</param>
        public void EditEntry(String oldEventTitle, CalendarEvent newEvent)
        {
            CalendarDatabase db = ReadFrom(filePath);
            if (db == null)
            {
                MessageBox.Show(missingDatabaseWarning);
                return;
            }
            CalendarEvent calendarEvent = SearchEntry(db, oldEventTitle);
            if (calendarEvent != null)
            {
                db.Events.Remove(calendarEvent);
                db.Events.Add(newEvent);
                WriteTo(filePath, db);
            }
            else
            {
                MessageBox.Show(missingEventWarning);
            }
        }

        /// <summary>
        /// Removes the calendar event from the database.
        /// </summary>
        /// <param name="database">The database in question.</param>
        /// <param name="eventTitle">The event to remove.</param>
        public void RemoveEntry(String eventTitle)
        {
            CalendarDatabase database = ReadFrom(filePath);
            if (!File.Exists(filePath) || database == null)
            {
                MessageBox.Show(missingDatabaseWarning);
                return;
            }
            CalendarEvent calendarEvent = SearchEntry(database, eventTitle);
            if (calendarEvent != null)
            {
                database.Events.Remove(calendarEvent);
                WriteTo(filePath, database);
            }
            else
            {
                MessageBox.Show(missingEventWarning);
            }
        }
    }
}
