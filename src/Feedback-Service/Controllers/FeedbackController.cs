using Feedback_Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Controllers;

[ApiController]
[Route("feedback")]
public class FeedbackController : ControllerBase
{
    private static readonly List<FeedbackEntryDto> feedbackEntries = new()
    {
        new FeedbackEntryDto(Guid.NewGuid(), 1, Guid.NewGuid(), Guid.NewGuid(), "Patient was stressed", DateTimeOffset.UtcNow),
        new FeedbackEntryDto(Guid.NewGuid(), 2, Guid.NewGuid(), Guid.NewGuid(), "Patient was stressed from a scary movie", DateTimeOffset.UtcNow),
        new FeedbackEntryDto(Guid.NewGuid(), 3, Guid.NewGuid(), Guid.NewGuid(), "Patient slipped and fell", DateTimeOffset.UtcNow)
    };

    //GET /feedback
    // retrieves all feedback entries
    [HttpGet]
    public IEnumerable<FeedbackEntryDto> Get()
    {
        return feedbackEntries;
    }

    //GET /feedback/{id}
    // retrieves a specific feedback
    [HttpGet("{id}")]
    public ActionResult<FeedbackEntryDto> GetById(Guid id)
    {
        var feedbackEntry = feedbackEntries.Where(feedbackEntry => feedbackEntry.Id == id).SingleOrDefault();

        if (feedbackEntry == null)
        {
            return NotFound();
        }
        return feedbackEntry;
    }

    //GET /feedback/patient/{patientId}
    // retrieves all feedback entries based on a patient id
    [HttpGet("patient/{patientId}")]
    public IEnumerable<FeedbackEntryDto> GetFeedbackByPatienId(int patientId)
    {
        var feedbackEntry = feedbackEntries.Where(feedbackEntry => feedbackEntry.PatientId == patientId);
        System.Diagnostics.Debug.WriteLine(feedbackEntry);
        return feedbackEntry;
    }

    //POST /feedback
    // created a feedback entry
    [HttpPost]
    public ActionResult<FeedbackEntryDto> Post(CreateFeedbackEntryDto createFeedbackEntryDto)
    {
        // NEEDS TO BE UPDATED WITH PATIENT DATA
        // Instead of using feedbackEntries can use Patient data to check if the user exist where a feedback has been made for.
        // var existingPatientFeedbackEntry = feedbackEntries.Where(feedbackEntry => feedbackEntry.PatientId == createFeedbackEntryDto.PatientId).SingleOrDefault();

        // if (existingPatientFeedbackEntry == null)
        // {
        //     return NotFound();
        // }

        var feedbackEntry = new FeedbackEntryDto(Guid.NewGuid(), createFeedbackEntryDto.PatientId, createFeedbackEntryDto.AuthorId, createFeedbackEntryDto.StressMeassurementId, createFeedbackEntryDto.FeedbackComment, DateTimeOffset.UtcNow);
        feedbackEntries.Add(feedbackEntry);

        return CreatedAtAction(nameof(GetFeedbackByPatienId), new { patientId = feedbackEntry.PatientId }, feedbackEntry);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateFeedbackEntryDto updateFeedbackEntryDto)
    {
        var existingFeedbackEntry = feedbackEntries.Where(feedbackEntry => feedbackEntry.Id == id).SingleOrDefault();
        if (existingFeedbackEntry == null)
        {
            return NotFound();
        }

        var updatedFeedbackEntry = existingFeedbackEntry with
        {
            PatientId = updateFeedbackEntryDto.PatientId,
            AuthorId = updateFeedbackEntryDto.AuthorId,
            StressMeassurementId = updateFeedbackEntryDto.StressMeassurementId,
            FeedbackComment = updateFeedbackEntryDto.FeedbackComment,
        };

        var index = feedbackEntries.FindIndex(existingFeedbackEntry => existingFeedbackEntry.Id == id);
        feedbackEntries[index] = updatedFeedbackEntry;

        return NoContent();
    }

    //DELETE  /feedback/{id}
    // Removes a feedback entry by id of the entry
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = feedbackEntries.FindIndex(existingFeedbackEntry => existingFeedbackEntry.Id == id);
        
        if (index < 0)
        {
            return NotFound();
        }

        feedbackEntries.RemoveAt(index);

        return NoContent();
    }
}