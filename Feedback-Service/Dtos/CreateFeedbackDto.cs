using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Feedback_Service.Dtos;

public record CreateFeedbackDto
(
    [Required] Guid PatientId,
    [Required] Guid AuthorId,
    [Required] Guid StressMeasurementId,
    [Required] DateTimeOffset CreatedStressMeassurementDate,
    [Required] string Comment
);