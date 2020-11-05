using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using uCondition.Core.Data.Models;
using uCondition.Core.Interfaces;
using uCondition.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace uCondition.Core.Controllers
{
    public class UConditionApiController : UmbracoAuthorizedApiController
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly PropertyEditorCollection _propertyEditors;
        private readonly IGlobalConditionsRepository _globalConditionsRepository;
        private readonly IPredicateManager _predicateManager;

        public UConditionApiController(IDataTypeService dataTypeService, IGlobalConditionsRepository globalConditionsRepository,
            PropertyEditorCollection propertyEditors, IPredicateManager predicateManager)
        {
            _dataTypeService = dataTypeService;
            _globalConditionsRepository = globalConditionsRepository;
            _propertyEditors = propertyEditors;
            _predicateManager = predicateManager;
        }

        #region Editor
        /// <summary>
        /// /umbraco/backoffice/Api/uConditionApi/GetPredicates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<uCondition.Models.Predicate> GetPredicates()
        {
            return _predicateManager.GetPredicates();
        }


        /// <summary>
        /// /umbraco/backoffice/Api/uConditionApi/GetPropertyEditorByName
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetPropertyEditorByName(string datatypeName)
        {
            var datatype = _dataTypeService.GetDataType(datatypeName);

            if (datatype == null)
            {
                return null;
            }

            if (!_propertyEditors.TryGet(datatype.EditorAlias, out var propertyType))
            {
                return null;
            }
            ////var prevaluesDb = _dataTypeService.GetPreValuesCollectionByDataTypeId(datatype.Id);

            var preValuesFromDb = (ValueListConfiguration)datatype.Configuration;
            var preValues = new Dictionary<string, object>();

            foreach (var item in preValuesFromDb.Items)
            {
                preValues.Add(item.Id.ToString(), item.Value);
            }
            //if (prevaluesDb.IsDictionaryBased)
            //{
            //    if (prevaluesDb.PreValuesAsDictionary.Count > 0 && stringIsNumber(prevaluesDb.PreValuesAsDictionary.First().Key))
            //    {
            //        preValues.Add("items", prevaluesDb.PreValuesAsDictionary.Select(c => new { id = c.Value.Id, value = c.Value.Value }));//.Value));
            //    }
            //    else
            //    {
            //        preValues = prevaluesDb.PreValuesAsDictionary.ToDictionary(k => k.Key, v => v.Value.Value != null && v.Value.Value.StartsWith("{") ? JsonConvert.DeserializeObject(v.Value.Value) : (object)v.Value.Value);
            //    }
            //}
            //else
            //{
            //    prevaluesDb.PreValuesAsArray.ToList().ForEach(p => preValues.Add(propertyType.PreValueEditor.Fields[p.SortOrder].Key, p.Value));
            //}


            ////TODO: needed?
            //foreach (var p in propertyType.PreValueEditor.Fields.Where(c => c.View == "hidden"))
            //{
            //    preValues.Add(p.Key, "1");
            //}

            //if (propertyType.DefaultPreValues != null)
            //{
            //    foreach (var defaultPreValue in propertyType.DefaultPreValues.Where(c => !preValues.ContainsKey(c.Key)))
            //    {
            //        preValues.Add(defaultPreValue.Key, defaultPreValue.Value);
            //    }
            //}

            return new { view = propertyType.GetValueEditor().View, prevalues = preValues };
        }

        /// <summary>
        /// Get prevalues by id
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        //[HttpGet]
        //public object GetPrevalues(int[] Ids)
        //{
        //    return Ids.Select(s => umbraco.library.GetPreValueAsString(s));
        //}
        #endregion


        #region Global Conditions
        [HttpGet]
        public IEnumerable<GlobalConditionModel> GetAllGlobalConditions()
        {
            var globalConditions = _globalConditionsRepository.GetAll();

            return globalConditions.Select(c => Mappers.DataToModel(c));
        }

        [HttpGet]
        public uConditionModel GetGlobalConditionData(string guid)
        {
            return JsonConvert.DeserializeObject<uConditionModel>(_globalConditionsRepository.GetSingle(guid).Data);
        }

        [HttpPost]
        public GlobalConditionModel SaveGlobalCondition(GlobalConditionModel model)
        {
            var globalCondition = Mappers.ModelToData(model);

            if (model.Id == 0)
                _globalConditionsRepository.Insert(globalCondition);
            else
                _globalConditionsRepository.Update(globalCondition);

            return Mappers.DataToModel(globalCondition);
        }

        [HttpDelete]
        public void DeleteGlobalCondition(int id)
        {
            _globalConditionsRepository.Delete(id);
        }
        #endregion

        //private bool StringIsNumber(string value)
        //{
        //    return int.TryParse(value, out _);
        //}

    }

    //public static class ObjectToDictionaryHelper
    //{
    //    public static IDictionary<string, object> ToDictionary(this object source)
    //    {
    //        return source.ToDictionary<object>();
    //    }

    //    public static IDictionary<string, T> ToDictionary<T>(this object source)
    //    {
    //        if (source == null)
    //            ThrowExceptionWhenSourceArgumentIsNull();

    //        var dictionary = new Dictionary<string, T>();
    //        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
    //            AddPropertyToDictionary<T>(property, source, dictionary);

    //        return dictionary;
    //    }

    //    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
    //    {
    //        object value = property.GetValue(source);
    //        if (IsOfType<T>(value))
    //            dictionary.Add(property.Name, (T)value);
    //    }

    //    private static bool IsOfType<T>(object value)
    //    {
    //        return value is T;
    //    }

    //    private static void ThrowExceptionWhenSourceArgumentIsNull()
    //    {
    //        throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
    //    }
    //}

}
