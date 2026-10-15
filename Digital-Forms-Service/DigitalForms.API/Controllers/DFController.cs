using DigitalForms.BL.Interfaces;
using DigitalForms.BL.Models;
using DigitalForms.BL.Serialized.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text; 

namespace DigitalForms.API.Controllers
{

    [ApiController]
    public class DFController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DigitalForms.BL.Interfaces.ISerializer _objserializer;
        public DFController(IUnitOfWork unitOfWork, DigitalForms.BL.Interfaces.ISerializer objserializer)
        {
            _unitOfWork = unitOfWork;
            _objserializer = objserializer;
        }


        // Get specific Forms or all forms.
        // GET: api/getConfiguration/abcdef
        // GET: api/getFormById/abcdef

        [HttpGet("api/getConfiguration/{formId}")]
        [HttpGet("api/getFormById/{formId}")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Forms>> getConfiguration(string formId)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            return Ok(await Task.FromResult(_objserializer.getConfiguration(formId.Trim() != string.Empty ? formId.Trim() : string.Empty)));
        }

        #region commenting out as it is not utilised by any application.
        // Get all Forms data.
        // GET: api/getAllForms
        //[HttpGet("api/getAllForms")]
        //[ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<Forms>>> getAllForms()
        //{
        //    if (_unitOfWork.FormsRepository == null)
        //        return NotFound();

        //    return Ok(await Task.FromResult(_objserializer.getConfiguration(string.Empty)));
        //}
        #endregion

