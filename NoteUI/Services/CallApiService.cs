using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoteUI.Services
{
    public class CallApiService
    {
        public async Task<string> Post(string url, object data)
        {
            HttpClient client = new HttpClient();

            string stringData = JsonConvert.SerializeObject(data);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, contentData).Result;

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> Get(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}