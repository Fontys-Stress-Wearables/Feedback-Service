using Feedback_Service.Entities;
using MongoDB.Driver;

namespace Feedback_Service.Repository;

public class FeedbackRepository : IFeedbackRepository
{
    private const string collectionName = "feedbacks";
    private readonly IMongoCollection<FeedbackEntity> dbCollection;
    private readonly FilterDefinitionBuilder<FeedbackEntity> filterBuilder = Builders<FeedbackEntity>.Filter;

    public FeedbackRepository(IMongoDatabase database)
    {
        dbCollection = database.GetCollection<FeedbackEntity>(collectionName);
    }

    // retrieving all the feedbacks stored in the database
    public async Task<IReadOnlyCollection<FeedbackEntity>> GetAll()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    // retrieving specific feedback stored in the database
    public async Task<FeedbackEntity> Get(Guid id)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntity> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    // retrieving all feedbacks by a patientId stored in the database
    public async Task<IReadOnlyCollection<FeedbackEntity>> GetPatientFeedbacks(int patientId)
    {
        //filter to find item based on id
        FilterDefinition<FeedbackEntity> filter = filterBuilder.Eq(entity => entity.PatientId, patientId);
        return await dbCollection.Find(filter).ToListAsync();
    }

    //creating a feedback to store in the database
    public async Task Create(FeedbackEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    //updating a feedback that is stored in the database
    public async Task Update(FeedbackEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<FeedbackEntity> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);

        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    //removing a feedback that is stored in the database
    public async Task Remove(Guid id)
    {
        FilterDefinition<FeedbackEntity> filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}