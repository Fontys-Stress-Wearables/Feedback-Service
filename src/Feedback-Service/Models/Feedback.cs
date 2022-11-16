using Feedback_Service.Dtos;

namespace Feedback_Service.Models;

public class Feedback
{
    public Guid Id { get; set; }
    public int PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string FeedbackComment { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }
    public FeedbackDto AsDto()
    {
        return new FeedbackDto(
            Id, PatientId, AuthorId, StressMeassurementId, FeedbackComment, CreatedDate
        );
    }
}