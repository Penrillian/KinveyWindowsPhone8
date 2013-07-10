using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Com.Penrillian.Kinvey
{
    internal class Factory : IFactory
    {
        private static Factory _factory;

        public static Factory Instance
        {
            get
            {
                return _factory ?? (_factory = new Factory());
            }
        }

        private Factory()
        {

        }

        public T Get<T>(params object[] args)
        {
            if (typeof(T) == typeof(IHttpClient))
            {
                return (T) ((IHttpClient) new ProxyHttpClient(new HttpClient()));
            }

            if (typeof (T) == typeof (IHttpResponseMessage))
            {
                return (T) ((IHttpResponseMessage) new ProxyHttpResponseMessage(args[0] as HttpResponseMessage));
            }

            if (typeof(T) == typeof(IHttpRequestHeaders))
            {
                return (T) ((IHttpRequestHeaders)new ProxyHttpRequestHeaders(args[0] as HttpRequestHeaders));
            }

            if (typeof(T) == typeof(IHttpContent))
            {
                var httpContent = args[0] as HttpContent;
                if(null != httpContent)
                    return (T)((IHttpContent)new ProxyHttpContent(httpContent));

                var stringContent = args[0] as string;
                if(null != stringContent)
                    return (T)((IHttpContent)new ProxyHttpContent(new StringContent(stringContent, Encoding.UTF8, "application/json")));
            }

            if (typeof(T) == typeof(HttpContent))
            {
                var proxyContent = args[0] as ProxyHttpContent;
                if (proxyContent != null) return (T)(object)(proxyContent.Content);
            }

            return default(T);
        }
    }
}
