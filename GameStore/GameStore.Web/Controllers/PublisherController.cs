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
    public class PublisherController : BaseController
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger _logger;
        private readonly ILanguageService _languageService;

        public PublisherController(
            IPublisherService publisherService,
            ILogger logger,
            IAuthentication auth,
            ILanguageService lang) : base(auth)
        {
            _publisherService = publisherService;
            _logger = logger;
            _languageService = lang;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ViewResult NewPublisher()
        {
            var newPiblisher = new PublisherViewModel
            {
                PublisherTranslate = new List<PublisherTranslateModel>
                {
                    new PublisherTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new PublisherTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
                }
            };
            return View("NewPublisher", newPiblisher);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult NewPublisher(PublisherViewModel newPublisher)
        {
            if (ModelState.IsValid)
            {
                var publisherDto = Mapper.Map<PublisherViewModel, PublisherDTO>(newPublisher);

                _publisherService.NewPublisher(publisherDto);
                _logger.Info("New publisher was added");
                return GetAll();
            }

            newPublisher.PublisherTranslate = new List<PublisherTranslateModel>
                {
                    new PublisherTranslateModel
                    {
                        LanguageId = 1,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(1))
                    },
                    new PublisherTranslateModel
                    {
                        LanguageId = 2,
                        Language = Mapper.Map<LanguageModel>(_languageService.GetById(2))
                    }
            };
            return View("NewPublisher", newPublisher);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PublisherDetailsById(int id)
        {
            if (id != 0)
            {
                var publisher = _publisherService.GetById(id, CurrentLangCode);
                if (publisher != null)
                {
                    var publisherVm = Mapper.Map<PublisherDTO, PublisherViewModel>(publisher);
                    return View("PublisherDetails", publisherVm);
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PublisherDetails(string companyName)
        {
            if (!string.IsNullOrEmpty(companyName))
            {
                var publisher = _publisherService.GetByName(companyName, CurrentLangCode);
                if (publisher != null)
                {
                    var publisherVm = Mapper.Map<PublisherDTO, PublisherViewModel>(publisher);
                    return View("PublisherDetails", publisherVm);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        public ActionResult GetAll()
        {
            var publisher = _publisherService.GetAllPublisher(CurrentLangCode).ToList();
            if (publisher.Any())
            {
                var publisherVm = Mapper.Map<List<PublisherDTO>, List<PublisherViewModel>>(publisher);
                _logger.Info("User got all publishers");
                return View("AllPublishers", publisherVm);
            }

            return View("AllPublishers", new List<PublisherViewModel>());
        }

        [Authorize(Roles = "Manager")]
        public ActionResult RemovePublisher(string companyname)
        {
            _publisherService.RemovePublisher(companyname);
            _logger.Info($"Publisher with name:{companyname} was removed");
            return GetAll();
        }

        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult EditPublisher(string companyname)
        {
            var publisher = _publisherService.GetByName(companyname, CurrentLangCode);

            if (publisher != null)
            {
                var publisherVm = Mapper.Map<PublisherDTO, PublisherViewModel>(publisher);
                return View(publisherVm);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult Edit(PublisherDTO editPublisher)
        {
            _publisherService.EditPublisher(editPublisher);
            _logger.Info($"Publisher with id:{editPublisher.Id} was edited");
            return GetAll();
        }
    }
}