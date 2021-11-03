using DataModel.Model;
using DataModel.Mapping;
using DataModel.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.AlbumNS;
using AutoMapper;
using System;
using Tests.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.ErrorCodesNS;
using System.Linq;
using DataModel.Enums;

namespace AlbumTests
{
    [TestClass]
    public class AlbumServiceTests
    {
        private Context context;
        private AlbumService albumService;
        private MapperConfiguration mapperConfiguration;
        private Mapper autoMapper;
        private TestingFactory contextFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            contextFactory = new TestingFactory();
            context = contextFactory.Context;
            mapperConfiguration = new MapperConfiguration(c => c.AddMaps(new List<Type> { typeof(AlbumMapping)}));
            autoMapper = new Mapper(mapperConfiguration);
            albumService = new AlbumService(context);
        }

        // Test names explained: MethodsName_ConditionToBeTested_Return

        #region GetAllFilteredAsync

        [TestMethod]
        public async Task GetAllFilteredAsync_AllOkWithoutFilter_ReturnsTrueAndAllAlbums()
        {
            (bool success, object result) = await albumService.GetAllFilteredAsync();

            Assert.IsTrue(success);

            Assert.IsInstanceOfType(result, typeof(List<AlbumDTO>));

            List<AlbumDTO> resultAsList = (List<AlbumDTO>)result;

            Assert.AreEqual(1, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetAllFilteredAsync_AllOkWithExistantAlbumsTitle_ReturnsTrueAndFilteredAlbum()
        {
            (bool success, object result) = await albumService.GetAllFilteredAsync(CreateTestData.FIRST_TITLE_NAME);

            Assert.IsTrue(success);

            Assert.IsInstanceOfType(result, typeof(List<AlbumDTO>));

            List<AlbumDTO> resultAsList = (List<AlbumDTO>)result;

            Assert.AreEqual(1, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetAllFilteredAsync_AllOkWithNonExistantAlbumsTitle_ReturnsTrueAndEmptyList()
        {
            (bool success, object result) = await albumService.GetAllFilteredAsync(CreateTestData.FIRST_TITLE_NAME);

            Assert.IsTrue(success);

            Assert.IsInstanceOfType(result, typeof(List<AlbumDTO>));

            List<AlbumDTO> resultAsList = (List<AlbumDTO>)result;

            Assert.AreEqual(0, resultAsList.Count);
        }

        #endregion GetAllFilteredAsync

        #region GetByIdAsync
        [TestMethod]
        public async Task GetByIdAsync_NonExistantId_ReturnsFalseAndNotFoundError()
        {
            (bool success, object result) = await albumService.GetByIdAsync(Guid.NewGuid());

            Assert.IsFalse(success);

            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.NotFound, resultAsEnum);
        }

        [TestMethod]
        public async Task GetByIdAsync_AllOkWithoutFilter_ReturnsTrueAndAllAlbums()
        {
            Album existantAlbum = context.Albums.FirstOrDefault(k => k.Title == CreateTestData.FIRST_TITLE_NAME);

            (bool success, object result) = await albumService.GetByIdAsync(existantAlbum.Id);

            Assert.IsTrue(success);

            Assert.IsInstanceOfType(result, typeof(AlbumDTO));

            AlbumDTO resultAsDTO = (AlbumDTO)result;

            Assert.AreEqual(existantAlbum.Id, resultAsDTO.Id); ;
        }

        #endregion GetByIdAsync

        #region UpdateAsync

        [TestMethod]
        public async Task UpdateAsync_AllOk_ReturnsTrueAndNull()
        {
            Album existantAlbum = context.Albums.FirstOrDefault(k => k.Title == CreateTestData.FIRST_TITLE_NAME);

            existantAlbum.Title = "New Art";

            (bool success, object result) = await albumService.UpdateAsync(autoMapper.Map<AlbumDTO>(existantAlbum));

            Assert.IsTrue(success);

            Assert.IsNull(result);

            Assert.AreEqual(existantAlbum.Title, context.Albums.FirstOrDefault(k => k.Id == existantAlbum.Id).Title);
        }

        [TestMethod]
        public async Task UpdateAsync_Null_ReturnsFalseAndNullError()
        {
            (bool success, object result) = await albumService.UpdateAsync(null);

            Assert.IsFalse(success);

            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.Null, resultAsEnum);
        }

        [TestMethod]
        public async Task UpdateAsync_NonExistantAlbum_ReturnsFalseAndNotFoundError()
        {
            AlbumDTO album = new AlbumDTO() { Id = Guid.NewGuid() };

            (bool success, object result) = await albumService.UpdateAsync(album);

            Assert.IsFalse(success);

            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.NotFound, resultAsEnum);
        }

        [TestMethod]
        public async Task UpdateAsync_EmptyMandatoryField_ReturnsFalseAndEmptyMandatoryFieldsError()
        {
            Album existantAlbum = context.Albums.FirstOrDefault(k => k.Title == CreateTestData.FIRST_TITLE_NAME);

            existantAlbum.Title = " ";

            (bool success, object result) = await albumService.UpdateAsync(autoMapper.Map<AlbumDTO>(existantAlbum));

            Assert.IsFalse(success);
            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.EmptyMandatoryFields, resultAsEnum);
        }

