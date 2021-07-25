using Cradiator.App;
using Cradiator.Config;
using Cradiator.Views;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Services
{
	[TestFixture]
	public class CruiseNinjaModule_Tests
	{
		[Test, Ignore("No longer valid because config.Load must be called earlier, reconsider test")]
		public void CanBoot()
		{
			var view = A.Fake<ICradiatorView>();
			var configSettings = A.Fake<IConfigSettings>();
			configSettings.ProjectNameRegEx = ".*";
			configSettings.CategoryRegEx = ".*";
			var boot = new Bootstrapper(configSettings, view);
			boot.CreateKernel();

			A.CallTo(() => configSettings.Load()).MustHaveHappened();
		}
	}
}