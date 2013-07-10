using System.Net.Http;

namespace Com.Penrillian.Kinvey
{
    internal interface IHttpResponseMessage
    {
        IHttpContent Content { get; }
    }

    internal class ProxyHttpResponseMessage : IHttpResponseMessage
    {
        private readonly HttpResponseMessage _responseMessage;

        public ProxyHttpResponseMessage(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public IHttpContent Content
        {
            get { return Factory.Instance.Get<IHttpContent>(_responseMessage.Content); }
        }
    }
}