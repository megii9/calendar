using System;
using System.Web.Http;
using Calendar.DataAccess;
using Calendar.Implementation;
using Calendar.Interface;
using Calendar.Domain;

namespace Calendar.Web.Controllers
{
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        private readonly IEventRepository eventRepository;
        private readonly IMailService mailService;
        private readonly IUsersRepository usersRepository;

        public EventController()
        {
            this.eventRepository = new EventRepository();
            this.usersRepository = new UsersRepository();
            mailService = new MailService(usersRepository);

        }
        public EventController(IEventRepository eventRepository, IUsersRepository usersRepository, IMailService mailService)
        {
            this.usersRepository = usersRepository;
            this.mailService = mailService;
            this.eventRepository = eventRepository;
        }
        

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateEvent(Event newEvent)
        {
            eventRepository.Create(newEvent);
            mailService.Send(EventStatus.New, newEvent);

            return Ok();
        }

        [HttpDelete]
        [Route("")]
        public IHttpActionResult RemoveEvent(int eventId)
        {
            var eventToRemove = eventRepository.GetById(eventId);
            if (eventToRemove != null)
            {
                eventRepository.Remove(eventId);
                mailService.Send(EventStatus.Removed, eventToRemove);
            }
            return Ok();
        }

        [Route("")]
        public IHttpActionResult Get(DateTime date, PeriodType period = PeriodType.Day)
        {
            var events = eventRepository.Get(date, period);
            return Ok(events);
        }
    }
}
