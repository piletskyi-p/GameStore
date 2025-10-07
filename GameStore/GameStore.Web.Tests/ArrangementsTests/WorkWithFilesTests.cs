using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using GameStore.Bll.DTO;
using GameStore.Web.Arrangements;
using NUnit.Framework;

namespace GameStore.Web.Tests.ArrangementsTests
{
    public class WorkWithFilesTests
    {
        private WorkWithFiles _workWithFiles;

        [SetUp]
        public void Setup()
        {
            _workWithFiles = new WorkWithFiles();
        }

        [Test]
        public void CreateFilePdf_GetFile_ReturnCorrectType()
        {
            var orderDto = new OrderDTO
            {
                OrderDetails = new List<OrderDetailsDTO>()
            };
            var games = new List<GameDTO>
            {
                new GameDTO(),
                new GameDTO()
            };
            MemoryStream Stream = new MemoryStream();
            var file = new FileStreamResult(Stream, "ME");

            var result = _workWithFiles.CreateFilePdf(orderDto, games);

            Assert.AreEqual(file.GetType(), result.GetType());
        }
    }
}