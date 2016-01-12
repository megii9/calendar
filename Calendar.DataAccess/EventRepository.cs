using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Calendar.Domain;
using Calendar.Interface;
using Dapper;

namespace Calendar.DataAccess
{
    public class EventRepository : IEventRepository
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Calendar.DB;Integrated Security=True;";
        private string insertStatement = @"
INSERT INTO [dbo].[Events]
([Id],[CategoryId], [CreatedBy],[Date],[TimeFrom],[TimeTo],[IsAllDayEvent],[Subject],[Body])
VALUES(@Id, @CategoryId, @CreatedBy, @Date, @TimeFrom, @TimeTo,@IsAllDayEvent, @Subject, @Body)";

        private string removeStatement = @"
DELETE FROM [dbo].[Events] WHERE Id = @eventId";

        private string selectByIdStatement = @"
SELECT
	Events.Id EventId,
	Events.Subject,
	Events.Body,
	Events.Date,
	Events.TimeFrom,
	Events.TimeTo,
	Events.IsAllDayEvent,
	Users.Id CreatedById,
	Users.Email CreatedByEmail,
	Categories.Id CategoryId,
	Categories.Name CategoryName
FROM Events
INNER JOIN Users ON Events.CreatedBy = Users.Id 
INNER JOIN Categories ON Events.CategoryId = Categories.Id
WHERE Events.Id = @eventId ";

        private string selectByDatesStatement = @"
SELECT
	Events.Id EventId,
	Events.Subject,
	Events.Body,
	Events.Date,
	Events.TimeFrom,
	Events.TimeTo,
	Events.IsAllDayEvent,
	Users.Id CreatedById,
	Users.Email CreatedByEmail,
	Categories.Id CategoryId,
	Categories.Name CategoryName
FROM Events
INNER JOIN Users ON Events.CreatedBy = Users.Id 
INNER JOIN Categories ON Events.CategoryId = Categories.Id
WHERE Events.Date BETWEEN @dateFrom AND @dateTo";

        //public void Create(Event newEvent)
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        var cmd = new SqlCommand(insertStatement, connection);
        //        cmd.Parameters.Add("Id", SqlDbType.Int).Value = newEvent.Id;
        //        cmd.Parameters.Add("CategoryId", SqlDbType.Int).Value = newEvent.Category.Id;
        //        cmd.Parameters.Add("CreatedBy", SqlDbType.Int).Value = newEvent.CreatedBy.Id;
        //        cmd.Parameters.Add("Date", SqlDbType.Date).Value = newEvent.Date;
        //        cmd.Parameters.Add("TimeFrom", SqlDbType.Time).Value = newEvent.From;
        //        cmd.Parameters.Add("TimeTo", SqlDbType.Time).Value = newEvent.To;
        //        cmd.Parameters.Add("IsAllDayEvent", SqlDbType.Bit).Value = newEvent.IsAllDatEvent;
        //        cmd.Parameters.Add("Subject", SqlDbType.NVarChar).Value = newEvent.Subject;
        //        cmd.Parameters.Add("Body", SqlDbType.NVarChar).Value = newEvent.Body;
        //        cmd.ExecuteNonQuery();
        //    }
        //}


        public void Create(Event newEvent)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(insertStatement, new
                {
                    Id = newEvent.Id,
                    CategoryId = newEvent.Category.Id,
                    CreatedBy = newEvent.CreatedBy.Id,
                    Date = newEvent.Date,
                    TimeFrom = newEvent.From,
                    TimeTo = newEvent.To,
                    IsAllDayEvent = newEvent.IsAllDatEvent,
                    Subject = newEvent.Subject,
                    Body = newEvent.Body
                });
            }
        }

        public void Remove(int eventId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(removeStatement, new { eventId });
            }
        }

        public Event GetById(int eventId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.Query(selectByIdStatement, new { eventId })
                    .SingleOrDefault();

                return result == null 
                    ? null 
                    : ResultToEvent(result);
            }
        }

        public IEnumerable<Event> Get(DateTime date, PeriodType periodType)
        {
            var periodDates = CalculateDates(date, periodType);

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query(selectByDatesStatement, new {periodDates.DateFrom, periodDates.DateTo})
                    .Select(ResultToEvent);
            }
        }

        private PeriodDates CalculateDates(DateTime date, PeriodType periodType)
        {
            switch (periodType)
            {
                case PeriodType.Day:
                    return new PeriodDates
                    {
                        DateFrom = date.Date,
                        DateTo = date.Date
                    };
                case PeriodType.WorkingWeek:
                    var dayOfWeek = date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;
                    var daysToMonday = (int)DayOfWeek.Monday - dayOfWeek;
                    var daysToFriday = (int)DayOfWeek.Friday - dayOfWeek;
                    return new PeriodDates
                    {
                        DateFrom = date.AddDays(daysToMonday).Date,
                        DateTo = date.AddDays(daysToFriday).Date
                    };
                case PeriodType.Month:
                    return new PeriodDates
                    {
                        DateFrom = new DateTime(date.Year, date.Month, 1).Date,
                        DateTo = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).Date
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(periodType), periodType, null);
            }
        }

        private class PeriodDates
        {
            internal DateTime DateFrom { get; set; }
            internal DateTime DateTo { get; set; }
        }

        private Event ResultToEvent(dynamic result)
        {
            return new Event
            {
                Id = result.EventId,
                Body = result.Body,
                Subject = result.Subject,
                Date = result.Date,
                From = result.TimeFrom,
                To = result.TimeTo,
                IsAllDatEvent = result.IsAllDayEvent,
                Category = new Category
                {
                    Id = result.CategoryId,
                    Name = result.CategoryName
                },
                CreatedBy = new User
                {
                    Id = result.CreatedById,
                    Email = result.CreatedByEmail
                }
            };
        }
    }
}
