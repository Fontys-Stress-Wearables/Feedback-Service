//Translating the FeedbackEntry Entities to a Dto
// Transforming the entities to dto's, we would be able to use the dto's in for example a api calls
using Feedback_Service.Dtos;
using Feedback_Service.Entities;

namespace Feedback_Service;

public static class Extensions
{
    //transofrming feedback entry entity from the repository to a feedback entry dto
    public static FeedbackEntryDto AsDto(this FeedbackEntry feedbackEntry)
    {
        return new FeedbackEntryDto(feedbackEntry.Id, feedbackEntry.PatientId, feedbackEntry.AuthorId,
                                    feedbackEntry.StressMeassurementId, feedbackEntry.FeedbackComment,
                                    feedbackEntry.CreatedDate);
    }
}
