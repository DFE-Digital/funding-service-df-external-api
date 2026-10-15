using DigitalForms.BL.Models;
using DigitalForms.BL.Serialized.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace DigitalForms.BL.Interfaces
{
    public interface ISerializer
    {
        Forms getConfiguration(string formId, bool useredcontext = false);
        IEnumerable<FormConfiguration> listFormConfigurations();
        bool checkFormExists(string formid, string formname);
        List<KeyValuePair<string, bool>> checkMultipleFormExists(string[] formid);
        Task<Forms> addConfiguration(Forms form, StringBuilder sb);
        Task<int> deleteConfiguration(string formId, StringBuilder sb);
        Task<Forms> UpdateConfiguration(Forms updatedform, StringBuilder sb);
        Task<bool> uploadProvidersMapping(DigitalForms.BL.Serialized.Models.ProviderMappings mapping, StringBuilder sb);
        Task<bool> CheckProvidersMappingById(string id, int ukprn, int urn, string admincode, StringBuilder sb);
        IAsyncEnumerable<object> getResponses(string FormId, string StartDate, string EndDate);
        IAsyncEnumerable<object> getResponseQuestion(string FormId, string StartDate, string EndDate);
        IAsyncEnumerable<object> getResponseQuestionData(string FormId, string StartDate, string EndDate);
        bool checkResponseExists(string responseid);
        bool getResponseById(string submissionId);
        List<Tuple<string, string, string>> getSubmittedStatusbyParentId(string formId, string ukprn, bool isUAT);
        Task<bool> changeformstatus(string formid, string status);
        Task<bool> createResponseDoc(DigitalForms.BL.Serialized.Models.Responses responsedata, StringBuilder sb);
        Task<bool> AddorUpdateDraftResponse(JObject json);
        Task<Dictionary<string, object>> GetDraftResponse(string id);
        Task<SubmissionForm> GetSubmissionFormLog(string id);
        Task<bool> PostSubmissionFormLog(SubmissionForm submissionForm);
        Task<DocData> GetDocumentCapture(string fileId);
        Task<bool> PostDocumentCapture(DocData dCData);
        Task<Forms> GetRepeatableFormsData(string id, Dictionary<string, string> queryParams, bool includeAll);
        Task<bool> PostRepeatableFormsData(JObject json);
        Task<bool> DeleteRepeatableFormsData(string id);
        Task<bool> updateParentChild(ParentChildUpdate data);
        Task<UserDetail> GetUser(Guid userId);
        Task<bool> updateUser(UserDetail user);
        // duplicate overload removed
    }
}
