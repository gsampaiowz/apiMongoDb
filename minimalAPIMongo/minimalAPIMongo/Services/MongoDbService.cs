using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {
        // Armazenar a configuração da aplicação
        private readonly IConfiguration _configuration;

        // Armazenar a referência ao MongoDb
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Contém a configuração necessária para acesso ao MongoDb
        /// </summary>
        /// <param name="configuration">Objeto contendo toda a config da aplicação</param>
        public MongoDbService(IConfiguration configuration)
        {
            //Atribui a config recebida em _configuration
            _configuration = configuration;
            
            //Acessa a string de conexão
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //Transforma a string obtida em MongoUrl
            var mongoUrl = MongoUrl.Create(connectionString);

            //Cria um client 
            var mongoClient = new MongoClient(mongoUrl);

            //Obtém a referência ao MongoDb
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propriedade para acessar o banco de dados => retorna os dados em _database
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
