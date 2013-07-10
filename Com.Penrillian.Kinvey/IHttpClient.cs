using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.Penrillian.Kinvey
{
    internal interface IHttpClient
    {
        Uri BaseAddress { set; }

        IHttpRequestHeaders DefaultRequestHeaders { get; }

        Task<IHttpResponseMessage> DeleteAsync(Uri requestUri);

        Task<IHttpResponseMessage> PutAsync(Uri requestUri, IHttpContent content);

        Task<IHttpResponseMessage> PostAsync(Uri requestUri, IHttpContent content);

        Task<IHttpResponseMessage> GetAsync(Uri requestUri);

    }

    internal class ProxyHttpClient : IHttpClient
    {
        private readonly HttpClient _client;

        public ProxyHttpClient(HttpClient client)
        {
            _client = client;
        }

        public Uri BaseAddress
        {
            set { _client.BaseAddress = value; }
        }

        public IHttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return Factory.Instance.Get<IHttpRequestHeaders>(_client.DefaultRequestHeaders);
            }
        }

        public async Task<IHttpResponseMessage> GetAsync(Uri requestUri)
        {
            return Factory.Instance.Get<IHttpResponseMessage>(await _client.GetAsync(requestUri));
        }

        public async Task<IHttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return Factory.Instance.Get<IHttpResponseMessage>(await _client.DeleteAsync(requestUri));
        }

        public async Task<IHttpResponseMessage> PutAsync(Uri requestUri, IHttpContent content)
        {

            var rawContent = Factory.Instance.Get<HttpContent>(content);
            return Factory.Instance.Get<IHttpResponseMessage>(await _client.PutAsync(requestUri, rawContent));
        }

        public async Task<IHttpResponseMessage> PostAsync(Uri requestUri, IHttpContent content)
        {
            var rawContent = Factory.Instance.Get<HttpContent>(content);
            return Factory.Instance.Get<IHttpResponseMessage>(await _client.PostAsync(requestUri, rawContent));
        }
    }
}