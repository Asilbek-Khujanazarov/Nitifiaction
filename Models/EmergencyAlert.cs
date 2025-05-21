namespace PatientRecovery.NotificationService.Models
{
    public class EmergencyAlert
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Guid PatientId { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string Priority { get; set; }
        public Guid? DoctorId { get; set; }
    }
}