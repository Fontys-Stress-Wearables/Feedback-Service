using Feedback_Service.Models;

namespace Feedback_Service.Interfaces;

public interface IFeedbackService
{
    // (Guid Id, Guid PatientId, Guid AuthorId, Guid StressMeassurementId, string FeedbackComment, DateTimeOffset CreatedDate)
    public IEnumerable<Feedback> GetAll();

    public Feedback GetFeedback(Guid Id, Guid PatientId, Guid AuthorId, Guid StressMeassurementId, string FeedbackComment, DateTimeOffset CreatedDate);

    // public Feedback CreateFeedbackEntry(Guid PatientId, Guid StressMeassurementId, string FeedbackComment);
    public Feedback CreateFeedbackEntryDto(Guid PatientId, Guid AuthorId, Guid StressMeassurementId, string FeedbackComment);

    public Feedback UpdateFeedbackEntry(Guid PatientId, Guid AuthorId, Guid StressMeassurementId, string FeedbackComment);
    // public Feedback UpdateFeedbackEntry(Guid PatientId, Guid StressMeassurementId, string FeedbackComment);

    public void RemoveFeedbackEntry(Guid Id);

}