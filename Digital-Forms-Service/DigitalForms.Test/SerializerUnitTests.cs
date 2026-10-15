using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DigitalForms.BL.Interfaces;
using DigitalForms.BL.Models;
using DigitalForms.BL.Serialized.Models;
using Moq;
using NUnit.Framework;

namespace DigitalForms.Test
{
    [TestFixture]
    public class SerializerUnitTests
    {
        [Test]
        public void CheckFormExists_DelegatesToRepository()
        {
            var mockFormsRepo = new Mock<IFormsRepository>();
            mockFormsRepo.Setup(r => r.checkFormExists("f1", null)).Returns(true);

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.SetupGet(u => u.FormsRepository).Returns(mockFormsRepo.Object);

            var serializer = new Serializer(mockUow.Object);

            var result = serializer.checkFormExists("f1", null);

            Assert.IsTrue(result);
            mockFormsRepo.Verify(r => r.checkFormExists("f1", null), Times.Once);
        }

        [Test]
        public void CheckMultipleFormExists_ReturnsRepositoryResult()
        {
            var input = new string[] { "a", "b" };
            var expected = new List<KeyValuePair<string, bool>> { new KeyValuePair<string, bool>("a", true), new KeyValuePair<string, bool>("b", false) };

            var mockFormsRepo = new Mock<IFormsRepository>();
            mockFormsRepo.Setup(r => r.checkMultipleFormExists(input)).Returns(expected);

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.SetupGet(u => u.FormsRepository).Returns(mockFormsRepo.Object);

            var serializer = new Serializer(mockUow.Object);

            var result = serializer.checkMultipleFormExists(input);

            Assert.AreEqual(expected, result);
            mockFormsRepo.Verify(r => r.checkMultipleFormExists(input), Times.Once);
        }

        [Test]
        public void CheckResponseExists_ReturnsTrueWhenFound()
        {
            var mockResponseRepo = new Mock<DigitalForms.BL.Interfaces.IGenericRepository<DigitalForms.BL.Models.Response>>();
            mockResponseRepo.Setup(r => r.Find(It.IsAny<Expression<Func<DigitalForms.BL.Models.Response, bool>>>()))
                .Returns(new List<DigitalForms.BL.Models.Response> { new DigitalForms.BL.Models.Response { Id = "resp-1" } });

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.SetupGet(u => u.ResponseRepository).Returns(mockResponseRepo.Object);

            var serializer = new Serializer(mockUow.Object);

            var result = serializer.checkResponseExists("resp-1");

            Assert.IsTrue(result);
            mockResponseRepo.Verify(r => r.Find(It.IsAny<Expression<Func<DigitalForms.BL.Models.Response, bool>>>()), Times.Once);
        }

        [Test]
        public async Task ChangeFormStatus_UpdatesStatus_WhenFormAndStatusExist()
        {
            var form = new Form { FormId = "f1", Fid = 10, Fsid = 1 };

            var mockFormsRepo = new Mock<IFormsRepository>();
            mockFormsRepo.Setup(r => r.getFormById("f1")).Returns(form);
            mockFormsRepo.Setup(r => r.Update(It.IsAny<Form>()));

            var formStatuses = new List<FormStatus> { new FormStatus { Fsid = 2, Status = "new" } };
            var mockFormStatusRepo = new Mock<DigitalForms.BL.Interfaces.IGenericRepository<FormStatus>>();
            mockFormStatusRepo.Setup(r => r.GetAll()).Returns(formStatuses);

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.SetupGet(u => u.FormsRepository).Returns(mockFormsRepo.Object);
            mockUow.SetupGet(u => u.formStatusRepository).Returns(mockFormStatusRepo.Object);
            mockUow.Setup(u => u.Complete()).ReturnsAsync(1);

            var serializer = new Serializer(mockUow.Object);

            var result = await serializer.changeformstatus("f1", "new");

            Assert.IsTrue(result);
            mockFormsRepo.Verify(r => r.Update(It.Is<Form>(f => f.Fsid == 2)), Times.Once);
            mockUow.Verify(u => u.Complete(), Times.Once);
        }
    }
}
