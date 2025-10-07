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
    public class PublishersController : BaseApiController
    {
        private readonly IPublisherService _publisherService;
        private readonly ILanguageService _languageService;

        public PublishersController(IPublisherService publisherService, ILanguageService languageService)
        {
            _publisherService = publisherService;
            _languageService = languageService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var publisherDto = _publisherService.GetAllPublisher(CurrentLangCode).ToList();
            foreach (var publisher in publisherDto)
            {
                foreach (var game in publisher.Games)
                {
                    game.Genres.Clear();
                    game.Platforms.Clear();
                    game.Publisher = null;
                }
            }

            return Content(HttpStatusCode.OK, publisherDto);
        }

        public IHttpActionResult GetDetails(int id)
        {
            var publisherDto = _publisherService.GetById(id, CurrentLangCode);

            if (publisherDto == null)
            {
                return NotFound();
            }

            foreach (var game in publisherDto.Games)
            {
                game.Genres.Clear();
                game.Platforms.Clear();
                game.Publisher = null;
            }

            return Content(HttpStatusCode.OK, publisherDto);
        }

        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Post([FromBody]PublisherViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var publisherDto = Mapper.Map<PublisherDTO>(value);
            _publisherService.NewPublisher(publisherDto);

            return Content(HttpStatusCode.OK, "Publisher was changed");
        }

        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Put(int id, [FromBody]PublisherViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var publisherDto = Mapper.Map<PublisherDTO>(value);
            if (publisherDto.PublisherTranslate != null && publisherDto.PublisherTranslate.Any())
            {
                foreach (var translate in publisherDto.PublisherTranslate)
                {
                    translate.Language = _languageService.GetById(translate.LanguageId);
                }
            }

            _publisherService.EditPublisher(publisherDto);

            return Content(HttpStatusCode.Created, "Publisher was added");
        }

        [Authorize(Roles = "Publisher,Manager")]
        public IHttpActionResult Delete(int id)
        {
            _publisherService.RemovePublisherById(id);

            return Content(HttpStatusCode.OK, "Publisher was deleted");
        }

        [HttpGet]
        public IHttpActionResult Games(int id)
        {
            var publisher = _publisherService.GetById(id, "en");
            if (publisher == null)
            {
                return NotFound();
            }

            var games = publisher.Games.ToList();
         
            return Content(HttpStatusCode.OK, Mapper.Map<List<GameDTO>>(games));
        }
    }
}