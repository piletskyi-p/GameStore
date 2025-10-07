using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;

namespace GameStore.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataBaseConnection _context;
        private readonly IMongoDbContext _mongoContext;

        public UnitOfWork(
            IDataBaseConnection context,
            IMongoDbContext mongoContext,
            IGameRepository gameRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Comment> commentRepository,
            IGenericRepository<Genre> genreRepository,
            IGenericRepository<Platform> platformRepository,
            IGenericRepository<Publisher> publisherRepository,
            IGenericRepository<OrderDetails> orderDetailsRepository,
            IGenericRepository<Language> languageRepository,
            IGenericRepository<UserToken> userTokenRepository,
            IGenericRepository<Image> imageRepository,
            IGenericRepository<Rate> rateRepository,
            IFacadeOrder ordersRepository,
            IMongoRepository<OrderMongo> orderMongoRepository,
            IMongoRepository<Shippers> sippersRepository)
        {
            _context = context;
            _mongoContext = mongoContext;
            GameRepository = gameRepository;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            CommentRepository = commentRepository;
            GenreRepository = genreRepository;
            PlatformRepository = platformRepository;
            PublisherRepository = publisherRepository;
            OrderDetailsRepository = orderDetailsRepository;
            OrdersRepository = ordersRepository;
            MongoOrderRepository = orderMongoRepository;
            MongoShippersRepository = sippersRepository;
            LanguageRepository = languageRepository;
            UserTokenRepository = userTokenRepository;
            ImageRepository = imageRepository;
            RateRepository = rateRepository;
        }

        public IGameRepository GameRepository { get; }

        public IGenericRepository<Role> RoleRepository { get; }

        public IGenericRepository<User> UserRepository { get; }

        public IGenericRepository<Comment> CommentRepository { get; }

        public IGenericRepository<Genre> GenreRepository { get; }

        public IGenericRepository<Platform> PlatformRepository { get; }

        public IGenericRepository<OrderDetails> OrderDetailsRepository { get; }

        public IGenericRepository<Publisher> PublisherRepository { get; }

        public IMongoRepository<OrderMongo> MongoOrderRepository { get; }

        public IFacadeOrder OrdersRepository { get; }

        public IMongoRepository<Shippers> MongoShippersRepository { get; }

        public IGenericRepository<Language> LanguageRepository { get; }

        public IGenericRepository<UserToken> UserTokenRepository { get; }

        public IGenericRepository<Image> ImageRepository { get; }

        public IGenericRepository<Rate> RateRepository { get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
