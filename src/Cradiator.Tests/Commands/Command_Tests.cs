using System.Windows;
using Cradiator.Commands;
using Cradiator.Views;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Commands
{
	[TestFixture]
	public class Command_Tests
	{
		ICradiatorView _view;

		[SetUp]
		public void SetUp()
		{
			_view = A.Fake<ICradiatorView>();
		}

		[Test]
		[RequiresSTA]
		public void GoesFullscreen_IfNotAlready_Fullscreen()
		{
			var window = new Window
			{
				WindowStyle = WindowStyle.SingleBorderWindow,
				Topmost = false,
				WindowState = WindowState.Normal
			};

			A.CallTo(() => _view.Window).Returns(window);

			var command = new FullscreenCommand(_view);
			command.Execute(null);

		  command.CanExecute(null).ShouldBe(true);
			window.WindowStyle.ShouldBe(WindowStyle.None);
			window.Topmost.ShouldBe(true);
			window.WindowState.ShouldBe(WindowState.Maximized);
		}

		[Test]
		[RequiresSTA]
		public void GoesNotFullscreen_WhenAlready_FullScreen()
		{
			var window = new Window
			{
				WindowStyle = WindowStyle.None,
				Topmost = true,
				WindowState = WindowState.Maximized
			};

			A.CallTo(() => _view.Window).Returns(window);

			var command = new FullscreenCommand(_view);
			command.Execute(null);

			window.WindowStyle.ShouldBe(WindowStyle.SingleBorderWindow);
			window.Topmost.ShouldBe(false);
			window.WindowState.ShouldBe(WindowState.Normal);
		}

		[Test]
		public void CanRefresh()
		{
			var command = new RefreshCommand(_view);
			command.Execute(null);

			command.CanExecute(null).ShouldBe(true);
			A.CallTo(() => _view.UpdateScreen()).MustHaveHappened();
		}

		[Test]
		public void CanShowSettingsCommand()
		{
			var settingsWindow = A.Fake<ISettingsWindow>();

			var command = new ShowSettingsCommand(_view, settingsWindow);
			command.Execute(null);

			command.CanExecute(null).ShouldBe(true);
			A.CallTo(() => settingsWindow.ShowDialog()).MustHaveHappened();
		}
	}
}