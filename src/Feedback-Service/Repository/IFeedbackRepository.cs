using Feedback_Service.Models;

namespace Feedback_Service.Repository;

public interface IFeedbackRepository
{
    Task<IReadOnlyCollection<Feedback>> GetAll();
    Task<Feedback> Get(Guid id);
    Task<IReadOnlyCollection<Feedback>> GetPatientFeedbacks(int patientId);
    Task Create(Feedback entity);
    Task Update(Feedback entity);
    Task Remove(Guid id);
}
