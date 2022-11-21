using System.Diagnostics;
using Feedback_Service.Dtos;
using Feedback_Service.Interfaces;
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
    // retrieves all feedbacks
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
    // retrieves all feedbacks from a specific patient based on their patient id
    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetPatientFeedbackById(Guid patientId)
    {
        var feedback = await _feedbackService.GetPatientFeedbackById(patientId);
        
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

    //PUT /feedback
    // updates a feedback
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFeedback(Guid id, UpdateFeedbackDto updateFeedbackDto)
    {
        var feedback = await _feedbackService.UpdateFeedback(id, updateFeedbackDto);

        if (feedback == null) return NotFound();

        return NoContent();
    }

    //DELETE  /feedback/{id}
    // Removes a feedback
    [HttpDelete("{id}")]
    public async Task<IActionResult?> DeleteFeedback(Guid id)
    {
        var feedback = await _feedbackService.DeleteFeedback(id);

        if (feedback == null) return NotFound();

        return NoContent();
    }
}