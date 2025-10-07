using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Models.LanguageModels;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Pagination;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GameController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IPlatformService _platformService;
        private readonly IPublisherService _publisherService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public GameController(
            IGameService gameService,
            IPlatformService platformService,
            IGenreService genreService,
            IPublisherService publisherService,
            ILogger logger,
            ILanguageService lang,
            IAuthentication auth,
            IUserService userService) : base(auth)
        {
            _gameService = gameService;
            _platformService = platformService;
            _genreService = genreService;
            _publisherService = publisherService;
            _logger = logger;
            _languageService = lang;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Publisher, Manager")]
        public ActionResult NewGame()
        {
            var publishers = Mapper
                .Map<List<PublisherDTO>, List<PublisherViewModel>>(_publisherService
                    .GetAllPublisher(CurrentLangCode).ToList());
            var game = new GameViewModel
            {
                Genres = Mapper.Map<List<GenreDTO>, List<GenreViewModel>>(_genreService
                    .GetAll(CurrentLangCode).ToList()),
                Platforms = Mapper
                    .Map<List<PlatformDTO>, List<PlatformViewModel>>(_platformService
                        .GetAll(CurrentLangCode).ToList()),
                PublisherSelect = new SelectList(publishers, "Id", "CompanyName"),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>(),
                PublicationDate = DateTime.UtcNow,
                GameTranslates = new List<GameTranslateModel>
                {
                    new GameTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new GameTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
                }
            };
            return View("NewGame", game);
        }

        [HttpPost]
        [Authorize(Roles = "Publisher, Manager")]
        public ActionResult NewGame(GameViewModel newGame)
        {
            var gameForCheck = _gameService.GetGameByKey(newGame.Key, CurrentLangCode);
            if (gameForCheck != null)
            {
                ModelState.AddModelError("Key", "Game with this key has already existed!");
            }

            if (newGame.File != null)
            {
                string extension = Path.GetExtension(newGame.File.FileName);
                if (extension != ".jpg" || extension != ".jpeg" || extension != ".png")
                {
                    ModelState.AddModelError("File", "Incorrect type of file.");
                }
            }

            if (ModelState.IsValid)
            {
                if (newGame.File != null)
                {
                    string extension = Path.GetExtension(newGame.File.FileName);

                    var trailingPath = newGame.Key + extension;
                    newGame.Image = new ImageViewModel
                    {
                        GameKey = newGame.Key,
                        Name = trailingPath
                    };
                    string fullPath = Path.Combine(Server.MapPath("~/Content/Images/Games/"), trailingPath);
                    newGame.File.SaveAs(fullPath);
                }
               
                var gameDto = Mapper.Map<GameViewModel, GameDTO>(newGame);

                _gameService.Create(gameDto);
                _logger.Info($"New game ({gameDto.Name}, key: {gameDto.Key}) was added to DB");

                return Filter(new FilterViewModel());
            }

            var publishers = Mapper
                .Map<List<PublisherDTO>, List<PublisherViewModel>>(_publisherService
                    .GetAllPublisher(CurrentLangCode).ToList());
            var game = new GameViewModel
            {
                Genres = Mapper.Map<List<GenreDTO>, List<GenreViewModel>>(_genreService
                    .GetAll(CurrentLangCode).ToList()),
                Platforms = Mapper
                    .Map<List<PlatformDTO>, List<PlatformViewModel>>(_platformService
                        .GetAll(CurrentLangCode).ToList()),
                PublisherSelect = new SelectList(publishers, "Id", "CompanyName"),
                PlatformsIds = newGame.PlatformsIds ?? new List<int>(),
                GenresIds = newGame.GenresIds ?? new List<int>(),
                PublicationDate = newGame.PublicationDate,
                GameTranslates = new List<GameTranslateModel>
                {
                    new GameTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new GameTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
                }
            };

            return View("NewGame", game);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAllGames(int page = 1)
        {
            int numberOfGame = GetGameCount();
            int pageSize = 4;
            var game = _gameService.GetGamesByRange(page, pageSize);
            var gameVm = Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(game);
            _logger.Info("Admin got all games");

            var gameVmList = gameVm.ToList();
            List<GameViewModel> gamesPerPages = gameVmList.ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = numberOfGame };
            IndexGameViewModel ivm = new IndexGameViewModel { PageInfo = pageInfo, Games = gamesPerPages };

            return View("AllGames", ivm);
        }

        [ActionName("download")]
        [HttpGet]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult DownloadGame(string gamekey)
        {
            if (string.IsNullOrEmpty(gamekey))
            {
                ModelState.AddModelError("Key", "Enter game's key");
            }

            if (ModelState.IsValid)
            {
                string filePath = Server.MapPath("~/App_Data/Task1.docx");
                string fileName = "Task1.docx";

                _logger.Info($"Admin got file by game key: {gamekey}");

                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public int GetGameCount()
        {
            return _gameService.GetAllGames().Count();
        }

        [Authorize(Roles = "Publisher, Manager")]
        public ActionResult Remove(int id)
        {
            if (id != 0)
            {
                _gameService.UpdateForRemove(id);
                var games = _gameService.GetAllGames();
                var gameVm = Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(games);
                _logger.Info($"Game with id:{id} was deleted");
                return Filter(new FilterViewModel());
            }

            _logger.Warn("Game wasn't deleted");
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Authorize(Roles = "Publisher, Manager")]
        public ActionResult EditGame(int Id)
        {
            var game = _gameService.GetItemById(Id, CurrentLangCode);
            var gameView = Mapper.Map<GameViewModel>(game);

            gameView.PlatformsIds = game.Platforms.Select(id => id.Id).ToList();

            gameView.GenresIds = game.Genres.Select(id => id.Id).ToList();

            gameView.Genres = Mapper.Map<List<GenreDTO>, List<GenreViewModel>>(_genreService
                .GetAll(CurrentLangCode).ToList());
            gameView.Platforms = Mapper
                .Map<List<PlatformDTO>, List<PlatformViewModel>>(_platformService
                    .GetAll(CurrentLangCode).ToList());
            var publishers = Mapper
                .Map<List<PublisherDTO>, List<PublisherViewModel>>(_publisherService
                    .GetAllPublisher(CurrentLangCode).ToList());
            gameView.PublisherSelect = new SelectList(publishers, "Id", "CompanyName");
            gameView.Image = Mapper.Map<ImageViewModel>(game.Image);
            
            return View("EditGame", gameView);
        }

        [HttpPost]
        [Authorize(Roles = "Publisher, Manager")]
        public ActionResult EditGame(GameViewModel editGame)
        {
            if (editGame.File != null)
            {
                string extension = Path.GetExtension(editGame.File.FileName);
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ModelState.AddModelError("File", "Incorrect type of file.");
                }
            }

            if (ModelState.IsValid)
            {
                if (editGame.File != null)
                {
                    string extension = Path.GetExtension(editGame.File.FileName);

                    var trailingPath = editGame.Key + extension;
                    string path = Request.MapPath("~/Content/Images/Games/" + trailingPath);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    editGame.Image = new ImageViewModel
                    {
                        GameKey = editGame.Key,
                        Name = trailingPath
                    };
                    string fullPath = Path.Combine(Server.MapPath("~/Content/Images/Games/"), trailingPath);
                    editGame.File.SaveAs(fullPath);
                }

                var gameDto = Mapper.Map<GameViewModel, GameDTO>(editGame);
                _gameService.Edit(gameDto);
                _logger.Info($"Game with id:{editGame.Id} was edited.");

                return Filter(new FilterViewModel());
            }

            _logger.Warn("Game wasn't edited");
            return EditGame(editGame.Id);
        }

        [HttpGet]
        public ActionResult Filter(FilterViewModel filter, int page = 1)
        {
            var games = _gameService.GetAllGames().ToList();
            var genres = _genreService.GetAll(CurrentLangCode).ToList();
            var platforms = _platformService.GetAll(CurrentLangCode).ToList();
            var publishers = _publisherService.GetAllPublisher(CurrentLangCode).ToList();

            filter = new FilterViewModel
            {
                Genres = Mapper.Map<List<GenreViewModel>>(genres),
                Platforms = Mapper.Map<List<PlatformViewModel>>(platforms),
                Publishers = Mapper.Map<List<PublisherViewModel>>(publishers),
                Games = new List<GameViewModel>(),
                GameNames = games.Select(game => game.Name).ToList(),
                GenresId = new List<int>(),
                PlatformsId = new List<int>(),
                PublishersId = new List<int>()
            };

            foreach (var genreDto in genres)
            {
                foreach (var genreVm in filter.Genres)
                {
                    if (genreDto.Id == genreVm.ParentId)
                    {
                        genreVm.ParentName = genreDto.Name;
                    }
                }
            }

            int pageSize = 10;
            filter.Games = Mapper.Map<List<GameViewModel>>(games.Skip((page - 1) * pageSize).Take(pageSize).ToList());
            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = games.Count };
            var indexView = new FilterIndexViewModel { PageInfo = pageInfo, FilterModel = filter };
            indexView.FilterModel.NumberOfItemPage = 10;

            return View("GameFilters", indexView);
        }

        [HttpGet]
        public ActionResult Filters(FilterViewModel gameFilter, int page = 1)
        {
            var filterDto = Mapper.Map<FilterDTO>(gameFilter);
            int currentGameCount = _gameService.FilterCount(filterDto);
            int pageSize = gameFilter.NumberOfItemPage == 0 ? currentGameCount : gameFilter.NumberOfItemPage;
            filterDto.Games = _gameService.Filter(filterDto, page, pageSize);
            
            gameFilter = Mapper.Map<FilterViewModel>(filterDto);

            var genres = _genreService.GetAll(CurrentLangCode).ToList();
            var platforms = _platformService.GetAll(CurrentLangCode).ToList();
            var publishers = _publisherService.GetAllPublisher(CurrentLangCode).ToList();

            gameFilter.Genres = Mapper.Map<List<GenreViewModel>>(genres);
            gameFilter.Platforms = Mapper.Map<List<PlatformViewModel>>(platforms);
            gameFilter.Publishers = Mapper.Map<List<PublisherViewModel>>(publishers);

            var games = _gameService.GetAllGames().ToList();
            gameFilter.GameNames = games.Select(game => game.Name).ToList();

            //int pageSize = gameFilter.NumberOfItemPage == 0 ? gameFilter.Games.Count : gameFilter.NumberOfItemPage;
            var pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = currentGameCount
            };
            //gameFilter.Games = gameFilter.Games.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var indexView = new FilterIndexViewModel { PageInfo = pageInfo, FilterModel = gameFilter };

            return View("GameFilters", indexView);
        }

        public ActionResult GetDeletedGames(int page = 1)
        {
            var gamesDto = _gameService.GetDeletedGames();
            var games = Mapper.Map<IEnumerable<GameViewModel>>(gamesDto);

            int pageSize = 10;
            var gemesTemp = games.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = games.Count() };
            var indexView = new IndexGameViewModel { PageInfo = pageInfo, Games = gemesTemp.ToList() };

            return View("AllGames", indexView);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult GetGameDetailsByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                ModelState.AddModelError("Key", "Enter game's key");
            }

            if (ModelState.IsValid)
            {
                var game = _gameService.GetGameByKey(key, CurrentLangCode);
                if (game == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                _logger.Info($"Admin got game details by key: {key}");

                var gameForView = Mapper.Map<GameDTO, GameViewModel>(game);
                if (gameForView.Publisher == null)
                {
                    gameForView.Publisher = new PublisherViewModel
                    {
                        CompanyName = "No publisher"
                    };
                }

                return View("GameDetails", gameForView);
            }

            _logger.Warn($"Admin didn't get game details by key: {key}");

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<string> GetImageAsync(string key)
        {
            var image = await _gameService.AsyncGetImageByKey(key);

            return image.Name;
        }

        public string GetImage(string key)
        {
            var image = _gameService.GetImageByKey(key);

            return image.Name;
        }

        public ActionResult VoteForGame(string key, double mark, string userName)
        {
            _gameService.SetRating(key, mark, userName);

            return Filters(new FilterViewModel(), 1);
        }

        public ActionResult Recommended(string username, int page = 1)
        {
            var games = new List<GameDTO>();

            if (username == "anonym")
            {
                var allGames = _gameService.GetAllGames().ToList();
                games = allGames.OrderBy(i => i.Rating).Where(i => i.Rating >= 4).ToList();
            }
            else
            {
                var user = _userService.GetUser(username);
                var markedGamesByUser = user.Rates.Select(i => i.Game);
                markedGamesByUser = markedGamesByUser.Where(i => i.Rating >= 4).ToList();

                if (markedGamesByUser.Any())
                {
                    games.AddRange(markedGamesByUser.OrderBy(i => i.Rating));

                    var genresMarkedGamesByUser = markedGamesByUser.Select(i => i.Genres);
                    var gamesWithRelativeGenres = new List<GameDTO>();

                    foreach (var g in genresMarkedGamesByUser)
                    {
                        foreach (var genre in g)
                        {
                            var temp = genre.Games.Where(i => i.Rating >= 4).ToList();
                            gamesWithRelativeGenres.AddRange(temp);
                        }
                    }

                    games.AddRange(gamesWithRelativeGenres.Distinct().OrderBy(i => i.Rating));
                }

            }

            var genres = _genreService.GetAll(CurrentLangCode).ToList();
            var platforms = _platformService.GetAll(CurrentLangCode).ToList();
            var publishers = _publisherService.GetAllPublisher(CurrentLangCode).ToList();

            var filter = new FilterViewModel
            {
                Genres = Mapper.Map<List<GenreViewModel>>(genres),
                Platforms = Mapper.Map<List<PlatformViewModel>>(platforms),
                Publishers = Mapper.Map<List<PublisherViewModel>>(publishers),
                Games = new List<GameViewModel>(),
                GameNames = games.Select(game => game.Name).ToList(),
                GenresId = new List<int>(),
                PlatformsId = new List<int>(),
                PublishersId = new List<int>()
            };

            foreach (var genreDto in genres)
            {
                foreach (var genreVm in filter.Genres)
                {
                    if (genreDto.Id == genreVm.ParentId)
                    {
                        genreVm.ParentName = genreDto.Name;
                    }
                }
            }

            int pageSize = 10;
            filter.Games = Mapper.Map<List<GameViewModel>>(games.Skip((page - 1) * pageSize).Take(pageSize).ToList());
            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = games.Count };
            var indexView = new FilterIndexViewModel { PageInfo = pageInfo, FilterModel = filter };
            indexView.FilterModel.NumberOfItemPage = 10;

            return View("GameFilters", indexView);
        }
    }
}