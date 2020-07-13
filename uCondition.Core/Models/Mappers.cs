using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Data.Models;

namespace uCondition.Core.Models
{
    public class Mappers
    {
        public static GlobalConditionModel DataToModel(GlobalCondition data)
        {
            var model = new GlobalConditionModel();
            model.Name = data.Name;
            model.Guid = data.Guid;
            model.Created = data.Created.ToString("yyyy/MM/dd");
            model.LastUpdated = data.LastUpdated.ToString("yyyy/MM/dd");
            model.Id = data.Id;
            model.Condition = JsonConvert.DeserializeObject<uConditionModel>(data.Data);
            return model;
        }

        public static GlobalCondition ModelToData(GlobalConditionModel model)
        {
            var data = new GlobalCondition();
            data.Name = model.Name;
            data.Guid = model.Guid;
            data.Created = !string.IsNullOrEmpty(model.Created)? DateTime.ParseExact(model.Created, "yyyy/MM/dd", CultureInfo.InvariantCulture) : DateTime.Now;
            data.LastUpdated = !string.IsNullOrEmpty(model.LastUpdated) ? DateTime.ParseExact(model.LastUpdated, "yyyy/MM/dd", CultureInfo.InvariantCulture) : DateTime.Now;
            data.Id = model.Id;
            data.Data = JsonConvert.SerializeObject(model.Condition);
            return data;
        }
    }
}
