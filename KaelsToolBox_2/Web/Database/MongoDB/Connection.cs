using MongoDB.Bson;
using MongoDB.Driver;

namespace KaelsToolBox_2.Web.Database.MongoDB;

public class Connection(string connectionUri)
{
    readonly MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionUri);
    MongoClient? client;

    public bool Connect()
    {
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        client = new MongoClient(settings);
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));

            if (result is null) return false;
            if (result["ok"].ToInt32() == 1) return true;

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"While trying to connect:\r\n    {ex}");
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="collection"></param>
    /// <param name="callback">First T is the old document, second T is the new document</param>
    public void Watch<T>(string database, string collection, Action<ChangeStreamUpdateDescription, T, T> callback) where T : DatabaseObject
    {
        Task.Run(() =>
        {
            var options = new ChangeStreamOptions();
            options.FullDocument = ChangeStreamFullDocumentOption.UpdateLookup;
            options.FullDocumentBeforeChange = ChangeStreamFullDocumentBeforeChangeOption.WhenAvailable;

            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>().Match("{ operationType: { $in: [ 'insert', 'replace' ] } }");

            var cursor = GetDatabase(database).GetCollection<T>(collection).Watch(pipeline, options);

            var enumerator = cursor.ToEnumerable().GetEnumerator();

            while (enumerator.MoveNext())
            {
                ChangeStreamDocument<T> doc = enumerator.Current;

                callback.Invoke(doc.UpdateDescription, doc.FullDocumentBeforeChange, doc.FullDocument);
            }
        }).Start();
    }

    #region CRUD

    public int Count<T>(string database, string collection) where T : DatabaseObject
    {
        return (int)GetDatabase(database).GetCollection<T>(collection).CountDocuments("{}");
    }

    /// <summary>
    /// Get the database with the specified name
    /// </summary>
    /// <param name="database">The name of the database</param>
    /// <returns>The IMongoDatabase instance</returns>
    private IMongoDatabase GetDatabase(string database)
    {
        if (client is null)
        {
            Console.WriteLine("Client is null, please use Connect() first");
            return null!;
        }

        return client.GetDatabase(database);
    }

    /// <summary>
    /// Get all documents of type T from the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the documents</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <returns>A list of documents of type T</returns>
    public List<T> GetAll<T>(string database, string collection) where T : DatabaseObject
    {
        return GetDatabase(database).GetCollection<T>(collection).Find(item => true).ToList();
    }

    /// <summary>
    /// Get all documents of type T from the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the documents</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <returns>A list of documents of type T</returns>
    public Task<IAsyncCursor<T>> GetAllAsync<T>(string database, string collection) where T : DatabaseObject
    {
        return GetDatabase(database).GetCollection<T>(collection).FindAsync(item => true);
    }

    /// <summary>
    /// Get all documents of type T from the specified collection in the database using a filter
    /// </summary>
    /// <typeparam name="T">The type of the documents</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <param name="filter">The filter definition</param>
    /// <returns>A list of documents of type T</returns>
    public List<T> GetAll<T>(string database, string collection, FilterDefinition<T> filter) where T : DatabaseObject
    {
        return GetDatabase(database).GetCollection<T>(collection).Find(filter).ToList();
    }

    /// <summary>
    /// Get a document of type T by its id from the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the document</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <param name="id">The id of the document</param>
    /// <returns>The document of type T</returns>
    public T? Get<T>(string database, string collection, string id) where T : DatabaseObject
    {
        return GetDatabase(database).GetCollection<T>(collection).Find(item => item.Id == id).FirstOrDefault();
    }

    /// <summary>
    /// Insert a document of type T into the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the document</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <param name="obj">The document to insert</param>
    public void Insert<T>(string database, string collection, T obj) where T : DatabaseObject
    {
        obj.DateCreated = DateTime.UtcNow;
        obj.LastUpdated = DateTime.UtcNow;
        GetDatabase(database).GetCollection<T>(collection).InsertOne(obj);
    }

    /// <summary>
    /// Delete a document of type T by its id from the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the document</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <param name="id">The id of the document to delete</param>
    public void Delete<T>(string database, string collection, T obj) where T : DatabaseObject
    {
        obj.DateDeleted = DateTime.UtcNow;
        obj.IsDeleted = true;
        Update(database, collection, obj);
    }

    /// <summary>
    /// Update a document of type T in the specified collection in the database
    /// </summary>
    /// <typeparam name="T">The type of the document</typeparam>
    /// <param name="database">The name of the database</param>
    /// <param name="collection">The name of the collection</param>
    /// <param name="obj">The document to update</param>
    public void Update<T>(string database, string collection, T obj) where T : DatabaseObject
    {
        obj.LastUpdated = DateTime.UtcNow;
        GetDatabase(database).GetCollection<T>(collection).ReplaceOne(item => item.Id == obj.Id, obj);
    }
    #endregion
}
