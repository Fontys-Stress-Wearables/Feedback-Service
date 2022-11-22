using Feedback_Service.Dtos;
using Feedback_Service.Interfaces;
using Feedback_Service.Models;
using Feedback_Service.Repository;
using Feedback_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Tests.Service;

public class FeedbackServiceTests
{
    private readonly Mock<IFeedbackRepository> _mockRepository;
    public FeedbackServiceTests()
    {
        _mockRepository = new Mock<IFeedbackRepository>();
    }
    
    // Get All - Happy Flow 
    [Fact]
    public void GetAll_ReturnsAllFeedback()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAll())
            .ReturnsAsync(GetFeedbacks());
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetAll();

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        var returnValue = task.Result;
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal("Hello World", feedback!.Comment);
    }
    
    // // Get All - Sad Flow 
    // [Fact]
    // public void GetAll_ReturnsNoFeedback_WhenNoFeedbackFound()
    // {
    //     // Arrange
    //     _mockRepository.Setup(service => service.GetAll())
    //         .ReturnsAsync(new List<FeedbackDto>());
    //     var controller = new FeedbackController(_mockService.Object);
    //
    //     // Act
    //     var result = controller.GetAll();
    //
    //     // Assert
    //     var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
    //     var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
    //     var returnValue = Assert.IsType<List<FeedbackDto>>(okResult.Value);
    //     Assert.Empty(returnValue);
    // }
    
    
    
    // Get Specific Feedback - Happy Flow 
    [Fact]
    public void GetFeedbackById_ReturnsSpecificFeedback()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockRepository.Setup(repo => repo.Get(testSessionGuid))
            .ReturnsAsync(GetFeedbackById(testSessionGuid));
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        var feedback = Assert.IsType<FeedbackDto>(task.Result);
        Assert.Equal("Hello World", feedback!.Comment);
    }
    
    // Get Specific Feedback - Sad Flow 
    [Fact]
    public void GetFeedbackById_ReturnsNoFeedback_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        
        _mockRepository.Setup(repo => repo.Get(testSessionGuid))
            .ReturnsAsync((Feedback)null!);
        
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        Assert.Null(task.Result);
    }
    
    
    
    // // Get All Feedback by specific Patient - Happy Flow 
    // [Fact]
    // public void GetPatientFeedbackById_ReturnsAllFeedbackByPatient()
    // {
    //     // Arrange
    //     Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
    //     
    //     _mockService.Setup(service => service.GetPatientFeedbackById(testSessionGuid))
    //         .ReturnsAsync(GetPatientFeedbacksById(testSessionGuid));
    //     
    //     var controller = new FeedbackController(_mockService.Object);
    //
    //     // Act
    //     var result = controller.GetPatientFeedbackById(testSessionGuid);
    //
    //     // Assert
    //     var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
    //     var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
    //     var returnValue = Assert.IsType<List<FeedbackDto>>(okResult.Value);
    //     Assert.NotEmpty(returnValue);
    //     var feedback = returnValue.FirstOrDefault();
    //     Assert.Equal(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), feedback!.Id);
    //     Assert.Equal("Hello World", feedback!.Comment);
    // }
    //
    // // All Feedback by specific Patient - Sad Flow 
    // [Fact]
    // public void GetPatientFeedbackById_ReturnsNoFeedbackByPatient_WhenNoFeedbackFound()
    // {
    //     // Arrange
    //     Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B04DA");
    //     _mockService.Setup(service => service.GetPatientFeedbackById(testSessionGuid))
    //         .ReturnsAsync(new List<FeedbackDto>());
    //
    //     
    //     var controller = new FeedbackController(_mockService.Object);
    //
    //     // Act
    //     var result = controller.GetPatientFeedbackById(testSessionGuid);
    //
    //     // Assert
    //     var task = Assert.IsType<Task<ActionResult<IEnumerable<FeedbackDto>>>>(result);
    //     var actionResult = Assert.IsType<OkObjectResult>(task.Result.Result);
    //     var returnValue = Assert.IsType<List<FeedbackDto>>(actionResult.Value);
    //     Assert.Empty(returnValue);
    // }
    //
    //
    
    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback()
    {
        // Arrange
        var service = new FeedbackService(_mockRepository.Object);

        CreateFeedbackDto createIssueDto = new CreateFeedbackDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                                                                    "Lorem Ipsum");

        // Act
        var result = service.CreateFeedback(createIssueDto);
        
        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        var feedback = task.Result;
        Assert.Equal("Lorem Ipsum", feedback.Comment);
    }

    // Updates a Feedback for specific Patient - Happy Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockRepository.Setup(service => service.Get(testSessionGuid))
            .ReturnsAsync(GetFeedback(testSessionGuid));
        
        var service = new FeedbackService(_mockRepository.Object);
        UpdateFeedbackDto updateFeedback = new UpdateFeedbackDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
            new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
            "Not Lorem Ipsum");
        
        // Act
        var result = service.UpdateFeedback(testSessionGuid, updateFeedback);
        
        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        var updatedIssue = task.Result;
        Assert.Equal("Not Lorem Ipsum", updatedIssue.Comment);
    }
    
    // Updates a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockRepository.Setup(service => service.Get(testSessionGuid))
            .ReturnsAsync((Feedback) null!);
        var service = new FeedbackService(_mockRepository.Object);
        
        UpdateFeedbackDto updateFeedbackDto = new UpdateFeedbackDto(new Guid(), new Guid(), new Guid(), 
            "Not Lorem Ipsum");
        
        // Act
        var result = service.UpdateFeedback(testSessionGuid, updateFeedbackDto);
        
        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        Assert.Null(task.Result);
    }

    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");
        _mockRepository.Setup(repo => repo.Get(testSessionGuid))
            .ReturnsAsync(GetFeedback(testSessionGuid));
        
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.DeleteFeedback(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<FeedbackDto?>>(result);
        var deletedIssue = task.Result;
        Assert.Equal("Lorem Ipsum", deletedIssue.Comment);
    }
    
    // Delete a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");

        _mockRepository.Setup(repo => repo.Get(testSessionGuid))
            .ReturnsAsync((Feedback) null!);
        
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.DeleteFeedback(testSessionGuid);
        
        // Assert
        var task = Assert.IsType<Task<FeedbackDto?>>(result);
        Assert.Null(task.Result);
    }
    
    private IEnumerable<Feedback> GetPatientFeedbacksById(Guid patientId)
    {
        List<Feedback> feedbacks = new List<Feedback>
        {
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"),
                PatientId = patientId, 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
                Comment = "Hello World",
                CreatedDate = DateTimeOffset.Now
            },
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                PatientId = patientId, 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                Comment = "Lorem Ipsum",
                CreatedDate = DateTimeOffset.Now
            }
        };
        return feedbacks;
    }
    
    private Feedback GetFeedbackById(Guid id)
    {
        return new Feedback()
        {
            Id = id,
            PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), 
            AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
            StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
            Comment = "Hello World",
            CreatedDate = DateTimeOffset.Now
        };
    }
    
    private IReadOnlyCollection<Feedback> GetFeedbacks()
    {
        List<Feedback> feedbacks = new List<Feedback>
        {
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
                PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"), 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0000"), 
                Comment = "Hello World",
                CreatedDate = DateTimeOffset.Now
            },
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0ABC"),
                PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"), 
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"), 
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"), 
                Comment = "Lorem Ipsum",
                CreatedDate = DateTimeOffset.Now
            }
        };

        return feedbacks;
    }
    private Feedback GetFeedback(Guid id)
    {
        return new Feedback()
        {
            Id = id,
            PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
            AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
            StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"),
            Comment = "Lorem Ipsum",
            CreatedDate = DateTimeOffset.Now
        };
    }
}