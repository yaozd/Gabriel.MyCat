using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastJSON;

namespace Gabriel.MyCat.Util
{
    public class FastJsonHelper
    {
        private static readonly JSONParameters _jsonParameters = InitJsonParameters();

        private static JSONParameters InitJsonParameters()
        {
            return new JSONParameters
            {
                UseExtensions = false,
                UsingGlobalTypes = false,
                SerializeNullValues=false,
                UseUTCDateTime=false
            };
        }

        public static string ToJsJson(object item)
        {
            #if DEBUG
            return JSON.ToNiceJSON(item, _jsonParameters);
            #else
            return JSON.ToJSON(item, _jsonParameters);
            #endif

        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            return (T)fastJSON.JSON.ToObject<T>(jsonString, _jsonParameters);
        }
    }
}
