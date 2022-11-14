using Feedback_Service.Dtos;
using Feedback_Service.Entities;
using Feedback_Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Controllers;

[ApiController]
[Route("feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackEntriesRepository feedbackEntriesRepository;

    public FeedbackController(IFeedbackEntriesRepository feedbackEntriesRepository)
    {
        this.feedbackEntriesRepository = feedbackEntriesRepository;
    }

    //GET /feedback
    // retrieves all feedback entries
    [HttpGet]
    public async Task<IEnumerable<FeedbackEntryDto>> Get()
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetAll())
                                .Select(feedbackEntry => feedbackEntry.AsDto());
        return feedbackEntries;
    }

    //GET /feedback/{id}
    // retrieves a specific feedback
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedbackEntryDto>> GetFeedbackEntryById(Guid id)
    {
        var feedbackEntry = await feedbackEntriesRepository.Get(id);

        if (feedbackEntry == null)
        {
            return NotFound();
        }
        return feedbackEntry.AsDto();
    }

    //GET /feedback/patient/{patientId}
    // retrieves all feedback entries based on a patient id
    [HttpGet("patient/{patientId}")]
    public async Task<IEnumerable<FeedbackEntryDto>> GetPatientFeedbackEntryById(int patientId)
    {
        var feedbackEntries = (await feedbackEntriesRepository.GetPatientFeedbacks(patientId))
                            .Select(feedbackEntry => feedbackEntry.AsDto());

        System.Diagnostics.Debug.WriteLine(feedbackEntries);
        return feedbackEntries;
    }

    //POST /feedback
    // created a feedback entry
    [HttpPost]
    public async Task<ActionResult<FeedbackEntryDto>> Post(CreateFeedbackEntryDto createFeedbackEntryDto)
    {
        var feedbackEntry = new FeedbackEntry
        {
            // Id = Guid.NewGuid(),
            PatientId = createFeedbackEntryDto.PatientId,
            AuthorId = createFeedbackEntryDto.AuthorId,
            StressMeassurementId = createFeedbackEntryDto.StressMeassurementId,
            FeedbackComment = createFeedbackEntryDto.FeedbackComment,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await feedbackEntriesRepository.Create(feedbackEntry);

        return CreatedAtAction(nameof(GetFeedbackEntryById), new { id = feedbackEntry.Id }, feedbackEntry);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, UpdateFeedbackEntryDto updateFeedbackEntryDto)
    {
        var existingfeedbackEntry = await feedbackEntriesRepository.Get(id);

        if (existingfeedbackEntry == null)
        {
            return NotFound();
        }

        existingfeedbackEntry.PatientId = updateFeedbackEntryDto.PatientId;
        existingfeedbackEntry.AuthorId = updateFeedbackEntryDto.AuthorId;
        existingfeedbackEntry.StressMeassurementId = updateFeedbackEntryDto.StressMeassurementId;
        existingfeedbackEntry.FeedbackComment = updateFeedbackEntryDto.FeedbackComment;
        existingfeedbackEntry.CreatedDate = DateTimeOffset.UtcNow;

        await feedbackEntriesRepository.Update(existingfeedbackEntry);

        return NoContent();
    }

    //DELETE  /feedback/{id}
    // Removes a feedback entry by id of the entry
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var index = await feedbackEntriesRepository.Get(id);

        if (index == null)
        {
            return NotFound();
        }

        await feedbackEntriesRepository.Remove(index.Id);

        return NoContent();
    }
}