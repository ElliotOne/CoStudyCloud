namespace CoStudyCloud.Infrastructure.GoogleCalendar
{
    public class GoogleCalendarSettings : IGoogleCalendarSettings
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string[] Scope { get; set; }
        public required string ApplicationName { get; set; }
        public required string User { get; set; }
        public required string CalendarId { get; set; }
    }
}
