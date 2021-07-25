using System;
using Cradiator.App;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Views;
using FakeItEasy;
using Ninject;
using NUnit.Framework;

namespace Cradiator.Tests.Views
{
	[TestFixture]
	public class Presenter_Tests
	{
		ICradiatorView _view;
		IConfigSettings _configSettings;
		ISkinLoader _skinLoader;
		IKernel _kernel;
		IScreenUpdater _screenUpdater;
		IConfigFileWatcher _configFileWatcher;

		[SetUp]
		public void SetUp()
		{
			_view = A.Fake<ICradiatorView>();
			_configSettings = A.Fake<IConfigSettings>();

			A.CallTo(() => _configSettings.ProjectNameRegEx).Returns(".*");
			A.CallTo(() => _configSettings.CategoryRegEx).Returns(".*");

			_skinLoader = A.Fake<ISkinLoader>();
			_screenUpdater = A.Fake<IScreenUpdater>();
			_configFileWatcher = A.Fake<IConfigFileWatcher>();

			var bootstrapper = new Bootstrapper(_configSettings, _view);
			_kernel = bootstrapper.CreateKernel();
			_kernel.Rebind<ISkinLoader>().ToConstant(_skinLoader);
			_kernel.Rebind<IScreenUpdater>().ToConstant(_screenUpdater);
			_kernel.Rebind<IConfigFileWatcher>().ToConstant(_configFileWatcher);
		}

		[Test, Ignore("test takes too long")]
		[STAThread]
		public void CanCreatePresenter()
		{
			var presenter = _kernel.Get<CradiatorPresenter>();
			presenter.Init();

			A.CallTo(() => _configSettings.AddObserver(presenter)).MustHaveHappened();
			A.CallTo(() => _skinLoader.Load(A<Skin>._)).MustHaveHappened();
			A.CallTo(() => _screenUpdater.Update()).MustHaveHappened();
			A.CallTo(() => _configFileWatcher.Start()).MustHaveHappened();
		}
	}
}