        [TestMethod]
        public async Task UpdateAsync_TitleDuplicated_ReturnsFalseAndTitleDuplicatedsError()
        {
            Album existantAlbum = context.Albums.FirstOrDefault(k => k.Title == CreateTestData.FIRST_TITLE_NAME);

            existantAlbum.Title = CreateTestData.SECOND_TITLE_NAME;

            (bool success, object result) = await albumService.UpdateAsync(autoMapper.Map<AlbumDTO>(existantAlbum));

            Assert.IsFalse(success);
            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.TitleDuplicated, resultAsEnum);
        }

        #endregion UpdateAsync

        #region CreateAsync
        [TestMethod]
        public async Task CreateAsync_AllOk_ReturnsTrueAndNull()
        {
            AlbumDTO album = new AlbumDTO() { Id = Guid.NewGuid(), ArtistName = "Bob Marley", Title = "Catch a Fire", Stock = 1, Type = AlbumType.Vinil };

            (bool success, object result) = await albumService.CreateAsync(album);

            Assert.IsTrue(success);

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result, typeof(AlbumDTO));

            AlbumDTO resultAsDTO = (AlbumDTO)result;

            Assert.AreEqual(album.Title, resultAsDTO.Title);

            Assert.AreEqual(album.ArtistName, resultAsDTO.ArtistName);

            Assert.AreEqual(album.Stock, resultAsDTO.Stock);

            Assert.AreEqual(album.Type, resultAsDTO.Type);

        }

        [TestMethod]
        public async Task CreateAsync_Null_ReturnsFalseAndNullError()
        {
            (bool success, object result) = await albumService.CreateAsync(null);

            Assert.IsFalse(success);

            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.Null, resultAsEnum);
        }


        [TestMethod]
        public async Task CreateAsync_EmptyMandatoryField_ReturnsFalseAndEmptyMandatoryFieldsError()
        {
            AlbumDTO album = new AlbumDTO() { Id = Guid.NewGuid(), ArtistName = "Bob Marley", Title = "", Stock = 1, Type = AlbumType.Vinil };

            (bool success, object result) = await albumService.CreateAsync(album);

            Assert.IsFalse(success);
            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.EmptyMandatoryFields, resultAsEnum);
        }

        [TestMethod]
        public async Task CreateAsync_TitleDuplicated_ReturnsFalseAndTitleDuplicatedsError()
        {
            AlbumDTO album = new AlbumDTO() { Id = Guid.NewGuid(), ArtistName = "Bob Marley", Title = CreateTestData.SECOND_TITLE_NAME, Stock = 1, Type = AlbumType.Vinil };

            (bool success, object result) = await albumService.CreateAsync(album);

            Assert.IsFalse(success);
            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.TitleDuplicated, resultAsEnum);
        }
        #endregion CreateAsync

        #region DeleteAsync

        [TestMethod]
        public async Task DeleteAsync_NonExistantAlbum_ReturnsFalseAndNotFound()
        {

            (bool success, object result) = await albumService.DeleteAsync(Guid.NewGuid());

            Assert.IsFalse(success);
            Assert.IsInstanceOfType(result, typeof(ErrorCodes));

            ErrorCodes resultAsEnum = (ErrorCodes)result;

            Assert.AreEqual(ErrorCodes.NotFound, resultAsEnum);
        }

        [TestMethod]
        public async Task DeleteAsync_AllOk_ReturnsTrueAndNull()
        {
            Album existantAlbum = context.Albums.FirstOrDefault(k => k.Title == CreateTestData.FIRST_TITLE_NAME);

            (bool success, object result) = await albumService.DeleteAsync(existantAlbum.Id);

            Assert.IsTrue(success);

            Assert.IsNull(result);
        }
        #endregion DeleteAsync
    }
}
