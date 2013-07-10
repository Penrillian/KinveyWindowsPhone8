using System;
using System.Threading.Tasks;
using Com.Penrillian.Kinvey.OAuth;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    internal class KinveyUserService : KinveyUserService<KinveyUser>, IKinveyUserService
    {
        public KinveyUserService(IFactory factory) : base(factory)
        {
        }
    }

    internal class KinveyUserService<T> : IKinveyUserService<T> where T : KinveyUser
    {
        private readonly IFactory _factory;
        private readonly IHttpClient _httpClient;
        private readonly IHttpRequestHeaders _headers;

        internal KinveyUserService(IFactory factory)
        {
            _factory = factory;

            _httpClient = _factory.Get<IHttpClient>();
            _headers = _httpClient.DefaultRequestHeaders;

            InitClient();
        }

        private void InitClient()
        {
            _headers.Add("Authorization", new[]
                {
                    string.Format("Basic {0}", KinveySettings.Get().AppAuthToken)
                });
            _headers.Add("X-Kinvey-Api-Version", new[]
                {
                    "2"
                });

            _httpClient.BaseAddress = new Uri("https://baas.kinvey.com/");
        }

        public async Task<KinveyUser> LogIn(string username, string password)
        {
            var uri = new Uri(string.Format("/user/{0}/login", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoAuth(uri, username, password);
        }

        public async Task<KinveyUser> SignUp(string username, string password)
        {
            var uri = new Uri(string.Format("/user/{0}/", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoAuth(uri, username, password);
        }

        public async Task<KinveyFacebookUser> SignUp(FacebookIdentityToken socialIdentity)
        {
            var uri = new Uri(string.Format("/user/{0}/", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoSocialAuth<KinveyFacebookUser, FacebookIdentity>(uri, socialIdentity);
        }

        public async Task<KinveyTwitterUser> SignUp(TwitterIdentityToken socialIdentity)
        {
            var uri = new Uri(string.Format("/user/{0}/", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoSocialAuth<KinveyTwitterUser, TwitterIdendity>(uri, socialIdentity);
        }

        public async Task<KinveyGooglePlusUser> SignUp(GooglePlusIdentityToken socialIdentity)
        {
            var uri = new Uri(string.Format("/user/{0}/", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoSocialAuth<KinveyGooglePlusUser, GooglePlusIdentity>(uri, socialIdentity);
        }

        public async Task<KinveyLinkedInUser> SignUp(LinkedInIdentityToken socialIdentity)
        {
            var uri = new Uri(string.Format("/user/{0}/", KinveySettings.Get().AppKey), UriKind.Relative);
            return await DoSocialAuth<KinveyLinkedInUser, LinkedInIdentity>(uri, socialIdentity);
        }

        public async Task LogOut()
        {
            var content = _factory.Get<IHttpContent>(string.Empty);
            var uri = new Uri("/user/badger/_logout", UriKind.Relative);

            await _httpClient.PostAsync(uri, content);

            KinveySettings.Get().UserAuthToken = null;
            KinveySettings.Get().CurrentUser = null;

            _headers.Remove("Authorization");
            _headers.Add("Authorization", new[]
                {
                    string.Format("Basic {0}", KinveySettings.Get().AppAuthToken)
                });
        }

        public async Task<T> Read(string id)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}", KinveySettings.Get().AppKey, id), UriKind.Relative);
            var response = await _httpClient.GetAsync(uri);
            return await ResponseOrError<T>(response);
        }

        public async Task<T> Update(T user)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}", KinveySettings.Get().AppKey, user.Id), UriKind.Relative);
            var requestContent = GetContent(user);
            var response = await _httpClient.PutAsync(uri, requestContent);
            return await ResponseOrError<T>(response);
        }

        public async Task SoftDelete(string id)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}/?soft=true", KinveySettings.Get().AppKey, id), UriKind.Relative);
            var response = await _httpClient.DeleteAsync(uri);
            await ResponseOrError<T>(response);
        }

        public async Task SoftDelete(T user)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}/?soft=true", KinveySettings.Get().AppKey, user.Id), UriKind.Relative);
            var response = await _httpClient.DeleteAsync(uri);
            await ResponseOrError<T>(response);
        }

        public async Task HardDelete(string id)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}/?hard=true", KinveySettings.Get().AppKey, id), UriKind.Relative);
            var response = await _httpClient.DeleteAsync(uri);
            await ResponseOrError<T>(response);
        }

        public async Task HardDelete(T user)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}/?hard=true", KinveySettings.Get().AppKey, user.Id), UriKind.Relative);
            var response = await _httpClient.DeleteAsync(uri);
            await ResponseOrError<T>(response);
        }

        public async Task Restore(string id)
        {
            var uri = new Uri(string.Format("/user/{0}/{1}/_restore", KinveySettings.Get().AppKey, id), UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, _factory.Get<IHttpContent>(string.Empty));
            await ResponseOrError<T>(response);
        }

        public async Task VerifyEmail(string username)
        {
            var uri = new Uri(string.Format("/rpc/{0}/{1}/user-email-verification-initiate", KinveySettings.Get().AppKey, username), UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, _factory.Get<IHttpContent>(string.Empty));
            await ResponseOrError<T>(response);
        }

        public async Task PasswordResetEmail(string email)
        {
            var uri = new Uri(string.Format("/rpc/{0}/{1}/user-password-reset-initiate", KinveySettings.Get().AppKey, email), UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, _factory.Get<IHttpContent>(string.Empty));
            await ResponseOrError<T>(response);
        }

        public async Task PasswordResetUsername(string username)
        {
            var uri = new Uri(string.Format("/rpc/{0}/{1}/user-password-reset-initiate", KinveySettings.Get().AppKey, username), UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, _factory.Get<IHttpContent>(string.Empty));
            await ResponseOrError<T>(response);
        }

        private async Task<KinveyUser> DoAuth(Uri uri, string username, string password)
        {
            var content = GetContent(new LogInDetails { Username = username, Password = password });

            var response = await _httpClient.PostAsync(uri, content);
            var retVal = await ResponseOrError<T>(response);

            KinveySettings.Get().UserAuthToken = retVal.Metadata.AuthToken;
            KinveySettings.Get().CurrentUser = retVal;

            _headers.Remove("Authorization");
            _headers.Add("Authorization", new[]
                {
                    string.Format("Kinvey {0}", retVal.Metadata.AuthToken)
                });

            return retVal;
        }
        
        private async Task<TSocialUser> DoSocialAuth<TSocialUser, TSocialIdentity>(Uri uri, SocialIdentityToken identity) where TSocialUser : KinveySocialUser<TSocialIdentity>
        {
            var content = GetContent(identity);

            var response = await _httpClient.PostAsync(uri, content);
            var retVal = await ResponseOrError<TSocialUser>(response);

            KinveySettings.Get().UserAuthToken = retVal.Metadata.AuthToken;
            KinveySettings.Get().CurrentUser = retVal;

            _headers.Remove("Authorization");
            _headers.Add("Authorization", new[]
                {
                    string.Format("Kinvey {0}", retVal.Metadata.AuthToken)
                });

            return retVal;
        }

        private IHttpContent GetContent<TR>(TR tr)
        {
            return _factory.Get<IHttpContent>(JsonConvert.SerializeObject(tr));
        }

        private static async Task<TResponse> ResponseOrError<TResponse>(IHttpResponseMessage response)
        {
            var responseContent = response.Content;
            var responseJson = await responseContent.ReadAsStringAsync();
            var errorObject = JsonConvert.DeserializeObject<KinveyError>(responseJson);
            if (null != errorObject.Error)
                throw new KinveyException(errorObject);
            var responseObject = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return responseObject;
        }
    }
}