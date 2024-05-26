using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingoMedia_test.Models
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public bool EmailSent { get; set; } = false;
        [DisplayName("Send Email To")]
        [DataType(DataType.EmailAddress)]
        public string ToEmail { get; set; }
    }
}
