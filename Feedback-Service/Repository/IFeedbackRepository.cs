using System.Linq.Expressions;
using Feedback_Service.Models;

namespace Feedback_Service.Repository;

public interface IFeedbackRepository
{
    Task<IReadOnlyCollection<Feedback>> GetAll();
    Task<IReadOnlyCollection<Feedback>> GetAll(Expression<Func<Feedback, bool>> filter);
    Task<Feedback> Get(Guid id);
    Task<IReadOnlyCollection<Feedback>> GetPatientFeedbacks(Guid patientId);
    Task Create(Feedback entity);
    Task Update(Feedback entity);
    Task Remove(Guid id);
}
