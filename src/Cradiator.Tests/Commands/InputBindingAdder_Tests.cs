using System.Windows.Input;
using Cradiator.Commands;
using Cradiator.Views;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Commands
{
	[TestFixture]
	public class InputBindingAdder_Tests
	{
		ICradiatorView _view;
		ISettingsWindow _settingsWindow;

		[SetUp]
		public void SetUp()
		{
			_view = A.Fake<ICradiatorView>();
			_settingsWindow = A.Fake<ISettingsWindow>();
		}

		[Test]
		public void CanAddBindings()
		{
			var bindingAdder = new InputBindingAdder(_view,
				new CommandContainer(new FullscreenCommand(_view),
					new RefreshCommand(_view),
					new ShowSettingsCommand(_view, _settingsWindow)));

			bindingAdder.AddBindings();

			A.CallTo(() => _view.AddWindowBinding(A<InputBinding>._)).MustHaveHappened();
		}
	}
}