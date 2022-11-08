using Feedback_Service.Dtos;
using Feedback_Service.Entities;
using Feedback_Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Controllers;

[ApiController]
[Route("feedback")]
public class FeedbackController : ControllerBase
{
    private readonly FeedbackEntriesRepository feedbackEntriesRepository = new();

    //GET /feedback
    // retrieves all feedback entries
    [HttpGet]
    public async Task<IEnumerable<FeedbackEntryDto>> GetAsync()
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetAllAsync())
                                .Select(feedbackEntry => feedbackEntry.AsDto());
        return feedbackEntries;
    }

    //GET /feedback/{id}
    // retrieves a specific feedback
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedbackEntryDto>> GetFeedbackEntryByIdAsync(Guid id)
    {
        var feedbackEntry = await feedbackEntriesRepository.GetAsync(id);

        if (feedbackEntry == null)
        {
            return NotFound();
        }
        return feedbackEntry.AsDto();
    }

    //GET /feedback/patient/{patientId}
    // retrieves all feedback entries based on a patient id
    [HttpGet("patient/{patientId}")]
    public async Task<IEnumerable<FeedbackEntryDto>> GetPatientFeedbackEntryByIdAsync(int patientId)
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetPatientFeedbacksAsync(patientId))
                            .Select(feedbackEntry => feedbackEntry.AsDto());

        System.Diagnostics.Debug.WriteLine(feedbackEntries);
        return feedbackEntries;
    }

    //POST /feedback
    // created a feedback entry
    [HttpPost]
    public async Task<ActionResult<FeedbackEntryDto>> PostAsync(CreateFeedbackEntryDto createFeedbackEntryDto)
    {
        // NEEDS TO BE UPDATED WITH PATIENT DATA
        // Instead of using feedbackEntries can use Patient data to check if the user exist where a feedback has been made for.
        // var existingPatientFeedbackEntry = feedbackEntries.Where(feedbackEntry => feedbackEntry.PatientId == createFeedbackEntryDto.PatientId).SingleOrDefault();

        // if (existingPatientFeedbackEntry == null)
        // {
        //     return NotFound();
        // }

        var feedbackEntry = new FeedbackEntry
        {
            Id = Guid.NewGuid(),
            PatientId = createFeedbackEntryDto.PatientId,
            AuthorId = createFeedbackEntryDto.AuthorId,
            StressMeassurementId = createFeedbackEntryDto.StressMeassurementId,
            FeedbackComment = createFeedbackEntryDto.FeedbackComment,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await feedbackEntriesRepository.CreateAsync(feedbackEntry);

        return CreatedAtAction(nameof(GetFeedbackEntryByIdAsync), new { id = feedbackEntry.Id }, feedbackEntry);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateFeedbackEntryDto updateFeedbackEntryDto)
    {
        var existingfeedbackEntry = await feedbackEntriesRepository.GetAsync(id);

        if (existingfeedbackEntry == null)
        {
            return NotFound();
        }

        existingfeedbackEntry.PatientId = updateFeedbackEntryDto.PatientId;
        existingfeedbackEntry.AuthorId = updateFeedbackEntryDto.AuthorId;
        existingfeedbackEntry.StressMeassurementId = updateFeedbackEntryDto.StressMeassurementId;
        existingfeedbackEntry.FeedbackComment = updateFeedbackEntryDto.FeedbackComment;
        existingfeedbackEntry.CreatedDate = DateTimeOffset.UtcNow;

        await feedbackEntriesRepository.UpdateAsync(existingfeedbackEntry);

        return NoContent();
    }

    //DELETE  /feedback/{id}
    // Removes a feedback entry by id of the entry
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var index = await feedbackEntriesRepository.GetAsync(id);

        if (index == null)
        {
            return NotFound();
        }

        await feedbackEntriesRepository.RemoveAsync(index.Id);

        return NoContent();
    }
}