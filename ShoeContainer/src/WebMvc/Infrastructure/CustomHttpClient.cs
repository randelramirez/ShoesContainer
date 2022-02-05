using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ShoesOnContainers.Web.WebMvc.Infrastructure
{
    public class CustomHttpClient: IHttpClient
    {
        private readonly HttpClient client;
        private readonly ILogger<CustomHttpClient> logger;

        public CustomHttpClient(ILogger<CustomHttpClient> logger)
        {
            this.client = new HttpClient();
            this.logger = logger;
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry 
            // as it is disposed after each call

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };
            //if (requestId != null)
            //{
            //    requestMessage.Headers.Add("x-requestid", requestId);
            //}

            var response = await client.SendAsync(requestMessage);

            // raise exception if HttpResponseCode 500 
            // needed for circuit breaker to track fails

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }


        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item);
        }
        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await client.SendAsync(requestMessage);
        }

       
    }
}