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
    
    
    
    // Get Specific Feedback - Happy Flow 
    [Fact]
    public void GetFeedbackById_ReturnsSpecificFeedback()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockService.Setup(service => service.GetFeedbackById(testSessionGuid))
            .ReturnsAsync(GetFeedbackById(testSessionGuid));
        var controller = new FeedbackController(_mockService.Object);
        
        // Act
        var result = controller.GetFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<FeedbackDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<FeedbackDto>(okResult.Value);
        Assert.Equal("Hello World", returnValue!.Comment);
    }
    
    // Get Specific Feedback - Sad Flow 
    [Fact]
    public void GetFeedbackById_ReturnsNoFeedback_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        var mockService = new Mock<IFeedbackService>();
        mockService.Setup(service => service.GetFeedbackById(testSessionGuid))
            .ReturnsAsync((FeedbackDto) null!);
        
        var controller = new FeedbackController(_mockService.Object);
        
        // Act
        var result = controller.GetFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<FeedbackDto>>>(result);
        var actionResult = task.Result;
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }
    
    
    
    // Get All Feedback by specific Patient - Happy Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsAllFeedbackByPatient()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        
        _mockService.Setup(service => service.GetPatientFeedbackById(testSessionGuid))
            .ReturnsAsync(GetPatientFeedbacksById(testSessionGuid));
        
        var controller = new FeedbackController(_mockService.Object);

        // Act
        var result = controller.GetPatientFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<FeedbackDto>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), feedback!.Id);
        Assert.Equal("Hello World", feedback!.Comment);
    }
    
    // All Feedback by specific Patient - Sad Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsNoFeedbackByPatient_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B04DA");
        _mockService.Setup(service => service.GetPatientFeedbackById(testSessionGuid))
            .ReturnsAsync(new List<FeedbackDto>());

        
        var controller = new FeedbackController(_mockService.Object);

        // Act
        var result = controller.GetPatientFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<FeedbackDto>>(actionResult.Value);
        Assert.Empty(returnValue);
    }
    
    
    
    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.CreateFeedback(It.IsAny<CreateFeedbackDto>()))
            .ReturnsAsync(GetFeedbackDto(testSessionGuid));
        
        var controller = new FeedbackController(_mockService.Object);
        CreateFeedbackDto createIssueDto = new CreateFeedbackDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                                                                    "Lorem Ipsum");

        // Act
        var result = controller.CreateFeedback(createIssueDto);
        
        // Assert
        var task = Assert.IsType<Task<ActionResult<FeedbackDto>>>(result);
        var okResult = Assert.IsType<CreatedAtActionResult>(task.Result.Result);
        var feedback = Assert.IsType<FeedbackDto>(okResult.Value);
        Assert.Equal("Lorem Ipsum", feedback.Comment);
    }
    
    // Create a Feedback for specific Patient and checks what occurs when a feedback can not be made- Sad Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback_ReturnsBadRequest_WhenFeedbackCanNotBeMade()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.CreateFeedback(It.IsAny<CreateFeedbackDto>()))
            .ReturnsAsync((FeedbackDto) null!);
        
        var controller = new FeedbackController(_mockService.Object);
        
        CreateFeedbackDto createIssueDto = new CreateFeedbackDto(new Guid(), new Guid(), new Guid(), "");

        // Act
        var result = controller.CreateFeedback(createIssueDto);
        
        // Assert
        var task = Assert.IsType<Task<ActionResult<FeedbackDto>>>(result);
        var actionResult = task.Result;
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    

    // Updates a Feedback for specific Patient - Happy Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.UpdateFeedback(testSessionGuid, It.IsAny<UpdateFeedbackDto>()))
            .ReturnsAsync(GetFeedbackDto(testSessionGuid));
        
        var controller = new FeedbackController(_mockService.Object);
        UpdateFeedbackDto updateFeedbackDto = new UpdateFeedbackDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
            new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
            "Not Lorem Ipsum");
        
        // Act
        var result = controller.UpdateFeedback(testSessionGuid, updateFeedbackDto);
        
        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }
    
    // Updates a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.UpdateFeedback(testSessionGuid, It.IsAny<UpdateFeedbackDto>()))
            .ReturnsAsync((FeedbackDto) null!);
        
        var controller = new FeedbackController(_mockService.Object);
        UpdateFeedbackDto updateFeedbackDto = new UpdateFeedbackDto(new Guid(), new Guid(), new Guid(), 
            "Not Lorem Ipsum");
        
        // Act
        var result = controller.UpdateFeedback(testSessionGuid, updateFeedbackDto);
        
        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
    }

    

    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");
        _mockService.Setup(service => service.DeleteFeedback(testSessionGuid))
            .ReturnsAsync(GetFeedbackDto(testSessionGuid));
        
        var controller = new FeedbackController(_mockService.Object);
        
        // Act
        var result = controller.DeleteFeedback(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<IActionResult?>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }
    
    // Delete a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");

        _mockService.Setup(service => service.DeleteFeedback(testSessionGuid))
            .ReturnsAsync((FeedbackDto) null!);
        
        var controller = new FeedbackController(_mockService.Object);
        
        // Act
        var result = controller.DeleteFeedback(testSessionGuid);
        
        // Assert
        var task = Assert.IsType<Task<IActionResult?>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
    }
    
    private IEnumerable<FeedbackDto> GetPatientFeedbacksById(Guid patientId)
    {
        List<FeedbackDto> feedbackDtos = new List<FeedbackDto>
        {
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"),
                PatientId = patientId, 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
                Comment = "Hello World",
                CreatedDate = DateTimeOffset.Now
            }.AsDto(),
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                PatientId = patientId, 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                Comment = "Lorem Ipsum",
                CreatedDate = DateTimeOffset.Now
            }.AsDto()
        };
        return feedbackDtos;
    }
    
    private FeedbackDto GetFeedbackById(Guid id)
    {
        return new Feedback()
        {
            Id = id,
            PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), 
            AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
            StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
            Comment = "Hello World",
            CreatedDate = DateTimeOffset.Now
        }.AsDto();
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
    
    private FeedbackDto GetFeedbackDto(Guid id)
    {
        return new Feedback()
        {
            Id = id,
            PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
            AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
            StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"),
            Comment = "Lorem Ipsum",
            CreatedDate = DateTimeOffset.Now
        }.AsDto();
    }
}