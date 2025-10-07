using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Models.LanguageModels;
using GameStore.Web.Models.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public GenreController(
            IGameService gameService,
            IPlatformService platformService,
            IGenreService genreService,
            IPublisherService publisherService,
            ILogger logger,
            IAuthentication auth,
            ILanguageService lang) : base(auth)
        {
            _genreService = genreService;
            _logger = logger;
            _languageService = lang;
        }

        [Authorize(Roles = "Manager")]
        public ActionResult GetAllGenres()
        {
            var genresDto = _genreService.GetAll(CurrentLangCode).ToList();
            var genres = Mapper.Map<List<GenreViewModel>>(genresDto);

            foreach (var genreDto in genresDto)
            {
                foreach (var genreVm in genres)
                {
                    if (genreDto.Id == genreVm.ParentId)
                    {
                        genreVm.ParentName = genreDto.Name;
                    }
                }
            }

            _logger.Info("User got all genres");
            return View("AllGenres", genres);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            var genre = _genreService.GetById(id, CurrentLangCode);
            if (genre != null)
            {
                var genremv = Mapper.Map<GenreViewModel>(genre);
                
                return View("EditGenre", genremv);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(GenreViewModel genre)
        {
            if (ModelState.IsValid)
            {
                var genreDto = Mapper.Map<GenreDTO>(genre);
                foreach (var translate in genreDto.GenreTranslates)
                {
                    translate.Language = _languageService.GetById(translate.LanguageId);
                }

                _genreService.Update(genreDto);
                _logger.Info($"Genre with id {genre.Id} was edited.");
                return GetAllGenres();
            }

            return Edit(genre.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult NewGenre(int? parentId)
        {
            var newGenre = new GenreViewModel
            {
                ParentId = parentId,
                GenreTranslates = new List<GenreTranslateModel>
                {
                    new GenreTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new GenreTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
                }
            };
            return View("NewGenre", newGenre);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult NewGenre(GenreViewModel newGenre)
        {
            if (ModelState.IsValid)
            {
                var genreDto = Mapper.Map<GenreDTO>(newGenre);

                _genreService.Create(genreDto);
                _logger.Info($"Genre was added");
                return GetAllGenres();
            }

            newGenre.GenreTranslates = new List<GenreTranslateModel>
            {
                new GenreTranslateModel
                {
                    LanguageId = 1,
                    Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                },
                new GenreTranslateModel
                {
                    LanguageId = 2,
                    Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                }
            };
            return View("NewGenre", newGenre);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Remove(int id)
        {
            if (id != 0)
            {
                _genreService.Delete(id);
                _logger.Info($"Genre was deleted");
                return GetAllGenres();
            }

            _logger.Warn($"Genre wasn't deleted");
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}