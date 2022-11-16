using Feedback_Service.Dtos;

namespace Feedback_Service.Entities;

public class FeedbackEntity
{
    public Guid Id { get; set; }
    public int PatientId { get; set; }
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
