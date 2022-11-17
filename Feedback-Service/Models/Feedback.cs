using Feedback_Service.Dtos;

namespace Feedback_Service.Models;

public class Feedback
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeasurementId { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public FeedbackDto AsDto()
    {
        return new FeedbackDto(
            Id, PatientId, AuthorId, StressMeasurementId, Comment, CreatedDate
        );
    }
}
