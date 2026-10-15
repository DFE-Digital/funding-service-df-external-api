using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalForms.API.Controllers;
using DigitalForms.BL.Interfaces;
using DigitalForms.BL.Models;
using DigitalForms.BL.Serialized.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DigitalForms.Test
{
    [TestFixture]
    public class DFControllerUnitTests
    {
        private Mock<IUnitOfWork> _mockUow = null!;
        private Mock<DigitalForms.BL.Interfaces.ISerializer> _mockSerializer = null!;

        [SetUp]
        public void Setup()
        {
            _mockUow = new Mock<IUnitOfWork>();
            // Default: make FormsRepository non-null to avoid NotFound in most tests
            _mockUow.SetupGet(u => u.FormsRepository).Returns(Mock.Of<IFormsRepository>());
            // Default: make ResponsesRepository non-null to avoid NotFound for response-related endpoints
            _mockUow.SetupGet(u => u.ResponsesRepository).Returns(Mock.Of<IResponsesRepository>());
            // Default: make ResponseRepository non-null (singular) - used by createDocument and others
            _mockUow.SetupGet(u => u.ResponseRepository).Returns(Mock.Of<IGenericRepository<Response>>());
            // Default: make RepeatableFormsDataRepository non-null - used by repeatable forms endpoints
            _mockUow.SetupGet(u => u.RepeatableFormsDataRepository).Returns(Mock.Of<IGenericRepository<DigitalForms.BL.Models.RepeatableFormsData>>());
            _mockSerializer = new Mock<DigitalForms.BL.Interfaces.ISerializer>();
        }

        private DFController CreateController()
        {
            var controller = new DFController(_mockUow.Object, _mockSerializer.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            return controller;
        }

        [Test]
        public async Task GetConfiguration_ReturnsOk_FromSerializer()
        {
            var expected = new Forms { id = "f1" };
            _mockSerializer.Setup(s => s.getConfiguration("f1", It.IsAny<bool>())).Returns(expected);

            var controller = CreateController();
            var result = await controller.getConfiguration("f1");

            Assert.IsInstanceOf<ActionResult<Forms>>(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(expected, ok.Value);
        }

        [Test]
        public async Task ListFormConfigurations_ReturnsOk_FromSerializer()
        {
            var list = new List<FormConfiguration> { new FormConfiguration { Key = "k" } };
            _mockSerializer.Setup(s => s.listFormConfigurations()).Returns(list);

            var controller = CreateController();
            var result = await controller.listFormConfigurations();

            Assert.IsInstanceOf<ActionResult<IEnumerable<FormConfiguration>>>(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(list, ok.Value);
        }

        [Test]
        public async Task CheckFormExists_ReturnsOk_Bool()
        {
            _mockSerializer.Setup(s => s.checkFormExists(null, "name")).Returns(true);
            var controller = CreateController();
            var result = await controller.checkFormExists("name");
            Assert.IsInstanceOf<ActionResult<bool>>(result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task AddConfiguration_ReturnsConflict_WhenExists()
        {
            var forms = new Forms { id = "f1" };
            _mockSerializer.Setup(s => s.checkFormExists(forms.id, null)).Returns(true);
            var controller = CreateController();
            var result = await controller.addConfiguration(forms);
            Assert.IsInstanceOf<ActionResult<Forms>>(result);
            Assert.IsInstanceOf<ConflictObjectResult>(result.Result);
        }

        [Test]
        public async Task AddConfiguration_ReturnsOk_WhenCreated()
        {
            var forms = new Forms { id = "f2" };
            _mockSerializer.Setup(s => s.checkFormExists(forms.id, null)).Returns(false);
            _mockSerializer.Setup(s => s.addConfiguration(forms, It.IsAny<StringBuilder>())).ReturnsAsync(forms);
            var controller = CreateController();
            var result = await controller.addConfiguration(forms);
            Assert.IsInstanceOf<ActionResult<Forms>>(result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(forms, ok.Value);
        }

        [Test]
        public async Task DeleteConfiguration_ReturnsConflict_WhenNotExists()
        {
            _mockSerializer.Setup(s => s.checkFormExists("nof", null)).Returns(false);
            var controller = CreateController();
            var result = await controller.deleteConfiguration("nof");
            Assert.IsInstanceOf<ConflictObjectResult>(result);
        }

        [Test]
        public async Task DeleteConfiguration_ReturnsOk_WhenDeleted()
        {
            _mockSerializer.Setup(s => s.checkFormExists("f1", null)).Returns(true);
            _mockSerializer.Setup(s => s.deleteConfiguration("f1", It.IsAny<StringBuilder>())).ReturnsAsync(1);
            var controller = CreateController();
            var result = await controller.deleteConfiguration("f1");
            Assert.IsInstanceOf<OkObjectResult>(result);
            var ok = (OkObjectResult)result;
            Assert.AreEqual("Form data deleted successfully.", ok.Value);
        }

        [Test]
        public async Task UploadConfiguration_ReturnsNotFound_IfRepoNull()
        {
            _mockUow.SetupGet(u => u.FormsRepository).Returns((IFormsRepository)null);
            var controller = CreateController();
            var res = await controller.uploadConfiguration(new Forms { id = "x" });
            Assert.IsInstanceOf<ActionResult<Forms>>(res);
            Assert.IsInstanceOf<NotFoundResult>(res.Result);
        }

        [Test]
        public async Task CreateDocument_CallsSerializer()
        {
            var resp = new DigitalForms.BL.Serialized.Models.Responses { id = "r1" };
            _mockSerializer.Setup(s => s.createResponseDoc(It.IsAny<DigitalForms.BL.Serialized.Models.Responses>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.createDocument(resp);
            Assert.IsInstanceOf<ActionResult<bool>>(result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task UploadProvidersMapping_ReturnsOk()
        {
            _mockSerializer.Setup(s => s.uploadProvidersMapping(It.IsAny<DigitalForms.BL.Serialized.Models.ProviderMappings>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
            var controller = CreateController();
            var payload = new DigitalForms.BL.Serialized.Models.ProviderMappingsPayload { id = "p1", providers = "1,2", date = "d" };
            var result = await controller.uploadProvidersMapping(payload);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task CheckProvidersMappingById_ReturnsOkBool()
        {
            _mockSerializer.Setup(s => s.CheckProvidersMappingById("id", 1, 0, null, It.IsAny<StringBuilder>())).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.CheckProvidersMappingById("id", 1, 0, null);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task GetResponses_ReturnsEmptyResult_WhenNoParams()
        {
            var controller = CreateController();
            var result = await controller.GetResponses(null, null, null);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task CheckMultipleFormExists_ReturnsOk_List()
        {
            var expected = new List<KeyValuePair<string, bool>> { new KeyValuePair<string, bool>("a", true) };
            _mockSerializer.Setup(s => s.checkMultipleFormExists(It.IsAny<string[]>())).Returns(expected);
            var controller = CreateController();
            var result = await controller.checkMultipleFormExists(new string[] { "a" });
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(expected, ok.Value);
        }

        [Test]
        public async Task ChangeMultipleFormStatus_CallsSerializer_ForEach()
        {
            _mockSerializer.Setup(s => s.changeformstatus(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var controller = CreateController();
            var payload = new List<Dictionary<string, string>> { new Dictionary<string, string> { { "FormId", "f1" }, { "Status", "new" } } };
            var result = await controller.changeMultipleformstatus(payload);
            var ok = (OkObjectResult)result.Result;
            var list = (List<KeyValuePair<string, bool>>)ok.Value;
            Assert.IsTrue(list.Any(kv => kv.Key == "f1" && kv.Value == true));
        }

        [Test]
        public async Task DeleteMultipleConfiguration_ReturnsOk_WhenAllDeleted()
        {
            _mockSerializer.Setup(s => s.checkFormExists("f1", null)).Returns(true);
            _mockSerializer.Setup(s => s.deleteConfiguration("f1", It.IsAny<StringBuilder>())).ReturnsAsync(1);
            var controller = CreateController();
            var result = await controller.deleteMultipleConfiguration(new string[] { "f1" });
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateParentChild_ReturnsOk()
        {
            _mockSerializer.Setup(s => s.updateParentChild(It.IsAny<DigitalForms.BL.Serialized.Models.ParentChildUpdate>())).ReturnsAsync(true);
            var controller = CreateController();
            var data = new List<DigitalForms.BL.Serialized.Models.ParentChildUpdate> { new DigitalForms.BL.Serialized.Models.ParentChildUpdate { tableName = "Forms", formId = "f1", FieldChanges = new Dictionary<string, object>() } };
            var result = await controller.updateParentChild(data);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual("Updated tables", ok.Value);
        }

        [Test]
        public async Task GetSubmittedStatusByParentId_ReturnsOk()
        {
            _mockUow.SetupGet(u => u.ResponsesRepository).Returns(Mock.Of<IResponsesRepository>());
            var expected = new List<Tuple<string, string, string>> { Tuple.Create("a", "b", "c") };
            _mockSerializer.Setup(s => s.getSubmittedStatusbyParentId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(expected);
            var controller = CreateController();
            controller.Request.Headers["formid"] = "f1";
            controller.Request.Headers["ukprn"] = "1";
            controller.Request.Headers["isUAT"] = "false";
            var result = await controller.getSubmittedStatusbyParentId();
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(expected, ok.Value);
        }

        [Test]
        public async Task GetResponseById_ReturnsOkBool()
        {
            _mockUow.SetupGet(u => u.ResponsesRepository).Returns(Mock.Of<IResponsesRepository>());
            _mockSerializer.Setup(s => s.getResponseById("r1")).Returns(true);
            var controller = CreateController();
            var result = await controller.getResponseById("r1");
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task PostDraftResponse_ReturnsOk()
        {
            _mockSerializer.Setup(s => s.AddorUpdateDraftResponse(It.IsAny<Newtonsoft.Json.Linq.JObject>())).ReturnsAsync(true);
            var controller = CreateController();
            var json = JObject.Parse("{ \"id\": \"d1\" }");
            var result = await controller.PostDraftResponse(json);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task GetDraftResponse_ReturnsOk_Dictionary()
        {
            var dict = new Dictionary<string, object> { { "a", 1 } };
            _mockSerializer.Setup(s => s.GetDraftResponse("f1")).ReturnsAsync(dict);
            var controller = CreateController();
            var result = await controller.GetDraftResponse("f1");
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(dict, ok.Value);
        }

        [Test]
        public async Task GetSubmissionFormLog_ReturnsOk()
        {
            var sub = new SubmissionForm { id = "s1" };
            _mockSerializer.Setup(s => s.GetSubmissionFormLog("s1")).ReturnsAsync(sub);
            var controller = CreateController();
            var result = await controller.GetSubmissionFormLog("s1");
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(sub, ok.Value);
        }

        [Test]
        public async Task PostSubmissionFormLog_ReturnsOkBool()
        {
            var submission = new SubmissionForm { id = "s1" };
            _mockSerializer.Setup(s => s.PostSubmissionFormLog(submission)).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.PostSubmissionFormLog(submission);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task GetDocumentCapture_ReturnsOk()
        {
            var doc = new DocData { fileId = "d1" };
            _mockSerializer.Setup(s => s.GetDocumentCapture("d1")).ReturnsAsync(doc);
            var controller = CreateController();
            var result = await controller.getDocumentCapture("d1");
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(doc, ok.Value);
        }

        [Test]
        public async Task PostDocumentCaptureData_ReturnsOkBool()
        {
            var data = new DocData { fileId = "d1" };
            _mockSerializer.Setup(s => s.PostDocumentCapture(data)).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.PostDocumentCaptureData(data);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task GetRepeatableFormsData_ReturnsOk()
        {
            var form = new Forms { id = "f1" };
            _mockSerializer.Setup(s => s.GetRepeatableFormsData("f1", It.IsAny<Dictionary<string, string>>(), It.IsAny<bool>())).ReturnsAsync(form);
            var controller = CreateController();
            var result = await controller.GetRepeatableFormsData("f1", false);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(form, ok.Value);
        }

        [Test]
        public async Task PostRepeatableFormsData_ReturnsOkBool()
        {
            _mockSerializer.Setup(s => s.PostRepeatableFormsData(It.IsAny<Newtonsoft.Json.Linq.JObject>())).ReturnsAsync(true);
            var controller = CreateController();
            var json = JObject.Parse("{ \"dummy\": 1 }");
            var result = await controller.PostRepeatableFormsData(json);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        [Test]
        public async Task DeleteRepeatableFormsData_ReturnsOkBool()
        {
            _mockSerializer.Setup(s => s.DeleteRepeatableFormsData("id1")).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.DeleteRepeatableFormsData("id1");
            // Controller returns the bool directly as the ActionResult value
            Assert.IsTrue(result.Value);
        }

        [Test]
        public async Task GetUserDetailById_ReturnsOk()
        {
            var user = new UserDetail { Uid = 1, Name = "Test" };
            _mockSerializer.Setup(s => s.GetUser(It.IsAny<Guid>())).ReturnsAsync(user);
            var controller = CreateController();
            var result = await controller.getUserDetailById(Guid.NewGuid());
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(user, ok.Value);
        }

        [Test]
        public async Task UpdateUserDetail_ReturnsOkBool()
        {
            var user = new UserDetail { Uid = 1, Name = "Test" };
            _mockSerializer.Setup(s => s.updateUser(user)).ReturnsAsync(true);
            var controller = CreateController();
            var result = await controller.updateUserDetail(user);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(true, ok.Value);
        }

        // Helper to create IAsyncEnumerable for mocks
        private static async IAsyncEnumerable<object> ToAsyncEnum(params object[] items)
        {
            foreach (var i in items)
            {
                yield return i;
                await Task.Yield();
            }
        }

        [Test]
        public async Task UploadMultipleProvidersMapping_ReturnsOk()
        {
            _mockSerializer.Setup(s => s.uploadProvidersMapping(It.IsAny<ProviderMappings>(), It.IsAny<StringBuilder>())).ReturnsAsync(true);
            var controller = CreateController();
            var payload = new ProviderMappingsPayload { id = "f1,f2", providers = "1,2", date = "d" };
            var result = await controller.uploadMultipleProvidersMapping(payload);
            Assert.IsInstanceOf<ActionResult<string>>(result);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual("Ukprns Updated", ok.Value);
        }

        [Test]
        public async Task UploadConfiguration_Updates_WhenExists()
        {
            var forms = new Forms { id = "f1" };
            _mockSerializer.Setup(s => s.checkFormExists(forms.id, null)).Returns(true);
            _mockSerializer.Setup(s => s.UpdateConfiguration(forms, It.IsAny<StringBuilder>())).ReturnsAsync(forms);
            var controller = CreateController();
            var result = await controller.uploadConfiguration(forms);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(forms, ok.Value);
        }

        [Test]
        public async Task UploadConfiguration_Adds_WhenNotExists()
        {
            var forms = new Forms { id = "f2" };
            _mockSerializer.Setup(s => s.checkFormExists(forms.id, null)).Returns(false);
            _mockSerializer.Setup(s => s.addConfiguration(forms, It.IsAny<StringBuilder>())).ReturnsAsync(forms);
            var controller = CreateController();
            var result = await controller.uploadConfiguration(forms);
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(forms, ok.Value);
        }

        [Test]
        public async Task GetResponses_ReturnsEmptyResult_WhenHasData()
        {
            _mockSerializer.Setup(s => s.getResponses("f1", "s", "e")).Returns(ToAsyncEnum("r1", "r2"));
            var controller = CreateController();
            var result = await controller.GetResponses("f1", "s", "e");
            Assert.IsInstanceOf<EmptyResult>(result);
        }

        [Test]
        public async Task GetResponseQuestion_ReturnsEmptyResult_WhenHasData()
        {
            _mockSerializer.Setup(s => s.getResponseQuestion("f1", "s", "e")).Returns(ToAsyncEnum(new { q = 1 }));
            var controller = CreateController();
            var result = await controller.GetResponseQuestion("f1", "s", "e");
            Assert.IsInstanceOf<EmptyResult>(result);
        }

        [Test]
        public async Task GetResponseQuestionData_ReturnsEmptyResult_WhenHasData()
        {
            _mockSerializer.Setup(s => s.getResponseQuestionData("f1", "s", "e")).Returns(ToAsyncEnum(new { d = 1 }));
            var controller = CreateController();
            var result = await controller.GetResponseQuestionData("f1", "s", "e");
            Assert.IsInstanceOf<EmptyResult>(result);
        }

        [Test]
        public async Task GetDraftResponse_ReturnsOk()
        {
            var dict = new Dictionary<string, object> { { "k", "v" } };
            _mockSerializer.Setup(x => x.GetDraftResponse("id1")).ReturnsAsync(dict);
            var controller = CreateController();
            var result = await controller.GetDraftResponse("id1");
            var ok = (OkObjectResult)result.Result;
            Assert.AreEqual(dict, ok.Value);
        }
    }
}
