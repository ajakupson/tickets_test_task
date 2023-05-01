using System.ComponentModel.DataAnnotations;

namespace agileworks_1.Models
{
    public class Ticket
    {
        public Ticket() { }

        public Ticket(int id, string creationDate, string completionDate, string subject, string content, bool isClosed, int priority)
        {
            Id = id;
            CreationDate = creationDate;
            CompletionDate = completionDate;
            Subject = subject;
            Content = content;
            IsClosed = isClosed;
            Priority = priority;
        }

        public Ticket(string creationDate, string completionDate, string subject, string content, int priority)
        {
            CreationDate = creationDate;
            CompletionDate = completionDate;
            Subject = subject;
            Content = content;
            Priority = priority;
        }

        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^(\d{2})/(\d{2})/(\d{4}) (\d{2}):(\d{2}):(\d{2})")]
        public string CreationDate { get; set; }

        [RegularExpression(@"^(\d{2})/(\d{2})/(\d{4}) (\d{2}):(\d{2}):(\d{2})")]
        public string CompletionDate { get; set; }

        [Required]
        [MinLength(5)]
        public string Subject { get; set; }

        [Required]
        [MinLength(10)]
        public string Content { get; set; }

        public bool IsClosed { get; set; }

        public int Priority { get; set; }
    }
}
