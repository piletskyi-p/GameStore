using Autofac;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Observer;
using GameStore.Bll.Observer.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal;
using GameStore.Dal.Interfaces;
using GameStore.Dal.Repositories;

namespace GameStore.Bll.Infrastructure
{
    public class AutofacConfigBll
    {
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<PlatformService>().As<IPlatformService>();
            builder.RegisterType<GenreService>().As<IGenreService>();
            builder.RegisterType<PublisherService>().As<IPublisherService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<PayService>().As<IPayService>();
            builder.RegisterType<BanService>().As<IBanService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserTokenService>().As<IUserTokenService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<DataBaseConnection>().As<IDataBaseConnection>().InstancePerRequest();
            builder.RegisterType<EventLogger>().As<IEventLogger>();
            builder.RegisterType<ShippersService>().As<IShippersService>();
            builder.RegisterType<FacadeOrder>().As<IFacadeOrder>();
            builder.RegisterType<LanguageService>().As<ILanguageService>();
            builder.RegisterType<UserTokenService>().As<IUserTokenService>();
            builder.RegisterType<SenderService>().As<ISenderService>().SingleInstance();
            builder.RegisterType<Observer.Observer>().As<IObserver>();

            builder.RegisterType<GameRepository>().As<IGameRepository>();
            builder.RegisterType<MongoDbContext>().As<IMongoDbContext>().SingleInstance();
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>));
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
        }
    }
}