using Feedback_Service.Controllers;
using Feedback_Service.Dtos;
using Feedback_Service.Interfaces;
using Feedback_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controller;

public class FeedbackControllerTests
{
    private readonly Mock<IFeedbackService> _mockService;
    
    public FeedbackControllerTests()
    {
        _mockService = new Mock<IFeedbackService>();
    }
    
    // Get All - Happy Flow 
    [Fact]
    public void GetAll_ReturnsAllFeedback()
    {
        // Arrange
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(GetFeedbackDtos());
        var controller = new FeedbackController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<FeedbackDto>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"), feedback!.Id);
        Assert.Equal("Hello World", feedback!.Comment);
    }
    
    // Get All - Sad Flow 
    [Fact]
    public void GetAll_ReturnsNoFeedback_WhenNoFeedbackFound()
    {
        // Arrange
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(new List<FeedbackDto>());
        var controller = new FeedbackController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<FeedbackDto>>(okResult.Value);
        Assert.Empty(returnValue);
    }
    
    private IEnumerable<FeedbackDto> GetFeedbackDtos()
    {
        List<FeedbackDto> feedbackDtos = new List<FeedbackDto>
        {
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
                PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
                Comment = "Hello World",
                CreatedDate = DateTimeOffset.Now
            }.AsDto(),
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0ABC"),
                PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                Comment = "Lorem Ipsum",
                CreatedDate = DateTimeOffset.Now
            }.AsDto()
        };

        return feedbackDtos;
    }
}