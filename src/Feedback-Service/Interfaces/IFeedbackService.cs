
using Feedback_Service.Dtos;
namespace Feedback_Service.Interfaces;

public interface IFeedbackService
{
    public Task<IEnumerable<FeedbackDto>> GetAll();
    public Task<FeedbackDto?> GetFeedbackById(Guid Id);
    public Task<IEnumerable<FeedbackDto>> GetPatientFeedbackEntryById(int patientId);
    public Task<FeedbackDto?> CreateFeedback(CreateFeedbackDto createFeedbackDto);
    public Task<FeedbackDto?> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto);
    public Task<FeedbackDto?> DeleteFeedback(Guid id);

}