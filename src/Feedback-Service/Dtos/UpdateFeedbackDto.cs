namespace Feedback_Service.Dtos;

public class UpdateFeedbackDTO
{
    public Guid PatientId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string Feedback { get; set; } = "";
}