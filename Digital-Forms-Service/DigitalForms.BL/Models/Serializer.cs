using AutoMapper;
using Azure;
using DigitalForms.BL.Interfaces;
using DigitalForms.BL.Models;
using DigitalForms.BL.Models.Generated;
using DigitalForms.BL.Serialized.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectsComparer;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalForms.BL.Serialized.Models
{
    public class Serializer : ComparersFactory, DigitalForms.BL.Interfaces.ISerializer
    {
        private readonly IUnitOfWork _unitOfWork;
        public Serializer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            dictClassPropname.Add(typeof(BL.Models.Condition), "Name");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.ConditionDetail), "PropValue");
            dictClassPropname.Add(typeof(BL.Models.DesignDataSet), "DdsetId");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.DesignDataSetDetail), "Index");
            dictClassPropname.Add(typeof(BL.Models.Section), "Scname");
            dictClassPropname.Add(typeof(BL.Models.List), "Lstname");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.ListItem), "LstitemValue");
            dictClassPropname.Add(typeof(BL.Models.TabDetail), "TabName");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.TabChildDetail), "Value");
            dictClassPropname.Add(typeof(BL.Models.Page), "Path");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.PageChildSetting), "Path");
            dictClassPropname.Add(typeof(BL.Models.Calculation), "Name");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.CalculationComponentDetail), "Calcid");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.CalculationAdditionalSetting), "PropValue");
            dictClassPropname.Add(typeof(BL.Models.Component), "Name");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.ComponentAdditionalSetting), "PropValue");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.ComponentDatasetSchemaDetail), "ColumnId");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.ComponentDatasetSchemaPropertyDetail), "PropValue");
            dictClassPropname.Add(typeof(BL.Models.Output), "Name");
            dictClassPropname.Add(typeof(DigitalForms.BL.Models.OutputDetail), "PropValue");
            dictClassPropname.Add(typeof(BL.Models.Document), "FileId");
            dictClassPropname.Add(typeof(BL.Models.ParentChild), "Name");
            dictClassPropname.Add(typeof(BL.Models.ParentChildConfig), "Pcid");
            dictClassPropname.Add(typeof(BL.Models.ChildConfig), "Pccid");
            dictClassPropname.Add(typeof(BL.Models.DependentForm), "Ccid");

            dictClassreponame.Add(typeof(BL.Models.Condition), _unitOfWork.ConditionsRepository);
            dictClassreponame.Add(typeof(BL.Models.ConditionDetail), _unitOfWork.ConditionDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.DesignDataSet), _unitOfWork.DesignDataSetsRepository);
            dictClassreponame.Add(typeof(BL.Models.DesignDataSetDetail), _unitOfWork.DesignDataSetDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.Section), _unitOfWork.SectionsRepository);
            dictClassreponame.Add(typeof(BL.Models.List), _unitOfWork.ListsRepository);
            dictClassreponame.Add(typeof(BL.Models.ListItem), _unitOfWork.ListsItemsRepository);
            dictClassreponame.Add(typeof(BL.Models.TabDetail), _unitOfWork.TabDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.TabChildDetail), _unitOfWork.TabChildDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.Page), _unitOfWork.PagesRepository);
            dictClassreponame.Add(typeof(BL.Models.PageChildSetting), _unitOfWork.PageChildsRepository);
            dictClassreponame.Add(typeof(BL.Models.Calculation), _unitOfWork.CalculationsRepository);
            dictClassreponame.Add(typeof(BL.Models.Output), _unitOfWork.OutputsRepository);
            dictClassreponame.Add(typeof(BL.Models.OutputDetail), _unitOfWork.OutputDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.Document), _unitOfWork.DocumentsRepository);
            dictClassreponame.Add(typeof(BL.Models.Component), _unitOfWork.ComponentRepository);
            dictClassreponame.Add(typeof(DigitalForms.BL.Models.ComponentAdditionalSetting), _unitOfWork.ComponentAdditionalSettingsRepository);
            dictClassreponame.Add(typeof(DigitalForms.BL.Models.ComponentDatasetSchemaDetail), _unitOfWork.ComponentDatasetSchemaDetailsRepository);
            dictClassreponame.Add(typeof(DigitalForms.BL.Models.ComponentDatasetSchemaPropertyDetail), _unitOfWork.ComponentDatasetSchemaPropertyDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.CalculationAdditionalSetting), _unitOfWork.CalculationAdditionalSettingsRepository);
            dictClassreponame.Add(typeof(BL.Models.ParentChild), _unitOfWork.ParentChildRepository);
            dictClassreponame.Add(typeof(BL.Models.ParentChildConfig), _unitOfWork.ParentChildConfigRepository);
            dictClassreponame.Add(typeof(BL.Models.ChildConfig), _unitOfWork.ChildConfigRepository);
            dictClassreponame.Add(typeof(BL.Models.DependentForm), _unitOfWork.DependentFormRepository);
            dictClassreponame.Add(typeof(BL.Models.CalculationComponentDetail), _unitOfWork.CalculationComponentDetailsRepository);
            dictClassreponame.Add(typeof(BL.Models.DraftResponse), _unitOfWork.DraftResponseRepository);

            dictClassRltnname.Add(typeof(BL.Models.Condition), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.ConditionDetail), "Cnid");
            dictClassRltnname.Add(typeof(BL.Models.DesignDataSet), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.DesignDataSetDetail), "Ddsid");
            dictClassRltnname.Add(typeof(BL.Models.Section), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.List), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.ListItem), "Lstid");
            dictClassRltnname.Add(typeof(BL.Models.TabDetail), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.TabChildDetail), "Tabid");
            dictClassRltnname.Add(typeof(BL.Models.Page), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.PageChildSetting), "Pgid");
            dictClassRltnname.Add(typeof(BL.Models.Calculation), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.Component), "Pgid");
            dictClassRltnname.Add(typeof(BL.Models.CalculationAdditionalSetting), "Calcid");
            dictClassRltnname.Add(typeof(BL.Models.CalculationComponentDetail), "Calcid");
            dictClassRltnname.Add(typeof(BL.Models.ComponentAdditionalSetting), "Cmpid");
            dictClassRltnname.Add(typeof(BL.Models.ComponentDatasetSchemaDetail), "Cmpid");
            dictClassRltnname.Add(typeof(BL.Models.ComponentDatasetSchemaPropertyDetail), "Cdsdid");
            dictClassRltnname.Add(typeof(BL.Models.Output), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.OutputDetail), "Ouid");
            dictClassRltnname.Add(typeof(BL.Models.Document), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.ParentChild), "Fid");
            dictClassRltnname.Add(typeof(BL.Models.ParentChildConfig), "Pcid");
            dictClassRltnname.Add(typeof(BL.Models.ChildConfig), "Pccid");
            dictClassRltnname.Add(typeof(BL.Models.DependentForm), "Ccid");



            dictClassSubColl.Add("DigitalForms.BL.Models.Condition", "ConditionDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.DesignDataSet", "DesignDataSetDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.List", "ListItems");
            dictClassSubColl.Add("DigitalForms.BL.Models.TabDetail", "TabChildDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.Page", "PageChildSettings");
            dictClassSubColl.Add("DigitalForms.BL.Models.Page1", "Components");
            dictClassSubColl.Add("DigitalForms.BL.Models.Component", "ComponentAdditionalSettings");
            dictClassSubColl.Add("DigitalForms.BL.Models.Component1", "ComponentDatasetSchemaDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.ComponentDatasetSchemaDetail", "ComponentDatasetSchemaPropertyDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.Output", "OutputDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.Calculation", "CalculationComponentDetails");
            dictClassSubColl.Add("DigitalForms.BL.Models.Calculation1", "CalculationAdditionalSettings");
            dictClassSubColl.Add("DigitalForms.BL.Models.ParentChild", "ParentChildConfig");
            dictClassSubColl.Add("DigitalForms.BL.Models.ParentChildConfig", "ChildConfigs");
            dictClassSubColl.Add("DigitalForms.BL.Models.ChildConfig", "DependentForms");


            dictStringClassname.Add("ListItems", typeof(BL.Models.ListItem));
            dictStringClassname.Add("ConditionDetails", typeof(BL.Models.ConditionDetail));
            dictStringClassname.Add("DesignDataSetDetails", typeof(BL.Models.DesignDataSetDetail));
            dictStringClassname.Add("TabChildDetails", typeof(BL.Models.TabChildDetail));
            dictStringClassname.Add("PageChildSettings", typeof(BL.Models.PageChildSetting));
            dictStringClassname.Add("Components", typeof(BL.Models.Component));
            dictStringClassname.Add("ComponentAdditionalSettings", typeof(BL.Models.ComponentAdditionalSetting));
            dictStringClassname.Add("ComponentDatasetSchemaDetails", typeof(BL.Models.ComponentDatasetSchemaDetail));
            dictStringClassname.Add("ComponentDatasetSchemaPropertyDetails", typeof(BL.Models.ComponentDatasetSchemaPropertyDetail));
            dictStringClassname.Add("OutputDetails", typeof(BL.Models.OutputDetail));
            dictStringClassname.Add("CalculationComponentDetails", typeof(BL.Models.CalculationComponentDetail));
            dictStringClassname.Add("CalculationAdditionalSettings", typeof(BL.Models.CalculationAdditionalSetting));
            dictStringClassname.Add("ParentChildConfig", typeof(BL.Models.ParentChildConfig));
            dictStringClassname.Add("ChildConfigs", typeof(BL.Models.ChildConfig));
            dictStringClassname.Add("DependentForms", typeof(BL.Models.DependentForm));

            dictClassFKeyname.Add(typeof(BL.Models.DesignDataSet), "DOCID");
            dictClassFKeyname.Add(typeof(BL.Models.Component), "DOCID,DDSID,LSTID");
            dictClassFKeyname.Add(typeof(BL.Models.CalculationComponentDetail), "CALCID,CMPID");
            //dictClassFKeyname.Add(typeof(BL.Models.ChildConfig), "Ccid,Fid");
            dictClassFKeyname.Add(typeof(BL.Models.ParentChildConfig), "PCID");
            dictClassFKeyname.Add(typeof(BL.Models.ChildConfig), "PCCID");
            dictClassFKeyname.Add(typeof(BL.Models.DependentForm), "CCID");

        }

        #region Global Variables

        // Serialized forms
        List<Forms> Formslist = new List<Forms>();
        List<Page> Pageslist = new List<Page>();
        List<Component> Componentslist = new List<Component>();
        List<Options> Optionslist = new List<Options>();
        List<Schema> Schemalist = new List<Schema>();
        List<Values> Valuelist = new List<Values>();
        List<Next> Nextlist = new List<Next>();
        List<Tabdata> tabdatalist = new List<Tabdata>();
        List<Tab> tabslist = new List<Tab>();
        List<Column> columnlist = new List<Column>();
        List<Columnschema> columnschemalist = new List<Columnschema>();
        List<Condition> conditionslist = new List<Condition>();
        List<Calculation> calculationlist = new List<Calculation>();

        List<Output> outputslist = new List<Output>();
        List<Importeddataset> importeddatasetslist = new List<Importeddataset>();
        List<Document> documentslist = new List<Document>();
        List<Designeddataset> designdatasetslist = new List<Designeddataset>();
        List<Datum> Datumlist = new List<Datum>();

        // Forms
        List<BL.Models.Form> Formlist = new List<Form>();
        List<BL.Models.Page> Pagelist = new List<BL.Models.Page>();
        List<BL.Models.Component> Componentlist = new List<BL.Models.Component>();
        List<BL.Models.Component> masterComponentlist = new List<BL.Models.Component>();
        List<BL.Models.ComponentAdditionalSetting> ComponentAdditionalSettinglist = new List<BL.Models.ComponentAdditionalSetting>();
        List<BL.Models.CalculationAdditionalSetting> CalculationAdditionalSettinglist = new List<BL.Models.CalculationAdditionalSetting>();
        List<BL.Models.ComponentDatasetSchemaDetail> ComponentDatasetSchemaDetaillist = new List<BL.Models.ComponentDatasetSchemaDetail>();
        List<BL.Models.CalculationComponentDetail> CalculationComponentDetailist = new List<BL.Models.CalculationComponentDetail>();
        List<BL.Models.ComponentDatasetSchemaPropertyDetail> ComponentDatasetSchemaPropertyDetaillist = new List<BL.Models.ComponentDatasetSchemaPropertyDetail>();
        List<BL.Models.DatasetDataDetail> DatasetDataDetaillist = new List<BL.Models.DatasetDataDetail>();
        List<BL.Models.Condition> condnlist = new List<BL.Models.Condition>();
        List<BL.Models.ConditionDetail> condnsublist = new List<BL.Models.ConditionDetail>();
        List<BL.Models.Output> outputlist = new List<BL.Models.Output>();
        List<BL.Models.OutputDetail> outputsublist = new List<BL.Models.OutputDetail>();
        List<BL.Models.Document> documentlist = new List<BL.Models.Document>();
        List<BL.Models.DesignDataSet> designdatasetlist = new List<BL.Models.DesignDataSet>();
        List<Datum> datasubsetlist = new List<Datum>();
        List<BL.Models.DesignDataSetDetail> designdatasubsetlist = new List<DesignDataSetDetail>();
        Dictionary<string, List<string>> calccomparray = new Dictionary<string, List<string>>();

        Dictionary<Type, string> dictClassPropname = new Dictionary<Type, string>();
        Dictionary<Type, object> dictClassreponame = new Dictionary<Type, object>();
        Dictionary<Type, string> dictClassRltnname = new Dictionary<Type, string>();
        Dictionary<string, string> dictClassSubColl = new Dictionary<string, string>();
        Dictionary<string, Type> dictStringClassname = new Dictionary<string, Type>();

        Dictionary<Type, string> dictClassFKeyname = new Dictionary<Type, string>();

        ComparisonSettings settings = new ComparisonSettings
        {
            //Null and empty error lists are equal
            EmptyAndNullEnumerablesEqual = true

        };
        BaseComparer parentComparer = null;

        dynamic formid = null;
        dynamic pageid = null;
        Form dbform;
        #endregion

        public Forms getConfiguration(string formId, bool useredcontext = false)
        {
            Formslist = new List<Forms>();
            var sw = Stopwatch.StartNew();
            sw.Start();
            try
            {
                var mapper = MapperConfig.Mapper;

                var data = _unitOfWork.FormsRepository.getConfiguration(formId, useredcontext);

                foreach (Form objform in data)
                {
                    //Pages
                    Pageslist = new List<Page>();
                    objform.Pages.ToList().ForEach(page =>
                    {
                        //components
                        Componentslist = new List<Component>();
                        page.Components.ToList().ForEach(component =>
                        {
                            Optionslist = new List<Options>();
                            Schemalist = new List<Schema>();
                            Valuelist = new List<Values>();
                            tabdatalist = new List<Tabdata>();
                            columnlist = new List<Column>();
                            columnschemalist = new List<Columnschema>();

                            var settings = component.ComponentAdditionalSettings.ToList();
                            //options
                            var options = mapper.Map<IEnumerable<Options>, List<Options>>(settings.GroupBy(m => m.PropType = "options").Select(m => new Options
                            {
                                autocomplete = m.Where(w => w.PropName == "autocomplete").Select(m => m.PropValue).SingleOrDefault(),
                                classes = m.Where(w => w.PropName == "classes").Select(m => m.PropValue).SingleOrDefault(),
                                condition = m.Where(w => w.PropName == "condition").Select(m => m.PropValue).SingleOrDefault(),
                                maxDaysInPast = m.Where(w => w.PropName == "maxDaysInPast").Select(m => m.PropValue).SingleOrDefault(),
                                maxDaysInFuture = m.Where(w => w.PropName == "maxDaysInFuture").Select(m => m.PropValue).SingleOrDefault(),
                                rows = m.Where(w => w.PropName == "rows").Select(m => m.PropValue).SingleOrDefault(),
                                customValidationMessage = m.Where(w => w.PropName == "customValidationMessage").Select(m => m.PropValue).SingleOrDefault(),
                                hideTitle = (m.Where(w => w.PropName == "hideTitle").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "hideTitle").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                required = (m.Where(w => w.PropName == "required").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "required").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                optionalText = (m.Where(w => w.PropName == "optionalText").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "optionalText").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                hideResult = (m.Where(w => w.PropName == "hideResult").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "hideResult").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                format = m.Where(w => w.PropName == "format").Select(m => m.PropValue).SingleOrDefault(),
                                prefixType = component.Type == "Result" ? m.Where(w => w.PropName == "prefixType").Select(m => m.PropValue).SingleOrDefault() : null,
                                prefixValue = component.Type == "Result" ? m.Where(w => w.PropName == "prefixValue").Select(m => m.PropValue).SingleOrDefault() : null,
                                suffixValue = component.Type == "Result" ? m.Where(w => w.PropName == "suffixValue").Select(m => m.PropValue).SingleOrDefault() : null,
                                bold = component.Type == "Result" ? m.Where(w => w.PropName == "bold").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault() : null,
                                hideResultOnSummary = component.Type == "Result" ? m.Where(w => w.PropName == "hideResultOnSummary").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault() : null,
                                hideResultOnPage = component.Type == "Result" ? m.Where(w => w.PropName == "hideResultOnPage").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault() : null,
                                dateRangeStart = m.Where(w => w.PropName == "dateRangeStart").Select(m => m.PropValue).SingleOrDefault(),
                                dateRangeEnd = m.Where(w => w.PropName == "dateRangeEnd").Select(m => m.PropValue).SingleOrDefault()
                            }).ToList());
                            //values
                            var values = mapper.Map<IEnumerable<Values>, List<Values>>(settings.GroupBy(m => m.PropType = "values").Select(m => new Values
                            {
                                type = m.Where(w => w.PropName == "type").Select(m => m.PropValue).SingleOrDefault()
                            }).ToList());
                            //date
                            var date = mapper.Map<IEnumerable<date>, List<date>>(settings.GroupBy(m => m.PropType = "date").Select(m => new date
                            {
                                hideDay = string.IsNullOrEmpty((m.Where(w => w.PropName == "hideDay").Select(m => m.PropValue).SingleOrDefault())) ? null : (m.Where(w => w.PropName == "hideDay").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault()),
                                hideMonth = string.IsNullOrEmpty((m.Where(w => w.PropName == "hideMonth").Select(m => m.PropValue).SingleOrDefault())) ? null : (m.Where(w => w.PropName == "hideMonth").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault()),
                                hideYear = (m.Where(w => w.PropName == "hideYear").Select(m => m.PropValue).SingleOrDefault()) == null ? null : (m.Where(w => w.PropName == "hideYear").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault()),
                            }).ToList());
                            //schema
                            var schema = mapper.Map<IEnumerable<Schema>, List<Schema>>(settings.GroupBy(m => m.PropType = "schema").Select(m => new Schema
                            {
                                min = string.IsNullOrEmpty((m.Where(w => w.PropName == "min").Select(m => m.PropValue).SingleOrDefault())) ? null : (m.Where(w => w.PropName == "min").Select(m => (dynamic)(m.PropValue)).SingleOrDefault()),
                                max = string.IsNullOrEmpty((m.Where(w => w.PropName == "max").Select(m => m.PropValue).SingleOrDefault())) ? null : (m.Where(w => w.PropName == "max").Select(m => (dynamic)(m.PropValue)).SingleOrDefault()),
                                length = (m.Where(w => w.PropName == "length").Select(m => m.PropValue).SingleOrDefault()) == null ? null : (m.Where(w => w.PropName == "length").Select(m => Convert.ToInt32(m.PropValue)).SingleOrDefault()),
                                regex = m.Where(w => w.PropName == "regex").Select(m => m.PropValue).SingleOrDefault(),
                                precision = m.Where(w => w.PropName == "precision").Select(m => m.PropValue).SingleOrDefault(),
                                error = m.Where(w => w.PropName == "error").Select(m => m.PropValue).SingleOrDefault()
                            }).ToList());
                            //tab details
                            settings.ToList().ForEach(setting =>
                            {
                                var tabchilddata = mapper.Map<IEnumerable<TabChildDetail>, List<Tabdata>>(setting.TabChildDetails.Count > 0 ? setting.TabChildDetails.ToList() : new List<TabChildDetail>());
                                if (tabchilddata.Count > 0)
                                    tabdatalist.AddRange(tabchilddata);
                            });
                            //component schema for data import   
                            component.ComponentDatasetSchemaDetails.ToList().ForEach(column =>
                            {
                                if (column != null)
                                {
                                    var property = column.ComponentDatasetSchemaPropertyDetails.ToList();
                                    var columnschema = mapper.Map<IEnumerable<Columnschema>, List<Columnschema>>(property.GroupBy(g => g.Cdsdid).Select(s => new Columnschema
                                    {
                                        minNumber = s.Where(w => w.PropName == "minNumber").Select(m => m.PropValue).SingleOrDefault(),
                                        maxNumber = s.Where(w => w.PropName == "maxNumber").Select(m => m.PropValue).SingleOrDefault(),
                                        precisionNumber = s.Where(w => w.PropName == "precisionNumber").Select(m => m.PropValue).SingleOrDefault(),
                                        maxLength = s.Where(w => w.PropName == "maxLength").Select(m => m.PropValue).SingleOrDefault(),
                                        maxDaysInPast = s.Where(w => w.PropName == "maxDaysInPast").Select(m => m.PropValue).SingleOrDefault(),
                                        maxDaysInFuture = s.Where(w => w.PropName == "maxDaysInFuture").Select(m => m.PropValue).SingleOrDefault(),
                                        addressRequired = s.Where(w => w.PropName == "addressRequired").Select(m => m.PropValue).SingleOrDefault(),
                                        CDSPDID = s.Select(s => s.Cdspdid).FirstOrDefault()
                                    }).ToList());
                                    var columndata = mapper.Map<ComponentDatasetSchemaDetail, Column>(column);
                                    columndata.columnSchema = columnschema.FirstOrDefault();
                                    columnlist.Add(columndata);
                                }
                            });

                            var columns = mapper.Map<IEnumerable<Column>, List<Column>>(columnlist);
                            var tabdata = mapper.Map<IEnumerable<Tabdata>, List<Tabdata>>(tabdatalist);
                            //binding component data
                            var tmpcomponentdata = mapper.Map<IEnumerable<Component>, List<Component>>(settings.GroupBy(m => m.PropType = null).Select(m => new Component
                            {
                                name = m.Select(s => s.Cmp.Name).FirstOrDefault(),
                                type = m.Select(s => s.Cmp.Type).FirstOrDefault(),
                                hint = m.Select(s => s.Cmp.Hint).FirstOrDefault(),
                                calculationName = component.Type == "Result" ? m.Where(w => w.PropName == "calculationName").Select(m => m.PropValue).SingleOrDefault() : null,
                                isEditingTabs = (m.Select(s => s.Cmp.IsEditingTabs).FirstOrDefault()) == null ? null : m.Select(s => Convert.ToBoolean(s.Cmp.IsEditingTabs)).FirstOrDefault(),
                                nameHasError = (m.Select(s => s.Cmp.NameHasError).FirstOrDefault()) == null ? null : m.Select(s => Convert.ToBoolean(s.Cmp.NameHasError)).FirstOrDefault(),
                                componentEdited = (m.Select(s => s.Cmp.ComponentEdited).FirstOrDefault()) == null ? null : m.Select(s => Convert.ToBoolean(s.Cmp.ComponentEdited)).FirstOrDefault(),
                                _checked = (m.Select(s => s.Cmp.Checked).FirstOrDefault()) == null ? null : m.Select(s => Convert.ToBoolean(s.Cmp.Checked)).FirstOrDefault(),
                                content = m.Select(s => s.Cmp.Content).FirstOrDefault(),
                                prefixType = component.Type == "NumberField" ? m.Where(w => w.PropName == "prefixType").Select(m => m.PropValue).SingleOrDefault() : null,
                                prefixValue = component.Type == "NumberField" ? m.Where(w => w.PropName == "prefixValue").Select(m => m.PropValue).SingleOrDefault() : null,
                                suffixValue = component.Type == "NumberField" ? m.Where(w => w.PropName == "suffixValue").Select(m => m.PropValue).SingleOrDefault() : null,
                                selectedDocument = m.Where(w => w.PropName == "selectedDocument").Select(m => m.PropValue).SingleOrDefault(),
                                list = m.Where(w => w.PropName == "list").Select(m => m.PropValue).SingleOrDefault(),
                                displayName = m.Where(w => w.PropName == "displayName").Select(m => m.PropValue).SingleOrDefault(),
                                expression = m.Where(w => w.PropName == "expression").Select(m => m.PropValue).SingleOrDefault(),
                                dataset = m.Where(w => w.PropName == "dataset").Select(m => m.PropValue).SingleOrDefault(),
                                fileId = m.Where(w => w.PropName == "fileId").Select(m => m.PropValue).SingleOrDefault(),
                                selectTitle = m.Where(w => w.PropName == "selectTitle").Select(m => m.PropValue).SingleOrDefault(),
                                isBtnDisabled = (m.Where(w => w.PropName == "isBtnDisabled").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "isBtnDisabled").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                tabsNumber = (m.Where(w => w.PropName == "tabsNumber").Select(m => m.PropValue).SingleOrDefault()) == null ? null : (m.Where(w => w.PropName == "tabsNumber").Select(m => Convert.ToInt32(m.PropValue)).SingleOrDefault()),
                                totalTabs = (m.Where(w => w.PropName == "totalTabs").Select(m => m.PropValue).SingleOrDefault()) == null ? null : (m.Where(w => w.PropName == "totalTabs").Select(m => Convert.ToInt32(m.PropValue)).SingleOrDefault()),
                                reset = (m.Where(w => w.PropName == "reset").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "reset").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                allTabsSelected = (m.Where(w => w.PropName == "allTabsSelected").Select(m => m.PropValue).SingleOrDefault()) == null ? null : m.Where(w => w.PropName == "allTabsSelected").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                tabInputType = m.Where(w => w.PropName == "tabInputType").Select(m => m.PropValue).SingleOrDefault(),
                                paragraphVal = m.Where(w => w.PropName == "paragraphVal").Select(m => m.PropValue).SingleOrDefault(),
                                documentName = m.Where(w => w.PropName == "documentName").Select(m => m.PropValue).SingleOrDefault(),
                                columnNames = m.Where(w => w.PropName == "columnNames").Select(m => m.PropValue).SingleOrDefault() == null ? null : m.Where(w => w.PropName == "columnNames").Select(m => m.PropValue).SingleOrDefault().Replace("\"", "").Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray(),
                                addedFileTypes = m.Where(w => w.PropName == "addedFileTypes").Select(m => m.PropValue).SingleOrDefault() == null ? null : m.Where(w => w.PropName == "addedFileTypes").Select(m => m.PropValue).SingleOrDefault().Replace("\"", "").Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray(),
                                title = m.Where(w => w.PropName == "title").Select(m => m.PropValue).SingleOrDefault() ?? m.Select(s => s.Cmp.Title).FirstOrDefault(),
                                addTime = component.Type == "DateAndTimeField" ? m.Where(w => w.PropName == "addTime").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault():null,
                            }).ToList());
                            dynamic componentdata;
                            if (tmpcomponentdata.Count > 0)
                                componentdata = mapper.Map<Component, Component>(tmpcomponentdata.FirstOrDefault());
                            else
                                componentdata = mapper.Map<BL.Models.Component, Component>(component);

                            componentdata.options = options.Count() > 0 ? options.FirstOrDefault() : null;
                            componentdata.schema = schema.Count() > 0 ? schema.FirstOrDefault() : null;
                            componentdata.values = values.Count() > 0 ? values.FirstOrDefault() : null;
                            componentdata.tabData = tabdata.Count() > 0 ? tabdata.ToArray() : null;
                            componentdata.columns = columns.Count() > 0 ? columns.OrderBy(o => o.ColumnOrder).ToArray() : null;
                            componentdata.date = component.Type == "DateAndTimeField" && date.Count() > 0 ? date.FirstOrDefault() : null;
                            Componentslist.Add(componentdata);
                        });
                        var pagedata = mapper.Map<BL.Models.Page, Page>(page);
                        var pagecomponentdata = mapper.Map<IEnumerable<Component>, List<Component>>(Componentslist);
                        var pagechilddata = page.PageChildSettings.ToList();
                        var nextdata = mapper.Map<IEnumerable<PageChildSetting>, List<Next>>(pagechilddata);
                        pagedata.next = nextdata.ToArray();
                        pagedata.components = pagecomponentdata.ToArray();
                        Pageslist.Add(pagedata);
                    });

                    //calculation
                    calculationlist = new List<Calculation>();
                    objform.Calculations.ToList().ForEach(calculations =>
                    {
                        Datumlist = new List<Datum>();

                        var settings = calculations.CalculationAdditionalSettings.ToList();
                        //options
                        var Datum = mapper.Map<IEnumerable<Datum>, List<Datum>>(settings.Where(w => w.PropType == "datum").GroupBy(m => m.RowNumber).Select(m => new Datum
                        {
                            index = m.Where(w => w.PropName == "index").Select(m => m.PropValue).SingleOrDefault(),
                            type = m.Where(w => w.PropName == "type").Select(m => m.PropValue).SingleOrDefault(),
                            value = m.Where(w => w.PropName == "value").Select(m => m.PropValue).SingleOrDefault(),
                            bold = Convert.ToBoolean(m.Where(w => w.PropName == "bold").Select(m => m.PropValue).SingleOrDefault()),
                            calc = Convert.ToBoolean(m.Where(w => w.PropName == "calc").Select(m => m.PropValue).SingleOrDefault()),
                            designedDataSetId = m.Where(w => w.PropName == "designedDataSetId").Select(m => m.PropValue).SingleOrDefault(),
                            _checked = Convert.ToBoolean(m.Where(w => w.PropName == "_checked").Select(m => m.PropValue).SingleOrDefault()),
                        }).ToList());

                        var ComputeList = mapper.Map<IEnumerable<ComputeList>, List<ComputeList>>(settings.Where(w => w.PropType == "computelist").GroupBy(m => m.RowNumber).Select(m => new ComputeList
                        {
                            id = m.Where(w => w.PropName == "id").Select(m => m.PropValue).SingleOrDefault(),
                            type = m.Where(w => w.PropName == "type").Select(m => m.PropValue).SingleOrDefault(),
                            order = m.Where(w => w.PropName == "order").Select(m => Convert.ToInt32(m.PropValue)).SingleOrDefault(),
                            value = m.Where(w => w.PropName == "value").Select(m => m.PropValue).SingleOrDefault(),
                            entity = m.Where(w => w.PropName == "entity").Select(m => m.PropValue).SingleOrDefault(),
                        }).ToList());
                        var calculationsMappedValue = settings.Where(w => w.PropName == "calculationsmapped")
                                    .Select(w => w.PropValue)
                                    .FirstOrDefault();

                        var CalculationsMapped = !string.IsNullOrEmpty(calculationsMappedValue)
                            ? calculationsMappedValue.Split(',').Select(s => s.Trim()).ToArray()
                            : Array.Empty<string>();

                        var Calculationdata = mapper.Map<BL.Models.Calculation, Calculation>(calculations);
                        Calculationdata.datasets = Datum.ToArray();
                        Calculationdata.computeList = ComputeList.ToArray();
                        Calculationdata.calculationsMapped = CalculationsMapped.ToArray();
                        calculationlist.Add(Calculationdata);
                    });

                    //conditions
                    conditionslist = new List<Condition>();
                    objform.Conditions.ToList().ForEach(con =>
                    {
                        var condetails = con.ConditionDetails.ToList();
                        var condn = mapper.Map<Condition, Condition>(new Condition
                        {
                            displayName = con.DisplayName,
                            name = con.Name,
                            value = mapper.Map<Value, Value>(new Value
                            {
                                conditions = mapper.Map<IEnumerable<Condition1>, Condition1[]>(condetails.GroupBy(g => g.SubsetNo).Select(s => new Condition1
                                {
                                    field = new Field
                                    {
                                        name = s.Where(w => w.PropName == "name" && w.PropType == "field").Select(m => m.PropValue).SingleOrDefault(),
                                        display = s.Where(w => w.PropName == "display" && w.PropType == "field").Select(m => m.PropValue).SingleOrDefault(),
                                        type = s.Where(w => w.PropName == "type" && w.PropType == "field").Select(m => m.PropValue).SingleOrDefault()
                                    },
                                    value = new Value1
                                    {
                                        value = s.Where(w => w.PropName == "value" && w.PropType == "value").Select(m => m.PropValue).SingleOrDefault(),
                                        display = s.Where(w => w.PropName == "display" && w.PropType == "value").Select(m => m.PropValue).SingleOrDefault(),
                                        type = s.Where(w => w.PropName == "type" && w.PropType == "value").Select(m => m.PropValue).SingleOrDefault()
                                    },
                                    _operator = s.Where(w => w.PropName == "operator" && w.PropType == "operator").Select(m => m.PropValue).SingleOrDefault(),
                                    conditionType = s.Where(w => w.PropName == "conditionType" && w.PropType == "conditionType").Select(m => m.PropValue).SingleOrDefault(),
                                    datasetId = s.Where(w => w.PropName == "datasetId" && w.PropType == "datasetId").Select(m => m.PropValue).SingleOrDefault(),
                                    coordinator = s.Where(w => w.PropName == "coordinator" && w.PropType == "coordinator").Select(m => m.PropValue).SingleOrDefault(),
                                })).ToArray(),
                                name = con.DisplayName
                            })
                        });
                        conditionslist.Add(condn);
                    });
                    //outputs
                    outputslist = new List<Output>();
                    objform.Outputs.ToList().ForEach(op =>
                    {
                        var opt = mapper.Map<Output, Output>(new Output
                        {
                            name = op.Name,
                            title = op.Title,
                            type = op.Type,
                            outputConfiguration = mapper.Map<Outputconfiguration, Outputconfiguration>(op.OutputDetails?.GroupBy(g => g.Ouid).Select(s => new Outputconfiguration
                            {
                                addReferencesToPersonalisation = (s.Where(w => w.PropName == "addReferencesToPersonalisation").Select(m => m.PropValue).SingleOrDefault()) == null ? null : s.Where(w => w.PropName == "addReferencesToPersonalisation").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                                apiKey = s.Where(w => w.PropName == "apiKey").Select(m => m.PropValue).SingleOrDefault(),
                                emailAddress = s.Where(w => w.PropName == "emailAddress").Select(m => m.PropValue).SingleOrDefault(),
                                emailField = s.Where(w => w.PropName == "emailField").Select(m => m.PropValue).SingleOrDefault(),
                                personalisation = (s.Where(w => w.PropName == "personalisation").Select(m => m.PropValue).SingleOrDefault()) == null ? null : s.Where(w => w.PropName == "personalisation").Select(m => m.PropValue).SingleOrDefault().Replace("\"", "").Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray(),
                                templateId = s.Where(w => w.PropName == "templateId").Select(m => m.PropValue).SingleOrDefault()
                            }).FirstOrDefault())
                        });
                        outputslist.Add(opt);
                    });

                    //imported dataset
                    importeddatasetslist = mapper.Map<IEnumerable<BL.Models.Document>, List<Importeddataset>>(objform.Documents.Where(w => string.IsNullOrEmpty(w.FilePath)));
                    //documents
                    documentslist = mapper.Map<IEnumerable<BL.Models.Document>, List<Document>>(objform.Documents.Where(w => !string.IsNullOrEmpty(w.FilePath)));
                    //tabs
                    tabslist = new List<Tab>();
                    objform.TabDetails.ToList().ForEach(tb =>
                    {
                        var tab = mapper.Map<Tab, Tab>(new Tab
                        {
                            id = tb.TabName,
                            tabData = mapper.Map<IEnumerable<TabChildDetail>, List<Tabdata>>(tb.TabChildDetails).ToArray(),
                        });
                        tabslist.Add(tab);
                    });

                    //designdataset
                    designdatasetslist = new List<Designeddataset>();
                    objform.DesignDataSets.ToList().ForEach(dd =>
                    {
                        var ddata = dd.DesignDataSetDetails.GroupBy(g => g.Rowno).Select(a => a.ToArray()).ToArray();
                        var ddst = mapper.Map<Designeddataset, Designeddataset>(new Designeddataset
                        {

                            csvUsed = dd.CsvUsed,
                            id = dd.DdsetId,
                            keyIdentifier = dd.KeyIdentifier,
                            title = dd.Title,
                            uploadedDate = dd.UploadedOn,
                            data = mapper.Map<DesignDataSetDetail[][], Datum[][]>(ddata)
                        });

                        designdatasetslist.Add(ddst);
                    });

                    var formmapping = mapper.Map<Form, Forms>(objform);
                    formmapping.parentDetails = _unitOfWork.FormsRepository.getParentById(formId);
                    formmapping.parentChild = _unitOfWork.FormsRepository.getParentChild(formmapping.parentChild);

                    formmapping.calculations = calculationlist.ToArray();
                    formmapping.pages = Pageslist.ToArray();
                    formmapping.conditions = conditionslist.ToArray();
                    formmapping.outputs = outputslist.ToArray();
                    formmapping.importedDataSets = importeddatasetslist.ToArray();
                    formmapping.documents = documentslist.ToArray();
                    formmapping.designedDataSets = designdatasetslist.ToArray();
                    formmapping.tabs = tabslist.ToArray();
                    Formslist.Add(formmapping);
                }

            }
            catch (Exception ex) { throw ex; }
            sw.Stop();
            Console.WriteLine("getConfiguration: " + sw.ElapsedMilliseconds);
            return Formslist.FirstOrDefault();
        }

        public IEnumerable<FormConfiguration> listFormConfigurations()
        {
            var formlist = new List<FormConfiguration>();
            var sw = Stopwatch.StartNew();
            sw.Start();
            try
            {
                var mapper = MapperConfig.Mapper;
                var data = _unitOfWork.FormsRepository.listFormConfigurations();
                formlist = mapper.Map<IEnumerable<Form>, List<FormConfiguration>>(data);
            }
            catch (Exception ex) { throw ex; }
            sw.Stop();
            Console.WriteLine("listFormConfigurations: " + sw.ElapsedMilliseconds);
            return formlist;

        }

        public bool checkFormExists(string formid, string formname)
        {
            bool result = false;
            var sw = Stopwatch.StartNew();
            sw.Start();
            result = _unitOfWork.FormsRepository.checkFormExists(formid, formname);
            sw.Stop();
            Console.WriteLine("checkFormExists: " + sw.ElapsedMilliseconds);
            return result;
        }

        public List<KeyValuePair<string, bool>> checkMultipleFormExists(string[] formid)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var result = _unitOfWork.FormsRepository.checkMultipleFormExists(formid);
            sw.Stop();
            Console.WriteLine("checkMultipleFormExists: " + sw.ElapsedMilliseconds);
            return result;
        }
        public async Task<bool> changeformstatus(string formid, string status)
        {
            bool result = false;
            var sw = Stopwatch.StartNew();
            sw.Start();
            var existingData = _unitOfWork.FormsRepository.getFormById(formid);

            if (existingData != null)
            {
                List<FormStatus> formStatusList = _unitOfWork.formStatusRepository.GetAll().ToList();
                long? newFsid = formStatusList.FirstOrDefault(fs => fs.Status == status)?.Fsid;

                if (newFsid.HasValue)
                {
                    existingData.Fsid = newFsid.Value;
                    existingData.LastUpdatedOn = DateTime.Now;
                    _unitOfWork.FormsRepository.Update(existingData);
                    await _unitOfWork.Complete();
                    result = true;
                    // Status change successful
                }
            }
            sw.Stop();
            Console.WriteLine("changeformstatus: " + sw.ElapsedMilliseconds);
            return result;
        }
        public bool checkResponseExists(string responseid)
        {

            bool result = false;
            var sw = Stopwatch.StartNew();
            sw.Start();

            result = _unitOfWork.ResponseRepository.Find(x => x.Id == responseid).Any();
            sw.Stop();
            Console.WriteLine("checkResponseExists: " + sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<bool> createResponseDoc(Responses responsedata, StringBuilder sb)
        {
            var result = false;
            bool newuser = false;
            bool neworg = false;
            UserDetail? userDetail = null;
            UserOrganisationDetail? userOrganisationDetail = null;
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.ResponseMapper;

            List<BL.Models.ResponseQuestionDatum> questdatalist = new List<ResponseQuestionDatum>();
            List<BL.Models.ResponseQuestion> questlist = new List<ResponseQuestion>();
            var organisationlist = new List<OrganisationDetail>();
            IEnumerable<UserDetail> createuser = Enumerable.Empty<UserDetail>();
            List<OrganisationDetail> createorg = new List<OrganisationDetail>();

            try
            {
                if (responsedata.user != null)
                {
                    var (userdata, newUser) = await GetorCreateUser(responsedata.user, mapper);
                    var (organisationdata, newOrg) = await GetorCreateOrganisation(responsedata.user?.organization, mapper);
                    var userorgdata = await GetorCreateUserOrganisation(userdata, organisationdata);
                    userDetail = userdata;
                    userOrganisationDetail = userorgdata;
                }
                var ResponseMetadata = new MetaData
                {
                    PropName = "paymentSkipped",
                    PropType = "paymentSkipped",
                    PropValue = responsedata.metadata?.paymentSkipped.ToString()
                };

                //response questions 

                responsedata.questions?.ToList().ForEach(quest =>
                {
                    questdatalist = new List<ResponseQuestionDatum>();
                    quest.fields.ToList().ForEach(questdata => questdatalist.Add(
                         new ResponseQuestionDatum
                         {
                             Title = questdata.title,
                             Key = questdata.key,
                             Answer = (questdata.answer != null) ? ((questdata.answer.GetType() == typeof(string)) ? Convert.ToString(questdata.answer) : string.Join(',', questdata.answer)) : string.Empty,
                             Type = questdata.type
                         }));

                    var question = new ResponseQuestion();
                    question.Question = quest.question;
                    question.ResponseQuestionData = questdatalist.Where(w => w.Answer != "CompNotRendered").ToList();
                    questlist.Add(question);
                });
                var data = mapper.Map<Responses, BL.Models.Response>(responsedata);
                data.ResponseQuestions = questlist;
                data.ResponseStatus = true;
                data.isUAT = responsedata.isUAT;
                data.UpdatedOn = DateTime.Now;
                if (newuser)
                {
                    data.User = userDetail;
                    data.UpdatedBy = 0;
                    data.UserId = 0;
                    data.User.Status = "1";
                    data.UserOrganisationDetails = userOrganisationDetail;
                }
                else if (responsedata.user == null)
                {
                    data.UpdatedBy = null;
                    data.UserId = null;
                }
                else
                {
                    data.UserId = userDetail.Uid;
                    data.UserOrganisationDetails = userOrganisationDetail;
                    data.UpdatedBy = userDetail.Uid;
                    data.User = userDetail;
                    data.User.Status = "1";
                }

                data.Mtdt = ResponseMetadata;
                _unitOfWork.ResponseRepository.AddRange(new List<BL.Models.Response> { data });
                await _unitOfWork.Complete();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            sw.Stop();
            sb.AppendLine("Response Creation: " + sw.ElapsedMilliseconds);
            Console.WriteLine(sb.ToString());
            return result;
        }
        public async Task<bool> AddorUpdateDraftResponse(JObject json)
        {
            var result = false;
            var sw = Stopwatch.StartNew();
            var draftResponse = new DraftResponse();
            var properties = typeof(DraftResponse).GetProperties().Select(p => p.Name).ToHashSet();

            // Deserialize JSON into DraftResponse object
            foreach (var property in json.Properties())
            {
                if (properties.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                {
                    var prop = typeof(DraftResponse).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        var value = property.Value.ToObject(prop.PropertyType);
                        prop.SetValue(draftResponse, value);
                    }
                }
                else
                {
                    draftResponse.otherobjects ??= new Dictionary<string, object>();
                    object rawValue;
                    switch (property.Value.Type)
                    {
                        case JTokenType.Boolean:
                            rawValue = property.Value.ToObject<bool>();
                            break;
                        case JTokenType.Null:
                            rawValue = null;
                            break;
                        default:
                            rawValue = property.Value;
                            break;
                    }
                    draftResponse.otherobjects[property.Name] = rawValue;
                }
            }
            var mapper = MapperConfig.DraftResponseMapper;
            try
            {
                if (draftResponse.user != null)
                {
                    var (userdata, newUser) = await GetorCreateUser(draftResponse.user, mapper);
                    var (organisationdata, newOrg) = await GetorCreateOrganisation(draftResponse.user.organization, mapper);
                    var userorgdata = await GetorCreateUserOrganisation(userdata, organisationdata);

                    draftResponse.UserOrganisationDetails = userorgdata;
                }
                if (draftResponse.Id != null)
                    draftResponse.Formid = draftResponse.Id.Substring(0, 10);
                var responsedata = new List<BL.Models.DraftResponse>();
                BL.Models.Output output = null;
                BL.Models.Response response = null;
                if (draftResponse.Formid != null)
                {
                    long fid = await _unitOfWork.FormsRepository.getFormFID(draftResponse.Formid);
                    output = await _unitOfWork.OutputsRepository.FindAsync(W => W.Fid == fid).FirstOrDefaultAsync();
                    response = await _unitOfWork.ResponseRepository.FindAsync(W => W.Id == draftResponse.Reference).FirstOrDefaultAsync();

                }
                draftResponse.Progresses = new Dictionary<string, object>();
                draftResponse.PreviousPages = new Dictionary<string, object>();
                if (draftResponse.Progress != null)
                {
                    draftResponse.Progresses.Add("Progress", string.Join(",", draftResponse.Progress));
                }
                if (draftResponse.PreviousPage != null)
                {
                    draftResponse.PreviousPages.Add("PreviousPage", draftResponse.PreviousPage.ToString());
                }

                AddDataToList(draftResponse, draftResponse.FormData, output, response, "FormData", responsedata, mapper);
                AddDataToList(draftResponse, draftResponse.SelectField, output, response, "SelectField", responsedata, mapper);
                AddDataToList(draftResponse, draftResponse.DataImportStatus, output, response, "DataImportStatus", responsedata, mapper);
                AddDataToList(draftResponse, draftResponse.otherobjects, output, response, "External", responsedata, mapper);
                AddDataToList(draftResponse, draftResponse.Progresses, output, response, "Progress", responsedata, mapper);
                AddDataToList(draftResponse, draftResponse.PreviousPages, output, response, "PreviousPage", responsedata, mapper);
                List<BL.Models.DraftResponse> dbData = await _unitOfWork.DraftResponseRepository.GetDraftResponse(draftResponse.Id);
                //_unitOfWork.DraftResponseRepository.Find(g => g.Id == draftResponse.Id).ToList();
                int count = 0;
                foreach (var item in responsedata)
                {
                    var matchingResponse = dbData.FirstOrDefault(r => r.Key == item.Key && r.Type == item.Type);
                    if (matchingResponse != null)
                    {
                        if (!AreEqual(matchingResponse, item))
                        {
                            _unitOfWork.DraftResponseRepository.Detach(matchingResponse);
                            item.DRID = matchingResponse.DRID;
                            item.UpdatedOn = DateTime.Now;
                            _unitOfWork.DraftResponseRepository.Update(item);
                        }
                    }
                    else
                    {
                        item.UpdatedOn = DateTime.Now;
                        _unitOfWork.DraftResponseRepository.Add(item);
                    }
                    count++;
                    if (count % 50 == 0)
                    {
                        await _unitOfWork.Complete();
                    }
                }

                foreach (var item in dbData)
                {
                    if (!responsedata.Any(s => s.Key == item.Key && s.Type == item.Type))
                    {
                        _unitOfWork.DraftResponseRepository.Remove(item);
                    }
                }
                await _unitOfWork.Complete();
                result = true;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while processing the draft response.", ex);
            }
            finally
            {
                Console.WriteLine("AddorUpdateDraftResponse: " + sw.ElapsedMilliseconds);
                sw.Stop();
            }
            return result;
        }
        private bool AreEqual(BL.Models.DraftResponse a, BL.Models.DraftResponse b)
        {
            var obj = new GetComparer<BL.Models.DraftResponse>();
            var comparer = obj.GetComparerobj<BL.Models.DraftResponse>();
            comparer.AddComparerOverride("DRID", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("UpdatedOn", DoNotCompareValueComparer.Instance);
            //comparer.AddComparerOverride("UserOrganisationDetails", DoNotCompareValueComparer.Instance);
            bool result = comparer.Compare(a, b);
            return result;
        }

        public async Task<Dictionary<string, object>> GetDraftResponse(string id)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.DraftResponseMapper;
            List<BL.Models.DraftResponse> draftResponses = await _unitOfWork.DraftResponseRepository.GetDraftResponse(id);

            BL.Models.DraftResponse draftResponsesdata = draftResponses.FirstOrDefault();
            DraftResponse draftResponse = mapper.Map<BL.Models.DraftResponse, DraftResponse>(draftResponsesdata);

            var serializedDict = new Dictionary<string, object>();
            if (draftResponsesdata != null)
            {
                List<BL.Models.Output> Outputs = _unitOfWork.ResponsesRepository.GetOutput(draftResponsesdata.OUID).ToList();
                UserOrganisationDetail UserOrganisationDetails = _unitOfWork.ResponsesRepository.GetUserOrgDetails(draftResponsesdata.UserOrgID).FirstOrDefault();
                Users user = new Users();
                if (UserOrganisationDetails != null)
                {
                    user = mapper.Map<UserDetail, Users>(UserOrganisationDetails.User);
                    user.organization = mapper.Map<OrganisationDetail, Organization>(UserOrganisationDetails.OrganisationDetails);
                }
                outputslist = new List<Output>();
                Outputs.ToList().ForEach(op =>
                {
                    var opt = mapper.Map<Output, Output>(new Output
                    {
                        name = op.Name,
                        title = op.Title,
                        type = op.Type,
                        outputConfiguration = mapper.Map<Outputconfiguration, Outputconfiguration>(op.OutputDetails?.GroupBy(g => g.Ouid).Select(s => new Outputconfiguration
                        {
                            addReferencesToPersonalisation = (s.Where(w => w.PropName == "addReferencesToPersonalisation").Select(m => m.PropValue).SingleOrDefault()) == null ? null : s.Where(w => w.PropName == "addReferencesToPersonalisation").Select(m => Convert.ToBoolean(m.PropValue)).SingleOrDefault(),
                            apiKey = s.Where(w => w.PropName == "apiKey").Select(m => m.PropValue).SingleOrDefault(),
                            emailAddress = s.Where(w => w.PropName == "emailAddress").Select(m => m.PropValue).SingleOrDefault(),
                            emailField = s.Where(w => w.PropName == "emailField").Select(m => m.PropValue).SingleOrDefault(),
                            personalisation = (s.Where(w => w.PropName == "personalisation").Select(m => m.PropValue).SingleOrDefault()) == null ? null : s.Where(w => w.PropName == "personalisation").Select(m => m.PropValue).SingleOrDefault().Replace("\"", "").Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray(),
                            templateId = s.Where(w => w.PropName == "templateId").Select(m => m.PropValue).SingleOrDefault()
                        }).FirstOrDefault())
                    });
                    outputslist.Add(opt);
                });

                var formData = draftResponses
                               .Where(w => w.Type == "FormData")
                               .GroupBy(kvp => kvp.Key)
                               .ToDictionary(g => g.Key, g => (object)g.First().Answer);

                var selectField = draftResponses
                                  .Where(w => w.Type == "SelectField")
                                  .GroupBy(kvp => kvp.Key)
                                  .ToDictionary(g => g.Key, g => (object)g.First().Answer);

                var dataImportStatus = draftResponses
                                       .Where(w => w.Type == "DataImportStatus")
                                       .GroupBy(kvp => kvp.Key)
                                       .ToDictionary(g => g.Key, g => (object)g.First().Answer);

                var external = draftResponses
                              .Where(w => w.Type == "External")
                              .GroupBy(kvp => kvp.Key)
                              .ToDictionary(g => g.Key, g => (object)g.First().Answer);

                string progress = draftResponses.Where(w => w.Type == "Progress")?.Select(s => s.Answer.ToString()).FirstOrDefault();
                string[] progresses = progress?.Split(',');
                string PreviousPage = draftResponses.Where(w => w.Type == "PreviousPage")?.Select(s => s.Answer.ToString()).FirstOrDefault();

                var finalJsonDict = new Dictionary<string, object>();

                foreach (var kvp in external)
                {
                    finalJsonDict[kvp.Key] = kvp.Value;
                }
                if (formData.Any())
                    finalJsonDict["formData"] = formData;
                if (selectField.Any())
                    finalJsonDict["selectField"] = selectField;
                if (progresses != null)
                    finalJsonDict["progress"] = progresses;
                if (dataImportStatus.Any())
                    finalJsonDict["dataImportStatus"] = dataImportStatus;
                if (draftResponse.Formid != null)
                    finalJsonDict["Formid"] = draftResponse.Formid;
                if (draftResponse.FormDataId != null)
                    finalJsonDict["FormDataId"] = draftResponse.FormDataId;
                if (draftResponse.UserCompletedSummary != null)
                    finalJsonDict["userCompletedSummary"] = draftResponse.UserCompletedSummary;
                if (draftResponse.Reference != null)
                    finalJsonDict["reference"] = draftResponse.Reference;
                if (draftResponse.ReferenceIsStored != null)
                    finalJsonDict["referenceIsStored"] = draftResponse.ReferenceIsStored;
                if (PreviousPage != null)
                    finalJsonDict["previousPage"] = PreviousPage;
                if (outputslist.Any())
                    finalJsonDict["outputs"] = outputslist;
                if (user.email != null)
                    finalJsonDict["user"] = user;
                foreach (var kvp in finalJsonDict)
                {
                    object deserializedValue = kvp.Value;
                    if (kvp.Value is string jsonValue && IsJson(jsonValue))
                    {
                        deserializedValue = JsonConvert.DeserializeObject(jsonValue);
                        serializedDict[kvp.Key] = deserializedValue.ToString();
                    }
                    else if (kvp.Value is Dictionary<string, object> dictionaryValue)
                    {
                        dictionaryValue = DeserializeDictionaryValues(dictionaryValue);
                        deserializedValue = dictionaryValue;
                        serializedDict[kvp.Key] = deserializedValue;
                    }

                    if (deserializedValue is int || deserializedValue is long || deserializedValue is double || deserializedValue is float || deserializedValue is decimal)
                    {
                        deserializedValue = deserializedValue.ToString();
                    }

                    serializedDict[kvp.Key] = deserializedValue;
                }
            }
            sw.Stop();
            Console.WriteLine("GetDraftResponse: " + sw.ElapsedMilliseconds);
            return serializedDict;
        }

        private Dictionary<string, object> DeserializeDictionaryValues(Dictionary<string, object> dictionary)
        {
            var deserializedDictionary = new Dictionary<string, object>();
            foreach (var kvp in dictionary)
            {
                object deserializedValue = kvp.Value;
                if (kvp.Value is string jsonValue && IsJson(jsonValue))
                {
                    deserializedValue = JsonConvert.DeserializeObject(jsonValue);
                }
                else if (kvp.Value is Dictionary<string, object> nestedDictionary)
                {
                    nestedDictionary = DeserializeDictionaryValues(nestedDictionary);
                    deserializedValue = nestedDictionary;
                }
                deserializedDictionary[kvp.Key] = deserializedValue;
            }
            return deserializedDictionary;
        }

        public async Task<UserDetail> GetUser(Guid userId)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var result = await _unitOfWork.FormsRepository.GetUserDetails(userId).FirstOrDefaultAsync();
            sw.Stop();
            Console.WriteLine("GetUser: " + sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<bool> updateUser(UserDetail user)
        {
            bool result = false;
            var sw = Stopwatch.StartNew();
            sw.Start();
            var existingUser = await _unitOfWork.FormsRepository.GetUserDetails(user.UserId).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                // Map updated fields to existingUser if needed
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Status = user.Status;

                _unitOfWork.userRepository.Update(existingUser);
                await _unitOfWork.Complete();
                result = true;
            }
            sw.Stop();
            Console.WriteLine("updateUser: " + sw.ElapsedMilliseconds);
            return result;
        }

        private async Task<(UserDetail entity, bool isNew)> GetorCreateUser(Users user, IMapper mapper)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var createuser = await _unitOfWork.FormsRepository.GetUserDetails(user.id).ToListAsync();
            if (!createuser.Any())
            {
                var userdata = mapper.Map<Users, BL.Models.UserDetail>(user);
                _unitOfWork.userRepository.AddRange(new List<BL.Models.UserDetail> { userdata });
                return (userdata, true);
            }
            sw.Stop();
            Console.WriteLine("GetorCreateUser: " + sw.ElapsedMilliseconds);
            return (createuser.FirstOrDefault(), false);
        }

        private async Task<(OrganisationDetail entity, bool isNew)> GetorCreateOrganisation(Organization organization, IMapper mapper)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var createorg = await _unitOfWork.FormsRepository.GetOrganisationDetail(organization.ukprn).ToListAsync();
            if (!createorg.Any())
            {
                var organisationdata = mapper.Map<Organization, BL.Models.OrganisationDetail>(organization);
                _unitOfWork.organisationRepository.AddRange(new List<BL.Models.OrganisationDetail> { organisationdata });
                return (organisationdata, true);
            }
            sw.Stop();
            Console.WriteLine("GetorCreateOrganisation: " + sw.ElapsedMilliseconds);
            return (createorg.FirstOrDefault(), false);
        }

        private async Task<UserOrganisationDetail> GetorCreateUserOrganisation(UserDetail userdata, OrganisationDetail organisationdata)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var createuserorg = await _unitOfWork.FormsRepository.GetUserOrganisationDetail(userdata.Uid, organisationdata.Orgid).OrderByDescending(o => o.UserOrgID).ToListAsync();
            if (!createuserorg.Any())
            {
                var userorgdata = new BL.Models.UserOrganisationDetail
                {
                    User = userdata,
                    OrganisationDetails = organisationdata
                };
                _unitOfWork.userOrganisationRepository.Add(userorgdata);
                return userorgdata;
            }
            sw.Stop();
            Console.WriteLine("GetorCreateOrganisation: " + sw.ElapsedMilliseconds);
            return createuserorg.FirstOrDefault();
        }

        private void AddDataToList<T>(DraftResponse model, Dictionary<string, T> dictionary, BL.Models.Output output, BL.Models.Response response, string type, List<BL.Models.DraftResponse> data, IMapper mapper)
        {
            if (dictionary != null)
            {
                foreach (var item in dictionary)
                {
                    var model1 = mapper.Map<DraftResponse, BL.Models.DraftResponse>(model);
                    model1.Outputs = output;
                    model1.Responses = response;
                    model1.Type = type;
                    model1.Key = item.Key;
                    model1.Answer = ConvertValueToString(item.Value);
                    data.Add(model1);
                }
            }
        }
        private string ConvertValueToString<T>(T value)
        {
            if (value is bool boolValue)
            {
                return boolValue.ToString().ToLower();
            }
            else if (value is null)
            {
                return null;
            }
            return value.ToString();
        }
        private bool IsJson(string str)
        {
            bool result = false;
            try
            {
                JToken.Parse(str);
                result = true;
            }
            catch (JsonReaderException)
            {
                result = false;
            }
            return result;
        }

        public async Task<SubmissionForm> GetSubmissionFormLog(string id)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.SubmissionMapper;
            SubmissionFormLog submissionFormLog = await _unitOfWork.SubmissionFormLogRepository.FindAsync(g => g.id == id).AsNoTracking().FirstOrDefaultAsync();
            SubmissionForm data = mapper.Map<BL.Models.SubmissionFormLog, SubmissionForm>(submissionFormLog);
            sw.Stop();
            Console.WriteLine("GetSubmissionFormLog: " + sw.ElapsedMilliseconds);
            return data;
        }
        public async Task<bool> PostSubmissionFormLog(SubmissionForm submissionForm)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.SubmissionMapper;

            var existingSubmissionFormLog = await _unitOfWork.SubmissionFormLogRepository
                                              .FindAsync(g => g.id == submissionForm.id)
                                              .FirstOrDefaultAsync();

            if (existingSubmissionFormLog != null)
            {
                mapper.Map(submissionForm, existingSubmissionFormLog);
                _unitOfWork.SubmissionFormLogRepository.Update(existingSubmissionFormLog);
            }
            else
            {
                SubmissionFormLog newSubmissionFormLog = mapper.Map<SubmissionForm, SubmissionFormLog>(submissionForm);
                newSubmissionFormLog.Createdon = DateTime.Now;
                _unitOfWork.SubmissionFormLogRepository.Add(newSubmissionFormLog);
            }

            await _unitOfWork.Complete();
            sw.Stop();
            Console.WriteLine("PostSubmissionFormLog: " + sw.ElapsedMilliseconds);
            return true;
        }

        public async Task<DocData> GetDocumentCapture(string fileId)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.DCDataMapper;
            DCData DCData = await _unitOfWork.DCDATARepository.FindAsync(g => g.fileId == fileId).AsNoTracking().FirstOrDefaultAsync();
            DocData data = mapper.Map<BL.Models.DCData, DocData>(DCData);
            sw.Stop();
           Console.WriteLine("DCData: " + sw.ElapsedMilliseconds);
            return data;
        }
        public async Task<bool> PostDocumentCapture(DocData dCData)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var mapper = MapperConfig.DCDataMapper;
            long? id = Convert.ToInt64(dCData.DCDID); 
            var existingDCData = await _unitOfWork.DCDATARepository
                                              .FindAsync(g => g.DCDID == id)
                                              .FirstOrDefaultAsync();

            if (existingDCData != null)
            {
                mapper.Map(dCData, existingDCData);
                _unitOfWork.DCDATARepository.Update(existingDCData);
            }
            else
            {
                DCData newDCData = mapper.Map< DocData, DCData>(dCData);
                _unitOfWork.DCDATARepository.Add(newDCData);
            }

            await _unitOfWork.Complete();
            sw.Stop();
           Console.WriteLine("DCData: " + sw.ElapsedMilliseconds);
            return true;
        }

        public async Task<Forms> GetRepeatableFormsData(string id, Dictionary<string, string> queryParams,bool includeAll)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            string fid = id.Substring(0, 10);
            long formid = await _unitOfWork.FormsRepository.getFormFID(fid);
            Forms form = new();
            List<RepeatableFormsMapper> repeatableFormsMapperaddlist = new();

            var repeatableFormsDatum = await _unitOfWork.RepeatableFormsDataRepository
                .FindAsync(g => g.Status != true && g.ID.StartsWith(fid))
                .ToListAsync();

            var existingData = repeatableFormsDatum.FirstOrDefault(w => w.ID == id);
            bool isExistingUkprn = existingData != null;

            RepeatableFormsData repeatableFormsData = existingData ?? repeatableFormsDatum.FirstOrDefault();

            if (repeatableFormsData == null)
                return form;



            string decompressedJson = Decompress(repeatableFormsData?.FormData);
            var deserializedData = JsonConvert.DeserializeObject<JObject>(decompressedJson);
            form = deserializedData?.ToObject<Forms>() ?? new Forms();

            if (!isExistingUkprn)
            {
                bool isUat = form.formStatus != "Published";
                form.ukprn = isUat ? id.Substring(10, id.Length - 13) : id.Substring(10, id.Length - 10);
                string jsonData = JsonConvert.SerializeObject(form, Formatting.Indented);
                byte[] compressedData = Compress(jsonData);
                _unitOfWork.RepeatableFormsDataRepository.Detach(repeatableFormsData);
                repeatableFormsData.RFID = 0;
                repeatableFormsData.ID = fid + form.ukprn + (isUat ? "UAT" : "");
                repeatableFormsData.UpdatedOn = DateTime.Now;
                repeatableFormsData.FormData = compressedData;
                _unitOfWork.RepeatableFormsDataRepository.Add(repeatableFormsData);
            }
            var formPages = new List<Page>(form.pages);

            foreach (var item in queryParams)
            {
                string sectionname = await _unitOfWork.SectionsRepository
                                    .FindAsync(g => (g.Scnumbercomp == item.Key || g.Scconditioncomp == item.Key) && g.Fid == formid).AsNoTracking()
                                    .OrderByDescending(g => g.Scnumbercomp != null && g.Scnumbercomp == item.Key)
                                    .Select(s => s.Scname)
                                    .FirstOrDefaultAsync();

                int repeatCount = Convert.ToInt32(item.Value);
                List<long> existingFormMapperIds = await _unitOfWork.RepeatableFormsMapperRepository.FindAsync(g => g.RFID == repeatableFormsData.RFID).AsNoTracking().Select(s => s.RFSID).ToListAsync();

                var repeatableSectionsData = await _unitOfWork.RepeatableSectionsDataRepository
               .FindAsync(g => g.Status != true &&
                               g.SectionName == sectionname &&
                               existingFormMapperIds.Contains(g.RFSID) &&
                               (includeAll || g.NoofRepeats == repeatCount))
               .OrderByDescending(g => g.RFSID)
               .AsNoTracking()
               .FirstOrDefaultAsync();


                if (repeatableSectionsData?.SectionData != null)
                {
                    string decompressedSectionJson = Decompress(repeatableSectionsData.SectionData);
                    var deserializedPage = JsonConvert.DeserializeObject<JObject>(decompressedSectionJson);
                    formPages.AddRange(deserializedPage["pages"].ToObject<List<Page>>());

                    if (!isExistingUkprn)
                        repeatableFormsMapperaddlist.Add(new RepeatableFormsMapper
                        {
                            RepeatableFormsDatum = repeatableFormsData,
                            RepeatableSectionsDatum = repeatableSectionsData
                        });
                }


            }

            form.pages = formPages.ToArray();


            if (!isExistingUkprn)
            {
                _unitOfWork.RepeatableFormsMapperRepository.AddRange(repeatableFormsMapperaddlist);
                await _unitOfWork.Complete();
            }
            sw.Stop();
            Console.WriteLine("GetRepeatableFormsData: " + sw.ElapsedMilliseconds);
            return form;
        }

        public async Task<bool> PostRepeatableFormsData(JObject json)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            string ukprn = json["ukprn"]?.ToString();
            string id = json["id"]?.ToString();
            string isUat = json["formStatus"]?.ToString();
            RepeatableFormsData repeatableFormsData = new RepeatableFormsData { ID = id + ukprn + (isUat == "Published" ? "" : "UAT"), UpdatedOn = DateTime.Now };
            Forms form = json.ToObject<Forms>(); Forms updatedform = json.ToObject<Forms>();
            var repeatableSections = form?.sections?.Where(w => w.repeatableSection == true).ToHashSet();
            var repeatableSectionNames = repeatableSections.Select(s => s.name).ToHashSet();
            updatedform.pages = updatedform.pages.Where(p => !repeatableSectionNames.Contains(p.section) || (string.IsNullOrEmpty(p.pageSequence) ? true : Convert.ToInt16(p.pageSequence) <= 1)).ToArray();

            string jsonData = JsonConvert.SerializeObject(updatedform, Formatting.Indented);
            List<KeyValuePair<string, string>> sectionlist = new List<KeyValuePair<string, string>>();
            foreach (var item in repeatableSections)
            {
                updatedform.pages.Where(w => w.section == item.name);
                new KeyValuePair<string, string>("Key1", "Value1");
            }
            byte[] compressedData = Compress(jsonData);
            repeatableFormsData.FormData = compressedData;
            JObject repeatableFormsjsonData = (JObject)JToken.FromObject(repeatableFormsData);
            var existingrepeatableFormsData = await _unitOfWork.RepeatableFormsDataRepository.FindAsync(g => g.Status != true && g.ID == repeatableFormsData.ID).FirstOrDefaultAsync();
            if (existingrepeatableFormsData != null)
            {
                string decompressedJson = Decompress(existingrepeatableFormsData.FormData);
                JObject deserializedData = JsonConvert.DeserializeObject<JObject>(decompressedJson);
                bool changed = !AreFormsEqual(repeatableFormsjsonData, deserializedData);
                if (!changed)
                {
                    repeatableFormsData = existingrepeatableFormsData; //return true;
                }
                else
                {
                    _unitOfWork.RepeatableFormsDataRepository.Detach(existingrepeatableFormsData);
                    repeatableFormsData.Status = existingrepeatableFormsData.Status;
                    repeatableFormsData.CreatedOn = existingrepeatableFormsData.CreatedOn;
                    repeatableFormsData.RFID = existingrepeatableFormsData.RFID;
                    _unitOfWork.RepeatableFormsDataRepository.Update(repeatableFormsData);
                }
            }
            else { repeatableFormsData.CreatedOn = DateTime.Now; _unitOfWork.RepeatableFormsDataRepository.Add(repeatableFormsData); }
            List<RepeatableSectionsData> repeatableSectionsDataList = new List<RepeatableSectionsData>();
            List<RepeatableSectionsData> repeatableSectionsDatAddList = new List<RepeatableSectionsData>();
            List<RepeatableFormsMapper> repeatableFormsMapperList = new List<RepeatableFormsMapper>();
            foreach (Section item in repeatableSections)
            {
                var repeatablePages = form?.pages?.Where(p => p.section == item.name && (string.IsNullOrEmpty(p.pageSequence) ? true : Convert.ToInt16(p.pageSequence) > 1)).ToList();
                var Pages = new { pages = repeatablePages };
                JObject pagesJsonobject = JObject.FromObject(Pages);
                string PagesJson = JsonConvert.SerializeObject(Pages, Formatting.Indented);
                byte[] compressedPages = Compress(PagesJson);
                int noofRepeats = Convert.ToInt32(item.triggerCompValue);

                RepeatableSectionsData repeatableSectionsData = new RepeatableSectionsData();
                repeatableSectionsData.FID = id;
                repeatableSectionsData.NoofRepeats = noofRepeats;
                repeatableSectionsData.SectionName = item.name;
                repeatableSectionsData.SectionData = compressedPages;


                List<long> existingFormMapperIds = await _unitOfWork.RepeatableFormsMapperRepository
                                                                            .FindAsync(g => g.RFID == repeatableFormsData.RFID)
                                                                            .AsNoTracking()
                                                                            .Select(s => s.RFSID)
                                                                            .ToListAsync();

                List<RepeatableSectionsData> existingSectionsData = await _unitOfWork.RepeatableSectionsDataRepository
                                                                            .FindAsync(g => g.Status != true && g.SectionName == item.name && existingFormMapperIds.Contains(g.RFSID))
                                                                            .ToListAsync();
                bool matchedsectionwithcount = false;
                RepeatableSectionsData matchedSectionData = null;

                // Check if any of the existing sections data match
                foreach (var sectionDataItem in existingSectionsData)
                {
                    string decompressedSectionData = Decompress(sectionDataItem.SectionData);
                    JObject deserializedSectionData = JsonConvert.DeserializeObject<JObject>(decompressedSectionData);

                    bool changed = !AreFormsEqual(pagesJsonobject, deserializedSectionData);

                    if (!changed && repeatableSectionsData.NoofRepeats == sectionDataItem.NoofRepeats)
                    {
                        matchedSectionData = sectionDataItem;
                        matchedsectionwithcount = true;
                        break;
                    }
                }

                if (matchedsectionwithcount)
                {
                    repeatableSectionsData = matchedSectionData;
                }
                else
                {
                    foreach (var oldSection in existingSectionsData)
                    {
                        if (existingFormMapperIds.Contains(oldSection.RFSID))
                        {
                            oldSection.Status = true;
                            repeatableSectionsDataList.Add(oldSection);
                        }
                    }
                    repeatableSectionsDatAddList.Add(repeatableSectionsData);
                }
                var existingrepeatableFormsMapper = _unitOfWork.RepeatableFormsMapperRepository.Find(g => g.RepeatableFormsDatum == repeatableFormsData && g.RepeatableSectionsDatum == repeatableSectionsData).ToHashSet();
                if (!existingrepeatableFormsMapper.Any())
                {
                    RepeatableFormsMapper repeatableFormsMapper = new RepeatableFormsMapper();
                    repeatableFormsMapper.RepeatableFormsDatum = repeatableFormsData;
                    repeatableFormsMapper.RepeatableSectionsDatum = repeatableSectionsData;
                    repeatableFormsMapperList.Add(repeatableFormsMapper);
                }
            }
            _unitOfWork.RepeatableSectionsDataRepository.UpdateRange(repeatableSectionsDataList);
            _unitOfWork.RepeatableSectionsDataRepository.AddRange(repeatableSectionsDatAddList);
            _unitOfWork.RepeatableFormsMapperRepository.AddRange(repeatableFormsMapperList);
            await _unitOfWork.Complete();
            sw.Stop();
            Console.WriteLine("PostRepeatableFormsData: " + sw.ElapsedMilliseconds);
            return true;
        }

        private bool AreFormsEqual(JObject a, JObject b)
        {
            if (a == null || b == null)
                return false;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None
            };

            JToken pagesA = a["pages"] ?? a;
            JToken pagesB = b["pages"] ?? b;
            return JToken.DeepEquals(pagesA, pagesB);
        }

        private static byte[] Compress(string data)
        {
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(byteArray, 0, byteArray.Length);
                }
                return ms.ToArray();
            }
        }


        private static string Decompress(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
            using (MemoryStream outputMs = new MemoryStream())
            {
                gzip.CopyTo(outputMs);
                return System.Text.Encoding.UTF8.GetString(outputMs.ToArray());
            }
        }

        public async Task<bool> DeleteRepeatableFormsData(string id)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            bool result = false;
            var existingrepeatableFormsData = _unitOfWork.RepeatableFormsDataRepository
                                               .Find(g => g.ID.Length >= 10 && g.ID.Substring(0, 10) == id)
                                               .ToList();
            if (existingrepeatableFormsData.Any())
            {

                foreach (RepeatableFormsData item in existingrepeatableFormsData)
                {

                    item.Status = true;
                    item.UpdatedOn = DateTime.Now;
                    _unitOfWork.RepeatableFormsDataRepository.Update(item);

                    var mapperData = _unitOfWork.RepeatableFormsMapperRepository
                        .Find(g => g.RFID == item.RFID)
                        .Select(s => s.RFSID)
                        .ToList();

                    var sectionsData = _unitOfWork.RepeatableSectionsDataRepository
                        .Find(g => mapperData.Contains(g.RFSID))
                        .ToList();
                    foreach (RepeatableSectionsData sectionitem in sectionsData)
                    {
                        sectionitem.Status = true;
                        _unitOfWork.RepeatableSectionsDataRepository.Update(sectionitem);
                    }
                }
            }
            else
            {
                result = false;

            }
            result = true;
            await _unitOfWork.Complete();
            sw.Stop();
            Console.WriteLine("DeleteRepeatableFormsData: " + sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<Forms> addConfiguration(Forms form, StringBuilder sb)
        {
            var result = new Forms();
            try
            {
                var data = ConvertFormstoForm(form, sb, true);
                var sw = Stopwatch.StartNew();
                sw.Start();
                _unitOfWork.FormsRepository.AddRange(new List<Form>() { data });
                await _unitOfWork.Complete();
                sw.Stop();
                sb.AppendLine("form saving in db: " + sw.ElapsedMilliseconds);
                sw = new Stopwatch();
                sw.Start();
                result = getConfiguration(data.FormId, true);
                sw.Stop();
                sb.AppendLine("retrieving Cretaed form: " + sw.ElapsedMilliseconds);
                Console.WriteLine(sb.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public async Task<bool> addParentChild(ParentChild parentChild)
        //{
        //    //var result = new Forms();

        //    try
        //    {
        //        List<ChildConfig> ccObject = new List<ChildConfig>();
        //        if(parentChild.parentChildConfig.childConfigs != null)
        //        {
        //            foreach(ChildConfiguration item in parentChild.parentChildConfig.childConfigs)
        //            {
        //                var formObject =   _unitOfWork.FormsRepository.getFormById(item.childId);
        //               var mapper = MapperConfig.InitializeParentChildMapper();
        //                var result = mapper.Map<ChildConfiguration, ChildConfig>(item);
        //                result.Fid = formObject.Fid;
        //                ccObject.Add(result);
        //            }
        //            _unitOfWork.ChildConfigRepository.AddRange(ccObject);
        //            await _unitOfWork.Complete();
        //        }
        //        //List<BL.Models.ChildConfig> childData = new List<ChildConfig>();
        //        //Console.Write( childConfig);
        //        //_unitOfWork.FormsRepository.Add(new List<ChildConfig>() { childConfig });
        //        //var data = ConvertFormstoForm(form, sb);
        //        //var sw = Stopwatch.StartNew();
        //        //sw.Start();
        //        //_unitOfWork.FormsRepository.AddRange(new List<Form>() { data });
        //        //await _unitOfWork.Complete();
        //        //sw.Stop();
        //        //sb.AppendLine("form saving in db: " + sw.ElapsedMilliseconds);
        //        //sw = new Stopwatch();
        //        //sw.Start();
        //        //result = getConfiguration(data.FormId);
        //        //sw.Stop();
        //        //sb.AppendLine("retrieving Cretaed form: " + sw.ElapsedMilliseconds);
        //        //Console.Write(sb.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return true;
        //}
        public async Task<int> deleteConfiguration(string formId, StringBuilder sb)
        {
            int result = 0;
            try
            {
                var sw = Stopwatch.StartNew();
                //var form = _unitOfWork.FormsRepository.getConfiguration(formId).FirstOrDefault();
                //if (form != null)
                //{
                //    _unitOfWork.FormsRepository.RemoveRange(new List<Form>() { form });
                //    result = await _unitOfWork.Complete();
                //}
                result = await _unitOfWork.FormsRepository.Delete(formId);
                sw.Stop();
                sb.AppendLine("Form deletion : " + sw.ElapsedMilliseconds);
                Console.WriteLine(sb.ToString());
            }
            catch (Exception ex) { throw ex; }

            return result;
        }
        //public async Task<bool> deleteMultipleConfiguration(string[] formId)
        //{
        //    int result = 0;
        //    try
        //    {
        //        var sw = Stopwatch.StartNew();
        //        //var form = _unitOfWork.FormsRepository.getConfiguration(formId).FirstOrDefault();
        //        //if (form != null)
        //        //{
        //        //    _unitOfWork.FormsRepository.RemoveRange(new List<Form>() { form });
        //        //    result = await _unitOfWork.Complete();
        //        //}
        //        result = await _unitOfWork.FormsRepository.Delete(formId);
        //        sw.Stop();
        //        sb.AppendLine("Form deletion : " + sw.ElapsedMilliseconds);
        //        Console.WriteLine(sb.ToString());
        //    }
        //    catch (Exception ex) { throw ex; }

        //    return result;
        //}
        public async Task<Forms> UpdateConfiguration(Forms updatedform, StringBuilder sb)
        {

            var result = new Forms();
            try
            {
                var mapper1 = MapperConfig.InitialiseMapperforUpdateSametype();

                var sw = Stopwatch.StartNew();
                dbform = _unitOfWork.FormsRepository.getConfiguration(updatedform?.id).FirstOrDefault();
                sw.Stop();
                sb.AppendLine("form retrieval in db: " + sw.ElapsedMilliseconds);
                sw.Start();
                var updateddata = ConvertFormstoForm(updatedform, sb);
                formid = dbform.Fid;

                //comparing Condition array and update them into ef
                var srccnd = (updateddata?.Conditions == null ? new List<BL.Models.Condition>() : updateddata?.Conditions?.ToList());
                var destcnd = dbform?.Conditions?.ToList();
                var compcondresult = CompareObjects<BL.Models.Condition>(srccnd, destcnd);
                if (compcondresult.Count() > 0)
                {
                    EFCrud<BL.Models.Condition>(compcondresult, srccnd, destcnd, mapper1);
                }

                ///comparing documents array
                var srcdoc = (updateddata?.Documents == null ? new List<BL.Models.Document>() : updateddata?.Documents?.ToList());
                var destdoc = dbform?.Documents?.ToList();
                var compdocresult = CompareObjects<BL.Models.Document>(srcdoc, destdoc);
                if (compdocresult.Count() > 0)
                {
                    EFCrud<BL.Models.Document>(compdocresult, srcdoc, destdoc, mapper1);
                }

                ///comparing sections
                var srcsec = (updateddata?.Sections == null ? new List<BL.Models.Section>() : updateddata?.Sections?.ToList());
                var destsec = dbform?.Sections?.ToList();
                var compsecresult = CompareObjects<BL.Models.Section>(srcsec, destsec);
                if (compsecresult.Count() > 0)
                {
                    EFCrud<BL.Models.Section>(compsecresult, srcsec, destsec, mapper1);
                }

                ///comparing ParentChild
                List<BL.Models.ParentChild> srcpac = new List<BL.Models.ParentChild>();
                List<BL.Models.ParentChild> destpac = new List<BL.Models.ParentChild>();
                if (updateddata?.ParentChild != null)
                    srcpac.Add(updateddata?.ParentChild);
                if (dbform?.ParentChild != null)
                    destpac.Add(dbform?.ParentChild);
                var compacresult = CompareObjects<BL.Models.ParentChild>(srcpac, destpac);
                if (compacresult.Count() > 0)
                {
                    EFCrud<BL.Models.ParentChild>(compacresult, srcpac, destpac, mapper1);
                }

                ///comparing DesignDataSets
                var srcdds = (updateddata?.DesignDataSets == null ? new List<BL.Models.DesignDataSet>() : updateddata?.DesignDataSets?.ToList());
                var destdds = dbform?.DesignDataSets?.ToList();
                var compddsresult = CompareObjects<BL.Models.DesignDataSet>(srcdds, destdds);
                if (compddsresult.Count() > 0)
                {
                    EFCrud<BL.Models.DesignDataSet>(compddsresult, srcdds, destdds, mapper1);
                }

                ///comparing Lists
                var srclst = (updateddata?.Lists == null ? new List<BL.Models.List>() : updateddata?.Lists?.ToList());
                var destlst = dbform?.Lists?.ToList();

                List<BL.Models.List> updatedsrclst = new List<BL.Models.List>();
                foreach (BL.Models.List item in srclst)
                {
                    if (item.ListItems.Any())
                    {
                        List<ListItem> updatedlstItems = new List<ListItem>();
                        List<ListItem> lstItems = item.ListItems.ToList();
                        for (int j = 0; j < lstItems.Count(); j++)
                        {
                            ListItem lstItem = lstItems[j];
                            lstItem.LSTOrder = j + 1;
                            updatedlstItems.Add(lstItem);
                        }
                        item.ListItems = updatedlstItems;
                    }
                    updatedsrclst.Add(item);
                }
                srclst = updatedsrclst;
                var complstresult = CompareObjects<BL.Models.List>(srclst, destlst);
                if (complstresult.Count() > 0)
                {
                    EFCrud<BL.Models.List>(complstresult, srclst, destlst, mapper1);
                }

                ///comparing Outputs
                var srcops = (updateddata?.Outputs == null ? new List<BL.Models.Output>() : updateddata?.Outputs?.ToList());
                var destops = dbform?.Outputs?.ToList();
                var compopsresult = CompareObjects<BL.Models.Output>(srcops, destops);
                if (compopsresult.Count() > 0)
                {
                    EFCrud<BL.Models.Output>(compopsresult, srcops, destops, mapper1);
                }

                ///comparing TabDetails
                var srctab = (updateddata?.TabDetails == null ? new List<BL.Models.TabDetail>() : updateddata?.TabDetails?.ToList());
                var desttab = dbform?.TabDetails?.ToList();
                var comptabresult = CompareObjects<BL.Models.TabDetail>(srctab, desttab);
                if (comptabresult.Count() > 0)
                {
                    EFCrud<BL.Models.TabDetail>(comptabresult, srctab, desttab, mapper1);
                }

                ///comparing comps
                for (int i = 0; i < updateddata.Pages.Count; i++)
                {

                    var srcpage = updateddata?.Pages.ElementAt(i);
                    var destpage = dbform?.Pages?.FirstOrDefault(fi => fi.Path == srcpage.Path);

                    if ((destpage != null) && (srcpage.Components?.ToList().Count > 0 || destpage.Components?.ToList().Count > 0))
                    {
                        List<BL.Models.Component> srcComponents = new List<BL.Models.Component>();

                        foreach (BL.Models.Component item in srcpage.Components)
                        {
                            if (item.ComponentDatasetSchemaDetails.Any())
                            {
                                List<ComponentDatasetSchemaDetail> UpdatedComponentDatasetSchemaDetailList = new List<ComponentDatasetSchemaDetail>();
                                List<ComponentDatasetSchemaDetail> ComponentDatasetSchemaDetailList = item.ComponentDatasetSchemaDetails.ToList();
                                for (int j = 0; j < ComponentDatasetSchemaDetailList.Count(); j++)
                                {
                                    ComponentDatasetSchemaDetail componentDatasetSchemaDetail = ComponentDatasetSchemaDetailList[j];
                                    componentDatasetSchemaDetail.ColumnOrder = j + 1;
                                    UpdatedComponentDatasetSchemaDetailList.Add(componentDatasetSchemaDetail);
                                }
                                item.ComponentDatasetSchemaDetails = UpdatedComponentDatasetSchemaDetailList;
                            }
                            srcComponents.Add(item);
                        }
                        srcpage.Components = srcComponents;
                        var comppgresult = CompareObjects<BL.Models.Component>(srcpage.Components?.ToList(), destpage.Components?.ToList());
                        if (comppgresult.Count() > 0)
                        {
                            pageid = destpage.Pgid;
                            EFCrud<BL.Models.Component>(comppgresult, srcpage.Components.ToList(), destpage.Components.ToList(), mapper1);
                        }
                    }
                    var srcpglist = (srcpage != null) ? new List<BL.Models.Page> { srcpage } : new List<BL.Models.Page>();
                    var destpglist = (destpage != null) ? new List<BL.Models.Page> { destpage } : new List<BL.Models.Page>();
                    var comppgaresult = CompareObjects<BL.Models.Page>(srcpglist, destpglist);
                    if (comppgaresult.Count() > 0)
                    {
                        EFCrud<BL.Models.Page>(comppgaresult, srcpglist, destpglist, mapper1);
                    }
                }
                await _unitOfWork.Complete();

                ///comparing Calculations
                var srccalc = (updateddata?.Calculations == null ? new List<BL.Models.Calculation>() : updateddata?.Calculations?.ToList());
                var destcalc = dbform?.Calculations?.ToList();
                var compcalcresult = CompareObjects<BL.Models.Calculation>(srccalc, destcalc);
                if (compcalcresult.Count() > 0)
                {
                    EFCrud<BL.Models.Calculation>(compcalcresult, srccalc, destcalc, mapper1);
                }

                //handling exception case where a page is deleted from db
                if (updateddata?.Pages?.Count != dbform?.Pages?.Count)
                {
                    var comparepgaresult = CompareObjects<BL.Models.Page>(updateddata?.Pages.ToList(), dbform?.Pages.ToList());
                    if (comparepgaresult.Count() > 0)
                    {
                        EFCrud<BL.Models.Page>(comparepgaresult, updateddata?.Pages.ToList(), dbform?.Pages.ToList(), mapper1);
                    }
                }

                /////comparing Pages
                //var srcpag = (updateddata?.Pages == null ? new List<BL.Models.Page>() : updateddata?.Pages?.ToList());
                //var destpag = (dbform?.Pages == null) ? new List<BL.Models.Page>() : dbform?.Pages?.ToList();
                //var comppgaresult = CompareObjects<BL.Models.Page>(srcpag, destpag);
                //if (comppgaresult.Count() > 0)
                //{
                //    EFCrud<BL.Models.Page>(comppgaresult, srcpag, destpag, mapper1);
                //}

                dbform.Fs = updateddata.Fs;
                dbform.CreatedByUser = updateddata.CreatedByUser;
                dbform.LastUpdatedByUser = updateddata.LastUpdatedByUser;
                dbform.ConfirmationMsg = updateddata.ConfirmationMsg;
                dbform.Declaration = updateddata.Declaration;
                dbform.Displayname = updateddata.Displayname;
                dbform.Name = updateddata.Name;
                dbform.PhaseBanner = updateddata.PhaseBanner;
                dbform.SignInRequired = updateddata.SignInRequired;
                dbform.SkipSummary = updateddata.SkipSummary;
                dbform.StartPage = updateddata.StartPage;
                dbform.Url = updateddata.Url;
                dbform.Version = updateddata.Version;
                dbform.LastUpdatedOn = updateddata.LastUpdatedOn == null ? DateTime.UtcNow : updateddata.LastUpdatedOn;
                dbform.File = updateddata.File;
                dbform.LastDownloaded = updateddata.LastDownloaded;
                dbform.CustomSummaryMessage = updateddata.CustomSummaryMessage;
                dbform.FeedbackForm = updateddata.FeedbackForm;
                _unitOfWork.FormsRepository.Update(dbform);

                await _unitOfWork.Complete();
                sw.Stop();
                sb.AppendLine("form saving in db: " + sw.ElapsedMilliseconds);

                sw.Start();
                result = getConfiguration(updateddata?.FormId, true);
                sw.Stop();
                sb.AppendLine("retrieving Cretaed form: " + sw.ElapsedMilliseconds);
                Console.WriteLine(sb.ToString());

            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<bool> updateParentChild(ParentChildUpdate data)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            Type proptype = Type.GetType(data.tableName);
            //dynamic repo = dictClassreponame[proptype];

            var existingformData = _unitOfWork.FormsRepository.getFormById(data.formId);
            switch (data.tableName.ToLower())
            {
                case "forms":
                    //var existingData = _unitOfWork.FormsRepository.getFormById(data.formId);
                    if (existingformData == null)
                    {
                        return false;
                    }

                    var formStatuses = _unitOfWork.formStatusRepository.GetAll();
                    var formStatusDictionary = formStatuses.ToDictionary(fs => fs.Status.ToLower(), fs => fs.Fsid);

                    foreach (var fieldChange in data.FieldChanges)
                    {
                        if (fieldChange.Key.ToLower() == "formstatus")
                        {
                            if (!formStatusDictionary.TryGetValue(fieldChange.Value.ToString().ToLower(), out long statusId))
                            {
                                return false;
                            }
                            existingformData.Fsid = statusId;
                        }
                        else
                        {
                            var property = typeof(Form).GetProperty(fieldChange.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                            if (property != null && property.CanWrite)
                            {
                                if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                                {
                                    if (bool.TryParse(fieldChange.Value.ToString(), out bool boolValue))
                                    {
                                        property.SetValue(existingformData, boolValue);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        object convertedValue = Convert.ChangeType(fieldChange.Value.ToString(), property.PropertyType);
                                        property.SetValue(existingformData, convertedValue);
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    existingformData.LastUpdatedOn = DateTime.Now;
                    _unitOfWork.FormsRepository.Update(existingformData);
                    await _unitOfWork.Complete();
                    break;

                //case "documents":
                //    var existingData = _unitOfWork.DocumentsRepository.GetAll().Where(w => w.Fid == existingformData.Fid).FirstOrDefault();
                //    if (existingData == null)
                //    {
                //        return false;
                //    }


                //foreach (var fieldChange in data.fieldChanges)
                //{
                //    var property = typeof(Document).GetProperty(fieldChange.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                //    if (property != null && property.CanWrite)
                //    {
                //        if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                //        {
                //            if (bool.TryParse(fieldChange.Value, out bool boolValue))
                //            {
                //                property.SetValue(existingData, boolValue);
                //            }
                //            else
                //            {
                //                return false;
                //            }
                //        }
                //        else
                //        {
                //            try
                //            {
                //                object convertedValue = Convert.ChangeType(fieldChange.Value, property.PropertyType);
                //                property.SetValue(existingData, convertedValue);
                //            }
                //            catch (Exception)
                //            {
                //                return false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}

                //_unitOfWork.DocumentsRepository.Update(existingformData);
                //await _unitOfWork.Complete();
                //break;

                default:
                    return false;
            }
            sw.Stop();
            Console.WriteLine("updateParentChild: " + sw.ElapsedMilliseconds);
            return true;
        }

        public async Task<bool> uploadProvidersMapping(ProviderMappings mapping, StringBuilder sb)
        {
            var result = false;

            var admincodelist = new List<AdminCodeProvidersData>();
            var ukprnlist = new List<UKPRNProvidersData>();
            var urnlist = new List<URNProvidersData>();

            try
            {
                var sw = Stopwatch.StartNew();
                sw.Start();
                DateTime dateTime;
                if (!string.IsNullOrEmpty(mapping.date))
                {
                    dateTime = DateTime.ParseExact(mapping.date, "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                }
                else
                {
                    dateTime = DateTime.UtcNow;
                }
                List<ProviderMapping> existingdata = await _unitOfWork.FormsRepository.GetLatestProviderMapping(mapping.id, dateTime);

                if (existingdata != null && existingdata.Any())
                {
                    foreach (var item in existingdata)
                    {
                        item.Status = false;
                    }
                    _unitOfWork.ProviderMappingRepository.UpdateRange(existingdata);
                    await _unitOfWork.Complete();
                }

                mapping.providers.AdminCode.ToList().ForEach(admincode =>
                {
                    var data = new AdminCodeProvidersData()
                    {
                        AdminCode = admincode
                    };
                    admincodelist.Add(data);
                });


                mapping.providers.UKPRN.ToList().ForEach(ukprn =>
                {
                    var data = new UKPRNProvidersData()
                    {
                        Ukprn = ukprn
                    };
                    ukprnlist.Add(data);
                });


                mapping.providers.URN.ToList().ForEach(urn =>
                {
                    var data = new URNProvidersData()
                    {
                        Urn = urn
                    };
                    urnlist.Add(data);
                });


                var mapper = MapperConfig.InitializeProviderMappingMapper();
                var data = mapper.Map<ProviderMappings, ProviderMapping>(mapping);
                data.AdminCodeProvidersData = admincodelist;
                data.UKPRNProvidersData = ukprnlist;
                data.URNProvidersData = urnlist;
                data.Status = true;
                data.Desciption = "Form id - " + data.Fid;
                data.UpdatedOn = dateTime;
                _unitOfWork.ProviderMappingRepository.AddRange(new List<ProviderMapping> { data });
                await _unitOfWork.Complete();
                result = true;
                sw.Stop();
                sb.AppendLine("uploadProvidersMapping: " + sw.ElapsedMilliseconds);
                Console.WriteLine(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<bool> CheckProvidersMappingById(string id, int ukprn, int urn, string admincode, StringBuilder sb)
        {
            bool result = false, foundukprn = false, foundurn = false, foundadmincode = false;

            try
            {
                var sw = Stopwatch.StartNew();
                sw.Start();

                // var data = await _unitOfWork.FormsRepository.GetLatestProviderMapping(id);
                result = await _unitOfWork.FormsRepository.GetLatestProviderMappingbyId(id, ukprn, urn, admincode);

                //foundukprn = data.UKPRNProvidersData.Where(w => w.Ukprn == ukprn).Any();
                //foundurn = data.URNProvidersData.Where(w => w.Urn == urn).Any();
                //foundadmincode = data.AdminCodeProvidersData.Where(w => w.AdminCode == admincode).Any();
                //result = (foundukprn ? foundukprn : (foundurn ? foundurn : (foundadmincode ? foundadmincode : false)));
                sw.Stop();
                sb.AppendLine("CheckProvidersMappingById: " + sw.ElapsedMilliseconds);
                Console.WriteLine(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private Form ConvertFormstoForm(Forms Serializedform, StringBuilder sb, bool addconfig = false)
        {
            var result = new Form();
            try
            {
                var sw = Stopwatch.StartNew();
                var mapper = MapperConfig.Mapper;
                var data = mapper.Map<Forms, Form>(Serializedform);

                var createuser = _unitOfWork.userRepository.Find(g => g.UserId == data.CreatedByUser.UserId);
                if (createuser.Any()) { data.CreatedByUser = createuser.FirstOrDefault(); }

                var modifyuser = _unitOfWork.userRepository.Find(g => g.UserId == data.LastUpdatedByUser.UserId);
                data.LastUpdatedByUser = (modifyuser.Any()) ? modifyuser.FirstOrDefault() : null;
                if (data.LastUpdatedByUser == null)
                    data.LastUpdatedByUser = data.CreatedByUser;

                var formstatus = _unitOfWork.formStatusRepository.Find(g => g.Status == data.Fs.Status);
                if (formstatus.Any()) { data.Fs = formstatus.FirstOrDefault(); }

                //documents & imported dataset
                if (Serializedform.documents?.ToList().Count > 0)
                    documentlist = mapper.Map<List<Document>, List<BL.Models.Document>>(Serializedform.documents.ToList());
                if (Serializedform.importedDataSets?.ToList().Count > 0)
                    documentlist.AddRange(mapper.Map<List<Importeddataset>, List<BL.Models.Document>>(Serializedform.importedDataSets.ToList()));
                data.Documents = documentlist.ToArray();


                //tabs to tabs
                if (Serializedform.tabs?.ToList().Count > 0)
                    data.TabDetails = mapper.Map<List<Tab>, List<TabDetail>>(Serializedform.tabs.ToList());

                //designdataset
                if (Serializedform.designedDataSets?.ToList().Count > 0)
                {
                    designdatasetlist = new List<BL.Models.DesignDataSet>();
                    Serializedform.designedDataSets?.ToList().ForEach(dd =>
                    {
                        designdatasubsetlist = new List<DesignDataSetDetail>();
                        datasubsetlist = new List<Datum>();
                        dd.data.ToList().ForEach(data => datasubsetlist.AddRange(data.ToList()));

                        datasubsetlist.ToList().ForEach(dsd => designdatasubsetlist.Add(new DesignDataSetDetail
                        {
                            Bold = dsd.bold,
                            Index = dsd.index,
                            Type = dsd.type,
                            Value = dsd.value,
                            MarkForCalculation = dsd.calc,
                            MarkAsNumber = dsd.numeric,
                            Format = dsd.format,
                            _checked = dsd._checked,
                            Rowno = Convert.ToInt16(dsd.index.Split("-")[0])
                        }));

                        var ddst = mapper.Map<DesignDataSet, DesignDataSet>(new DesignDataSet
                        {

                            CsvUsed = dd.csvUsed,
                            DdsetId = dd.id,
                            KeyIdentifier = dd.keyIdentifier,
                            Title = dd.title,
                            UploadedOn = dd.uploadedDate,
                            DesignDataSetDetails = designdatasubsetlist
                        });



                        ddst.Doc = ((data.Documents?.Count > 0 && data.Documents?.Where(w => w.FileId == ddst.CsvUsed).ToList()?.Count > 0) ?
                                        data.Documents?.Where(w => w.FileId == ddst.CsvUsed)?.FirstOrDefault() : null);

                        designdatasetlist.Add(ddst);
                    });
                    data.DesignDataSets = designdatasetlist.ToArray();
                }

                //outputs
                if (Serializedform.outputs?.ToList().Count > 0)
                {
                    outputlist = new List<BL.Models.Output>();
                    Serializedform.outputs?.ToList().ForEach(op =>
                    {
                        outputsublist = new List<BL.Models.OutputDetail>();
                        foreach (var obj in typeof(Outputconfiguration).GetProperties())
                        {
                            dynamic data = obj.GetValue(op.outputConfiguration);
                            if (op.outputConfiguration != null && data != null)
                                outputsublist.Add(new OutputDetail { PropName = obj.Name, PropValue = (obj.PropertyType.Name == "String[]") ? string.Join(",", ((string[])data).Select(o => "\"" + o + "\"").ToArray()) : Convert.ToString(data), PropType = null });
                        }

                        var opt = mapper.Map<BL.Models.Output, BL.Models.Output>(new BL.Models.Output
                        {
                            Name = op.name,
                            Title = op.title,
                            Type = op.type,
                            OutputDetails = outputsublist
                        });
                        outputlist.Add(opt);
                    });
                    data.Outputs = outputlist.ToArray();
                }



                //conditions
                if (Serializedform.conditions?.ToList().Count > 0)
                {
                    condnlist = new List<BL.Models.Condition>();
                    Serializedform.conditions?.ToList().ForEach(con =>
                    {

                        condnsublist = new List<BL.Models.ConditionDetail>();
                        int subsetno = 1;
                        con.value?.conditions?.ToList().ForEach(subcon =>
                        {
                            if (subcon.conditionType != null)
                                condnsublist.Add(new ConditionDetail { PropName = "conditionType", PropValue = subcon.conditionType, PropType = "conditionType", SubsetNo = subsetno });

                            if (subcon.coordinator != null)
                                condnsublist.Add(new ConditionDetail { PropName = "coordinator", PropValue = subcon.coordinator, PropType = "coordinator", SubsetNo = subsetno });

                            if (subcon.datasetId != null)
                                condnsublist.Add(new ConditionDetail { PropName = "datasetId", PropValue = subcon.datasetId, PropType = "datasetId", SubsetNo = subsetno });

                            foreach (var obj in typeof(Field).GetProperties().OrderBy(d => d.Name))
                            {
                                if (subcon.field != null && obj.GetValue(subcon.field) != null)
                                    condnsublist.Add(new ConditionDetail { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(subcon.field)), PropType = obj.ReflectedType.Name.ToLower(), SubsetNo = subsetno });
                            }

                            if (subcon._operator != null)
                                condnsublist.Add(new ConditionDetail { PropName = "operator", PropValue = subcon._operator, PropType = "operator", SubsetNo = subsetno });

                            foreach (var obj in typeof(Value1).GetProperties().OrderBy(d => d.Name))
                            {
                                if (subcon.value != null && obj.GetValue(subcon.value) != null)
                                    condnsublist.Add(new ConditionDetail { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(subcon.value)), PropType = "value", SubsetNo = subsetno });
                            }
                            subsetno += 1;
                        });


                        var condn = mapper.Map<BL.Models.Condition, BL.Models.Condition>(new BL.Models.Condition
                        {
                            DisplayName = con.displayName,
                            Name = con.name,
                            ConditionDetails = condnsublist
                        });
                        condnlist.Add(condn);
                    });

                    data.Conditions = condnlist;
                }
                //Pages
                if (Serializedform.pages?.ToList().Count > 0)
                {
                    Pagelist = new List<BL.Models.Page>();

                    Serializedform.pages.ToList().ForEach(page =>
                    {
                        Componentlist = new List<BL.Models.Component>();
                        int cmporder = 0;
                        page.components.ToList().ForEach(component =>
                        {
                            cmporder += 1;
                            ComponentAdditionalSettinglist = new List<BL.Models.ComponentAdditionalSetting>();
                            CalculationAdditionalSettinglist = new List<BL.Models.CalculationAdditionalSetting>();
                            ComponentDatasetSchemaDetaillist = new List<BL.Models.ComponentDatasetSchemaDetail>();
                            CalculationComponentDetailist = new List<BL.Models.CalculationComponentDetail>();
                            ComponentDatasetSchemaPropertyDetaillist = new List<BL.Models.ComponentDatasetSchemaPropertyDetail>();
                            DatasetDataDetaillist = new List<BL.Models.DatasetDataDetail>();


                            if (component.type == "List" && component.options != null && string.IsNullOrEmpty(component.options?.format))
                            {
                                component.options.format = "bullets";
                            }

                            var compOptionProps = new string[] { "prefixType", "prefixValue", "suffixType", "suffixValue" };

                            foreach (var obj in typeof(Options).GetProperties())
                            {
                                if (component.options != null && obj.GetValue(component.options) != null)
                                    //if (component.options != null && obj.GetValue(component.options) != null && component.type == "NumberField" && !compOptionProps.Contains(obj.Name))
                                    ComponentAdditionalSettinglist.Add(new ComponentAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(component.options)), PropType = obj.ReflectedType.Name.ToLower() });
                            }

                            foreach (var obj in typeof(Values).GetProperties())
                            {
                                if (component.values != null && obj.GetValue(component.values) != null)
                                    ComponentAdditionalSettinglist.Add(new ComponentAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(component.values)), PropType = obj.ReflectedType.Name.ToLower() });
                            }

                            foreach (var obj in typeof(Schema).GetProperties())
                            {
                                if (component.schema != null && obj.GetValue(component.schema) != null)
                                    ComponentAdditionalSettinglist.Add(new ComponentAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(component.schema)), PropType = obj.ReflectedType.Name.ToLower() });
                            }

                            foreach (var obj in typeof(date).GetProperties())
                            {
                                if (component.date != null && obj.GetValue(component.date) != null)
                                    ComponentAdditionalSettinglist.Add(new ComponentAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(component.date)), PropType = obj.ReflectedType.Name.ToLower() });
                            }

                            if (component.columns != null)
                            {
                                component.columns.ToList().ForEach(col =>
                                {
                                    long cdspdid = 0;
                                    ComponentDatasetSchemaPropertyDetaillist = new List<ComponentDatasetSchemaPropertyDetail>();
                                    List<ComponentDatasetSchemaPropertyDetail> UpdatedComponentDatasetSchemaPropertyDetaillist = new List<ComponentDatasetSchemaPropertyDetail>();

                                    foreach (var obj in typeof(Columnschema).GetProperties())
                                    {
                                        if (col.columnSchema != null && obj.Name.ToLower() != "cdspdid" && obj.GetValue(col.columnSchema) != null)
                                            ComponentDatasetSchemaPropertyDetaillist.Add(new ComponentDatasetSchemaPropertyDetail { Cdsdid = addconfig ? 0 : col.CDSDID, PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(col.columnSchema)), PropType = obj.ReflectedType.Name.ToLower() });

                                        if (obj.Name.ToLower() == "cdspdid")
                                            cdspdid = addconfig ? 0 : Convert.ToInt64(obj.GetValue(col.columnSchema));
                                    }
                                    foreach (ComponentDatasetSchemaPropertyDetail item in ComponentDatasetSchemaPropertyDetaillist)
                                    {
                                        item.Cdspdid = cdspdid;
                                        UpdatedComponentDatasetSchemaPropertyDetaillist.Add(item);
                                    }

                                    var columndata = mapper.Map<Column, ComponentDatasetSchemaDetail>(col);
                                    columndata.ColumnSchema = UpdatedComponentDatasetSchemaPropertyDetaillist.Count > 0;
                                    columndata.ComponentDatasetSchemaPropertyDetails = UpdatedComponentDatasetSchemaPropertyDetaillist;
                                    columndata.Cdsdid = addconfig ? 0 : col.CDSDID;
                                    ComponentDatasetSchemaDetaillist.Add(columndata);
                                });
                            }


                            var ComponentDatasetSchemaDetails = mapper.Map<IEnumerable<ComponentDatasetSchemaDetail>, List<ComponentDatasetSchemaDetail>>(ComponentDatasetSchemaDetaillist);
                            // var tabdata = mapper.Map<IEnumerable<Tabdata>, List<Tabdata>>(tabdatalist);
                            //binding component data


                            var componentdata = mapper.Map<Component, BL.Models.Component>(component);
                            var compprops = new string[] { "name", "type", "hint", "isEditingTabs", "nameHasError", "componentEdited", "_checked", "content", "options", "schema", "values", "tabData", "columns", "date" };

                            foreach (var obj in typeof(Component).GetProperties())
                            {

                                dynamic data1 = obj.GetValue(component);
                                //if (!compprops.Contains(obj.Name) && data1 != null && component.type == "Result" && !compOptionProps.Contains(obj.Name))
                                if (!compprops.Contains(obj.Name) && data1 != null)
                                {
                                    ComponentAdditionalSettinglist.Add(new ComponentAdditionalSetting { PropName = obj.Name, PropValue = (obj.PropertyType.Name == "String[]") ? string.Join(",", ((string[])data1).Select(o => "\"" + o + "\"").ToArray()) : Convert.ToString(data1), PropType = null });
                                }
                                if (obj.Name == "tabData" && component.type == "Tabs")
                                {
                                    var cadstgs = new ComponentAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(true), PropType = null };
                                    ComponentAdditionalSettinglist.Add(cadstgs);
                                    data.TabDetails.ToList().ForEach(tb =>
                                    {
                                        if (tb != null && tb.TabChildDetails.Count == component.totalTabs)
                                        {
                                            tb.TabChildDetails.ToList().ForEach(tbc =>
                                            {
                                                tbc.Cadst = cadstgs;
                                            });
                                        }

                                    });
                                }

                            }

                            //additional settings
                            componentdata.ComponentAdditionalSettings = ComponentAdditionalSettinglist;
                            componentdata.AdditionalSettings = ComponentAdditionalSettinglist.Count > 0;
                            //component schema i.e., dataimport settings
                            componentdata.ComponentDatasetSchemaDetails = ComponentDatasetSchemaDetaillist;

                            //document key settings
                            componentdata.Doc = (component.selectedDocument != null) ?
                             ((data.Documents.Count > 0 && data.Documents.Where(w => w.FileId == component.selectedDocument).ToList().Count > 0) ?
                                     data.Documents.Where(w => w.FileId == component.selectedDocument).FirstOrDefault() : null)
                                     : null;

                            //list key settings
                            componentdata.Lst = (componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "list").ToList().Count > 0) ?
                                                     ((data.Lists.Count > 0 && data.Lists.Where(w => w.Lstname == componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "list").FirstOrDefault().PropValue).ToList().Count > 0) ?
                                                     data.Lists.Where(w => w.Lstname == componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "list").FirstOrDefault().PropValue).FirstOrDefault() : null)
                                                 : null;

                            //designdataset key settings

                            componentdata.Dds = (componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "dataset").ToList().Count > 0) ?
                                                ((data.DesignDataSets.Count > 0 && data.DesignDataSets.Where(w => w.DdsetId == componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "dataset").FirstOrDefault().PropValue).ToList().Count > 0) ?
                                                     data.DesignDataSets.Where(w => w.DdsetId == componentdata.ComponentAdditionalSettings.Where(w => w.PropName == "dataset").FirstOrDefault().PropValue).FirstOrDefault() : null)
                                                 : !(string.IsNullOrEmpty(componentdata.Content)) ?
                                                 ((data.DesignDataSets.Count > 0 && data.DesignDataSets.Where(w => w.DdsetId == componentdata.Content).ToList().Count > 0) ?
                                                     data.DesignDataSets.Where(w => w.DdsetId == componentdata.Content).FirstOrDefault() : null) : null;
                            componentdata.Status = true;

                            if (componentdata.Type == "Result")
                            {
                                var expressiondata = componentdata.ComponentAdditionalSettings.Where(cads => cads.PropName == "expression")?.FirstOrDefault()?.PropValue;
                                if (expressiondata != null)
                                {
                                    List<string> tmpcomparray = new List<string>();
                                    tmpcomparray.AddRange(expressiondata.Split("("));
                                    if (!calccomparray.Where(x => x.Key == componentdata.Name).Any())
                                        calccomparray.Add(componentdata.Name, tmpcomparray);

                                }
                            }
                            componentdata.CmpOrder = cmporder;
                            Componentlist.Add(componentdata);
                            masterComponentlist.Add(componentdata);
                        });
                        var pagedata = mapper.Map<Page, BL.Models.Page>(page);
                        pagedata.Components = Componentlist.ToArray();
                        pagedata.Next = pagedata?.PageChildSettings?.Count > 0 ? "1" : "0";
                        pagedata.Controller = page.controller;
                        Pagelist.Add(pagedata);
                    });
                }
                var parentChildlist = new List<BL.Models.ParentChild>();
                //ParentChild
                if (data.ParentChild?.ParentChildConfig != null)
                {
                    data.ParentChild.IsParentChildConfig = true;
                    if (data.ParentChild.ParentChildConfig.ChildConfigs != null && data.ParentChild.ParentChildConfig.ChildConfigs.Any())
                    {
                        data.ParentChild.ParentChildConfig.IsChildConfigs = true;
                        data.ParentChild?.ParentChildConfig?.ChildConfigs.ToList().ForEach(pchildconfig =>
                        {
                            if (pchildconfig.DependentForms != null && pchildconfig.DependentForms.Any())
                            {
                                pchildconfig.IsDependentForms = true;

                                pchildconfig.DependentForms.ToList().ForEach(pchildDependen =>
                                {
                                    pchildDependen.MainParentId = pchildconfig.ParentId;
                                });
                            }
                        });
                    }
                }

                //data.ParentChild = parentChildlist.ToArray();

                var calcomplist = new List<CalculationComponentDetail>();
                var calclist = new List<BL.Models.Calculation>();
                //tie the calculation and components
                if (calccomparray.Count > 0)
                {
                    calccomparray.ToList().ForEach(calcomp =>
                    {
                        var calculation = data.Calculations.Where(cal => cal.Name == calcomp.Key).FirstOrDefault();
                        if (calculation != null)
                        {
                            var componentnamearray = calcomp.Value.ToList();
                            calcomplist = new List<CalculationComponentDetail>();
                            componentnamearray.ForEach(cmp =>
                            {
                                if (!string.IsNullOrEmpty(cmp) && cmp.IndexOf(')') > 0)
                                {
                                    string cmpname = cmp;
                                    if (cmp.Split('~').Length > 1)
                                    {
                                        cmpname = cmp.Substring(0, cmp.IndexOf(')')).Split('~')[0];
                                    }
                                    else
                                    {
                                        cmpname = cmp.Substring(0, cmp.IndexOf(')'));
                                    }

                                    var cmpdata = masterComponentlist.Where(c => c.Name == cmpname).FirstOrDefault();
                                    if (cmpdata != null)
                                    {
                                        var calccompdata = mapper.Map<CalculationComponentDetail, CalculationComponentDetail>(new CalculationComponentDetail
                                        {
                                            Calc = calculation,
                                            Cmp = cmpdata
                                        });
                                        calcomplist.Add(calccompdata);
                                    }
                                }
                            });
                            calculation.CalculationComponentDetails = calcomplist.Count > 0 ? calcomplist : null;
                            Datum[] datum = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.datasets;
                            ComputeList[] computeList = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.computeList;
                            string[] calculationsMapped = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.calculationsMapped;
                            int i = 1, j = 1;
                            CalculationAdditionalSettinglist = new List<CalculationAdditionalSetting>();
                            if (datum != null)
                            {
                                foreach (var item in datum)
                                {
                                    foreach (var obj in typeof(Datum).GetProperties())
                                    {
                                        if (item != null && obj.GetValue(item) != null)
                                            //if (component.options != null && obj.GetValue(component.options) != null && component.type == "NumberField" && !compOptionProps.Contains(obj.Name))
                                            CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(item)), PropType = obj.ReflectedType.Name.ToLower(), RowNumber = i });
                                    }
                                    i++;
                                }
                            }
                            if (computeList != null)
                            {
                                foreach (var item in computeList)
                                {
                                    foreach (var obj in typeof(ComputeList).GetProperties())
                                    {
                                        if (item != null && obj.GetValue(item) != null)
                                            CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(item)), PropType = obj.ReflectedType.Name.ToLower(), RowNumber = j });
                                    }
                                    j++;
                                }
                            }
                            if (calculationsMapped != null && calculationsMapped.Length > 0)
                            {
                                CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting
                                {
                                    PropName = "calculationsmapped",
                                    PropValue = string.Join(",", calculationsMapped),
                                    PropType = "calculationsmapped",
                                });
                            }
                            calculation.CalculationAdditionalSettings = CalculationAdditionalSettinglist.Count > 0 ? CalculationAdditionalSettinglist : null;
                            calclist.Add(calculation);
                        }
                    });
                }

                data.Calculations?.ToList().ForEach(calc =>
                {
                    List<string> existingCalc = calclist.Select(s => s.Name).ToList();
                    var calculation = data.Calculations.Where(cal => !existingCalc.Contains(cal.Name)).FirstOrDefault();
                    if (calculation != null)
                    {
                        var serializedCalc = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault();
                        var componentnamearray = serializedCalc.components.Select(s => s.name).ToList();
                        calcomplist = new List<CalculationComponentDetail>();
                        componentnamearray.ForEach(cmp =>
                        {
                            var cmpdata = masterComponentlist.Where(c => c.Name == cmp).FirstOrDefault();
                            if (cmpdata != null)
                            {
                                var calccompdata = mapper.Map<CalculationComponentDetail, CalculationComponentDetail>(new CalculationComponentDetail
                                {
                                    Calc = calculation,
                                    Cmp = cmpdata
                                });
                                calcomplist.Add(calccompdata);
                            }
                        });
                        calculation.CalculationComponentDetails = calcomplist.Count > 0 ? calcomplist : null;
                        var cmpdata = masterComponentlist;
                        Datum[] datum = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.datasets;
                        ComputeList[] computeList = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.computeList;
                        string[] calculationsMapped = Serializedform.calculations.Where(w => w.name == calculation.Name).FirstOrDefault()?.calculationsMapped;
                        int i = 1, j = 1;
                        CalculationAdditionalSettinglist = new List<CalculationAdditionalSetting>();
                        if (datum != null)
                        {
                            foreach (var item in datum)
                            {
                                foreach (var obj in typeof(Datum).GetProperties())
                                {
                                    if (item != null && obj.GetValue(item) != null)
                                        //if (component.options != null && obj.GetValue(component.options) != null && component.type == "NumberField" && !compOptionProps.Contains(obj.Name))
                                        CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(item)), PropType = obj.ReflectedType.Name.ToLower(), RowNumber = i });
                                }
                                i++;
                            }
                        }
                        if (computeList != null)
                        {
                            foreach (var item in computeList)
                            {
                                foreach (var obj in typeof(ComputeList).GetProperties())
                                {
                                    if (item != null && obj.GetValue(item) != null)
                                        CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting { PropName = obj.Name, PropValue = Convert.ToString(obj.GetValue(item)), PropType = obj.ReflectedType.Name.ToLower(), RowNumber = j });
                                }
                                j++;
                            }
                        }
                        if (calculationsMapped != null && calculationsMapped.Length > 0)
                        {
                            CalculationAdditionalSettinglist.Add(new CalculationAdditionalSetting
                            {
                                PropName = "calculationsmapped",
                                PropValue = string.Join(",", calculationsMapped),
                                PropType = "calculationsmapped",
                            });
                        }
                        calculation.CalculationAdditionalSettings = CalculationAdditionalSettinglist.Count > 0 ? CalculationAdditionalSettinglist : null;
                        calclist.Add(calculation);
                    }
                });

                data.Calculations = calclist.Count > 0 ? calclist : null;
                data.Pages = Pagelist.ToArray();
                data.Status = true;
                result = data;
                sw.Stop();
                sb.AppendLine("ConvertFormstoForm: " + sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private IEnumerable<Difference> CompareObjects<T>(IList<T> a, IList<T> b) where T : class
        {
            IEnumerable<Difference> differences;

            var comparer = (ObjectsComparer.IComparer<IList<T>>)new CustomComparer<T>(settings, parentComparer, this);
            comparer.AddComparerOverride(typeof(long), DoNotCompareValueComparer.Instance, mem => mem.Name.ToLower().Contains("id"));
            comparer.AddComparerOverride("Cn", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("FidNavigation", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Cmp", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Calc", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Dds", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Lst", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Doc", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Pg", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Cdsd", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Sub", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("UidNavigation", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Ou", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Rsp", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Tab", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Cadst", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Fs", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("DesignDataSets", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Components", DoNotCompareValueComparer.Instance);
            //comparer.AddComparerOverride("CalculationComponentDetails", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("ResponseSubmisstionData", DoNotCompareValueComparer.Instance);
            //comparer.AddComparerOverride("TabChildDetails", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("DatasetDataDetails", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Calculations", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Documents", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Fees", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Lists", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("MetaData", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Outputs", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Pages", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("ProviderMappings", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Responses", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Sections", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("TabDetails", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("Forms", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("OrganisationDetails", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("DFChildConfigs", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("CCParentChildConfig", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("PCCParentChild", DoNotCompareValueComparer.Instance);
            comparer.AddComparerOverride("UserOrganisationDetails", DoNotCompareValueComparer.Instance);

            var isEqual = comparer.Compare(a, b, out differences);
            Console.WriteLine(isEqual ? $"{typeof(T).Name} Objects are equal" : string.Join(Environment.NewLine, differences));
            return differences;
        }

        private IEnumerable<T> EFCrud<T>(IEnumerable<Difference> compcondresult, IList<T> srcdata, IList<T> destdata, Mapper mapper1) where T : class
        {

            var resultlist = new List<T>();
            string distinctpropname = null, keyrltns = null, subcolls = null, classname = null;
            dynamic repo = null, parentkeyvalue = null;
            Type proptype = null, subproptype = null;
            PropertyInfo srcprop = null, destprop = null, parentprop = null;

            proptype = typeof(T);
            dictClassPropname.TryGetValue(proptype, out distinctpropname);
            repo = dictClassreponame[proptype];
            dictClassRltnname.TryGetValue(proptype, out keyrltns);
            dictClassSubColl.TryGetValue(proptype.ToString(), out subcolls);

            var updatelist = new List<T>();
            var addlist = new List<T>();
            var removelist = new List<T>();

            dynamic latestdata = null, datatoupdate = null, src = null, dest = null;

            var prop = proptype.GetProperty(keyrltns);
            if (destdata.Count > 0)
                parentkeyvalue = destdata.Select(fi => prop.GetValue(fi)).FirstOrDefault();
            else if (keyrltns != null && keyrltns == "Fid")
                parentkeyvalue = formid;
            else if (keyrltns != null && keyrltns == "Pgid")
                parentkeyvalue = pageid;

            var obj = new GetComparer<T>();
            var comparer = obj.GetComparerobj<T>();

            destprop = proptype.GetProperty(distinctpropname);

            foreach (var condresult in compcondresult)
            {

                if (condresult.DifferenceType == DifferenceTypes.NumberOfElementsMismatch)
                {
                    dictClassPropname.TryGetValue(proptype, out distinctpropname);
                    if (srcdata.Count == destdata.Count)
                    {
                        for (int i = 0; i < srcdata.Count; i++)
                        {
                            destprop = proptype.GetProperty(distinctpropname);
                            var tempdest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(srcdata[i])));
                            if (tempdest != null)
                            {
                                var result = comparer.Compare(srcdata[i], tempdest);
                                if (!result)
                                {
                                    tempdest = Mapobjects<T>(proptype, srcdata[i], tempdest, mapper1, dictClassPropname, keyrltns, condresult);
                                    updatelist.Add(tempdest);
                                    //update data                       
                                    //repo.Update(tempdest);
                                }
                            }
                            else
                            {

                                var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                latestdata = mapperconfig.Map<T, T>(srcdata[i]);
                                //add data
                                latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);

                                bool matchFound = false;
                                foreach (var item in addlist)
                                {
                                    if (comparer.Compare(item, latestdata))
                                    {
                                        matchFound = true;
                                        break;
                                    }
                                }
                                if (!matchFound)
                                {
                                    addlist.Add(latestdata);
                                }
                                //repo.Add(latestdata);
                            }

                        }

                    }
                    else
                    {
                        int i = srcdata.Count;
                        int j = destdata.Count;
                        int ij = 0;

                        if (i < j)
                        {
                            for (int k = 0; k < i; k++)
                            {
                                destprop = proptype.GetProperty(distinctpropname);
                                var tempdest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(srcdata[k])));
                                ij = k + 1;
                                if (tempdest != null)
                                {
                                    var result = comparer.Compare(srcdata[k], tempdest);
                                    if (!result)
                                    {
                                        tempdest = Mapobjects<T>(proptype, srcdata[k], tempdest, mapper1, dictClassPropname, keyrltns, condresult);
                                        updatelist.Add(tempdest);
                                        //update data                       
                                        //repo.Update(tempdest);
                                    }
                                }
                                else
                                {
                                    var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                    latestdata = mapperconfig.Map<T, T>(srcdata[k]);
                                    //add data
                                    latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                                    //repo.Add(latestdata);
                                    bool matchFound = false;
                                    foreach (var item in addlist)
                                    {
                                        if (comparer.Compare(item, latestdata))
                                        {
                                            matchFound = true;
                                            break;
                                        }
                                    }
                                    if (!matchFound)
                                    {
                                        addlist.Add(latestdata);
                                    }
                                }
                            }

                            for (int k = 0; k < j; k++)
                            {
                                destprop = proptype.GetProperty(distinctpropname);

                                var destItem = destdata[k];

                                var existsInSrc = srcdata.Any(s =>
                                    destprop.GetValue(s).Equals(destprop.GetValue(destItem))
                                );

                                if (!existsInSrc)
                                    removelist.Add(destItem);
                            }
                        }
                        else if (i > j)
                        {
                            for (int k = 0; k < j; k++)
                            {
                                destprop = proptype.GetProperty(distinctpropname);
                                var tempdest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(srcdata[k])));
                                ij = k + 1;
                                if (tempdest != null)
                                {
                                    var result = comparer.Compare(srcdata[k], tempdest);
                                    if (!result)
                                    {
                                        tempdest = Mapobjects<T>(proptype, srcdata[k], tempdest, mapper1, dictClassPropname, keyrltns, condresult);
                                        //update data                       
                                        //repo.Update(tempdest);
                                        updatelist.Add(tempdest);
                                    }
                                }
                                else
                                {
                                    var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                    latestdata = mapperconfig.Map<T, T>(srcdata[k]);
                                    //add data
                                    latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                                    //repo.Add(latestdata);
                                    bool matchFound = false;
                                    foreach (var item in addlist)
                                    {
                                        if (comparer.Compare(item, latestdata))
                                        {
                                            matchFound = true;
                                            break;
                                        }
                                    }
                                    if (!matchFound)
                                    {
                                        addlist.Add(latestdata);
                                    }
                                }
                            }

                            for (int k = ij; k < i; k++)
                            {
                                var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                latestdata = mapperconfig.Map<T, T>(srcdata[k]);
                                latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                                //add data
                                //repo.Add(latestdata);

                                bool matchFound = false;
                                foreach (var item in addlist)
                                {
                                    if (comparer.Compare(item, latestdata))
                                    {
                                        matchFound = true;
                                        break;
                                    }
                                }
                                if (!matchFound)
                                {
                                    addlist.Add(latestdata);
                                }
                            }
                        }
                    }
                }
                else if (condresult.DifferenceType == DifferenceTypes.MissedElementInFirstObject)
                {
                    dictClassPropname.TryGetValue(proptype, out distinctpropname);
                    destprop = proptype.GetProperty(distinctpropname);
                    latestdata = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(condresult.Value1));
                    if (proptype == typeof(BL.Models.Page))
                    {
                        latestdata.Next = latestdata?.PageChildSettings?.Count > 0 ? "1" : "0";
                    }
                    //delete the data
                    //repo.Remove(latestdata);
                    removelist.Add(latestdata);
                }
                else if (condresult.DifferenceType == DifferenceTypes.MissedElementInSecondObject)
                {
                    dictClassPropname.TryGetValue(proptype, out distinctpropname);
                    srcprop = proptype.GetProperty(distinctpropname);
                    src = srcdata.FirstOrDefault(fi => srcprop.GetValue(fi).Equals(condresult.Value1));
                    var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                    latestdata = mapperconfig.Map<T, T>(src);
                    latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                    if (proptype == typeof(BL.Models.Page))
                    {
                        latestdata.Next = latestdata?.PageChildSettings?.Count > 0 ? "1" : "0";
                    }
                    //Add new data
                    //repo.Add(latestdata);
                    bool matchFound = false;

                    foreach (var item in addlist)
                    {
                        if (comparer.Compare(item, latestdata))
                        {
                            matchFound = true;
                            break;
                        }
                    }
                    if (!matchFound)
                    {
                        addlist.Add(latestdata);
                    }
                }
                else if ((!condresult.MemberPath.Contains("IsReadOnly")) && (condresult.DifferenceType == DifferenceTypes.ValueMismatch))
                {
                    dictClassPropname.TryGetValue(proptype, out distinctpropname);

                    if (condresult.MemberPath.Split('.').Count() > 0)
                    {
                        var ChangesIdentifiedInClass = Regex.Replace(condresult.MemberPath, "[\\[\\=\\d{0,}\\]]", "");
                        classname = (condresult.MemberPath.Substring(condresult.MemberPath.LastIndexOf("].") + 2).Split('.').Length > 1) ? ChangesIdentifiedInClass.Split('.')[ChangesIdentifiedInClass.Split('.').Length - 3] : ChangesIdentifiedInClass.Split('.')[ChangesIdentifiedInClass.Split('.').Length - 2];

                        if (!proptype.Name.Contains(classname))
                        {
                            parentprop = proptype.GetProperty(classname);

                            dictStringClassname.TryGetValue(classname, out subproptype);
                            dictClassPropname.TryGetValue(subproptype, out distinctpropname);

                            srcprop = subproptype.GetProperty(condresult.MemberPath.Substring(condresult.MemberPath.LastIndexOf('.') + 1));
                            if (srcprop == null) srcprop = subproptype.GetProperty(distinctpropname);

                            foreach (T srcobj in srcdata)
                            {
                                dest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(srcobj)));
                                if (dest != null)
                                {
                                    var result = comparer.Compare(srcobj, dest);
                                    if (!result)
                                    {
                                        dest = Mapobjects<T>(proptype, srcobj, dest, mapper1, dictClassPropname, keyrltns, condresult);
                                        latestdata = UpdateFKRelations<T>(proptype, srcobj);
                                        latestdata = mapper1.Map<T, T>(latestdata, dest);
                                        updatelist.Add(latestdata);
                                    }
                                }
                            }
                        }
                        else
                        {
                            srcprop = proptype.GetProperty(condresult.MemberPath.Substring(condresult.MemberPath.LastIndexOf('.') + 1));
                            if (srcprop == null) srcprop = proptype.GetProperty(distinctpropname);
                            dictClassPropname.TryGetValue(proptype, out distinctpropname);
                            destprop = proptype.GetProperty(distinctpropname);


                            var tempSrcData = srcdata.Where(fi => (srcprop.GetValue(fi) != null && srcprop.GetValue(fi).Equals((srcprop.PropertyType == typeof(bool?)) ? (string.IsNullOrWhiteSpace(condresult.Value1) ? (bool?)null : Convert.ToBoolean(condresult.Value1)) : (condresult.Value1))));
                            if (tempSrcData.Count() == 1)
                            {
                                src = srcdata.FirstOrDefault(fi => (srcprop.GetValue(fi) != null && srcprop.GetValue(fi).Equals((srcprop.PropertyType == typeof(bool?)) ? (string.IsNullOrWhiteSpace(condresult.Value1) ? (bool?)null : Convert.ToBoolean(condresult.Value1)) : (condresult.Value1))));

                                if (src != null)
                                {
                                    dest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(src)));

                                    dest = Mapobjects<T>(proptype, src, dest, mapper1, dictClassPropname, keyrltns, condresult);
                                    src = UpdateFKRelations<T>(proptype, src);
                                    latestdata = mapper1.Map<T, T>(src, dest);
                                    //update data
                                    //repo.Update(latestdata);
                                    updatelist.Add(latestdata);
                                }
                            }
                            else
                            {
                                foreach (T srcobj in srcdata)
                                {
                                    dest = destdata.FirstOrDefault(fi => destprop.GetValue(fi).Equals(destprop.GetValue(srcobj)));
                                    if (dest != null)
                                    {
                                        var result = comparer.Compare(srcobj, dest);
                                        if (!result)
                                        {
                                            dest = Mapobjects<T>(proptype, srcobj, dest, mapper1, dictClassPropname, keyrltns, condresult);
                                            latestdata = UpdateFKRelations<T>(proptype, srcobj);
                                            latestdata = mapper1.Map<T, T>(latestdata, dest);
                                            updatelist.Add(latestdata);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            repo.UpdateRange(updatelist);
            repo.AddRange(addlist);
            repo.RemoveRange(removelist);
            if (proptype == typeof(BL.Models.Page))
                _unitOfWork.Complete().Wait();
            resultlist.AddRange(updatelist);
            resultlist.AddRange(addlist);
            resultlist.AddRange(removelist);
            return resultlist;
        }

        private T Mapobjects<T>(Type proptype, dynamic src, dynamic dest, Mapper mapper1, Dictionary<Type, string> dictClassPropname, string keyrltns, Difference condresult) where T : class
        {
            var destchcon = new List<ConditionDetail>();
            var destchdds = new List<DesignDataSetDetail>();
            var destchlst = new List<ListItem>();
            var destchops = new List<OutputDetail>();
            var destchtab = new List<TabChildDetail>();
            var destchpg = new List<PageChildSetting>();
            var destcmps = new List<BL.Models.Component>();
            var destcals = new List<BL.Models.Calculation>();
            var destcmpads = new List<BL.Models.ComponentAdditionalSetting>();
            var destcalads = new List<BL.Models.CalculationAdditionalSetting>();
            var destcalcmd = new List<BL.Models.CalculationComponentDetail>();
            var destcmpsch = new List<BL.Models.ComponentDatasetSchemaDetail>();
            var destcmpschprop = new List<BL.Models.ComponentDatasetSchemaPropertyDetail>();
            var destpchild = new BL.Models.ParentChild();
            var destpchildconf = new List<BL.Models.ParentChildConfig>();
            var destcconf = new List<BL.Models.ChildConfig>();
            var destdepend = new List<BL.Models.DependentForm>();


            dynamic obj = null, comparer = null, obj1 = null, comparer1 = null;

            //condition
            if (proptype == typeof(BL.Models.Condition))
            {
                obj = new GetComparer<ConditionDetail>();
                comparer = obj.GetComparerobj<ConditionDetail>();
                destchcon = Mapsubobjects<ConditionDetail>(typeof(ConditionDetail), src.ConditionDetails, dest.ConditionDetails, mapper1, comparer, dest.Cnid, condresult);
                // dest.ConditionDetails = destchcon.ToArray();               
            }
            /// the below said doesnt have child objects so they are commented out.
            ////documents
            //if (proptype == typeof(BL.Models.Document))
            //{
            //    repo = _unitOfWork.DocumentsRepository;
            //}

            ////calculations
            //if (proptype == typeof(BL.Models.Calculation))
            //{
            //    repo = _unitOfWork.CalculationsRepository;
            //}

            ////sections
            //if (proptype == typeof(BL.Models.Section))
            //{
            //    repo = _unitOfWork.SectionsRepository;
            //}

            //designdataset
            if (proptype == typeof(BL.Models.DesignDataSet))
            {
                obj = new GetComparer<DesignDataSetDetail>();
                comparer = obj.GetComparerobj<DesignDataSetDetail>();
                destchdds = Mapsubobjects<DesignDataSetDetail>(typeof(DesignDataSetDetail), src.DesignDataSetDetails, dest.DesignDataSetDetails, mapper1, comparer, dest.Ddsid, condresult);
                //dest.DesignDataSetDetails = destchdds.ToArray();
            }


            //List
            if (proptype == typeof(BL.Models.List))
            {
                obj = new GetComparer<ListItem>();
                comparer = obj.GetComparerobj<ListItem>();
                destchlst = Mapsubobjects<ListItem>(typeof(ListItem), src.ListItems, dest.ListItems, mapper1, comparer, dest.Lstid, condresult);
                //dest.ListItems = destchlst.ToArray();
            }

            //output
            if (proptype == typeof(BL.Models.Output))
            {
                obj = new GetComparer<OutputDetail>();
                comparer = obj.GetComparerobj<OutputDetail>();
                destchops = Mapsubobjects<OutputDetail>(typeof(OutputDetail), src.OutputDetails, dest.OutputDetails, mapper1, comparer, dest.Ouid, condresult);
                //dest.OutputDetails = destchops.ToArray();
            }

            //tabs
            if (proptype == typeof(BL.Models.TabDetail))
            {
                obj = new GetComparer<TabChildDetail>();
                comparer = obj.GetComparerobj<TabChildDetail>();
                destchtab = Mapsubobjects<TabChildDetail>(typeof(TabChildDetail), src.TabChildDetails, dest.TabChildDetails, mapper1, comparer, dest.Tabid, condresult);
                //dest.TabChildDetails = destchtab.ToArray();
            }

            //pages
            if (proptype == typeof(BL.Models.Page))
            {
                obj = new GetComparer<PageChildSetting>();
                comparer = obj.GetComparerobj<PageChildSetting>();
                destchpg = Mapsubobjects<PageChildSetting>(typeof(PageChildSetting), src.PageChildSettings, dest.PageChildSettings, mapper1, comparer, dest.Pgid, condresult);
                //dest.PageChildSettings = destchtab.ToArray();
            }
            //calculation
            if (proptype == typeof(BL.Models.Calculation))
            {
                //Calc
                destcals = new List<BL.Models.Calculation>();

                //Calc additional settings.
                if ((src.CalculationAdditionalSettings != null && src.CalculationAdditionalSettings.Count > 0) || (dest.CalculationAdditionalSettings != null && dest.CalculationAdditionalSettings.Count > 0))
                {
                    obj = new GetComparer<CalculationAdditionalSetting>();
                    comparer = obj.GetComparerobj<CalculationAdditionalSetting>();
                    destcalads = Mapsubobjects<CalculationAdditionalSetting>(typeof(CalculationAdditionalSetting), src.CalculationAdditionalSettings, dest.CalculationAdditionalSettings, mapper1, comparer, dest.Calcid, condresult);
                    //dest.ComponentAdditionalSettings = destcmpads.ToArray();
                }
                //Calc additional settings.
                if ((src.CalculationComponentDetails != null && src.CalculationComponentDetails.Count > 0) || (dest.CalculationComponentDetails != null && dest.CalculationComponentDetails.Count > 0))
                {
                    obj = new GetComparer<CalculationComponentDetail>();
                    comparer = obj.GetComparerobj<CalculationComponentDetail>();
                    destcalcmd = Mapsubobjects<CalculationComponentDetail>(typeof(CalculationComponentDetail), src.CalculationComponentDetails, dest.CalculationComponentDetails, mapper1, comparer, dest.Calcid, condresult);
                    //dest.ComponentAdditionalSettings = destcmpads.ToArray();
                }
            }

            //Comp
            if (proptype == typeof(BL.Models.Component))
            {
                //comp
                destcmps = new List<BL.Models.Component>();

                //comp additional settings.
                if ((src.ComponentAdditionalSettings != null && src.ComponentAdditionalSettings.Count > 0) || (dest.ComponentAdditionalSettings != null && dest.ComponentAdditionalSettings.Count > 0))
                {
                    obj = new GetComparer<ComponentAdditionalSetting>();
                    comparer = obj.GetComparerobj<ComponentAdditionalSetting>();
                    destcmpads = Mapsubobjects<ComponentAdditionalSetting>(typeof(ComponentAdditionalSetting), src.ComponentAdditionalSettings, dest.ComponentAdditionalSettings, mapper1, comparer, dest.Cmpid, condresult);
                    //dest.ComponentAdditionalSettings = destcmpads.ToArray();
                }

                //comp schema.
                if ((src.ComponentDatasetSchemaDetails != null && src.ComponentDatasetSchemaDetails.Count > 0) || (dest.ComponentDatasetSchemaDetails != null && dest.ComponentDatasetSchemaDetails.Count > 0))
                {
                    obj = new GetComparer<ComponentDatasetSchemaDetail>();
                    comparer = obj.GetComparerobj<ComponentDatasetSchemaDetail>();

                    obj1 = new GetComparer<ComponentDatasetSchemaPropertyDetail>();
                    comparer1 = obj1.GetComparerobj<ComponentDatasetSchemaPropertyDetail>();
                    foreach (ComponentDatasetSchemaDetail item in src.ComponentDatasetSchemaDetails)
                    {
                        long id = item.Cdsdid;
                        List<ComponentDatasetSchemaDetail> destComponentDatasetSchemaDetails = dest.ComponentDatasetSchemaDetails;
                        ComponentDatasetSchemaDetail destComponentDatasetSchemaDetail = destComponentDatasetSchemaDetails.Where(w => w.Cdsdid == item.Cdsdid).FirstOrDefault();
                        if (destComponentDatasetSchemaDetail != null)
                        {
                            IList<ComponentDatasetSchemaPropertyDetail> list = destComponentDatasetSchemaDetail.ComponentDatasetSchemaPropertyDetails.ToList();
                            if (item.ComponentDatasetSchemaPropertyDetails.Count > 0)
                            {
                                destcmpschprop = Mapsubobjects<ComponentDatasetSchemaPropertyDetail>(typeof(ComponentDatasetSchemaPropertyDetail), item.ComponentDatasetSchemaPropertyDetails.ToList(), list, mapper1, comparer1, item.Cdsdid, condresult);
                            }
                        }
                        destcmpsch = Mapsubobjects<ComponentDatasetSchemaDetail>(typeof(ComponentDatasetSchemaDetail), src.ComponentDatasetSchemaDetails, dest.ComponentDatasetSchemaDetails, mapper1, comparer, dest.Cmpid, condresult);
                    }

                    //for (int cds = 0; cds < src.ComponentDatasetSchemaDetails.Count; cds++)
                    //{
                    //    //comp schema prop settings.
                    //    if (src.ComponentDatasetSchemaDetails[cds].ComponentDatasetSchemaPropertyDetails.Count > 0)
                    //    {
                    //        destcmpschprop1 = Mapsubobjects<ComponentDatasetSchemaPropertyDetail>(typeof(ComponentDatasetSchemaPropertyDetail), src.ComponentDatasetSchemaDetails[cds].ComponentDatasetSchemaPropertyDetails, dest.ComponentDatasetSchemaDetails[cds].ComponentDatasetSchemaPropertyDetails, mapper1, comparer1, dest.ComponentDatasetSchemaDetails[cds].Cdsdid, condresult);
                    //        //dest.ComponentDatasetSchemaDetails[cds].ComponentDatasetSchemaPropertyDetails = destcmpschprop.ToArray();
                    //    }

                    //    destcmpsch1 = Mapsubobjects<ComponentDatasetSchemaDetail>(typeof(ComponentDatasetSchemaDetail), src.ComponentDatasetSchemaDetails, dest.ComponentDatasetSchemaDetails, mapper1, comparer, dest.Cmpid, condresult);
                    //    //  dest.ComponentDatasetSchemaDetails = destcmpsch.ToArray();
                    //}
                }
                //    destcmps.Add(mapper1.Map<BL.Models.Component, BL.Models.Component>(src, dest));                
                //dest.Components = destcmps.ToArray();
            }

            //ParentChild
            if (proptype == typeof(BL.Models.ParentChild))
            {
                //comp
                destpchild = new BL.Models.ParentChild();

                //comp additional settings.
                if (src.ParentChildConfig != null)
                {
                    obj = new GetComparer<BL.Models.ParentChildConfig>();
                    comparer = obj.GetComparerobj<BL.Models.ParentChildConfig>();
                    destpchildconf = Mapsubobjects<BL.Models.ParentChildConfig>(typeof(BL.Models.ParentChildConfig), new List<BL.Models.ParentChildConfig>() { src.ParentChildConfig }, new List<BL.Models.ParentChildConfig>() { dest.ParentChildConfig }, mapper1, comparer, dest.Pcid, condresult);
                    //destpchildconf = pchildconf.FirstOrDefault();
                    //dest.ComponentAdditionalSettings = destcmpads.ToArray();
                }

                //IList<ChildConfig> destChildConfig = new List<ChildConfig>(src.ParentChildConfig.ChildConfigs);
                ////Child Config.
                //if (destChildConfig.Count > 0)
                //{
                //    obj = new GetComparer<ChildConfig>();
                //    comparer = obj.GetComparerobj<ChildConfig>();

                //    obj1 = new GetComparer<DependentForm>();
                //    comparer1 = obj1.GetComparerobj<DependentForm>();
                //    foreach (ChildConfig item in src.ParentChildConfig.ChildConfigs)
                //    {
                //        long id = item.Ccid;
                //        ChildConfig destChildConfigDetail = destChildConfig.Where(w => w.Ccid == item.Ccid).FirstOrDefault();
                //        if (destChildConfigDetail != null)
                //        {

                //            IList<DependentForm> list = destChildConfigDetail.DependentForms.ToList();
                //            if (item.DependentForms.Count > 0)
                //            {
                //                destdepend = Mapsubobjects<DependentForm>(typeof(DependentForm), item.DependentForms.ToList(), list, mapper1, comparer1, item.Ccid, condresult);
                //            }
                //        }
                //        destcconf = Mapsubobjects<ChildConfig>(typeof(ChildConfig), src.ParentChildConfig.ChildConfigs, dest.ParentChildConfig.ChildConfigs, mapper1, comparer, dest.ParentChildConfig.Pccid, condresult);
                //    }
                //}
            }
            return dest;
        }

        private IEnumerable<T> Mapsubobjects<T>(Type proptype, IList<T> src, IList<T> dest, Mapper mapper1, dynamic comparer, dynamic parentid, Difference condresult)
        {
            string distinctpropname = null, keyrltns = null, subcolls = null;
            dynamic repo = null, parentkeyvalue = null;

            dictClassPropname.TryGetValue(proptype, out distinctpropname);
            repo = dictClassreponame[proptype];
            dictClassRltnname.TryGetValue(proptype, out keyrltns);
            dictClassSubColl.TryGetValue(proptype.ToString(), out subcolls);

            var prop = proptype.GetProperty(keyrltns);
            if (dest.Count > 0)
                parentkeyvalue = dest.Select(fi => prop.GetValue(fi)).FirstOrDefault();

            if (parentkeyvalue == null) parentkeyvalue = parentid;

            var resultlist = new List<T>();
            var updatelist = new List<T>();
            var addlist = new List<T>();
            var removelist = new List<T>();

            if ((condresult.DifferenceType == DifferenceTypes.NumberOfElementsMismatch) || (condresult.DifferenceType == DifferenceTypes.ValueMismatch) && (src != null && dest != null))
            {
                IList<T> tempdestobj = new List<T>();
                if (src.Count == dest.Count)
                {
                    foreach (T srcobj in src)
                    {
                        var destprop = proptype.GetProperty(distinctpropname);
                        var additionalprop1 = proptype.GetProperty("SubsetNo");
                        var additionalprop2 = proptype.GetProperty("PropName");
                        var additionalprop3 = proptype.GetProperty("RowNumber");
                        var tempdest = dest.FirstOrDefault(fi => destprop.Name.Contains("PropValue") ?
                                    ((additionalprop1 != null) ?
                                    ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop1.GetValue(fi).Equals(additionalprop1.GetValue(srcobj)))
                                        && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj))))
                                    : (additionalprop3 != null) ?
                                     ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj)))
                                     && (additionalprop3.GetValue(fi).Equals(additionalprop3.GetValue(srcobj))))
                                    : ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj)))))
                                    : (destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))));
                        if (tempdest != null)
                        {
                            tempdestobj.Add(tempdest);
                            var result = comparer.Compare(srcobj, tempdest);
                            tempdest = mapper1.Map<T, T>(srcobj, tempdest);
                            if (!result)
                            {
                                updatelist.Add(tempdest);
                                //update data                       
                                // repo.Update(tempdest);
                            }
                        }
                        else
                        {
                            var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                            dynamic latestdata = mapperconfig.Map<T, T>(srcobj);
                            //add data
                            latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                            addlist.Add(latestdata);
                            //   repo.Add(latestdata);
                        }

                    }

                    foreach (T obj in dest)
                    {
                        if (!tempdestobj.Contains(obj))
                        {
                            removelist.Add(obj);
                        }
                    }

                }
                else
                {
                    int i = src.Count;
                    int j = dest.Count;

                    //int ij = 0;

                    if (i < j)
                    {
                        foreach (T srcobj in src)
                        {
                            var destprop = proptype.GetProperty(distinctpropname);
                            var additionalprop1 = proptype.GetProperty("SubsetNo");
                            var additionalprop2 = proptype.GetProperty("PropName");
                            var additionalprop3 = proptype.GetProperty("RowNumber");
                            var tempdest = dest.FirstOrDefault(fi => destprop.Name.Contains("PropValue") ?
                                    ((additionalprop1 != null) ?
                                    ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop1.GetValue(fi).Equals(additionalprop1.GetValue(srcobj)))
                                        && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj))))
                                    : (additionalprop3 != null) ?
                                     ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj)))
                                     && (additionalprop3.GetValue(fi).Equals(additionalprop3.GetValue(srcobj))))
                                    : ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj)))))
                                    : (destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))));
                            //ij = k + 1;
                            if (tempdest != null)
                            {
                                var result = comparer.Compare(srcobj, tempdest);
                                tempdestobj.Add(tempdest);
                                tempdest = mapper1.Map<T, T>(srcobj, tempdest);

                                if (!result)
                                {
                                    updatelist.Add(tempdest);
                                    //update data                       
                                    // repo.Update(tempdest);
                                }
                            }
                            else
                            {
                                var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                string checkIfEmptyOrNull = (string)srcobj.GetType().GetProperty("PropValue")?.GetValue(srcobj, null);
                                bool isTrulyEmpty = string.IsNullOrEmpty(checkIfEmptyOrNull);
                                if (!isTrulyEmpty)
                                {
                                    dynamic latestdata = mapperconfig.Map<T, T>(srcobj);
                                    //add data
                                    latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                                    addlist.Add(latestdata);
                                }

                            }
                        }

                        //for (int k = ij; k < j; k++)
                        //{

                        //    removelist.Add(dest[k]);
                        //    //remove data
                        //    //repo.Remove(dest[k]);
                        //}

                        foreach (T obj in dest)
                        {
                            if (!tempdestobj.Contains(obj))
                            {
                                removelist.Add(obj);
                            }
                        }
                    }
                    else if (i > j)
                    {
                        foreach (T srcobj in src)
                        {
                            var destprop = proptype.GetProperty(distinctpropname);
                            var additionalprop1 = proptype.GetProperty("SubsetNo");
                            var additionalprop2 = proptype.GetProperty("PropName");
                            var tempdest = dest.FirstOrDefault(fi => destprop.Name.Contains("PropValue") ?
                                    ((additionalprop1 != null) ?
                                    ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop1.GetValue(fi).Equals(additionalprop1.GetValue(srcobj)))
                                        && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj))))
                                        : ((destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))) && (additionalprop2.GetValue(fi).Equals(additionalprop2.GetValue(srcobj)))))
                                    : (destprop.GetValue(fi).Equals(destprop.GetValue(srcobj))));
                            // ij = k + 1;
                            if (tempdest != null)
                            {
                                var result = comparer.Compare(srcobj, tempdest);
                                tempdest = mapper1.Map<T, T>(srcobj, tempdest);
                                tempdestobj.Add(tempdest);
                                if (!result)
                                {
                                    updatelist.Add(tempdest);
                                    //update data                       
                                    //repo.Update(tempdest);
                                }
                            }
                            else
                            {
                                var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                                dynamic latestdata = mapperconfig.Map<T, T>(srcobj);
                                //add data
                                latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                                addlist.Add(latestdata);
                                //repo.Add(latestdata);
                            }
                        }

                        foreach (T obj in dest)
                        {
                            if (!tempdestobj.Contains(obj))
                            {
                                removelist.Add(obj);
                            }
                        }

                        //for (int k = ij; k < i; k++)
                        //{
                        //    var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
                        //    dynamic latestdata = mapperconfig.Map<T, T>(src[k]);
                        //    latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
                        //    addlist.Add(latestdata);
                        //    //add data
                        //    //repo.Add(latestdata);
                        //}
                    }
                }



                //foreach (dynamic obj in dest)
                //{            
                //    var destprop = proptype.GetProperty(distinctpropname);
                //    var additionalprop = proptype.GetProperty("PropName");
                //    var tempsrc = src.FirstOrDefault(fi => destprop.Name.Contains("PropValue") ? ((destprop.GetValue(fi).Equals(destprop.GetValue(obj))) && (additionalprop.GetValue(fi).Equals(additionalprop.GetValue(obj)))) : (destprop.GetValue(fi).Equals(destprop.GetValue(obj))));

                //    if (tempsrc == null)
                //    {
                //        Console.WriteLine("MissedElementInFirstObject " + destprop.GetValue(obj));
                //        if(!removelist.Contains(obj)) 
                //            removelist.Add(obj);
                //        //db obj is removed/modified by user in updated data, so this data has to be deleted in db.
                //    }
                //}
                //repo.RemoveRange(removelist);
            }
            //else if (condresult.DifferenceType == DifferenceTypes.ValueMismatch)
            //{
            //    if (src.Count == dest.Count)
            //    {

            //        for (int i = 0; i < src.Count; i++)
            //        {
            //            var subresult = comparer.Compare(src[i], dest[i]);
            //            if (!subresult)
            //            {
            //                dest[i] = mapper1.Map<T, T>(src[i], dest[i]);
            //                updatelist.Add(dest[i]);
            //            }

            //        }

            //    }
            //    else
            //    {
            //        int i = src.Count;
            //        int j = dest.Count;
            //        int ij = 0;
            //        if (i < j)
            //        {
            //            for (int k = 0; k < i; k++)
            //            {
            //                var subresult = comparer.Compare(src[k], dest[k]);                            
            //                if (!subresult)
            //                {
            //                    dest[k] = mapper1.Map<T, T>(src[k], dest[k]);
            //                    updatelist.Add(dest[k]);
            //                }
            //                ij = k + 1;
            //            }

            //            for (int k = ij; k < j; k++)
            //            {
            //                removelist.Add(dest[k]);
            //                //remove data
            //                //repo.Remove(dest[k]);
            //            }
            //        }
            //        else if (i > j)
            //        {
            //            for (int k = 0; k < j; k++)
            //            {
            //                var subresult = comparer.Compare(src[k], dest[k]);
            //                if (!subresult)
            //                {
            //                    dest[k] = mapper1.Map<T, T>(src[k], dest[k]);
            //                    updatelist.Add(dest[k]);
            //                }
            //                ij = k + 1;
            //            }

            //            for (int k = ij; k < i; k++)
            //            {
            //                var mapperconfig = MapperConfig.InitialiseMapperforAddingSametype();
            //                dynamic latestdata = mapperconfig.Map<T, T>(src[k]);
            //                latestdata = Adddata<T>(keyrltns, proptype, parentkeyvalue, subcolls, latestdata);
            //                addlist.Add(latestdata);
            //                //add data
            //                //repo.Add(latestdata);
            //            }
            //        }
            //    }
            //}
            if (updatelist.Count() > 0)
            {
                foreach (T obj in updatelist)
                {
                    object val = null;
                    bool isKeyValue = false;
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute))
                            as KeyAttribute;

                        if (attribute != null) // This property has a KeyAttribute
                        {
                            isKeyValue = true;
                            // Do something, to read from the property:
                            val = property.GetValue(obj);
                        }
                    }
                    if (!isKeyValue || Convert.ToInt64(val) != 0)
                        repo.Update(obj);
                }
            }
            //repo.UpdateRange(updatelist);
            repo.AddRange(addlist);
            repo.RemoveRange(removelist);
            _unitOfWork.Complete().Wait();
            resultlist.AddRange(dest);
            return resultlist;
        }

        private T Adddata<T>(string keyrltns, Type proptype, dynamic parentkeyvalue, dynamic subcolls, dynamic latestdata)
        {
            string fkeynames = null;
            bool firstiteration = true;
            string subfkeynames = null, subcoll1 = null;
            Type subproptype = null;
            dynamic prop1;
            var prop = proptype.GetProperty(keyrltns);
            dictClassFKeyname.TryGetValue(proptype, out fkeynames);

            //update parent id for the object
            foreach (string keyval in keyrltns.Split(','))
            {
                prop.SetValue(latestdata, parentkeyvalue);
            }

            //looping through to find the child and hierarchy of childs and update their foreign key relationships
            for (int addhierarchyloop = 0; addhierarchyloop < 2; addhierarchyloop++)
            {
                if (firstiteration)
                {
                    //update child at the first iteration
                    prop1 = (subcolls != null) ? proptype.GetProperty(subcolls) : null;
                    firstiteration = false;
                }
                else
                {
                    //update all the hierarchy objects
                    dictClassSubColl.TryGetValue(proptype.ToString() + addhierarchyloop, out subcoll1);
                    subcolls = subcoll1;
                    prop1 = (subcolls != null) ? proptype.GetProperty(subcolls) : null;

                }

                if ((subcolls != null) && (prop1 != null))
                {
                    var value1 = prop1.GetValue(latestdata);
                    dictStringClassname.TryGetValue(subcolls, out subproptype);
                    dictClassFKeyname.TryGetValue(subproptype, out subfkeynames);

                    if ((value1 != null) && (subfkeynames != null) && (dbform != null))
                    {
                        if (value1.GetType().IsSerializable)
                        {
                            foreach (dynamic obj in value1)
                            {
                                //hierarchy level fk relations update.
                                UpdateFKRelations<dynamic>(subproptype, obj);
                            }
                            prop1.SetValue(latestdata, value1.ToArray());
                        }
                        else
                        {
                            UpdateFKRelations<dynamic>(subproptype, value1);
                            prop1.SetValue(latestdata, value1);
                        }
                    }
                }
                //child level fk relations update
                latestdata = UpdateFKRelations<T>(proptype, latestdata);
            }

            return latestdata;
        }

        private T UpdateFKRelations<T>(Type proptype, dynamic latestdata)
        {
            string fkeynames = null;
            dictClassFKeyname.TryGetValue(proptype, out fkeynames);

            if ((fkeynames != null) && (dbform != null))
            {
                foreach (string key in fkeynames.Split(","))
                {
                    if ((key == "DOCID") && (latestdata.Doc != null))
                    {
                        string docprop = "";
                        dictClassPropname.TryGetValue(typeof(BL.Models.Document), out docprop);
                        PropertyInfo docproperty = typeof(BL.Models.Document).GetProperty(docprop);
                        var docdata = dbform.Documents.FirstOrDefault(fi => docproperty.GetValue(fi).Equals(latestdata.Doc.FileId));
                        if (docdata != null)
                        {
                            latestdata.Doc = docdata;
                            latestdata.Docid = docdata.Docid;
                        }
                    }

                    else if ((key == "DDSID") && (latestdata.Dds != null))
                    {
                        string ddsprop = "";
                        dictClassPropname.TryGetValue(typeof(BL.Models.DesignDataSet), out ddsprop);
                        PropertyInfo ddsproperty = typeof(BL.Models.DesignDataSet).GetProperty(ddsprop);
                        var ddsdata = dbform.DesignDataSets.FirstOrDefault(fi => ddsproperty.GetValue(fi).Equals(latestdata.Dds.DdsetId));
                        if (ddsdata != null)
                        {
                            latestdata.Dds = ddsdata;
                            latestdata.Ddsid = ddsdata.Ddsid;
                        }
                    }

                    else if ((key == "LSTID") && (latestdata.Lst != null))
                    {
                        string lstprop = "";
                        dictClassPropname.TryGetValue(typeof(BL.Models.List), out lstprop);
                        PropertyInfo lstproperty = typeof(BL.Models.List).GetProperty(lstprop);
                        var lstdata = dbform.Lists.FirstOrDefault(fi => lstproperty.GetValue(fi).Equals(latestdata.Lst.Lstname));
                        if (lstdata != null)
                        {
                            latestdata.Lst = lstdata;
                            latestdata.Lstid = lstdata.Lstid;
                        }
                    }
                    else if ((key == "CMPID") && (latestdata.Cmp != null))
                    {
                        string cmpprop = "";
                        dictClassPropname.TryGetValue(typeof(BL.Models.Component), out cmpprop);
                        PropertyInfo cmpproperty = typeof(BL.Models.Component).GetProperty(cmpprop);
                        var cmpdata = dbform.Pages.Select(f => f.Components.Where(fi => cmpproperty.GetValue(fi).Equals(latestdata.Cmp.Name)).FirstOrDefault()).Where(f => f != null).FirstOrDefault();
                        if (cmpdata != null)
                        {
                            latestdata.Cmp = cmpdata;
                            latestdata.Cmpid = cmpdata.Cmpid;
                        }
                    }

                    else if ((key == "CALCID") && (latestdata.Calc != null))
                    {
                        string calcprop = "";
                        dictClassPropname.TryGetValue(typeof(BL.Models.Calculation), out calcprop);
                        PropertyInfo calcproperty = typeof(BL.Models.Calculation).GetProperty(calcprop);
                        var calcdata = dbform.Calculations.FirstOrDefault(fi => calcproperty.GetValue(fi).Equals(latestdata.Calc.Name));
                        if (calcdata != null)
                        {
                            latestdata.Calc = calcdata;
                            latestdata.Calcid = calcdata.Calcid;
                        }
                    }

                }
            }
            return latestdata;
        }


        public IAsyncEnumerable<object> getResponses(string FormId, string StartDate, string EndDate)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var result = _unitOfWork.ResponsesRepository.GetResponses(FormId, StartDate, EndDate);
            sw.Stop();
            Console.WriteLine("getResponses: " + sw.ElapsedMilliseconds);
            return result;
        }
        public IAsyncEnumerable<object> getResponseQuestion(string FormId, string StartDate, string EndDate)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var result = _unitOfWork.ResponsesRepository.GetResponseQuestion(FormId, StartDate, EndDate);
            sw.Stop();
            Console.WriteLine("getResponseQuestion: " + sw.ElapsedMilliseconds);
            return result;
        }
        public IAsyncEnumerable<object> getResponseQuestionData(string FormId, string StartDate, string EndDate)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var result = _unitOfWork.ResponsesRepository.GetResponseQuestionData(FormId, StartDate, EndDate);
            sw.Stop();
            Console.WriteLine("getResponseQuestionData: " + sw.ElapsedMilliseconds);
            return result;
        }
        public bool getResponseById(string submissionId)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            var response = _unitOfWork.ResponsesRepository.checkResponseExists(submissionId);
            sw.Stop();
            Console.WriteLine("getResponseById: " + sw.ElapsedMilliseconds);
            return response;
        }
        public List<Tuple<string, string, string>> getSubmittedStatusbyParentId(string formId, string ukprn, bool isUAT)
        {
            var sw = Stopwatch.StartNew();
            sw.Start();
            List<Tuple<string, string, string>> response = _unitOfWork.ResponsesRepository.GetSubmittedStatusByParentId(formId, ukprn, isUAT).ToList();
            sw.Stop();
            Console.WriteLine("getSubmittedStatusbyParentId: " + sw.ElapsedMilliseconds);
            return response;
        }
    }
}
