using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web
{
    public static class API
    {
        private static HttpClient client = new HttpClient();

        public static async Task<string> Get(string url)
        {
            string found = string.Empty;

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode) found = await response.Content.ReadAsStringAsync();
            return found;
        }
    }
}
