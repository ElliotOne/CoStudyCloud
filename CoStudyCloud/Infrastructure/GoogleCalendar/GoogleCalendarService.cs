using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace CoStudyCloud.Infrastructure.GoogleCalendar
{
    /// <summary>
    /// Represents the default implementation of Google Calendar Service
    /// </summary>
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly IGoogleCalendarSettings _settings;

        public GoogleCalendarService(IGoogleCalendarSettings settings)
        {
            _settings = settings;
        }

        public async Task<Event> CreateEventAsync(Event request, string accessToken, CancellationToken cancellationToken)
        {
            var calendarService = GetCalendarService(accessToken);
            var eventRequest = calendarService.Events.Insert(request, _settings.CalendarId);
            var createdEvent = await eventRequest.ExecuteAsync(cancellationToken);
            return createdEvent;
        }

        public async Task<bool> DeleteEventAsync(string eventId, string accessToken)
        {
            var calendarService = GetCalendarService(accessToken);
            var eventRequest = calendarService.Events.Delete(_settings.CalendarId, eventId);
            var result = await eventRequest.ExecuteAsync(new CancellationToken());
            return string.IsNullOrEmpty(result);
        }

        private CalendarService GetCalendarService(string accessToken)
        {
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(accessToken),
                ApplicationName = _settings.ApplicationName,
            });
        }
    }
}
