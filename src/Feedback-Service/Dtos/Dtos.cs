using System.ComponentModel.DataAnnotations;

namespace Feedback_Service.Dtos;
    
// Dto that is returned from GET operations
public record FeedbackEntryDto(Guid Id, int PatientId, Guid AuthorId, Guid StressMeassurementId, string FeedbackComment, DateTimeOffset CreatedDate);

// DTO for creating a feedback entry
public record CreateFeedbackEntryDto([Required] int PatientId, [Required] Guid AuthorId, [Required] Guid StressMeassurementId, string FeedbackComment);

// DTO for updating a feedback entry
public record UpdateFeedbackEntryDto([Required] int PatientId, [Required] Guid AuthorId, [Required] Guid StressMeassurementId, string FeedbackComment);