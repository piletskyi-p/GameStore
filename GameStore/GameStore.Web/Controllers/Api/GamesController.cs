using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Models.ViewModels;

namespace GameStore.Web.Controllers.Api
{
    public class GamesController : BaseApiController
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IPlatformService _platformService;
        private readonly IPublisherService _publisherService;
        private readonly ILanguageService _languageService;

        public GamesController(
            IGameService gameService, 
            ILanguageService languageService,
            IGenreService genreService,
            IPlatformService platformService,
            IPublisherService publisherService)
        {
            _gameService = gameService;
            _languageService = languageService;
            _genreService = genreService;
            _platformService = platformService;
            _publisherService = publisherService;
        }

        public IHttpActionResult Get([FromUri]FilterViewModel gameFilter, int page)
        {
            if (gameFilter == null)
            {
                var game = _gameService.GetAllGames().ToList();

                return Content(HttpStatusCode.OK, Mapper.Map<List<GameDTO>>(game));
            }

            var filterDto = Mapper.Map<FilterDTO>(gameFilter);
            int pageSize = gameFilter.NumberOfItemPage != 0 ? gameFilter.NumberOfItemPage : 10;

            filterDto.Games = _gameService.Filter(filterDto, page, pageSize);
           
           
            filterDto.Games = filterDto.Games.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            
            return Content(HttpStatusCode.OK, Mapper.Map<List<GameDTO>>(filterDto.Games));
        }

        public IHttpActionResult GetDetails(int id)
        {
            var game = _gameService.GetItemById(id, CurrentLangCode);

            if (game == null)
            {
                return Content(HttpStatusCode.OK, Mapper.Map<GameDTO>(game));
            }

            return Content(HttpStatusCode.OK, Mapper.Map<GameDTO>(game));
        }

        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Post([FromBody]GameViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var genreDto = Mapper.Map<GameDTO>(value);
            _gameService.Edit(genreDto);

            return Content(HttpStatusCode.OK, "Game was changed");
        }

        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Put(int id, [FromBody] GameViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var gameDto = Mapper.Map<GameDTO>(value);
            _gameService.Create(gameDto);

            return Content(HttpStatusCode.Created, "Game was added");
        }
        
        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Delete(int id)
        {
            _gameService.Delete(id);

            return Content(HttpStatusCode.OK, "Game was deleted");
        }

        [HttpGet]
        public IHttpActionResult Genres(int id)
        {
            var game = _gameService.GetItemById(id, "en");
            if (game == null)
            {
                return NotFound();
            }

            var genres = game.Genres;
            foreach (var genre in genres)
            {
                genre.Games.Clear();
            }

            return Content(HttpStatusCode.OK, genres);
        }
    }
}