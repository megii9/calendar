using System;
using System.Net.Mail;
using Calendar.Domain;
using Calendar.Interface;

namespace Calendar.Implementation
{
    public class MailService : IMailService
    {
        private readonly IUsersRepository userRepository;

        public MailService(IUsersRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Send(EventStatus status, Event @event)
        { 
            var message = CreateMailMessage(status, @event);
            SetRecipients(message, @event);
            Send(message);
        }



        private static void Send(MailMessage message)
        {
            var client = new SmtpClient();
            client.Send(message);
        }

        private MailMessage CreateMailMessage(EventStatus status, Event @event)
        {
            var message = new MailMessage
            {
                Subject = PrepareSubject(status),
                Body = PrepareBodyAsHtml(@event),
                From = PrepareSender(@event),
                IsBodyHtml = true
            };
            return message;
        }

        private void SetRecipients(MailMessage message, Event @event)
        {
            var recipients = userRepository.GetEmailsByCategory(@event.Category);
            foreach (var email in recipients)
            {
                message.To.Add(new MailAddress(email));
            }
        }
        private MailAddress PrepareSender(Event @event)
        {
            return new MailAddress(@event.CreatedBy.Email);
        }

        private string PrepareBodyAsHtml(Event @event)
        {
            return string.Format("<h2>{0}</h2><p>{1}</p>",
                @event.Subject, @event.Body);
        }

        private string PrepareSubject(EventStatus status)
        {
            //return String.Format("{0} termin", status.ToString());
            switch (status)
            {
                case EventStatus.New:
                    return "Dodano nowy termin";
                case EventStatus.Updated:
                    return "Edytowano termin";
                case EventStatus.Removed:
                    return "Usunięto termin";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
