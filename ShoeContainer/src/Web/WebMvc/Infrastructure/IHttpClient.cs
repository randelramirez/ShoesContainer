﻿using System.Net.Http;
using System.Threading.Tasks;

namespace ShoesOnContainers.Web.WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item );

        Task<HttpResponseMessage> DeleteAsync(string uri  );

        Task<HttpResponseMessage> PutAsync<T>(string uri, T item );
    }
}