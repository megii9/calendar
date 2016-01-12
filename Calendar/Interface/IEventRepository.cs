using System;
using System.Collections;
using System.Collections.Generic;
using Calendar.Domain;

namespace Calendar.Interface
{
    public interface IEventRepository
    {
        void Create(Event newEvent);
        void Remove(int eventId);
        Event GetById(int eventId);
        IEnumerable<Event> Get(DateTime date, PeriodType periodType);
    }

    public enum PeriodType
    {
        Day,
        WorkingWeek,
        Month
    }
}
