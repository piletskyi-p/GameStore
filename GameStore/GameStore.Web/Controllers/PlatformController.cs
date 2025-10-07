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
    public class PlatformController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IPlatformService _platformService;
        private readonly IPublisherService _publisherService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public PlatformController(
            IGameService gameService,
            IPlatformService platformService,
            IGenreService genreService,
            IPublisherService publisherService,
            ILogger logger, 
            IAuthentication auth,
            ILanguageService lang) : base(auth)
        {
            _gameService = gameService;
            _platformService = platformService;
            _genreService = genreService;
            _publisherService = publisherService;
            _logger = logger;
            _languageService = lang;
        }

        [Authorize(Roles = "Manager")]
        public ActionResult GetAllPlatforms()
        {
            var platformsDto = _platformService.GetAll(CurrentLangCode).ToList();
            if (platformsDto.Count != 0)
            {
                var platforms = Mapper.Map<List<PlatformViewModel>>(platformsDto);
                _logger.Info("User got all platforms.");
                return View("AllPlatforms", platforms);
            }

            return View("AllPlatforms", new List<PlatformViewModel>());
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            var platformDto = _platformService.GetById(id, CurrentLangCode);
            if (platformDto != null)
            {
                var platformView = Mapper.Map<PlatformViewModel>(platformDto);
                return View("EditPlatform", platformView);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(PlatformViewModel platform)
        {
            if (ModelState.IsValid)
            {
                var platformDto = Mapper.Map<PlatformDTO>(platform);

                _platformService.Update(platformDto);
                _logger.Info($"Platform with id: {platform.Id} was edited.");
                return GetAllPlatforms();
            }

            return Edit(platform.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult NewPlatform()
        {
            var newplatform = new PlatformViewModel
            {
                PlatformTranslates = new List<PlatformTranslateModel>
                {
                    new PlatformTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new PlatformTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
                }
            };

            return View("NewPlatform", newplatform);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult NewPlatform(PlatformViewModel newPlatform)
        {
            if (ModelState.IsValid)
            {
                var platformDto = Mapper.Map<PlatformDTO>(newPlatform);
                foreach (var translate in platformDto.PlatformTranslates)
                {
                    translate.Language = _languageService.GetById(translate.LanguageId);
                }

                _platformService.Create(platformDto);
                _logger.Info("New platform was added");
                return GetAllPlatforms();
            }

            newPlatform.PlatformTranslates = new List<PlatformTranslateModel>
            {
                new PlatformTranslateModel
                {
                    LanguageId = 1,
                    Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                },
                new PlatformTranslateModel
                {
                    LanguageId = 2,
                    Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                }
            };
            return View("NewPlatform", newPlatform);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Remove(int id)
        {
            if (id != 0)
            {
                _platformService.Delete(id);
                _logger.Info("Platform was deleted");
                return GetAllPlatforms();
            }

            _logger.Warn("Platform wasn't deleted");
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}