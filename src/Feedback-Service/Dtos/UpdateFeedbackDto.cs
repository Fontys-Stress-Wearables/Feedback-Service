namespace Feedback_Service.Dtos;

public class UpdateFeedbackDto
{
    public int PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string? FeedbackComment { get; set; }
}