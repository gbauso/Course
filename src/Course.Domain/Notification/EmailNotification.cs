using Domain;

namespace Domain
{
    public class EmailNotification : Notification
    {
        public EmailNotification()
        {
            To = "student@email.com";
        }
        public EmailNotification(string to)
        {
            To = to;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
