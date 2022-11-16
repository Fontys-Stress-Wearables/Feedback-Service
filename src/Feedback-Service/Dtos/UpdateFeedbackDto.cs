using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Feedback_Service.Dtos;

public record UpdateFeedbackDto
(
    [Required] int PatientId,
    [Required] Guid AuthorId,
    [Required] Guid StressMeasurementId,
    [Optional] string? Comment
);