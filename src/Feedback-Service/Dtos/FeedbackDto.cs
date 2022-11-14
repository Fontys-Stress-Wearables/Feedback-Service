namespace Feedback_Service.Dtos;

public class FeedbackDTO
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string FeedbackComment { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }
}