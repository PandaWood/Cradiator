using System.Linq;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Services;
using Cradiator.Views;
using FakeItEasy;
using Ninject;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[Ignore("Integration tests")]
	[Category("Integration")]
	[TestFixture]
	public class BuildStatusFetcher_IntegrationTests
	{
		ICradiatorView _view;
		IConfigSettings _configSettings;
		IKernel _kernel;
		IWebClientFactory _factory;

		[SetUp]
		public void SetUp()
		{
			_view = A.Fake<ICradiatorView>();
			_configSettings = A.Fake<IConfigSettings>();
			A.CallTo(() => _configSettings.ProjectNameRegEx).Returns(".*");
			A.CallTo(() => _configSettings.CategoryRegEx).Returns(".*");
			A.CallTo(() => _configSettings.ServerNameRegEx).Returns(".*");

			_kernel = new StandardKernel(new CradiatorNinjaModule(_view, _configSettings));
			_factory = _kernel.Get<IWebClientFactory>();
			_kernel.Get<BuildDataTransformer>();
		}

		[Test]
		public void a_connectivity_exception_will_be_thrown_if_the_uri_isnt_resolveable()
		{
			Assert.Throws<FetchException>(() =>
			{
				var fetcher = new BuildDataFetcher(
					new ViewUrl("http://a.b.c.d.e.foo/ccnet/XmlStatusReport.aspx"), 
						_configSettings, _factory);
				fetcher.Fetch();
			}, "Unable to contact http://a.b.c.d.e.foo/ccnet/XmlStatusReport.aspx");
		}

		[Test, Ignore("this test is a little sensitive - and will fail if anything is going wrong at the thoughtworks URL")]
		public void can_get_projects_from_a_realaddress()
		{
			var fetcher = 
				new BuildDataFetcher(new ViewUrl("http://ccnetlive.thoughtworks.com/ccnet/XmlStatusReport.aspx"),
									 _configSettings, _factory);

			var fetch = fetcher.Fetch().ToList();
			fetch.Count.ShouldBeGreaterThan(0);
			fetch.ShouldContain(@"Project name=""CCNet""");
		}
	}
}