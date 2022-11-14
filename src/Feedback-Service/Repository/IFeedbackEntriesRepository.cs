using Feedback_Service.Entities;

namespace Feedback_Service.Repository;

public interface IFeedbackEntriesRepository
{
    Task Create(FeedbackEntry entity);
    Task<FeedbackEntry> Get(Guid id);
    Task<IReadOnlyCollection<FeedbackEntry>> GetAll();
    Task<IReadOnlyCollection<FeedbackEntry>> GetPatientFeedbacks(int patientId);
    Task Remove(Guid id);
    Task Update(FeedbackEntry entity);
}
