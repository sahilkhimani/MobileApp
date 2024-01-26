using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Helper
{
    public class CallApi
    {
        //4292
        //public static string _base = "https://103.245.193.86/api/";
        //public static string _base = "http://www.InoviMobileApp.somee.
        ///api/";
        Uri baseAddress = new Uri("https://InoviMobileApp.bsite.net/api/");

        //Uri baseAddress = new Uri("https://localhost:7155/api/");
        //Uri baseAddress = new Uri(_base);

        private readonly HttpClient _httpClient;

        public CallApi()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _httpClient.BaseAddress = baseAddress;
        }
        
        public async Task<string> consumeapi(string body = "", string apiPath = "")
        {
            string val = Preferences.Get("jwtKey", "");
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Preferences.Get("jwtKey", ""));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", ("Bearer" + Preferences.Get("jwtKey", "")));
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + apiPath, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        

    }

}
