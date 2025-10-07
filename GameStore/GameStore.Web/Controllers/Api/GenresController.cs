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
    public class GenresController : BaseApiController
    {
        private readonly IGenreService _genreService;
        private readonly IGameService _gameService;
        private readonly ILanguageService _languageService;

        public GenresController(IGenreService genreService, ILanguageService languageService, IGameService gameService)
        {
            _genreService = genreService;
            _languageService = languageService;
            _gameService = gameService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var genreDto = _genreService.GetAll(CurrentLangCode).ToList();
            foreach (var genre in genreDto)
            {
                foreach (var game in genre.Games)
                {
                    game.Genres.Clear();
                    game.Platforms.Clear();
                }
            }

            return Content(HttpStatusCode.OK, genreDto);
        }

        [HttpGet]
        public IHttpActionResult GetDetails(int id)
        {
            var genre = _genreService.GetById(id, CurrentLangCode);

            if (genre == null)
            {
                return NotFound();
            }

            foreach (var game in genre.Games)
            {
                game.Genres.Clear();
                game.Platforms.Clear();
            }

            return Content(HttpStatusCode.OK, genre);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]GenreViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var genreDto = Mapper.Map<GenreDTO>(value);
            _genreService.Create(genreDto);

            return Content(HttpStatusCode.OK, "Genre was changed");
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]GenreViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var genreDto = Mapper.Map<GenreDTO>(value);
            foreach (var translate in genreDto.GenreTranslates)
            {
                translate.Language = _languageService.GetById(translate.LanguageId);
            }

            _genreService.Update(genreDto);

            return Content(HttpStatusCode.Created, "Genre was added");
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _genreService.Delete(id);

            return Content(HttpStatusCode.OK, "Genre was deleted");
        }

        [HttpGet]
        public IHttpActionResult Games(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var games = _gameService.GetGamesByGenreId(id).ToList();

            return Content(HttpStatusCode.OK, Mapper.Map<List<GameDTO>>(games));
        }
    }
}