using GameStore.Bll.DTO;
using GameStore.Bll.Observer;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Entities.Translate;

namespace GameStore.Bll.Infrastructure
{
    public class MapProfile : AutoMapper.Profile
    {
        public MapProfile()
        {
            CreateMap<Game, GameDTO>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<Platform, PlatformDTO>();
            CreateMap<Publisher, PublisherDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDetails, OrderDetailsDTO>();
            CreateMap<Shippers, ShippersDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Role, RoleDTO>();
            CreateMap<GenreTranslate, GenreTranslateDto>();
            CreateMap<PlatformTranslate, PlatformTranslateDto>();
            CreateMap<PublisherTranslate, PublisherTranslateDto>();
            CreateMap<GameTranslate, GameTranslateDto>();
            CreateMap<UserToken, UserTokenDto>();
            CreateMap<Image, ImageDto>();
            CreateMap<User, UserInfo>();
            CreateMap<Order, OrderModel>();
            CreateMap<Rate, RateDTO>();

            CreateMap<GameDTO, Game>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<GenreDTO, Genre>();
            CreateMap<PlatformDTO, Platform>();
            CreateMap<PublisherDTO, Publisher>();
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderDetailsDTO, OrderDetails>();
            CreateMap<ShippersDTO, Shippers>();
            CreateMap<RateDTO, Rate>();

            CreateMap<OrderMongo, Order>();
            CreateMap<Order, OrderMongo>();
            CreateMap<UserDTO, User>();
            CreateMap<RoleDTO, Role>();
            CreateMap<GenreTranslateDto, GenreTranslate>();
            CreateMap<PlatformTranslateDto, PlatformTranslate>();
            CreateMap<PublisherTranslateDto, PublisherTranslate>();
            CreateMap<GameTranslateDto, GameTranslate>();

            CreateMap<UserTokenDto, UserToken>();
            CreateMap<ImageDto, Image>();
            CreateMap<UserInfo, User>();
            CreateMap<OrderModel, Order>();

            CreateMap<Platform, Platform>()
                .MaxDepth(2)
                .ForMember(platform => platform.Games, ignore => ignore.Ignore());

            CreateMap<Game, Game>()
                .MaxDepth(2)
                .ForPath(src => src.Publisher.Games, opt => opt.Ignore());

            CreateMap<Genre, Genre>()
                .MaxDepth(2)
                .ForMember(genre => genre.Games, ignore => ignore.Ignore());

            CreateMap<Publisher, Publisher>()
                .MaxDepth(2)
                .ForMember(publisher => publisher.Games, ignore => ignore.Ignore())
                .ForMember(publisher => publisher.Users, ignore => ignore.Ignore());
        }
    }
}