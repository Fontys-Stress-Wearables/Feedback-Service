using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Feedback_Service.Dtos;

public class CreateFeedbackDto
{
    public int PatientId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid StressMeassurementId { get; set; }
    public string? FeedbackComment { get; set; }
}

// using System.ComponentModel.DataAnnotations;
// using System.Runtime.InteropServices;

// namespace Feedback_Service.Dtos;

// public record CreateFeedbackDto
// (
//     [Required] int PatientId,
//     [Required] Guid AuthorId,
//     [Optional] Guid? StressMeassurementId,
//     [Optional] string? FeedbackComment
// );