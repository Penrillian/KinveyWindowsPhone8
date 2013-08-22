using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    internal class KinveyHandshake : IKinveyHandshake
    {
        private readonly IFactory _factory;
        private readonly IHttpClient _httpClient;

        internal KinveyHandshake(IFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.Get<IHttpClient>();

            var headers = _httpClient.DefaultRequestHeaders;
            headers.Add("Authorization", new []
                {
                    string.Format("Basic {0}", KinveySettings.Get().AppAuthToken)
                });
            headers.Add("Accept", new[]
                {
                    "application/json"
                });

            _httpClient.BaseAddress = new Uri("https://baas.kinvey.com/");
        }

        public async Task<KinveyHandshakeResponse> Do()
        {
            var uri = new Uri(string.Format("/appdata/{0}", KinveySettings.Get().AppKey), UriKind.Relative);

            var resp = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            return await JsonConvert.DeserializeObjectAsync<KinveyHandshakeResponse>(json).ConfigureAwait(false);
        }
    }
}