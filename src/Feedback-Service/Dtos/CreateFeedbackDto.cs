namespace Feedback_Service.Dtos;

public class CreateFeedbackDTO
{
    public Guid PatientId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string Feedback { get; set; } = "";
}