using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Bll.Infrastructure;
using GameStore.Web.Models.LanguageModels;
using GameStore.Web.Models.ViewModels;

namespace GameStore.Web
{
    public class AutomapperWebProfile : AutoMapper.Profile
    {
        public AutomapperWebProfile()
        {
            CreateMap<GameDTO, GameDetailsViewModel>();
            CreateMap<CommentDTO, CommentsViewModel>();

            CreateMap<GameDTO, GameViewModel>();
            CreateMap<GameViewModel, GameDTO>();

            CreateMap<CommentDTO, CommentForList>();
            CreateMap<CommentForList, CommentDTO>();

            CreateMap<PublisherDTO, PublisherViewModel>();
            CreateMap<PublisherViewModel, PublisherDTO>();

            CreateMap<GenreDTO, GenreViewModel>()
                .ForMember(f => f.Games, opts => opts.Ignore());
            CreateMap<GenreViewModel, GenreDTO>();

            CreateMap<PlatformDTO, PlatformViewModel>();
            CreateMap<PlatformViewModel, PlatformDTO>();

            CreateMap<OrderDTO, OrderViewModel>();
            CreateMap<OrderViewModel, OrderDTO>();

            CreateMap<OrderDetailsDTO, OrderDetailsViewModel>();
            CreateMap<OrderDetailsViewModel, OrderDetailsDTO>();

            CreateMap<UserDTO, UserViewModel>();
            CreateMap<UserViewModel, UserDTO>();

            CreateMap<UserDTO, EditUserViewModel>();
            CreateMap<EditUserViewModel, UserDTO>();

            CreateMap<GameTranslateDto, GameTranslateModel>();
            CreateMap<GameTranslateModel, GameTranslateDto>();

            CreateMap<GenreTranslateDto, GenreTranslateModel>();
            CreateMap<GenreTranslateModel, GenreTranslateDto>();

            CreateMap<PlatformTranslateDto, PlatformTranslateModel>();
            CreateMap<PlatformTranslateModel, PlatformTranslateDto>();

            CreateMap<PublisherTranslateDto, PublisherTranslateModel>();
            CreateMap<PublisherTranslateModel, PublisherTranslateDto>();

            CreateMap<LanguageDto, LanguageModel>();
            CreateMap<LanguageModel, LanguageDto>();

            CreateMap<ShippersDTO, ShippersViewModel>();
            CreateMap<ShippersViewModel, ShippersDTO>();

            CreateMap<ImageDto, ImageViewModel>();
            CreateMap<ImageViewModel, ImageDto>();

            CreateMap<PlatformDTO, PlatformDTO>()
                .MaxDepth(2)
                .ForMember(platform => platform.Games, ignore => ignore.Ignore());

            CreateMap<GameDTO, GameDTO>()
                .MaxDepth(2)
                .ForPath(src => src.Publisher.Games, opt => opt.Ignore());

            CreateMap<GenreDTO, GenreDTO>().PreserveReferences()
                .MaxDepth(2)
                .ForMember(genre => genre.Games, ignore => ignore.Ignore());

            CreateMap<PublisherDTO, PublisherDTO>()
                .MaxDepth(2)
                .ForMember(publisher => publisher.Games, ignore => ignore.Ignore());
        }

        public static void Run()
        {
            Mapper.Initialize(i =>
            {
                i.AddProfile<AutomapperWebProfile>();
                i.AddProfile<MapProfile>();
            });
        }
    }
}
