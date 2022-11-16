using Feedback_Service.Dtos;
using Feedback_Service.Entities;
using Feedback_Service.Interfaces;
using Feedback_Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback_Service.Controllers;

[ApiController]
[Route("feedback")]
public class FeedbackController : ControllerBase
{

    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    //GET /feedback
    // retrieves all feedback entries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetAll()
    {
        var feedbacks = await _feedbackService.GetAll();

        return Ok(feedbacks);
    }

    //GET /feedback/{id}
    // retrieves a specific feedback
    [HttpGet("{id}")]
    public async Task<ActionResult<FeedbackDto>> GetFeedbackById(Guid id)
    {
        var feedback = await _feedbackService.GetFeedbackById(id);

        if (feedback == null) return NotFound();

        return Ok(feedback);
    }

    //GET /feedback/patient/{patientId}
    // retrieves all feedback entries based on a patient id
    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetPatientFeedbackById(int patientId)
    {
        var feedback = await _feedbackService.GetPatientFeedbackById(patientId);
        if (feedback == null) return NotFound();

        return Ok(feedback);
    }

    //POST /feedback
    // created a feedback
    [HttpPost]
    public async Task<ActionResult<FeedbackDto>> CreateFeedback(CreateFeedbackDto createFeedbackDto)
    {
        var feedback = await _feedbackService.CreateFeedback(createFeedbackDto);
        if (feedback == null) return BadRequest();

        return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto)
    {
        var feedback = await _feedbackService.UpdateFeedback(id, updateFeedbackDto);

        if (feedback == null) return NotFound();

        return NoContent();
    }

    //DELETE  /feedback/{id}
    // Removes a feedback by id
    [HttpDelete("{id}")]
    public async Task<IActionResult?> DeleteFeedback(Guid id)
    {
        var feedback = await _feedbackService.DeleteFeedback(id);

        if (feedback == null) return NotFound();

        return NoContent();
    }
}