using System.Linq.Expressions;
using Feedback_Service.Dtos;
using Feedback_Service.Models;
using Feedback_Service.Repository;
using Feedback_Service.Services;

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

    // Get All - Sad Flow 
    [Fact]
    public void GetAll_ReturnsEmptyArray_WhenNoFeedbackFound()
    {
        // Arrange
        _mockRepository.Setup(service => service.GetAll())
            .ReturnsAsync(GetEmptyFeedbacks());

        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetAll();

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        Assert.Empty(task.Result);
    }

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

    // Get All Feedback by specific Patient - Happy Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsAllFeedbackByPatient()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockRepository.Setup(repo => repo.GetPatientFeedbacks(testSessionGuid))
            .ReturnsAsync(GetPatientFeedbacksById(testSessionGuid));
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetPatientFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        var returnValue = task.Result;
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal("Hello World", feedback!.Comment);
    }

    // All Feedback by specific Patient - Sad Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsNoFeedbackByPatient_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        _mockRepository.Setup(repo => repo.GetPatientFeedbacks(testSessionGuid))
            .ReturnsAsync(GetEmptyFeedbacks());
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetPatientFeedbackById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        Assert.Empty(task.Result);
    }

    // Get All Feedback by specific Patient with timespan of feedback- Happy Flow 
    [Fact]
    public void GetPatientFeedbackByTimeSpan_ReturnsAllFeedbackByPatient()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        DateTime testStartTime = new DateTime(2008, 5, 1, 8, 6, 32, 0);
        DateTime testEndTime = new DateTime(2008, 6, 1, 8, 6, 32, 0);

        Expression<Func<Feedback, bool>> filter = feedback => feedback.PatientId == testSessionGuid && feedback.CreatedStressMeasurementDate > testStartTime && feedback.CreatedStressMeasurementDate > testEndTime;

        _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Feedback, bool>>>()))
        // _mockRepository.Setup(repo => repo.GetAll())
            .ReturnsAsync(GetFeedbacks());
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetPatientFeedbackByTimeSpan(testSessionGuid, testStartTime, testEndTime);

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        var returnValue = task.Result;
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal("Hello World", feedback!.Comment);
    }

    // GetPatientFeedbackByTimeSpan and returns empty array when no feedback is found- SAD Flow 
    [Fact]
    public void GetPatientFeedbackByTimeSpan_ReturnsEmptyArray_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        DateTime testStartTime = new DateTime(2008, 5, 1, 8, 6, 32, 0);
        DateTime testEndTime = new DateTime(2008, 6, 1, 8, 6, 32, 0);

        Expression<Func<Feedback, bool>> filter = feedback => feedback.PatientId == testSessionGuid && feedback.CreatedStressMeasurementDate > testStartTime && feedback.CreatedStressMeasurementDate > testEndTime;

        _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Feedback, bool>>>()))
            // _mockRepository.Setup(repo => repo.GetAll())
            .ReturnsAsync(GetEmptyFeedbacks());
        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.GetPatientFeedbackByTimeSpan(testSessionGuid, testStartTime, testEndTime);

        // Assert
        var task = Assert.IsType<Task<IEnumerable<FeedbackDto>>>(result);
        Assert.Empty(task.Result);
    }

    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback()
    {
        // Arrange
        var service = new FeedbackService(_mockRepository.Object);

        CreateFeedbackDto createIssueDto = new CreateFeedbackDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
                                                                    new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"),
                                                                    new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
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
            new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
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
            .ReturnsAsync((Feedback)null!);
        var service = new FeedbackService(_mockRepository.Object);

        UpdateFeedbackDto updateFeedbackDto = new UpdateFeedbackDto(new Guid(), new Guid(), new Guid(), new DateTimeOffset(),
            "Not Lorem Ipsum");

        // Act
        var result = service.UpdateFeedback(testSessionGuid, updateFeedbackDto);

        // Assert
        var task = Assert.IsType<Task<FeedbackDto>>(result);
        Assert.Null(task.Result);
    }

    // Delete a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
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
            .ReturnsAsync((Feedback)null!);

        var service = new FeedbackService(_mockRepository.Object);

        // Act
        var result = service.DeleteFeedback(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<FeedbackDto?>>(result);
        Assert.Null(task.Result);
    }

    private IReadOnlyCollection<Feedback> GetPatientFeedbacksById(Guid patientId)
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
                CreatedStressMeasurementDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32,new TimeSpan(1, 0, 0)),
                CreatedCommentDate = DateTimeOffset.Now
            },
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                PatientId = patientId,
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"),
                Comment = "Lorem Ipsum",
                CreatedStressMeasurementDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32,new TimeSpan(1, 0, 0)),
                CreatedCommentDate = DateTimeOffset.Now
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
            CreatedStressMeasurementDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
            CreatedCommentDate = DateTimeOffset.Now
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
                CreatedStressMeasurementDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32,new TimeSpan(1, 0, 0)),
                CreatedCommentDate = DateTimeOffset.Now
            },
            new Feedback()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0ABC"),
                PatientId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                AuthorId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
                StressMeasurementId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0BBB"),
                Comment = "Lorem Ipsum",
                CreatedStressMeasurementDate = new DateTimeOffset(2008, 6, 1, 8, 6, 32,new TimeSpan(1, 0, 0)),
                CreatedCommentDate = DateTimeOffset.Now
            }
        };

        return feedbacks;
    }
    private IReadOnlyCollection<Feedback> GetEmptyFeedbacks()
    {
        List<Feedback> feedbacks = new List<Feedback>
        {
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
            CreatedStressMeasurementDate = new DateTimeOffset(2008, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0)),
            CreatedCommentDate = DateTimeOffset.Now
        };
    }
}