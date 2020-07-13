using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Interfaces
{
    public interface IFieldValues
    {
        string GetValue(string fieldName);
        T GetValue<T>(string fieldName);
        bool TryGetValue<T>(string fieldName, out T value);
    }
}
