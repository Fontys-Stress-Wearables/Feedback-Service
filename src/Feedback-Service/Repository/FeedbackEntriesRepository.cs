using Feedback_Service.Entities;
using MongoDB.Driver;

namespace Feedback_Service.Repository;

public class FeedbackEntriesRepository
{
    private const string collectionName = "feedback-entries";
    private readonly IMongoCollection<FeedbackEntry> dbCollection;
    private readonly FilterDefinitionBuilder<FeedbackEntry> filterBuilder = Builders<FeedbackEntry>.Filter;

    public FeedbackEntriesRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("Feedback");
        dbCollection = database.GetCollection<FeedbackEntry>(collectionName);
    }

    // retrieving all the feedback entry points in the database
    public async Task<IReadOnlyCollection<FeedbackEntry>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    // retrieving specific feedback entry point in the database
    public async Task<FeedbackEntry> GetAsync(Guid id)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    // retrieving all feedback entry points by a patientId in the database
    public async Task<IReadOnlyCollection<FeedbackEntry>> GetPatientFeedbacksAsync(int patientId)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.PatientId, patientId);
        return await dbCollection.Find(filter).ToListAsync();

        // return await dbCollection.Find(filter);
    }

    //creating a feedback entry point in the database
    public async Task CreateAsync(FeedbackEntry entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(FeedbackEntry entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);

        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<FeedbackEntry> filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}