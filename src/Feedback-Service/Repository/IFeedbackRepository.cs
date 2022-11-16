using Feedback_Service.Entities;

namespace Feedback_Service.Repository;

public interface IFeedbackRepository
{
    Task Create(FeedbackEntity entity);
    Task<FeedbackEntity> Get(Guid id);
    Task<IReadOnlyCollection<FeedbackEntity>> GetAll();
    Task<IReadOnlyCollection<FeedbackEntity>> GetPatientFeedbacks(int patientId);
    Task Remove(Guid id);
    Task Update(FeedbackEntity entity);
}
