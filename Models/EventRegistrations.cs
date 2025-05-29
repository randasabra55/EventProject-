namespace EventProject.Models
{
    public class EventRegistrations
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationAt { get; set; }

    }
}
