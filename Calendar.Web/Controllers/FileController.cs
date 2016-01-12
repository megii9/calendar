using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Calendar.DataAccess;
using Calendar.Interface;

namespace Calendar.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IEventRepository eventRepository;

        public FileController()
        {
            eventRepository = new EventRepository();
        }


        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Export(int id)
        {
            var evnt = eventRepository.GetById(id);
            var writer = new StringBuilder();
            writer.AppendLine("BEGIN:VCALENDAR");
            writer.AppendLine("VERSION:2.0");
            writer.AppendLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");
            writer.AppendLine("BEGIN:VEVENT");
            var dateStart = evnt.Date + evnt.From;
            writer.AppendLine(string.Format("DTSTART:{0}", dateStart.ToString("yyyyMMddTHHmmssZ")));
            var dateEnd = evnt.Date + evnt.To;
            writer.AppendLine(string.Format("DTEND:{0}", dateEnd.ToString("yyyyMMddTHHmmssZ")));
            writer.AppendLine(string.Format("SUMMARY:{0}", evnt.Body));
            writer.AppendLine("END:VEVENT");
            writer.AppendLine("END:VCALENDAR");

            var fileName = string.Format("event_{0}.ics", id);
            var bytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(bytes, "text/calendar", fileName);
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                using (var reader = new StreamReader(file.InputStream))
                {
                    var text = reader.ReadToEnd();
                    throw new Exception();
                }
            }

            return RedirectToAction("DocumentUploaded");
        }

        public ActionResult DocumentUploaded()
        {
            return View();
        }
    }
}