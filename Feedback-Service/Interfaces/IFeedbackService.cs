
using Feedback_Service.Dtos;
namespace Feedback_Service.Interfaces;

public interface IFeedbackService
{
    public Task<IEnumerable<FeedbackDto>> GetAll();
    public Task<FeedbackDto?> GetFeedbackById(Guid id);
    public Task<IEnumerable<FeedbackDto>> GetPatientFeedbackById(Guid patientId);
    public Task<FeedbackDto?> CreateFeedback(CreateFeedbackDto createFeedbackDto);
    public Task<FeedbackDto?> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto);
    public Task<FeedbackDto?> DeleteFeedback(Guid id);

}