using System;
using System.Linq;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Services;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class BuildDataFetcher_Tests
	{
		IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		[SetUp]
		public void SetUp()
		{
			_webClientFactory = A.Fake<IWebClientFactory>();
			_webClient = A.Fake<IWebClient>();
			A.CallTo(() => _webClientFactory.GetWebClient(A<string>._)).Returns(_webClient);
		}

		[Test]
		public void CanFetch()
		{
			const string Hello = "hello";

			A.CallTo(() => _webClient.DownloadString(A<string>._)).Returns(Hello);

			var fetcher = new BuildDataFetcher(new ViewUrl("http://test"), new ConfigSettings(), _webClientFactory);
			var fetchValue = fetcher.Fetch();

			fetchValue.First().ShouldBe(Hello);
		}

		[Test]
		public void CanUpdateSettings()
		{
			var fetcher = new BuildDataFetcher(new ViewUrl("http://bla"), 
				new ConfigSettings
				{
					URL = "http://bla"
				}, _webClientFactory);

			fetcher.Fetch();
			A.CallTo(() => _webClient.DownloadString(A<string>.That.IsEqualTo("http://bla"))).MustHaveHappened(1, Times.Exactly);

			fetcher.ConfigUpdated(new ConfigSettings { URL = "http://new"});
			fetcher.Fetch();

			A.CallTo(() => _webClient.DownloadString(A<string>.That.IsEqualTo("http://new"))).MustHaveHappened(1, Times.Exactly);
		}

		[Test]
		public void can_fetch_multiple_urls()
		{
			A.CallTo(() => _webClient.DownloadString(A<string>._)).Returns("url2").NumberOfTimes(1);
			A.CallTo(() => _webClient.DownloadString(A<string>._)).Returns("url1").NumberOfTimes(1);

			var fetcher = new BuildDataFetcher(new ViewUrl("http://url1 http://url2"), 
				new ConfigSettings(), _webClientFactory);

			var xmlResults = fetcher.Fetch().ToList();

			xmlResults.Count.ShouldBe(2);
			xmlResults[0].ShouldBe("url1");
			xmlResults[1].ShouldBe("url2");

			A.CallTo(() => _webClient.DownloadString(A<string>.That.IsEqualTo("http://url1"))).MustHaveHappened();
			A.CallTo(() => _webClient.DownloadString(A<string>.That.IsEqualTo("http://url2"))).MustHaveHappened(); 
		}
	}
}