using System.Linq.Expressions;
using Feedback_Service.Models;
using MongoDB.Driver;

namespace Feedback_Service.Repository;

public class FeedbackRepository : IFeedbackRepository
{
    private const string collectionName = "feedbacks";
    private readonly IMongoCollection<Feedback> dbCollection;
    private readonly FilterDefinitionBuilder<Feedback> filterBuilder = Builders<Feedback>.Filter;

    public FeedbackRepository(IMongoDatabase database)
    {
        dbCollection = database.GetCollection<Feedback>(collectionName);
    }

    // retrieving all the feedbacks stored in the database
    public async Task<IReadOnlyCollection<Feedback>> GetAll()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    // retrieving specific feedback stored in the database
    public async Task<Feedback> Get(Guid id)
    {
        //filter to find item based on id
        FilterDefinition<Feedback> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    // retrieving all feedbacks by a patientId stored in the database
    public async Task<IReadOnlyCollection<Feedback>> GetPatientFeedbacks(Guid patientId)
    {
        //filter to find item based on id
        FilterDefinition<Feedback> filter = filterBuilder.Eq(entity => entity.PatientId, patientId);
        return await dbCollection.Find(filter).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Feedback>> GetAll(Expression<Func<Feedback, bool>> filter)
    {
        return await dbCollection.Find(filter).ToListAsync();
    }

    //creating a feedback to store in the database
    public async Task Create(Feedback entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    //updating a feedback that is stored in the database
    public async Task Update(Feedback entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<Feedback> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);

        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    //removing a feedback that is stored in the database
    public async Task Remove(Guid id)
    {
        FilterDefinition<Feedback> filter = filterBuilder.Eq(entity => entity.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}