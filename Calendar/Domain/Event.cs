using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public User CreatedBy { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public DateTime Date { get; set; }
        public bool IsAllDatEvent { get; set; }
        public Category Category { get; set; }
    }
}
