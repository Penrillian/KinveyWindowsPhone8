using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Com.Penrillian.Kinvey
{
    internal interface IHttpContent
    {
        Task<string> ReadAsStringAsync();
        HttpContentHeaders Headers { get; }
    }

    internal class ProxyHttpContent : IHttpContent
    {
        internal readonly HttpContent Content;

        public ProxyHttpContent(HttpContent content)
        {
            Content = content;
        }

        public async Task<string> ReadAsStringAsync()
        {
            return await Content.ReadAsStringAsync();
        }

        public HttpContentHeaders Headers 
        { 
            get { return Content.Headers; }
        }
    }
}