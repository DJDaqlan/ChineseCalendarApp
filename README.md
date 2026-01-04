# ChineseCalendarApp
Uses Visual Studio (Windows) for an app that easily tracks the Lunarian Calendar, usually for Chinese cultures and events

# Overview
This app will display both a standard, Gregorian calendar and a Lunar calendar, which will also provide visual indication to any events that the user has inputted.

## Motivation
A lot of the current Gregorian to Chinese calendars are strictly conversion based, and provide no visual representation of the events that are ahead, or are completely filled to the brim with various events and information that provide more ambiguity than clarity to the Chinese calendar. This app brings in the conversion aspect with a more traditional appearance of a calendar so that users can adapt it however they want and include what they want to it isn't as congested as other websites/applications.

# Requirements
The operation itself does not have any external requirements and are all accessible/installable from the Visual Studio Installer.
## .NET Desktop Development
This was heavily relied upon because of the Windows and Pages that were used to develop this app. Additionally, a lot of resources were used to quickly learn navigating through Pages and the XAML format because it was quite new to me. For more information on coding in XAML, [see here for the Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/xaml/). In terms of navigating the pages and windows, I relied a lot on AI, namely ChatGPT to help understand the syntax.
## ChineseLunarSolarCalendar
This class was heavily relied on for the conversions to and from the Lunar calendar that extended from the DateTime class, including the leap months

# Directory Structure
In terms of structure. I classified it into clear dinstinct areas, of which have been highlighted below.
```
└── 📁ChineseCalendar
    └── 📁Assets
        └── 📁CalendarTemplates
            ├── GregTemplate.png
            ├── LunarTemplate.png
        └── 📁ChineseZodiacs
            ├── Boar.png
            ├── Dog.png
            ├── Dragon.png
            ├── Goat.png
            ├── Horse.png
            ├── Monkey.png
            ├── Ox.png
            ├── Rabbit.png
            ├── Rat.png
            ├── Rooster.png
            ├── Snake.png
            ├── Tiger.png
    └── 📁Data
        ├── CalendarDatabase.cs
        ├── CalendarEvent.cs
        ├── LunarDate.cs
    └── 📁Models
    └── 📁Services
        ├── CalendarService.cs
        ├── DataService.cs
        ├── DateConverterService.cs
    └── 📁Views
        ├── AddChineseEventPage.xaml
        ├── AddChineseEventPage.xaml.cs
        ├── CalendarWindow.xaml
        ├── CalendarWindow.xaml.cs
        ├── ChineseCalendarWindow.xaml
        ├── ChineseCalendarWindow.xaml.cs
        ├── ViewChineseEventPage.xaml
        ├── ViewChineseEventPage.xaml.cs
    ├── App.xaml
    ├── App.xaml.cs
    ├── AssemblyInfo.cs
    ├── ChineseCalendar.csproj
    ├── ChineseCalendar.csproj.user
    ├── MainWindow.xaml
    └── MainWindow.xaml.cs
```
## Assets
This folder holds the non-program related files that were important to the project. Namely the calendar templates and the Zodiac animals.

### Calendar templates
These templates were hand made with the help of [Figma](https://www.figma.com/), which provide simple templates to hold information about the dates. The colours chosen were simplistic so that any additional designs can be made if necessary.

### Zodiac animals
Because the Chinese culture follows the 12 Zodiac animals, I needed to source images that followed the simplistic design of the calendar whilst also being distinct enough to stand out from the calendar and be noticeable. Fortunately, I found images from [Ultimate Kilimanjaro](https://www.ultimatekilimanjaro.com/chinese-zodiac-animals-what-they-are-and-what-they-mean/) that fit my criteria. *Disclaimer: I claim to the best of my knowledge that none of these images were not my own creation, nor were they AI-generated. All credits to these images go to the Ultimate Kilimanjaro website.*

## Data
Initially this folder was intended to save the data, but since this project was strictly designed for Windows OS, there was no distinct way to determine the folder's directory, nor did I find a way to save the data into a relative directory (i.e. this one). Instead, this folder is used to save the classes that would then be converted into JSON files for storage and retrieval.

I decided to use JSON as a noSQL database storage because it was the one that was both human and computer readable. Additionally it can be expanded and modified with high flexibility to suit many different formats.

### Calendar Database
The base format of the Calendar Database contains the following:
- Database ID: The id in case we need to reference various databases
- Calendar Type: To differentiate whether the dates refer to the Gregorian calendar or the Chinese calendar
- Events: A list of the calendar events stored in the database

### Calendar Event
The base format of the calendar events stored within the database. It contains the following:
- Title: The name of the event
- Date: The date of the event in DD/MM/YYYY format
- Repeat Year: Whether the event repeats every year (annually)
- Repeat Month: Whether the event repeats every month (monthly)
- Repeat Day: Whether the event repeats every day (daily)

### Lunar Date
Because the ChineseLunarSolarCalendar class did not have a encapsulated object for their date, this class was used to make up for the flaw. It only includes the year, month and day.

## Services
Includes the classes that were made for various aspects throughout the solution, including conversion between the Gregorian DateTime class and the Lunar ChineseLunarSolarCalendar class, data management and calendar formatting.

## Views
This folder holds all of the windows and pages made for the solution, allowing the user to do the following:
1. View today's event
2. View the Gregorian Calendar, which includes all of the Lunar events (from the database) in the respective gregorian date.
3. View the Chinese Calendar, which includes all of the Lunar events (from the database) in the respective dates
4. View all of the events added to the database, with the option to remove selected events
5. Add an event to the database, which includes all of the necessary information.