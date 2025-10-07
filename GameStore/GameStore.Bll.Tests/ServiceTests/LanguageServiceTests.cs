using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class LanguageServiceTests
    {
        private LanguageService _languageService;
        private Mock<IUnitOfWork> _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _languageService = new LanguageService(_unitOfWork.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void GetAll_GetLamguages_GetNotNull()
        {
            IEnumerable<Language> languages = new List<Language>();
            _unitOfWork.Setup(unit => unit.LanguageRepository.Get()).Returns(languages);

            var result = _languageService.GetAll();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAll_GetLamguages_GetNotEmpty()
        {
            IEnumerable<Language> languages = new List<Language>
            {
                new Language()
            };
            _unitOfWork.Setup(unit => unit.LanguageRepository.Get()).Returns(languages);

            var result = _languageService.GetAll();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetById_GetLamguages_GetNotNull()
        {
            Language language = new Language();
            _unitOfWork.Setup(unit => unit.LanguageRepository.FindById(It.IsAny<int>()))
                .Returns(language);

            var result = _languageService.GetById(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetById_GetLamguages_GetWithCorrectId()
        {
            Language language = new Language
            {
                Id = 1
            };
            _unitOfWork.Setup(unit => unit.LanguageRepository.FindById(It.IsAny<int>()))
                .Returns(language);

            var result = _languageService.GetById(1);

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetByKey_GetLamguages_GetNotNull()
        {
            IEnumerable<Language> language = new List<Language>
            {
                new Language()
            };
            _unitOfWork.Setup(unit => unit.LanguageRepository
                    .Get(It.IsAny<Expression<Func<Language, bool>>>(),
                        It.IsAny<Expression<Func<Language, object>>[]>()))
                .Returns(language);

            var result = _languageService.GetByKey("En");

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetByKey_GetLamguages_GetWithCorrectId()
        {
            IEnumerable<Language> languages = new List<Language>
            {
                new Language
                {
                    Key = "En"
                }
            };
            _unitOfWork.Setup(unit => unit.LanguageRepository
                    .Get(It.IsAny<Expression<Func<Language, bool>>>(),
                        It.IsAny<Expression<Func<Language, object>>[]>()))
                .Returns(languages);

            var result = _languageService.GetByKey("En");

            Assert.AreEqual("En", result.Key);
        }
    }
}