using Feedback_Service.Dtos;
using Feedback_Service.Entities;
using Feedback_Service.Interfaces;
using Feedback_Service.Models;
using Feedback_Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository feedbackRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository)
    {
        this.feedbackRepository = feedbackRepository;
    }

    public async Task<IEnumerable<FeedbackDto>> GetAll()
    {
        var feedbackEntries = (await feedbackRepository.GetAll())
                                .Select(feedback => feedback.AsDto());
        return feedbackEntries;
    }

    public async Task<FeedbackDto?> GetFeedbackById(Guid id)
    {
        var feedback = await feedbackRepository.Get(id);

        if (feedback == null) return null;

        return feedback.AsDto();
    }

    public async Task<IEnumerable<FeedbackDto>> GetPatientFeedbackById(int patientId)
    {
        var feedbackEntries = (await feedbackRepository.GetPatientFeedbacks(patientId))
                            .Select(feedback => feedback.AsDto());

        System.Diagnostics.Debug.WriteLine(feedbackEntries);
        return feedbackEntries;
    }

    public async Task<FeedbackDto?> CreateFeedback(CreateFeedbackDto createFeedbackDto)
    {
        var feedbackEntity = new FeedbackEntity
        {
            PatientId = createFeedbackDto.PatientId,
            AuthorId = createFeedbackDto.AuthorId,
            StressMeassurementId = createFeedbackDto.StressMeassurementId,
            FeedbackComment = createFeedbackDto.FeedbackComment,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await feedbackRepository.Create(feedbackEntity);
        return feedbackEntity.AsDto();
    }

    public async Task<FeedbackDto?> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto)
    {
        var existingFeedback = await feedbackRepository.Get(id);

        if (existingFeedback == null) return null!;

        existingFeedback.PatientId = updateFeedbackDto.PatientId;
        existingFeedback.AuthorId = updateFeedbackDto.AuthorId;
        existingFeedback.StressMeassurementId = updateFeedbackDto.StressMeassurementId;
        existingFeedback.FeedbackComment = updateFeedbackDto.FeedbackComment;
        existingFeedback.CreatedDate = DateTimeOffset.UtcNow;

        await feedbackRepository.Update(existingFeedback);

        return existingFeedback.AsDto();
    }

    public async Task<FeedbackDto?> DeleteFeedback(Guid id)
    {
        var index = await feedbackRepository.Get(id);

        if (index == null) return null!;

        await feedbackRepository.Remove(index.Id);

        return index.AsDto();
    }


}