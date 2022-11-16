using Feedback_Service.Dtos;
using Feedback_Service.Entities;
using Feedback_Service.Interfaces;
using Feedback_Service.Models;
using Feedback_Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackEntriesRepository feedbackEntriesRepository;

    public FeedbackService(IFeedbackEntriesRepository feedbackEntriesRepository)
    {
        this.feedbackEntriesRepository = feedbackEntriesRepository;
    }

    public async Task<IEnumerable<FeedbackDto>> GetAll()
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetAll())
                                .Select(feedbackEntry => feedbackEntry.AsDto());
        return feedbackEntries;
    }

    public async Task<FeedbackDto?> GetFeedbackById(Guid id)
    {
        var feedbackEntry = await feedbackEntriesRepository.Get(id);

        if (feedbackEntry == null) return null;

        return feedbackEntry.AsDto();
    }

    public async Task<IEnumerable<FeedbackDto>> GetPatientFeedbackEntryById(int patientId)
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetPatientFeedbacks(patientId))
                            .Select(feedbackEntry => feedbackEntry.AsDto());

        System.Diagnostics.Debug.WriteLine(feedbackEntries);
        return feedbackEntries;
    }

    public async Task<FeedbackDto?> CreateFeedback(CreateFeedbackDto createFeedbackDto)
    {
        var feedbackEntry = new FeedbackEntry
        {
            PatientId = createFeedbackDto.PatientId,
            AuthorId = createFeedbackDto.AuthorId,
            StressMeassurementId = createFeedbackDto.StressMeassurementId,
            FeedbackComment = createFeedbackDto.FeedbackComment,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await feedbackEntriesRepository.Create(feedbackEntry);
        return feedbackEntry.AsDto();
    }

    public async Task<FeedbackDto?> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto)
    {
        var existingfeedbackEntry = await feedbackEntriesRepository.Get(id);

        if (existingfeedbackEntry == null) return null!;

        existingfeedbackEntry.PatientId = updateFeedbackDto.PatientId;
        existingfeedbackEntry.AuthorId = updateFeedbackDto.AuthorId;
        existingfeedbackEntry.StressMeassurementId = updateFeedbackDto.StressMeassurementId;
        existingfeedbackEntry.FeedbackComment = updateFeedbackDto.FeedbackComment;
        existingfeedbackEntry.CreatedDate = DateTimeOffset.UtcNow;

        await feedbackEntriesRepository.Update(existingfeedbackEntry);

        return existingfeedbackEntry.AsDto();
    }

    public async Task<FeedbackDto?> DeleteFeedback(Guid id)
    {
        var index = await feedbackEntriesRepository.Get(id);

        if (index == null) return null!;

        await feedbackEntriesRepository.Remove(index.Id);

        return index.AsDto();
    }


}