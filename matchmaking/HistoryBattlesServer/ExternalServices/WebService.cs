using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace HistoryBattlesServer.ExternalServices
{
    //TODO: test
    public static class WebService
    {
        private static readonly HttpClient _client = new HttpClient();
        
        public static async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest data) {
            var response = await _client.PostAsJsonAsync(url, data);

            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
            
            var result = await response.Content.ReadAsAsync<TResponse>();
            return result;
        }
    }
}