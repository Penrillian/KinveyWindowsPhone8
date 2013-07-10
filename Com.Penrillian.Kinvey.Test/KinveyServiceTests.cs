using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.Test
{
    [TestFixture]
    public class KinveyServiceTests
    {
        #region Fields

        private Mock<IFactory> _factory;
        private Mock<IHttpClient> _client;
        private Mock<IHttpRequestHeaders> _headers;

        private Giraffe _requestObject;
        private string _requestJson;
        private Mock<IHttpContent> _requestContent;

        private Task<IHttpResponseMessage> _responseMessageTask;
        private Mock<IHttpResponseMessage> _responseMessage;
        private Mock<IHttpContent> _responseContent;

        private Giraffe _responseObject;
        private Task<string> _responseJsonTask;
        private string _responseJson;

        private Task<IHttpResponseMessage> _deleteResponseMessageTask;
        private Mock<IHttpResponseMessage> _deleteResponseMessage;
        private Mock<IHttpContent> _deleteResponseContent;

        private CountResponse _deleteResponseObject;
        private Task<string> _deleteResponseJsonTask;
        private string _deleteResponseJson;

        private Task<IHttpResponseMessage> _errorResponseMessageTask;
        private Mock<IHttpResponseMessage> _errorResponseMessage;
        private Mock<IHttpContent> _errorResponseContent;

        private KinveyError _errorResponseObject;
        private Task<string> _errorResponseJsonTask;
        private string _errorResponseJson;

        private Task<IHttpResponseMessage> _queryResponseMessageTask;
        private Mock<IHttpResponseMessage> _queryResponseMessage;
        private Mock<IHttpContent> _queryResponseContent;

        private List<Giraffe> _queryResponseObject;
        private string _queryResponseJson;
        private Task<string> _queryResponseJsonTask;

        private Task<IHttpResponseMessage> _countResponseMessageTask;
        private Mock<IHttpResponseMessage> _countResponseMessage;
        private Mock<IHttpContent> _countResponseContent;

        private CountResponse _countResponseObject;
        private string _countResponseJson;
        private Task<string> _countResponseJsonTask;

        #endregion

        #region Set Up

        [SetUp]
        public void SetUp()
        {
            _factory = new Mock<IFactory>();
            _client = new Mock<IHttpClient>();
            _headers = new Mock<IHttpRequestHeaders>();

            _queryResponseMessageTask = new Task<IHttpResponseMessage>(() => _queryResponseMessage.Object);
            _queryResponseMessage = new Mock<IHttpResponseMessage>();
            _queryResponseContent = new Mock<IHttpContent>();

            _queryResponseObject = new List<Giraffe>
                {
                    new Giraffe{Id="1", Name = "steve"},
                    new Giraffe{Id="2", Name = "steve"},
                    new Giraffe{Id="3", Name = "steve"},
                    new Giraffe{Id="4", Name = "steve"}
                };
            _queryResponseJson = JsonConvert.SerializeObject(_queryResponseObject);
            _queryResponseJsonTask = new Task<string>(() => _queryResponseJson);

            _countResponseMessageTask = new Task<IHttpResponseMessage>(() => _countResponseMessage.Object);
            _countResponseMessage = new Mock<IHttpResponseMessage>();
            _countResponseContent = new Mock<IHttpContent>();

            _countResponseObject = new CountResponse { Count = 8 };
            _countResponseJson = JsonConvert.SerializeObject(_countResponseObject);
            _countResponseJsonTask = new Task<string>(() => _countResponseJson);

            _deleteResponseMessageTask = new Task<IHttpResponseMessage>(() => _deleteResponseMessage.Object);
            _deleteResponseMessage = new Mock<IHttpResponseMessage>();
            _deleteResponseContent = new Mock<IHttpContent>();

            _deleteResponseObject = new CountResponse { Count = 21 };
            _deleteResponseJson = JsonConvert.SerializeObject(_deleteResponseObject);
            _deleteResponseJsonTask = new Task<string>(() => _deleteResponseJson);

            _responseMessageTask = new Task<IHttpResponseMessage>(() => _responseMessage.Object);
            _responseMessage = new Mock<IHttpResponseMessage>();
            _responseContent = new Mock<IHttpContent>();

            _responseObject = new Giraffe{ Id = "penguin"};
            _responseJson = JsonConvert.SerializeObject(_responseObject);
            _responseJsonTask = new Task<string>(() => _responseJson);

            _errorResponseMessageTask = new Task<IHttpResponseMessage>(() => _errorResponseMessage.Object);
            _errorResponseMessage = new Mock<IHttpResponseMessage>();
            _errorResponseContent = new Mock<IHttpContent>();

            _errorResponseObject = new KinveyError { Error = "turtle", Description = "meerkat"};
            _errorResponseJson = JsonConvert.SerializeObject(_errorResponseObject);
            _errorResponseJsonTask = new Task<string>(() => _errorResponseJson);

            _requestObject = new Giraffe{ Id = "tiger" };
            _requestJson = JsonConvert.SerializeObject(_requestObject);
            _requestContent = new Mock<IHttpContent>();

            _factory.Setup(f => f.Get<IHttpClient>())
                    .Returns(_client.Object);
            _factory.Setup(f => f.Get<IHttpContent>(_requestJson))
                    .Returns(_requestContent.Object);

            _client.SetupGet(c => c.DefaultRequestHeaders)
                   .Returns(_headers.Object);

            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                       {
                           if (uri.ToString().Contains("_count"))
                               _countResponseMessageTask.Start();
                           if (uri.ToString().Contains("?query"))
                               _queryResponseMessageTask.Start();
                           else
                            _responseMessageTask.Start();
                       })
                   .Returns((Uri uri) =>
                       {
                           if (uri.ToString().Contains("_count")) return _countResponseMessageTask;
                           if (uri.ToString().Contains("?query")) return _queryResponseMessageTask;
                           else return _responseMessageTask;
                       });
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_deleteResponseMessageTask.Start)
                   .Returns(_deleteResponseMessageTask);
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

            _deleteResponseMessage.SetupGet(r => r.Content)
                                  .Returns(_deleteResponseContent.Object);
            _deleteResponseContent.Setup(r => r.ReadAsStringAsync())
                                  .Callback(_deleteResponseJsonTask.Start)
                                  .Returns(_deleteResponseJsonTask);

            _queryResponseMessage.SetupGet(r => r.Content)
                                  .Returns(_queryResponseContent.Object);
            _queryResponseContent.Setup(r => r.ReadAsStringAsync())
                                  .Callback(_queryResponseJsonTask.Start)
                                  .Returns(_queryResponseJsonTask);

            _countResponseMessage.SetupGet(r => r.Content)
                                  .Returns(_countResponseContent.Object);
            _countResponseContent.Setup(r => r.ReadAsStringAsync())
                                  .Callback(_countResponseJsonTask.Start)
                                  .Returns(_countResponseJsonTask);
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
            KinveySettings.Get().UserAuthToken = "goat";
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
        [ExpectedException(typeof (ArgumentException))]
        public void UnmarkedClass()
        {
            new KinveyService<UnmarkedGiraffe>(_factory.Object);
        }

        [Test]
        public void AuthorizationHeaderSet()
        {
            new KinveyService<Giraffe>(_factory.Object);
            _headers.Verify(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void AuthorizationHeaderContent()
        {
            _headers.Setup(h => h.Add("Authorization", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "Kinvey goat" }, value));

            new KinveyService<Giraffe>(_factory.Object);
        }

        [Test]
        public void AcceptHeaderContent()
        {
            _headers.Setup(h => h.Add("Accept", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "application/json" }, value));

            new KinveyService<Giraffe>(_factory.Object);
        }

        [Test]
        public void ApiVersionHeaderSet()
        {
            new KinveyService<Giraffe>(_factory.Object);
            _headers.Verify(h => h.Add("X-Kinvey-Api-Version", Moq.It.IsAny<IEnumerable<string>>()), Times.Once());
        }

        [Test]
        public void ApiVersionHeaderContent()
        {
            _headers.Setup(h => h.Add("X-Kinvey-Api-Version", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "2" }, value));

            new KinveyService<Giraffe>(_factory.Object);
        }

        [Test]
        public void HostHeaderContent()
        {
            _headers.Setup(h => h.Add("Host", Moq.It.IsAny<IEnumerable<string>>()))
                    .Callback((string key, IEnumerable<string> value) => CollectionAssert.AreEqual(new[] { "baas.kinvey.com" }, value));

            new KinveyService<Giraffe>(_factory.Object);
        }

        [Test]
        public void BaseAddressSet()
        {
            new KinveyService<Giraffe>(_factory.Object);
            _client.VerifySet(c => c.BaseAddress = Moq.It.IsAny<Uri>(), Times.Once());
        }

        [Test]
        public void BaseAddressContent()
        {
            _client.SetupSet(c => c.BaseAddress = Moq.It.IsAny<Uri>())
                   .Callback((Uri uri) => Assert.AreEqual("https://baas.kinvey.com/", uri.ToString()))
                   .Verifiable();

            new KinveyService<Giraffe>(_factory.Object);
        }

        // ReSharper restore ObjectCreationAsStatement
        #endregion

        #region Create Tests

        [Test]
        public async void CreatePostViaClientWithContent()
        {
            await new KinveyService<Giraffe>(_factory.Object).Create(_requestObject);
            _client.Verify(c => c.PostAsync(Moq.It.IsAny<Uri>(), _requestContent.Object), Times.Once());
        }

        [Test]
        public async void CreateUriString()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Create(_requestObject);
        }

        [Test]
        public async void CreateUriType()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Create(_requestObject);
        }

        [Test]
        public async void CreateReturnValue()
        {
            var result = await new KinveyService<Giraffe>(_factory.Object).Create(_requestObject);
            Assert.AreEqual(_responseObject, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void CreateError()
        {
            _client.Setup(c => c.PostAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Create(_requestObject);
        }

        #endregion

        #region Read (By Id) Tests

        [Test]
        public async void ReadByIdGetViaClient()
        {
            await new KinveyService<Giraffe>(_factory.Object).Read("penguin");
            _client.Verify(c => c.GetAsync(It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void ReadByIdUriString()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                       {
                           _responseMessageTask.Start();
                           Assert.AreEqual("/appdata/badger/giraffe/penguin/", uri.ToString());
                       })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read("penguin");
        }

        [Test]
        public async void ReadByIdUriType()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read("penguin");
        }

        [Test]
        public async void ReadByIdReturnValue()
        {
            var result = await new KinveyService<Giraffe>(_factory.Object).Read("penguin");
            Assert.AreEqual(_responseObject, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void ReadByIdError()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read("penguin");
        }

        #endregion

        #region Read (By Query) Tests

        [Test]
        public async void ReadByQueryGetViaClient()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"steve\"}");
            await new KinveyService<Giraffe>(_factory.Object).Read(query.Object);
            _client.Verify(c => c.GetAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void ReadByQueryUriString()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"steve\"}");
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _queryResponseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/?query={\"name\":\"steve\"}", uri.ToString());
                   })
                   .Returns(_queryResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read(query.Object);
        }

        [Test]
        public async void ReadByQueryUriType()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"steve\"}");
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _queryResponseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_queryResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read(query.Object);
        }

        [Test]
        public async void ReadByQueryReturnValue()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"steve\"}");
            var result = await new KinveyService<Giraffe>(_factory.Object).Read(query.Object);
            CollectionAssert.AreEqual(_queryResponseObject, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void ReadByQueryError()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"steve\"}");
            _client.Setup(c => c.GetAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Read(query.Object);
        }

        #endregion

        #region Update Tests

        [Test]
        public async void UpdatePutViaClientWithContent()
        {
            await new KinveyService<Giraffe>(_factory.Object).Update(_requestObject);
            _client.Verify(c => c.PutAsync(Moq.It.IsAny<Uri>(), _requestContent.Object), Times.Once());
        }

        [Test]
        public async void UpdateUriString()
        {
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback((Uri uri, IHttpContent content) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/tiger", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Update(_requestObject);
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
            await new KinveyService<Giraffe>(_factory.Object).Update(_requestObject);
        }

        [Test]
        public async void UpdateReturnValue()
        {
            var result = await new KinveyService<Giraffe>(_factory.Object).Update(_requestObject);
            Assert.AreEqual(_responseObject, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void UpdateError()
        {
            _client.Setup(c => c.PutAsync(Moq.It.IsAny<Uri>(), Moq.It.IsAny<IHttpContent>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Update(_requestObject);
        }

        #endregion

        #region Delete (By Object) Tests

        [Test]
        public async void DeleteByObjectDeleteViaClient()
        {
            await new KinveyService<Giraffe>(_factory.Object).Delete(_requestObject);
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void DeleteByObjectUriString()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _deleteResponseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/?query={\"_id\": \"tiger\"}", uri.ToString());
                   })
                   .Returns(_deleteResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(_requestObject);
        }

        [Test]
        public async void DeleteByObjectUriType()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _deleteResponseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_deleteResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(_requestObject);
        }

        [Test]
        public async void DeleteByObjectReturn()
        {
            var result = await new KinveyService<Giraffe>(_factory.Object).Delete(_requestObject);
            Assert.AreEqual(21, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void DeleteByObjectError()
        {
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(_requestObject);
        }

        #endregion

        #region Delete (By Query) Tests

        [Test]
        public async void DeleteByQueryDeleteViaClient()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"badger\"}&limit=20&skip=10");
            await new KinveyService<Giraffe>(_factory.Object).Delete(query.Object);
            _client.Verify(c => c.DeleteAsync(Moq.It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void DeleteByQueryUriString()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"badger\"}&limit=20&skip=10");
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _deleteResponseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/?query={\"name\":\"badger\"}&limit=20&skip=10", uri.ToString());
                   })
                   .Returns(_deleteResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(query.Object);
        }

        [Test]
        public async void DeleteByQueryUriType()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"badger\"}&limit=20&skip=10");
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _deleteResponseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_deleteResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(query.Object);
        }

        [Test]
        public async void DeleteByQueryReturn()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"badger\"}&limit=20&skip=10");
            var result = await new KinveyService<Giraffe>(_factory.Object).Delete(query.Object);
            Assert.AreEqual(21, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void DeleteByQueryError()
        {
            var query = new Mock<KinveyQuery<Giraffe>>();
            query.Setup(q => q.ToString())
                 .Returns("?query={\"name\":\"badger\"}&limit=20&skip=10");
            _client.Setup(c => c.DeleteAsync(Moq.It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Delete(query.Object);
        }

        #endregion

        #region Count Tests

        [Test]
        public async void CountGetViaClient()
        {
            await new KinveyService<Giraffe>(_factory.Object).Count();
            _client.Verify(c => c.GetAsync(It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void CountUriString()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/_count", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Count();
        }

        [Test]
        public async void CountUriType()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Count();
        }

        [Test]
        public async void CountReturnValue()
        {
            var result = await new KinveyService<Giraffe>(_factory.Object).Count();
            Assert.AreEqual(8, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void CountError()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            await new KinveyService<Giraffe>(_factory.Object).Count();
        }

        #endregion

        #region Count (By Query) Tests

        [Test]
        public async void CountQueryGetViaClient()
        {
            var query = new KinveyQuery<Giraffe>().Limit(10)
                                                  .Skip(10)
                                                  .Constrain(g => g.Age, Is.GreaterThan(3).LessThan(6));
            await new KinveyService<Giraffe>(_factory.Object).Count(query);
            
            _client.Verify(c => c.GetAsync(It.IsAny<Uri>()), Times.Once());
        }

        [Test]
        public async void CountQueryUriString()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.AreEqual("/appdata/badger/giraffe/_count/?query={\"age\":{\"$gt\":3,\"$lt\":6}}&limit=10&skip=10", uri.ToString());
                   })
                   .Returns(_responseMessageTask);
            var query = new KinveyQuery<Giraffe>().Limit(10)
                                                  .Skip(10)
                                                  .Constrain(g => g.Age, Is.GreaterThan(3).LessThan(6));
            await new KinveyService<Giraffe>(_factory.Object).Count(query);
        }

        [Test]
        public async void CountQueryUriType()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback((Uri uri) =>
                   {
                       _responseMessageTask.Start();
                       Assert.False(uri.IsAbsoluteUri);
                   })
                   .Returns(_responseMessageTask);
            var query = new KinveyQuery<Giraffe>().Limit(10)
                                                  .Skip(10)
                                                  .Constrain(g => g.Age, Is.GreaterThan(3).LessThan(6));
            await new KinveyService<Giraffe>(_factory.Object).Count(query);
        }

        [Test]
        public async void CountQueryReturnValue()
        {
            var query = new KinveyQuery<Giraffe>().Limit(10)
                                                  .Skip(10)
                                                  .Constrain(g => g.Age, Is.GreaterThan(3).LessThan(6));
            var result = await new KinveyService<Giraffe>(_factory.Object).Count(query);
            Assert.AreEqual(8, result);
        }

        [Test]
        [ExpectedException(typeof(KinveyException), ExpectedMessage = "meerkat")]
        public async void CountQueryError()
        {
            _client.Setup(c => c.GetAsync(It.IsAny<Uri>()))
                   .Callback(_errorResponseMessageTask.Start)
                   .Returns(_errorResponseMessageTask);
            var query = new KinveyQuery<Giraffe>().Limit(10)
                                                  .Skip(10)
                                                  .Constrain(g => g.Age, Is.GreaterThan(3).LessThan(6));
            await new KinveyService<Giraffe>(_factory.Object).Count(query);
        }

        #endregion
    }
}
