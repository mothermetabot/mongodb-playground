using MongoDB.Driver;
using System.Diagnostics;

namespace MongoDb.Client.Test
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var count = 0;
            var db = GetDatabase();

            if (db == null) throw new ArgumentNullException(nameof(db));

            var collection = db.GetCollection<Data>("test-collection");

            var stopwatch = new Stopwatch();

            while (!stoppingToken.IsCancellationRequested)
            {
                var x = new Random().NextDouble() * new Random().Next(10000);

                var y = new Random().NextDouble() * new Random().Next(10000);

                var data = new Data()
                {
                    X = x,
                    Y = y
                };


                await collection.InsertOneAsync(data, new InsertOneOptions(), stoppingToken);

                if (count == 1000)
                    return;
                count++;
            }
        }


        private IMongoDatabase GetDatabase()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://netzsch-bot:9PGNOaFSqL08OSW9@cluster0.q0msgdt.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("proteus-now");

            return database;
        }
    }
}