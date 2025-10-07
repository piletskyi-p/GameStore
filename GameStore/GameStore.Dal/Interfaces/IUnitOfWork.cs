using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Mongo;

namespace GameStore.Dal.Interfaces
{
    public interface IUnitOfWork
    {
        IGameRepository GameRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Genre> GenreRepository { get; }
        IGenericRepository<Platform> PlatformRepository { get; }
        IGenericRepository<OrderDetails> OrderDetailsRepository { get; }
        IGenericRepository<Publisher> PublisherRepository { get; }
        IGenericRepository<Language> LanguageRepository { get; }
        IGenericRepository<UserToken> UserTokenRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        IGenericRepository<Rate> RateRepository { get; }


        IFacadeOrder OrdersRepository { get; }
        IMongoRepository<OrderMongo> MongoOrderRepository { get; }
        IMongoRepository<Shippers> MongoShippersRepository { get; }
        void Save();
    }
}
