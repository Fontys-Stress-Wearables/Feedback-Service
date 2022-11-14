using Feedback_Service.Entities;
using MongoDB.Driver;

namespace Feedback_Service.Repository;

public class FeedbackEntriesRepository : IFeedbackEntriesRepository
{
    private const string collectionName = "feedback-entries";
    private readonly IMongoCollection<FeedbackEntry> dbCollection;
    private readonly FilterDefinitionBuilder<FeedbackEntry> filterBuilder = Builders<FeedbackEntry>.Filter;

    public FeedbackEntriesRepository(IMongoDatabase database)
    {
        dbCollection = database.GetCollection<FeedbackEntry>(collectionName);
    }

    // retrieving all the feedback entry points in the database
    public async Task<IReadOnlyCollection<FeedbackEntry>> GetAll()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    // retrieving specific feedback entry point in the database
    public async Task<FeedbackEntry> Get(Guid id)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    // retrieving all feedback entry points by a patientId in the database
    public async Task<IReadOnlyCollection<FeedbackEntry>> GetPatientFeedbacks(int patientId)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.PatientId, patientId);
        return await dbCollection.Find(filter).ToListAsync();

        // return await dbCollection.Find(filter);
    }

    //creating a feedback entry point in the database
    public async Task Create(FeedbackEntry entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    public async Task Update(FeedbackEntry entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);

        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task Remove(Guid id)
    {
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}