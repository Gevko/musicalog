using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel.Model;
using Services.AlbumNS;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using DataModel.DTO;
using musicalogProj.Controllers.Album;

namespace Tests.ControllerTests
{
    [TestClass]
    public class AlbumControllerTests
    {
        private const string ERROR = "ERROR";
        private const string SUCCESS = "SUCCESS";

        [TestInitialize]
        public void TestInitialize()
        {

        }

        #region GetAllFilteredAsync
        [TestMethod]
        public async Task GetAllFilteredAsync_OkSituationWithoutFilter_ReturnsOkObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.GetAllFilteredAsync(It.IsAny<string>()))
                .ReturnsAsync((true, SUCCESS));

                var albumsController = new AlbumController(mockAlbumService.Object) {};

                IActionResult result = await albumsController.GetAllFilteredAsync("");

                mockAlbumService.Verify(srv => srv.GetAllFilteredAsync(""));

                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var obj = result as OkObjectResult;

                Assert.AreEqual(SUCCESS, obj.Value);
            }
        }

        [TestMethod]
        public async Task GetAllFilteredAsync_ErrorSituationWithoutFilter_ReturnsBadRequestObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.GetAllFilteredAsync(It.IsAny<string>()))
                .ReturnsAsync((false, ERROR));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                IActionResult result = await albumsController.GetAllFilteredAsync("");

                mockAlbumService.Verify(srv => srv.GetAllFilteredAsync(""));

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var obj = result as BadRequestObjectResult;

                Assert.AreEqual(ERROR, obj.Value);
            }
        }

        #endregion GetAllFilteredAsync

        #region GetByIdAsync
        [TestMethod]
        public async Task GetByIdAsyncc_Ok_ReturnsOkObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((true, SUCCESS));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                Guid id = Guid.NewGuid();

                IActionResult result = await albumsController.GetByIdAsync(id);

                mockAlbumService.Verify(srv => srv.GetByIdAsync(id));

                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var obj = result as OkObjectResult;

                Assert.AreEqual(SUCCESS, obj.Value);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ErrorSituation_ReturnsBadRequestObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((false, ERROR));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                Guid id = Guid.NewGuid();

                IActionResult result = await albumsController.GetByIdAsync(id);

                mockAlbumService.Verify(srv => srv.GetByIdAsync(id));

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var obj = result as BadRequestObjectResult;

                Assert.AreEqual(ERROR, obj.Value);
            }
        }
        #endregion GetByIdAsync

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync_ErrorSituation_ReturnsBadRequestObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.UpdateAsync(It.IsAny<AlbumDTO>()))
                .ReturnsAsync((false, ERROR));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                AlbumDTO album = new AlbumDTO();

                IActionResult result = await albumsController.UpdateAsync(album);

                mockAlbumService.Verify(srv => srv.UpdateAsync(album));

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var obj = result as BadRequestObjectResult;

                Assert.AreEqual(SUCCESS, obj.Value);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_OkSituation_ReturnsNoContentResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.UpdateAsync(It.IsAny<AlbumDTO>()))
                .ReturnsAsync((true, null));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                AlbumDTO album = new AlbumDTO();

                IActionResult result = await albumsController.UpdateAsync(album);

                mockAlbumService.Verify(srv => srv.UpdateAsync(album));

                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }
        #endregion UpdateAsync

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync_OkSituation_ReturnsOkObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.CreateAsync(It.IsAny<AlbumDTO>()))
                .ReturnsAsync((true, SUCCESS));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                AlbumDTO album = new AlbumDTO();

                IActionResult result = await albumsController.CreateAsync(album);

                mockAlbumService.Verify(srv => srv.CreateAsync(album));

                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var obj = result as OkObjectResult;

                Assert.AreEqual(SUCCESS, obj.Value);
            }
        }

        [TestMethod]
        public async Task CreateAsync_ErrorSituation_ReturnsBadRequestObject()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.CreateAsync(It.IsAny<AlbumDTO>()))
                .ReturnsAsync((false, ERROR));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                AlbumDTO album = new AlbumDTO();

                IActionResult result = await albumsController.CreateAsync(album);

                mockAlbumService.Verify(srv => srv.CreateAsync(album));

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var obj = result as BadRequestObjectResult;

                Assert.AreEqual(ERROR, obj.Value);
            }
        }
        #endregion CreateAsync

        #region DeleteAsync
        [TestMethod]
        public async Task GetAllFilteredAsync_OkSituation_ReturnsOkObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((true, SUCCESS));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                Guid id = Guid.NewGuid();

                IActionResult result = await albumsController.DeleteAsync(id);

                mockAlbumService.Verify(srv => srv.DeleteAsync(id));

                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var obj = result as OkObjectResult;

                Assert.AreEqual(SUCCESS, obj.Value);
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ErrorSituation_ReturnsBadRequestObjectResult()
        {
            using (var context = new Context())
            {
                var mockAlbumService = new Mock<IAlbumService>();

                mockAlbumService.Setup(srv => srv.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((false, ERROR));

                var albumsController = new AlbumController(mockAlbumService.Object) { };

                Guid id = Guid.NewGuid();

                IActionResult result = await albumsController.DeleteAsync(id);

                mockAlbumService.Verify(srv => srv.DeleteAsync(id));

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var obj = result as BadRequestObjectResult;

                Assert.AreEqual(ERROR, obj.Value);
            }
        }
        #endregion DeleteAsync
    }
}