        //Get dashboard data
        // GET: api/listFormConfigurations
        [HttpGet]
        [Route("api/listFormConfigurations")]
        [ProducesResponseType(typeof(FormConfiguration), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FormConfiguration>>> listFormConfigurations()
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            return Ok(await Task.FromResult(_objserializer.listFormConfigurations()));
        }

        // GET: api/checkFormExists/{formId}
        [HttpGet]
        [Route("api/checkFormExists/{formName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> checkFormExists(string? formName = null, string? formId = null)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            return Ok(await Task.FromResult(_objserializer.checkFormExists(formId, formName)));
        }


        //Add new form
        // Post: api/addConfiguration
        [HttpPost]
        [Route("api/addConfiguration")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Forms>> addConfiguration([FromBody] Forms forms)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();
            sw.Start();
            if (_objserializer.checkFormExists(forms.id, null))
                return Conflict(forms);
            sw.Stop();
            sb.AppendLine("finding form exists: " + sw.Elapsed.ToString());
            sw.Start();
            var result = await Task.FromResult(await _objserializer.addConfiguration(forms, sb));
            sw.Stop();
            sb.AppendLine("Add configuration: " + sw.Elapsed.ToString());
            return Ok(result);

        }

        [HttpDelete]
        [Route("api/deleteConfiguration/{formId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> deleteConfiguration(string formId)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();
            sw.Start();
            if (!_objserializer.checkFormExists(formId, null))
                return Conflict("Form doesnt exist in the db");
            sw.Stop();
            sb.AppendLine("finding form exists: " + sw.Elapsed.ToString());
            sw.Start();
            var result = await Task.FromResult(await _objserializer.deleteConfiguration(formId, sb));
            sw.Stop();
            sb.AppendLine("Delete Configuration: " + sw.Elapsed.ToString());
            if (result > 0) return Ok("Form data deleted successfully.");
            return BadRequest(result);

        }

        [HttpPut]
        [Route("api/uploadConfiguration")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Forms>> uploadConfiguration([FromBody] Forms forms)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();

            sw.Start();
            var dataexists = _objserializer.checkFormExists(forms.id, null);
            sw.Stop();
            sb.AppendLine("finding form exists: " + sw.Elapsed.ToString());
            dynamic result = null;
            sw.Start();
            if (dataexists)
                result = await Task.FromResult(await _objserializer.UpdateConfiguration(forms, sb));
            else
                result = await Task.FromResult(await _objserializer.addConfiguration(forms, sb));

            sw.Stop();
            sb.AppendLine("Update configuration : " + sw.Elapsed.ToString());
            return Ok(result);
        }


        //create new form
        // Post: api/createDocument
        [HttpPost]
        [Route("api/createDocument")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> createDocument([FromBody] Responses responsedata)
        {
            if (_unitOfWork.ResponseRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();
            sw.Start();
            if (_objserializer.checkResponseExists(responsedata.id))
                return Conflict(responsedata);
            var result = await Task.FromResult(await _objserializer.createResponseDoc(responsedata, sb));
            sw.Stop();
            sb.AppendLine("creating new response document : " + sw.Elapsed.ToString());
            return Ok(result);

        }

        //create DFE signin mapping for forms
        // Post: api/uploadProvidersMapping
        [HttpPost]
        [Route("api/uploadProvidersMapping")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> uploadProvidersMapping([FromBody] ProviderMappingsPayload mapping)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var parsedMapping = ProviderMappingsConverter.ConvertPayloadToMapping(mapping);
                var result = await Task.FromResult(await _objserializer.uploadProvidersMapping(parsedMapping, sb));
                sw.Stop();
                sb.AppendLine("Uploaded provider mapping : " + sw.Elapsed.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/uploadMultipleProvidersMapping")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<string>> uploadMultipleProvidersMapping([FromBody] ProviderMappingsPayload mapping)
        {
            string result = "Ukprns Updated";
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            var parsedMapping = ProviderMappingsConverter.ConvertPayloadToMapping(mapping);
            string[] formIds = parsedMapping.id.Split(",");
            foreach (string formId in formIds)
            {
                ProviderMappings providerMapping = new ProviderMappings();
                providerMapping.id = formId;
                providerMapping.providers = parsedMapping.providers;
                providerMapping.date = parsedMapping.date;
                StringBuilder sb = new StringBuilder();
                var sw = new Stopwatch();
                sw.Start();
                var res = await Task.FromResult(await _objserializer.uploadProvidersMapping(providerMapping, sb));
                sw.Stop();
                sb.AppendLine("Uploaded provider mapping : " + sw.Elapsed.ToString());
                if (!res)
                    result = "Failed to Upload Ukprns";
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/CheckProvidersMappingById")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> CheckProvidersMappingById([FromHeader] string id, [FromHeader] int ukprn, [FromHeader] int urn, [FromHeader] string? admincode)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            StringBuilder sb = new StringBuilder();
            var sw = new Stopwatch();
            sw.Start();
            var result = await _objserializer.CheckProvidersMappingById(id, ukprn, urn, admincode, sb);
            sw.Stop();
            sb.AppendLine("Uploaded provider mapping : " + sw.Elapsed.ToString());
            return Ok(result);

        }
        // Get all Responses.
        // GET: api/getResponses
        [HttpGet("api/getResponses")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetResponses([FromHeader] string? FormId, [FromHeader] string? StartDate, [FromHeader] string? EndDate)
        {
            if (FormId == null && StartDate == null && EndDate == null)
                return NoContent();

            Response.ContentType = "application/json";
            await using var writer = new StreamWriter(Response.Body);
            await writer.WriteAsync("[");

            var first = true;

            await foreach (var r in _objserializer.getResponses(FormId, StartDate, EndDate))
            {
                if (!first) await writer.WriteAsync(",");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                await writer.WriteAsync(JsonConvert.SerializeObject(r, settings));
                first = false;
            }

            await writer.WriteAsync("]");
            return new EmptyResult();
        }
        // Get all Response Question.
        // GET: api/getResponseQuestion
        [HttpGet("api/getResponseQuestion")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetResponseQuestion([FromHeader] string? FormId, [FromHeader] string? StartDate, [FromHeader] string? EndDate)
        {
            if (FormId == null && StartDate == null && EndDate == null)
                return NoContent();

            Response.ContentType = "application/json";
            await using var writer = new StreamWriter(Response.Body);
            await writer.WriteAsync("[");

            var first = true;

            await foreach (var r in _objserializer.getResponseQuestion(FormId, StartDate, EndDate))
            {
                if (!first) await writer.WriteAsync(",");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                await writer.WriteAsync(JsonConvert.SerializeObject(r, settings));
                first = false;
            }

            await writer.WriteAsync("]");
            return new EmptyResult();

        }
        // Get all Response Question Data.
        // GET: api/getResponseQuestionData
        [HttpGet("api/getResponseQuestionData")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetResponseQuestionData([FromHeader] string? FormId, [FromHeader] string? StartDate, [FromHeader] string? EndDate)
        {
            if (FormId == null && StartDate == null && EndDate == null)
                return NoContent();

            Response.ContentType = "application/json";
            await using var writer = new StreamWriter(Response.Body);
            await writer.WriteAsync("[");

            var first = true;

            await foreach (var r in _objserializer.getResponseQuestionData(FormId, StartDate, EndDate))
            {
                if (!first) await writer.WriteAsync(",");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                await writer.WriteAsync(JsonConvert.SerializeObject(r, settings));
                first = false;
            }

            await writer.WriteAsync("]");
            return new EmptyResult();

        }


        //[HttpGet("api/getMultipleFormByIds")]
        //[ProducesResponseType(typeof(List<Forms>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<List<Forms>>> GetMultipleForms()
        //{
        //    try
        //    {
        //        // Extract the array parameter from the request headers
        //        if (!HttpContext.Request.Headers.TryGetValue("FormIds", out var formIdsHeaderValue))
        //        {
        //            return BadRequest("Missing FormIds header");
        //        }

        //        // Deserialize the array parameter from the header value
        //        string[] formIds = formIdsHeaderValue.ToString().Split(',');

        //        if (_unitOfWork.FormsRepository == null)
        //        {
        //            return NotFound();
        //        }

        //        List<Forms> frms = new List<Forms>();
        //        foreach (string item in formIds)
        //        {
        //            frms.Add(_objserializer.getConfiguration(item.Trim()));
        //        }

        //        return frms;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        //    }
        //}
        [HttpPost]
        [Route("api/checkMultipleFormExists")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<List<KeyValuePair<string, bool>>>> checkMultipleFormExists([FromBody] string[] FormIds)
        {

            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            return Ok(await Task.FromResult(_objserializer.checkMultipleFormExists(FormIds)));

        }
        [HttpPost]
        [Route("api/changeMultipleformstatus")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<List<KeyValuePair<string, bool>>>> changeMultipleformstatus([FromBody] List<Dictionary<string, string>> formIds)
        {

            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            List<KeyValuePair<string, bool>> result = new List<KeyValuePair<string, bool>>();
            foreach (var form in formIds)
            {
                // Assuming you have logic to change the status and it returns a boolean indicating success
                bool success = await _objserializer.changeformstatus(form["FormId"], form["Status"]);
                result.Add(new KeyValuePair<string, bool>(form["FormId"], success));
            }
            return Ok(result);

        }

        [HttpDelete]
        [Route("api/deleteMultipleConfiguration")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> deleteMultipleConfiguration([FromBody] string[] FormIds)
        {
            bool result = true;
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            foreach (string formId in FormIds)
            {
                StringBuilder sb = new StringBuilder();
                var sw = new Stopwatch();
                sw.Start();
                if (!_objserializer.checkFormExists(formId, null))
                    return Conflict("Form doesnt exist in the db");
                sw.Stop();
                sb.AppendLine("finding form exists: " + sw.Elapsed.ToString());
                sw.Start();
                int res = await Task.FromResult(await _objserializer.deleteConfiguration(formId, sb));
                sw.Stop();
                sb.AppendLine("Delete Configuration: " + sw.Elapsed.ToString());
                if (res == 0)
                    result = false;
            }
            if (result) return Ok("Forms data deleted successfully.");
            return BadRequest(result);

        }

        ///updateParentChild, updateParentChild
        [HttpPost]
        [Route("api/updateParentChild")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<string>> updateParentChild([FromBody] List<ParentChildUpdate> data)
        {
            string result = "Updated tables";
            if (_unitOfWork.FormsRepository == null)
                return NotFound();
            foreach (var item in data)
            {
                var res = await Task.FromResult(await _objserializer.updateParentChild(item));
            }

            return Ok(result);
        }


     // Get response by id.
// GET: api/getSubmittedStatusbyParentId
[HttpGet("api/getSubmittedStatusbyParentId")]
[ProducesResponseType(typeof(List<Tuple<string, string, string>>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
public async Task<ActionResult<List<Tuple<string, string, string>>>> getSubmittedStatusbyParentId()
{
    if (_unitOfWork.ResponsesRepository == null)
        return NotFound();

    var formId = Request.Headers["formid"].FirstOrDefault();
    var ukprn = Request.Headers["ukprn"].FirstOrDefault();
    bool isUAT = Convert.ToBoolean(Request.Headers["isUAT"].FirstOrDefault());

    var result = await Task.FromResult(_objserializer.getSubmittedStatusbyParentId(formId, ukprn, isUAT));

    return Ok(result);
}


        // Get response by id.
        // GET: api/getResponseById
        [HttpGet("api/getResponseById/{submissionId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> getResponseById(string submissionId)
        {
            if (_unitOfWork.ResponsesRepository == null)
                return NotFound();

            return Ok(await Task.FromResult(_objserializer.getResponseById(submissionId)));
        }

        //Add CreateDraftResponse
        // Post: api/CreateDraftResponse
        [HttpPost]
        [Route("api/CreateDraftResponse")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> PostDraftResponse([FromBody] JObject json)
        {

            var result = await  _objserializer.AddorUpdateDraftResponse(json);
            return Ok(result);
        }

        // Get all Responses.
        // GET: api/GetDraftResponse0
        [HttpGet("api/GetDraftResponse/{formId}")]
        public async Task<ActionResult<Dictionary<string, object>>> GetDraftResponse(string formId)
       {
            if (_unitOfWork.ResponsesRepository == null)
                return NotFound();
            return Ok(await _objserializer.GetDraftResponse(formId));
        }

        // Get all SubmissionFormLog.
        // GET: api/GetSubmissionFormLog
        [HttpGet("api/getSubmissionFormLog/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubmissionForm>> GetSubmissionFormLog(string id)
        {
            return Ok(await _objserializer.GetSubmissionFormLog(id));
        }

        
        // Post all SubmissionFormLog.
        // Post: api/PostSubmissionFormLog

        [HttpPost]
        [Route("api/PostSubmissionFormLog")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PostSubmissionFormLog([FromBody] SubmissionForm submissionFormLog)
        {
            if (_unitOfWork.ResponsesRepository == null)
                return NotFound();

            return Ok(await _objserializer.PostSubmissionFormLog(submissionFormLog));
        }

        // DCDATA - Document capture specific APIs
        [HttpGet("api/GetDocumentCapture/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DocData>> getDocumentCapture(string id)
        {
            return Ok(await _objserializer.GetDocumentCapture(id));
        }


        // Create Document Capture Data.
        // Post: api/PostDocumentCaptureData

        [HttpPost]
        [Route("api/PostDocumentCaptureData")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PostDocumentCaptureData([FromBody] DocData dCData)
        {
            if (_unitOfWork.ResponsesRepository == null)
                return NotFound();

            return Ok(await _objserializer.PostDocumentCapture(dCData));
        }

        // Get all RepeatableFormData.
        // GET: api/GetRepeatableFormsData 
        [HttpGet("api/GetRepeatableFormsData/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Forms>> GetRepeatableFormsData(string id,[FromQuery] bool includeAll = false)
        { 
            Dictionary<string, string> queryParams = HttpContext.Request.Query
                    .Where(q => !string.Equals(q.Key, "includeAll", StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(q => q.Key, q => q.Value.ToString());
            return Ok(await _objserializer.GetRepeatableFormsData(id, queryParams, includeAll));
        }
        // Post all SubmissionFormLog.
        // Post: api/PostSubmissionFormLog
        [HttpPost]
        [Route("api/PostRepeatableFormsData")]
        [ProducesResponseType(typeof(Forms), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PostRepeatableFormsData([FromBody] JObject json)
        {
            if (_unitOfWork.RepeatableFormsDataRepository == null)
            {
                return NotFound();
            }

            try
            {
                var result = await _objserializer.PostRepeatableFormsData(json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


        [HttpDelete]
        [Route("api/DeleteRepeatableFormsData/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteRepeatableFormsData([FromRoute] string id)
        {
            if (_unitOfWork.RepeatableFormsDataRepository == null)
                return NotFound();

            return await _objserializer.DeleteRepeatableFormsData(id);
             
        }

        // Get user detail by id.
        // GET: api/getUserDetailById
        [HttpGet("api/getUserDetailById/{userId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetail>> getUserDetailById([FromRoute] Guid userId)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            var result = await _objserializer.GetUser(userId);
            return Ok(result);
        }

        // Update user detail.
        // PUT: api/updateUserDetail
        [HttpPut]
        [Route("api/updateUserDetail")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> updateUserDetail([FromBody] UserDetail user)
        {
            if (_unitOfWork.FormsRepository == null)
                return NotFound();

            var result = await _objserializer.updateUser(user);
            return Ok(result);
        }

    }
}
