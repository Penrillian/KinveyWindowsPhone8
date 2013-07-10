using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Penrillian.Kinvey.OAuth;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        #region Fields

        private Mock<IFactory> _factory;
        private Mock<IHttpClient> _client;
        private Mock<IHttpRequestHeaders> _headers;

        private Task<IHttpResponseMessage> _errorResponseMessageTask;
        private Mock<IHttpResponseMessage> _errorResponseMessage;
        private Mock<IHttpContent> _errorResponseContent;
        private Task<IHttpResponseMessage> _responseMessageTask;
        private Mock<IHttpResponseMessage> _responseMessage;
        private Mock<IHttpContent> _responseContent;

        private KinveyError _errorResponseObject;
        private string _errorResponseJson;
        private Task<string> _errorResponseJsonTask;
        private KinveyUser _responseObject;
        private Task<string> _responseJsonTask;
        private string _responseJson;

        private Mock<IHttpContent> _crudRequestContent;

        private LogInDetails _authRequestObject;
        private string _authRequestJson;
        private Mock<IHttpContent> _authRequestContent;

        private FacebookIdentityToken _socialRequestObject;
        private string _socialRequestJson;
        private Mock<IHttpContent> _socialRequestContent;

        #endregion

        #region Set Up

        [SetUp]
        public void SetUp()
        {
            _factory = new Mock<IFactory>();
            _client = new Mock<IHttpClient>();
            _headers = new Mock<IHttpRequestHeaders>();

            _errorResponseMessageTask = new Task<IHttpResponseMessage>(() => _errorResponseMessage.Object);
            _errorResponseMessage = new Mock<IHttpResponseMessage>();
            _errorResponseContent = new Mock<IHttpContent>();

            _errorResponseObject = new KinveyError { Description = "zebra", DebugInformation = "tortoise", Error = "dolphin"};
            _errorResponseJson = JsonConvert.SerializeObject(_errorResponseObject);
            _errorResponseJsonTask = new Task<string>(() => _errorResponseJson);

            _responseMessageTask = new Task<IHttpResponseMessage>(() => _responseMessage.Object);
            _responseMessage = new Mock<IHttpResponseMessage>();
            _responseContent = new Mock<IHttpContent>();

            _responseObject = new KinveyFacebookUser{ Id = "penguin", Email = "bear@turtle.com", Username = "tiger", Metadata = new KinveyMetadata{AuthToken = "eagle"}, SocialIdentity = new FacebookIdentity{Id="4854853453123", Gender = "male"}};
            _responseJson = JsonConvert.SerializeObject(_responseObject);
            _responseJsonTask = new Task<string>(() => _responseJson);

            _crudRequestContent = new Mock<IHttpContent>();

            _authRequestObject = new LogInDetails {Username = "tiger", Password = "seahorse"};
            _authRequestJson = JsonConvert.SerializeObject(_authRequestObject);
            _authRequestContent = new Mock<IHttpContent>();

            _socialRequestObject = new FacebookIdentityToken { AccessToken = "wasp", Expires = "beetle" };
            _socialRequestJson = JsonConvert.SerializeObject(_socialRequestObject);
            _socialRequestContent = new Mock<IHttpContent>();

            _factory.Setup(f => f.Get<IHttpClient>())
                    .Returns(_client.Object);
            _factory.Setup(f => f.Get<IHttpContent>(_responseJson))
                    .Returns(_crudRequestContent.Object);
            _factory.Setup(f => f.Get<IHttpContent>(_authRequestJson))
                    .Returns(_authRequestContent.Object);
            _factory.Setup(f => f.Get<IHttpContent>(_socialRequestJson))
                    .Returns(_socialRequestContent.Object);

            _client.SetupGet(c => c.DefaultRequestHeaders)
                   .Returns(_headers.Object);

            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_responseMessageTask.Start)
                   .Returns(_responseMessageTask);
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_responseMessageTask.Start)
                   .Returns(_responseMessageTask);
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_responseMessageTask.Start)
                   .Returns(_responseMessageTask);
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_responseMessageTask.Start)
                   .Returns(_responseMessageTask);

            _responseMessage.SetupGet(r => r.Content)
                            .Returns(_responseContent.Object);
            _responseContent.Setup(r => r.ReadAsStringAsync())
                            .Callback(_responseJsonTask.Start)
                            .Returns(_responseJsonTask);

            _errorResponseMessage.SetupGet(r => r.Content)
                            .Returns(_errorResponseContent.Object);
            _errorResponseContent.Setup(r => r.ReadAsStringAsync())
                            .Callback(_errorResponseJsonTask.Start)
                            .Returns(_errorResponseJsonTask);
        }

        [TearDown]
        public void TearDown()
        {
            _headers = null;
            _client = null;
            _factory = null;
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            KinveySettings.Get().AppKey = "badger";
            KinveySettings.Get().AppAuthToken = "goat";
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            KinveySettings.Reset();
        }

        #endregion

        #region Constructor Tests
        // ReSharper disable ObjectCreationAsStatement

        [Test]
        public void AuthorizationHeaderSet()
        {
            new KinveyUserService(_factory.Object);
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void AuthorizationHeaderContent()
        {
            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Basic goat" }, value));

            new KinveyUserService(_factory.Object);
        }

        [Test]
        public void AcceptHeaderContent()
        {
            _headers.Setup(h => h.Add("Accept", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "application/json" }, value));

            new KinveyUserService(_factory.Object);
        }

        [Test]
        public void ApiVersionHeaderSet()
        {
            new KinveyUserService(_factory.Object);
            _headers.Verify(h => h.Add("X-Kinvey-Api-Version", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void ApiVersionHeaderContent()
        {
            _headers.Setup(h => h.Add("X-Kinvey-Api-Version", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "2" }, value));

            new KinveyUserService(_factory.Object);
        }

        [Test]
        public void HostHeaderContent()
        {
            _headers.Setup(h => h.Add("Host", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "baas.kinvey.com" }, value));

            new KinveyUserService(_factory.Object);
        }

        [Test]
        public void BaseAddressSet()
        {
            new KinveyUserService(_factory.Object);
            _client.VerifySet(c => c.BaseAddress = Moq.It.IsAny<Uri>(), Times.Once());
        }

        [Test]
        public void BaseAddressContent()
        {
            _client.SetupSet(c => c.BaseAddress = Moq.It.IsAny<Uri>())
                   .Callback((Uri uri) => Assert.AreEqual("https://baas.kinvey.com/", uri.ToString()))
                   .Verifiable();

            new KinveyUserService(_factory.Object);
        }

        // ReSharper restore ObjectCreationAsStatement
        #endregion

        #region Read Tests

        [Test]
        public async void ReadGetViaClient()
        {
            await new KinveyUserService(_factory.Object).Read("penguin");
            _client.Verify(c => c.GetAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void ReadUriString()
        {
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                       {
                           _responseMessageTask.Start();
                           Assert.AreEqual("/user/badger/penguin", uri.ToString());
                       })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Read("penguin");
        }

        [Test]
        public async void ReadUriType()
        {
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Read("penguin");
        }

        [Test]
        public async void ReadReturnValue()
        {
            var result = await new KinveyUserService(_factory.Object).Read("penguin");
            Assert.AreEqual(_responseObject.Username, result.Username);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void ReadError()
        {
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).Read("penguin");
        }

        #endregion

        #region Update Tests

        [Test]
        public async void UpdatePutViaClientWithContent()
        {
            await new KinveyUserService(_factory.Object).Update(_responseObject);
            _client.Verify(c => c.PutAsync(Moq.It.IsAny<Uri>(), _crudRequestContent.Object), Times.Once());
        }

        [Test]
        public async void UpdateUriString()
        {
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Update(_responseObject);
        }

        [Test]
        public async void UpdateUriType()
        {
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Update(_responseObject);
        }

        [Test]
        public async void UpdateReturnValue()
        {
            var result = await new KinveyUserService(_factory.Object).Update(_responseObject);
            Assert.AreEqual(_responseObject.Username, result.Username);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void UpdateError()
        {
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).Update(_responseObject);
        }

        #endregion

        #region Soft Delete (By Id) Tests

        [Test]
        public async void SoftDeleteByIdDeleteViaClient()
        {
            await new KinveyUserService(_factory.Object).SoftDelete("penguin");
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void SoftDeleteByIdUriString()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin/?soft=true", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete("penguin");
        }

        [Test]
        public async void SoftDeleteByIdUriType()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete("penguin");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void SoftDeleteByIdError()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete("penguin");
        }

        #endregion

        #region Soft Delete (By Object) Tests

        [Test]
        public async void SoftDeleteByObjectDeleteViaClient()
        {
            await new KinveyUserService(_factory.Object).SoftDelete(_responseObject);
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void SoftDeleteByObjectUriString()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin/?soft=true", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete(_responseObject);
        }

        [Test]
        public async void SoftDeleteByObjectUriType()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete(_responseObject);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void SoftDeleteByObjectError()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).SoftDelete(_responseObject);
        }

        #endregion

        #region Hard Delete (By Id) Tests

        [Test]
        public async void HardDeleteByIdDeleteViaClient()
        {
            await new KinveyUserService(_factory.Object).HardDelete("penguin");
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void HardDeleteByIdUriString()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin/?hard=true", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete("penguin");
        }

        [Test]
        public async void HardDeleteByIdUriType()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete("penguin");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void HardDeleteByIdError()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete("penguin");
        }

        #endregion

        #region Hard Delete (By Object) Tests

        [Test]
        public async void HardDeleteByObjectDeleteViaClient()
        {
            await new KinveyUserService(_factory.Object).HardDelete(_responseObject);
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void HardDeleteByObjectUriString()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin/?hard=true", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete(_responseObject);
        }

        [Test]
        public async void HardDeleteByObjectUriType()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete(_responseObject);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void HardDeleteByObjectError()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).HardDelete(_responseObject);
        }

        #endregion

        #region Restore Tests

        [Test]
        public async void RestorePostViaClient()
        {
            await new KinveyUserService(_factory.Object).Restore("penguin");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()), Times.Once());
        }

        [Test]
        public async void RestoreUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/penguin/_restore", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Restore("penguin");
        }

        [Test]
        public async void RestoreUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).Restore("penguin");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void RestoreError()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).Restore("penguin");
        }

        #endregion

        #region Reset Password (By Email) Tests

        [Test]
        public async void PasswordResetEmailPostViaClient()
        {
            await new KinveyUserService(_factory.Object).PasswordResetEmail("bear@turtle.com");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()), Times.Once());
        }

        [Test]
        public async void PasswordResetEmailUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/rpc/badger/bear@turtle.com/user-password-reset-initiate", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetEmail("bear@turtle.com");
        }

        [Test]
        public async void PasswordResetEmailUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetEmail("bear@turtle.com");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void PasswordResetEmailError()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetEmail("bear@turtle.com");
        }

        #endregion

        #region Reset Password (By Username) Tests

        [Test]
        public async void PasswordResetUsernamePostViaClient()
        {
            await new KinveyUserService(_factory.Object).PasswordResetUsername("penguin");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()), Times.Once());
        }

        [Test]
        public async void PasswordResetUsernameUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/rpc/badger/penguin/user-password-reset-initiate", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetUsername("penguin");
        }

        [Test]
        public async void PasswordResetUsernameUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetUsername("penguin");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void PasswordResetUsernameError()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).PasswordResetUsername("penguin");
        }

        #endregion

        #region Email Verification Tests

        [Test]
        public async void VerifyEmailPostViaClient()
        {
            await new KinveyUserService(_factory.Object).VerifyEmail("penguin");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()), Times.Once());
        }

        [Test]
        public async void VerifyEmailUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/rpc/badger/penguin/user-email-verification-initiate", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).VerifyEmail("penguin");
        }

        [Test]
        public async void VerifyEmailUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).VerifyEmail("penguin");
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "zebra")]
        public async void VerifyEmailError()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyUserService(_factory.Object).VerifyEmail("penguin");
        }

        #endregion

        #region Log In Tests

        [Test]
        public async void LogInPostViaClientWithContent()
        {
            await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), _authRequestContent.Object), Times.Once());
        }

        [Test]
        public async void LogInUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/login", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
        }

        [Test]
        public async void LogInUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
        }

        [Test]
        public async void LogInReturnValue()
        {
            var result = await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
            Assert.AreEqual(_responseObject.Id, result.Id);
        }

        [Test]
        public async void LogInUserAuthTokenInSettings()
        {
            await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
            Assert.AreEqual(KinveySettings.Get().UserAuthToken, "eagle");
        }

        [Test]
        public async void LogInCurrentUserInSettings()
        {
            var result = await new KinveyUserService(_factory.Object).LogIn("tiger", "seahorse");
            Assert.AreEqual(KinveySettings.Get().CurrentUser, result);
        }

        [Test]
        public async void LogInUserAuthTokenInHeaders()
        {
            var userService = new KinveyUserService(_factory.Object);

            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Kinvey eagle" }, value));

            await userService.LogIn("tiger", "seahorse");
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Exactly(2));
        }

        #endregion

        #region Sign Up Tests

        [Test]
        public async void SignUpPostViaClientWithContent()
        {
            await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), _authRequestContent.Object), Times.Once());
        }

        [Test]
        public async void SignUpUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
        }

        [Test]
        public async void SignUpUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
        }

        [Test]
        public async void SignUpReturnValue()
        {
            var result = await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
            Assert.AreEqual(_responseObject.Username, result.Username);
        }

        [Test]
        public async void SignUpUserAuthTokenInSettings()
        {
            await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
            Assert.AreEqual(KinveySettings.Get().UserAuthToken, "eagle");
        }

        [Test]
        public async void SignUpCurrentUserInSettings()
        {
            var result = await new KinveyUserService(_factory.Object).SignUp("tiger", "seahorse");
            Assert.AreEqual(KinveySettings.Get().CurrentUser, result);
        }

        [Test]
        public async void SignUpUserAuthTokenInHeaders()
        {
            var userService = new KinveyUserService(_factory.Object);

            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Kinvey eagle" }, value));

            await userService.SignUp("tiger", "seahorse");
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Exactly(2));
        }

        #endregion

        #region Sign Up (Social) Tests

        [Test]
        public async void SignUpSocialPostViaClientWithContent()
        {
            await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
            _client.Verify(c => c.PostAsync(It.IsAny<Uri>(), _socialRequestContent.Object), Times.Once());
        }

        [Test]
        public async void SignUpSocialUriString()
        {
            _client.Setup(c => c.PostAsync(It.IsAny<Uri>(), It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
        }

        [Test]
        public async void SignUpSocialUriType()
        {
            _client.Setup(c => c.PostAsync(It.IsAny<Uri>(), It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
        }

        [Test]
        public async void SignUpSocialReturnValue()
        {
            var result = await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
            Assert.AreEqual(_responseObject.Username, result.Username);
        }

        [Test]
        public async void SignUpSocialUserAuthTokenInSettings()
        {
            await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
            Assert.AreEqual(KinveySettings.Get().UserAuthToken, "eagle");
        }

        [Test]
        public async void SignUpSocialCurrentUserInSettings()
        {
            var result = await new KinveyUserService(_factory.Object).SignUp(_socialRequestObject);
            Assert.AreEqual(KinveySettings.Get().CurrentUser, result);
        }

        [Test]
        public async void SignUpSocialUserAuthTokenInHeaders()
        {
            var userService = new KinveyUserService(_factory.Object);

            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Kinvey eagle" }, value));

            await userService.SignUp(_socialRequestObject);
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Exactly(2));
        }

        #endregion

        #region Log Out Tests

        [Test]
        public async void LogOutPostViaClient()
        {
            await new KinveyUserService(_factory.Object).LogOut();
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()), Times.Once());
        }

        [Test]
        public async void LogOutUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/user/badger/_logout", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).LogOut();
        }

        [Test]
        public async void LogOutUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyUserService(_factory.Object).LogOut();
        }

        [Test]
        public async void LogOutUserAuthTokenInSettings()
        {
            await new KinveyUserService(_factory.Object).LogOut();
            Assert.Null(KinveySettings.Get().UserAuthToken);
        }

        [Test]
        public async void LogOutCurrentUserInSettings()
        {
            await new KinveyUserService(_factory.Object).LogOut();
            Assert.Null(KinveySettings.Get().CurrentUser);
        }

        [Test]
        public async void LogOutAppAuthTokenInHeaders()
        {
            var userService = new KinveyUserService(_factory.Object);

            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Basic goat" }, value));

            await userService.LogOut();
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Exactly(2));
        }

        #endregion
    }
}
