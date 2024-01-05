using AutoMapper;
using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using CoStudyCloud.Core.ViewModels;
using CoStudyCloud.Infrastructure.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CoStudyCloud.Controllers
{
    public class StudySessionsController : Controller
    {
        private readonly IStudySessionRepository _studySessionRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IMapper _mapper;

        public StudySessionsController(
            IStudySessionRepository studySessionRepository,
            IStudyGroupRepository studyGroupRepository,
            IUserRepository userRepository,
            IGoogleCalendarService googleCalendarService,
            IMapper mapper)
        {
            _studySessionRepository = studySessionRepository;
            _studyGroupRepository = studyGroupRepository;
            _userRepository = userRepository;
            _googleCalendarService = googleCalendarService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return BadRequest();
            }

            var sessionsWithGroups =
                await _studySessionRepository.GetStudySessionsWithGroups(user.Id!);

            var studySessionIndexViewModel = new StudySessionIndexViewModel()
            {
                StudySessionWithGroupViewModels = _mapper.Map<
                IEnumerable<StudySessionWithGroup>, IEnumerable<StudySessionWithGroupViewModel>>(
                    sessionsWithGroups)
            };

            return View(studySessionIndexViewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewData[nameof(StudySessionFormViewModel.StudyGroupId)] = await GetStudyGroups();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudySessionFormViewModel studySessionFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData[nameof(StudySessionFormViewModel.StudyGroupId)] = await GetStudyGroups();

                return View(studySessionFormViewModel);
            }

            var accessToken =
                await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access_token");

            if (accessToken != null)
            {
                var emails =
                    (await _studyGroupRepository.GetEmailsInStudyGroup(studySessionFormViewModel.StudyGroupId!))
                    .ToList();

                Event newEvent = new Event()
                {
                    Summary = studySessionFormViewModel.Summary,
                    Description = studySessionFormViewModel.Description,
                    Start = new EventDateTime()
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(studySessionFormViewModel.StartDate.ToUniversalTime()),
                        TimeZone = "Etc/UTC"
                    },
                    End = new EventDateTime()
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(studySessionFormViewModel.EndDate.ToUniversalTime()),
                        TimeZone = "Etc/UTC"
                    },
                    Attendees = emails.Select(email => new EventAttendee { Email = email }).ToArray(),
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {
                            new EventReminder() { Method = "email", Minutes = 24 * 60 },
                            new EventReminder() { Method = "popup", Minutes = 10 },
                        }
                    }
                };

                var calendarEvent =
                    await _googleCalendarService.CreateEventAsync(newEvent, accessToken, CancellationToken.None);

                var studySession = _mapper.Map<StudySessionFormViewModel, StudySession>(studySessionFormViewModel);

                studySession.CalendarSyncId = calendarEvent.Id;
                studySession.CreateDate = DateTime.UtcNow;

                await _studySessionRepository.Add(studySession);
            }

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public async Task<SelectList> GetStudyGroups()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return new SelectList(null);
            }

            var studyGroups = await _studyGroupRepository.GetStudyGroups(user.Id!);

            return new SelectList(
                studyGroups,
                nameof(StudyGroup.Id),
                nameof(StudyGroup.Title));
        }
    }
}
