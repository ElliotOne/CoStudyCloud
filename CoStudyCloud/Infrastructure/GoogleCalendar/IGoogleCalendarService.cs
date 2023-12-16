using Google.Apis.Calendar.v3.Data;

namespace CoStudyCloud.Infrastructure.GoogleCalendar
{
    /// <summary>
    /// Represents Google Calendar Service interface
    /// </summary>
    public interface IGoogleCalendarService
    {
        Task<Event> CreateEventAsync(Event request, string accessToken, CancellationToken cancellationToken);
        Task<bool> DeleteEventAsync(string eventId, string accessToken);
    }
}
