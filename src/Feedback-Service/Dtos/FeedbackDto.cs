namespace Feedback_Service.Dtos;

public record FeedbackDto(
    Guid Id,
    int PatientId,
    Guid AuthorId,
    Guid StressMeassurementId,
    string Comment,
    DateTimeOffset CreatedDate
);