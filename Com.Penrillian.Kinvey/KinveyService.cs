using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    internal class KinveyService<T> : IKinveyService<T> where T : KinveyObject, new()
    {
        private readonly IFactory _factory;
        private readonly IHttpClient _httpClient;
        private readonly IHttpRequestHeaders _headers;

        private string _entityName;

        internal KinveyService(IFactory factory)
        {
            _factory = factory;

            _httpClient = _factory.Get<IHttpClient>();
            _headers = _httpClient.DefaultRequestHeaders;

            ReadAttributes();

            if(null == _entityName)
                throw new ArgumentException("Target class must be marked with the KinveyCollection attribute");

            InitClient();
        }

        private void InitClient()
        {
            _headers.Add("Authorization", new[]
                {
                    string.Format("Kinvey {0}", KinveySettings.Get().UserAuthToken)
                });
            _headers.Add("X-Kinvey-Api-Version", new[]
                {
                    "2"
                });

            _httpClient.BaseAddress = new Uri("https://baas.kinvey.com/");
        }

        private void ReadAttributes()
        {
            var t = typeof (T);
            var attributes = Attribute.GetCustomAttributes(t);

            foreach (var attribute in attributes
                .Where(attribute => attribute is KinveyCollectionAttribute)
                .Cast<KinveyCollectionAttribute>())
            {
                _entityName = attribute.CollectionName;
            }
        }

        #region Implementation of IKinveyService<T>

        public async Task<T> Create(T t)
        {
            var body = _factory.Get<IHttpContent>(JsonConvert.SerializeObject(t));
            var uri = new Uri(string.Format("/appdata/{0}/{1}/", KinveySettings.Get().AppKey, _entityName), UriKind.Relative);

            var response = await _httpClient.PostAsync(uri, body);
            return await GetResult<T>(response);
        }

        public async Task<T> Read(string id)
        {
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}/", KinveySettings.Get().AppKey, _entityName, id), UriKind.Relative);

            var response = await _httpClient.GetAsync(uri);
            return await GetResult<T>(response);
        }

        public async Task<IEnumerable<T>> Read(KinveyQuery<T> kinveyQuery)
        {
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}", KinveySettings.Get().AppKey, _entityName, kinveyQuery), UriKind.Relative);

            var response = await _httpClient.GetAsync(uri);
            return await GetResult<IEnumerable<T>>(response);
        }

        public async Task<T> Update(T t)
        {
            var body = _factory.Get<IHttpContent>(JsonConvert.SerializeObject(t));
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}", KinveySettings.Get().AppKey, _entityName, t.Id), UriKind.Relative);

            var response = await _httpClient.PutAsync(uri, body);
            return await GetResult<T>(response);
        }

        public async Task<int> Delete(T t)
        {
            var id = t.Id;
            var query = "{\"_id\": \"" + id + "\"}";
            var uri = new Uri(string.Format("/appdata/{0}/{1}/?query={2}", KinveySettings.Get().AppKey, _entityName, query), UriKind.Relative);

            var response = await _httpClient.DeleteAsync(uri);
            return (await GetResult<CountResponse>(response)).Count;
        }

        public async Task<int> Delete(KinveyQuery<T> query)
        {
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}", KinveySettings.Get().AppKey, _entityName, query), UriKind.Relative);

            var response = await _httpClient.DeleteAsync(uri);
            return (await GetResult<CountResponse>(response)).Count;
        }

        public async Task<int> Count()
        {
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}", KinveySettings.Get().AppKey, _entityName, "_count"), UriKind.Relative);

            var response = await _httpClient.GetAsync(uri);
            return (await GetResult<CountResponse>(response)).Count;
        }

        public async Task<int> Count(KinveyQuery<T> query)
        {
            var uri = new Uri(string.Format("/appdata/{0}/{1}/{2}/{3}", KinveySettings.Get().AppKey, _entityName, "_count", query), UriKind.Relative);

            var response = await _httpClient.GetAsync(uri);
            return (await GetResult<CountResponse>(response)).Count;
        }

        #endregion

        private static async Task<TR> GetResult<TR>(IHttpResponseMessage response)
        {
            var content = response.Content;
            var json = await content.ReadAsStringAsync();

            try
            {
                var errorObject = JsonConvert.DeserializeObject<KinveyError>(json);
                if (null != errorObject.Error)
                    throw new KinveyException(errorObject);
            }
            catch (JsonSerializationException)
            { }

            return JsonConvert.DeserializeObject<TR>(json);
        }
    }
}