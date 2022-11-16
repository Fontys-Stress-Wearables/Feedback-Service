using Feedback_Service.Entities;

namespace Feedback_Service.Repository;

public interface IFeedbackRepository
{
    Task<IReadOnlyCollection<FeedbackEntity>> GetAll();
    Task<FeedbackEntity> Get(Guid id);
    Task<IReadOnlyCollection<FeedbackEntity>> GetPatientFeedbacks(int patientId);
    Task Create(FeedbackEntity entity);
    Task Update(FeedbackEntity entity);
    Task Remove(Guid id);
}
