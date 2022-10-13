using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web.Helpers
{
    public static class Json
    {
        public static T? TypeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string JsonFromType<T>(T item, bool indented = false)
        {
            return indented ? JsonConvert.SerializeObject(item, Formatting.Indented) : JsonConvert.SerializeObject(item, Formatting.None);
        }
    }
}
