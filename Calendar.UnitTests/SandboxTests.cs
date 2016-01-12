using System;
using System.Collections.Generic;
using System.Linq;
using Calendar.DataAccess;
using Calendar.Domain;
using Calendar.Implementation;
using Calendar.Interface;
using Calendar.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calendar.UnitTests
{
    [TestClass]
    public class SandboxTests
    {
        [TestMethod]
        public void Send_Email()
        {
            var newEvent = new Event
            {
                Id = 2,
                Subject = "test",
                Body = "body",
                Category = new Category
                {
                    Id = 1
                },
                CreatedBy = new User
                {
                    Id = 1,
                    Email = "noreply@kalendarz.net"
                },
                Date = DateTime.Today,
                From = DateTime.Now.AddHours(-1).TimeOfDay,
                To = DateTime.Now.AddHours(1).TimeOfDay, 
            };

            IUsersRepository userRepository = new FakeUserRepo();
            var mailService = new MailService(userRepository);
            mailService.Send(EventStatus.Removed, newEvent);
        }

        [TestMethod]
        public void Create_Event()
        {
            var repo = new EventRepository();
            var newEvent = new Event
            {
                Id = 2,
                Subject = "test",
                Body = "body",
                Category = new Category
                {
                    Id = 1
                },
                CreatedBy = new User
                {
                    Id = 1,
                    Email = "noreply@kalendarz.net"
                },
                Date = DateTime.Today,
                From = DateTime.Now.AddHours(-1).TimeOfDay,
                To = DateTime.Now.AddHours(1).TimeOfDay,
            };
            repo.Create(newEvent);
        }

        [TestMethod]
        public void Remove_Event_Repo_Only()
        {
            var eventIdToRemove = 1;
            var eventRepository = new EventRepository();
            eventRepository.Remove(eventIdToRemove);
        }
        [TestMethod]
        public void Remove_Event()
        {
            var eventIdToRemove = 2;
            var eventRepository = new EventRepository();
            var userRepository = new FakeUserRepo();
            var mailService = new MailService(userRepository);

            var eventController = new EventController(eventRepository, userRepository, mailService);
            eventController.RemoveEvent(eventIdToRemove);
        }

        [TestMethod]
        public void EventRepo_Range()
        {
            var eventRepository = new EventRepository();
            var events = eventRepository.Get(DateTime.Now, PeriodType.Month);
            Assert.AreEqual(6, events.Count());
        }
    }

    class FakeUserRepo : IUsersRepository
    {
        public IEnumerable<string> GetEmailsByCategory(Category category)
        {
            return new string[]
            {
                "gosia.albertin@gmail.com",
                "megii9@wp.pl"
            };
        }
    }

    class FakeEventRepository : IEventRepository
    {
        public void Create(Event newEvent)
        {
            throw new NotImplementedException();
        }

        public void Remove(int eventId)
        {
        }

        public Event GetById(int eventId)
        {
            var newEvent = new Event
            {
                Id = 2,
                Subject = "test fromt fake db",
                Body = "body",
                Category = new Category
                {
                    Id = 1
                },
                CreatedBy = new User
                {
                    Id = 1,
                    Email = "noreply@kalendarz.net"
                },
                Date = DateTime.Today,
                From = DateTime.Now.AddHours(-1).TimeOfDay,
                To = DateTime.Now.AddHours(1).TimeOfDay,
            };
            return newEvent;
        }

        public IEnumerable<Event> Get(DateTime date, PeriodType periodType)
        {
            throw new NotImplementedException();
        }
    }
}
