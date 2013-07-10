using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Com.Penrillian.Kinvey
{
    internal interface IHttpRequestHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        void Add(string name, IEnumerable<string> values);
        bool Remove(string name);
    }

    internal class ProxyHttpRequestHeaders : IHttpRequestHeaders
    {
        private readonly HttpRequestHeaders _headers;

        public ProxyHttpRequestHeaders(HttpRequestHeaders headers)
        {
            _headers = headers;
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        public void Add(string name, IEnumerable<string> values)
        {
            _headers.Add(name, values);
        }

        public bool Remove(string name)
        {
            return _headers.Remove(name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _headers.GetEnumerator();
        }
    }
}