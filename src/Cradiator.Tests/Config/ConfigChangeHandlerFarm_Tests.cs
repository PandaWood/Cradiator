using System.Collections.Generic;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class ConfigChangeHandlerFarm_Tests
	{
		IConfigChangeHandler _updater;
		ConfigSettings _configSettings;

		[SetUp]
		public void SetUp()
		{
			_updater = A.Fake<IConfigChangeHandler>();
			_configSettings = new ConfigSettings();
		}

		[Test]
		public void CanUpdate_1_Updater()
		{
			var configUpdater = new ConfigChangeHandlerFarm(new List<IConfigChangeHandler> { _updater });
			configUpdater.UpdateAll(_configSettings);

			A.CallTo(() => _updater.ConfigUpdated(_configSettings)).MustHaveHappened();
		}

		[Test]
		public void CanUpdate_2_Updaters()
		{
			var updater2 = A.Fake<IConfigChangeHandler>();

			var configUpdater = new ConfigChangeHandlerFarm(new List<IConfigChangeHandler> {_updater, updater2});
			configUpdater.UpdateAll(_configSettings);

			A.CallTo(() => _updater.ConfigUpdated(_configSettings)).MustHaveHappened();
			A.CallTo(() => updater2.ConfigUpdated(_configSettings)).MustHaveHappened();
		}
	}
}