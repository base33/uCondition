using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using uCondition.Core.Data.Models;
using uCondition.Core.Models;
using uCondition.Interfaces;
using uCondition.Predicates.Members;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.WebApi;

namespace uCondition.Core.Controllers
{
    public class uConditionApiController : UmbracoAuthorizedApiController
    {
        #region Editor
        /// <summary>
        /// /umbraco/backoffice/Api/uConditionApi/GetPredicates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<uCondition.Models.Predicate> GetPredicates()
        {
            var predicateManager = new PredicateManager();
            return predicateManager.GetPredicates();
        }
        

        /// <summary>
        /// /umbraco/backoffice/Api/uConditionApi/GetPropertyEditorByName
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetPropertyEditorByName(string datatypeName)
        {
            var datatype = Umbraco.DataTypeService.GetDataTypeDefinitionByName(datatypeName);

            if (datatype == null)
            {
                return null;
            }

            var propertyType = PropertyEditorResolver.Current.GetByAlias(datatype.PropertyEditorAlias);
            var prevaluesDb = UmbracoContext.Application.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(datatype.Id);

            var preValues = new Dictionary<string, object>();
            if(prevaluesDb.IsDictionaryBased)
            {
                if (prevaluesDb.PreValuesAsDictionary.Count > 0 && stringIsNumber(prevaluesDb.PreValuesAsDictionary.First().Key))
                {
                    preValues.Add("items", prevaluesDb.PreValuesAsDictionary.Select(c => new { id = c.Value.Id, value = c.Value.Value }));//.Value));
                }
                else
                {
                    preValues = prevaluesDb.PreValuesAsDictionary.ToDictionary(k => k.Key, v => v.Value.Value != null && v.Value.Value.StartsWith("{") ? JsonConvert.DeserializeObject(v.Value.Value) : (object)v.Value.Value);
                }
            }
            else
            {
                prevaluesDb.PreValuesAsArray.ToList().ForEach(p => preValues.Add(propertyType.PreValueEditor.Fields[p.SortOrder].Key, p.Value));
            }


            ////TODO: needed?
            foreach (var p in propertyType.PreValueEditor.Fields.Where(c => c.View == "hidden"))
            {
                preValues.Add(p.Key, "1");
            }

            if (propertyType.DefaultPreValues != null)
            {
                foreach (var defaultPreValue in propertyType.DefaultPreValues.Where(c => !preValues.ContainsKey(c.Key)))
                {
                    preValues.Add(defaultPreValue.Key, defaultPreValue.Value);
                }
            }

            return new { view = propertyType.ValueEditor.View, prevalues = preValues };
        }

        /// <summary>
        /// Get prevalues by id
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetPrevalues(int[] Ids)
        {
            return Ids.Select(s => umbraco.library.GetPreValueAsString(s));
        }
        #endregion


        #region Global Conditions
        [HttpGet]
        public IEnumerable<GlobalConditionModel> GetAllGlobalConditions()
        {
            var globalConditionsRepo = new GlobalConditionsRepository();

            var globalConditions = globalConditionsRepo.GetAll(true);

            return globalConditions.Select(c => Mappers.DataToModel(c));
        }

        [HttpGet]
        public uConditionModel GetGlobalConditionData(string guid)
        {
            var globalConditionsRepo = new GlobalConditionsRepository();
            return JsonConvert.DeserializeObject<uConditionModel>(globalConditionsRepo.GetSingle(guid).Data);
        }

        [HttpPost]
        public GlobalConditionModel SaveGlobalCondition(GlobalConditionModel model)
        {
            var globalCondition = Mappers.ModelToData(model);

            var globalConditionsRepo = new GlobalConditionsRepository();

            var id = model.Id;
            if (model.Id == 0)
                globalConditionsRepo.Insert(globalCondition);
            else
                globalConditionsRepo.Update(globalCondition);

            return Mappers.DataToModel(globalCondition);
        }

        [HttpDelete]
        public void DeleteGlobalCondition(int id)
        {
            var globalConditionsRepo = new GlobalConditionsRepository();
            globalConditionsRepo.Delete(id);
        }
        #endregion

        private bool stringIsNumber(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }
        
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
