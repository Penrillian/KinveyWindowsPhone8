using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.Test
{
    [TestFixture]
    public class HandshakeTests
    {
        #region Fields

        private Mock<IFactory> _factory;
        private Mock<IHttpClient> _client;
        private Mock<IHttpRequestHeaders> _headers;

        private Task<IHttpResponseMessage> _responseMessageTask;
        private Mock<IHttpResponseMessage> _responseMessage;
        private Mock<IHttpContent> _responseContent;

        private KinveyHandshakeResponse _responseObject;
        private Task<string> _responseJsonTask;
        private string _responseJson;

        #endregion

        #region Set Up

        [SetUp]
        public void SetUp()
        {
            _factory = new Mock<IFactory>();
            _client = new Mock<IHttpClient>();
            _headers = new Mock<IHttpRequestHeaders>();

            _responseMessageTask = new Task<IHttpResponseMessage>(() => _responseMessage.Object);
            _responseMessage = new Mock<IHttpResponseMessage>();
            _responseContent = new Mock<IHttpContent>();

            _responseObject = new KinveyHandshakeResponse{Version = "9.9", Kinvey = "Hello Giraffe"};
            _responseJson = JsonConvert.SerializeObject(_responseObject);
            _responseJsonTask = new Task<string>(() => _responseJson);

            _factory.Setup(f => f.Get<IHttpClient>())
                    .Returns(_client.Object);

            _client.SetupGet(c => c.DefaultRequestHeaders)
                   .Returns(_headers.Object);
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_responseMessageTask.Start)
                   .Returns(_responseMessageTask);

            _responseMessage.SetupGet(r => r.Content)
                            .Returns(_responseContent.Object);
            _responseContent.Setup(r => r.ReadAsStringAsync())
                            .Callback(_responseJsonTask.Start)
                            .Returns(_responseJsonTask);
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
            new KinveyHandshake(_factory.Object);
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void AuthorizationHeaderContent()
        {
            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Basic goat" }, value));

            new KinveyHandshake(_factory.Object);
        }

        [Test]
        public void AcceptHeaderSet()
        {
            new KinveyHandshake(_factory.Object);
            _headers.Verify(h => h.Add("Accept", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void AcceptHeaderContent()
        {
            _headers.Setup(h => h.Add("Accept", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "application/json" }, value));

            new KinveyHandshake(_factory.Object);
        }

        [Test]
        public void BaseAddressSet()
        {
            new KinveyHandshake(_factory.Object);
            _client.VerifySet(c => c.BaseAddress = Moq.It.IsAny<Uri>(), Times.Once());
        }

        [Test]
        public void BaseAddressContent()
        {
            _client.SetupSet(c => c.BaseAddress = Moq.It.IsAny<Uri>())
                   .Callback((Uri uri) => Assert.AreEqual("https://baas.kinvey.com/", uri.ToString()))
                   .Verifiable();

            new KinveyHandshake(_factory.Object);
        }

        // ReSharper restore ObjectCreationAsStatement
        #endregion

        #region Handshake Tests

        [Test]
        public async void GetViaClient()
        {
            await new KinveyHandshake(_factory.Object).Do();
            _client.Verify(c => c.GetAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void UriString()
        {
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                       {
                           _responseMessageTask.Start();
                           Assert.AreEqual("/appdata/badger", uri.ToString());
                       })
                   .Returns(_responseMessageTask);
            await new KinveyHandshake(_factory.Object).Do();
        }

        [Test]
        public async void UriType()
        {
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyHandshake(_factory.Object).Do();
        }

        [Test]
        public async void ReturnValue()
        {
            var result = await new KinveyHandshake(_factory.Object).Do();
            Assert.AreEqual(_responseObject, result);
        }

        #endregion
    }
}
