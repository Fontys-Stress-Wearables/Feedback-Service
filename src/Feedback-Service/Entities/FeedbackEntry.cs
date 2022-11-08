namespace Feedback_Service.Entities;

public class FeedbackEntry
{
    public Guid Id { get; set; }
    public int PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string FeedbackComment { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }
}
