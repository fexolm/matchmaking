using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace HistoryBattlesServer.ExternalServices
{
    //TODO: test
    public static class WebService
    {
        public static TResponse Post<TResponse, TRequest>(string url, TRequest data) {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            string requestedData = JsonConvert.SerializeObject(data);
            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(requestedData);
            }

            var response = (HttpWebResponse) request.GetResponse();

            using (var streamReader = new StreamReader(response.GetResponseStream())) {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<TResponse>(result);
            }
        }
    }
}