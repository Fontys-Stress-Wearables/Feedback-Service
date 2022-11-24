namespace Feedback_Service.Dtos;

public record FeedbackDto(
    Guid Id,
    Guid PatientId,
    Guid AuthorId,
    Guid StressMeassurementId,
    string? Comment,
    DateTimeOffset CreatedCommentDate,
    DateTimeOffset CreatedStressMeasurementDate
);