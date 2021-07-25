using System;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using Cradiator.Views;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class ShowCountdownChangeHandler_Tests
	{
		ICradiatorView _view;
		ICountdownTimer _timer;
		IConfigChangeHandler _changeHandler;

		[SetUp]
		public void SetUp()
		{
			_view = A.Fake<ICradiatorView>();
			_timer = A.Fake<ICountdownTimer>();
			_changeHandler = new ShowCountdownChangeHandler(_timer, _view);
		}

		[Test]
		public void DoesNothing_If_ShowCountdown_IsFalse_And_IsSwitchedOff()
		{
			_changeHandler.ConfigUpdated(new ConfigSettings {ShowCountdown = false});
			A.CallTo(() => _timer.SwitchOff()).MustNotHaveHappened();
			A.CallTo(() => _timer.SwitchOn()).MustNotHaveHappened();
			A.CallTo(() => _view.Invoke(A<Action>._)).MustNotHaveHappened();
		}

		[Test]
		public void IsSwitchedOn_If_ShowCountdownTrue_And_WasSwitchedOff()
		{
			A.CallTo(() => _timer.IsSwitchedOn).Returns(false);

			_changeHandler.ConfigUpdated(new ConfigSettings {ShowCountdown = true});

			A.CallTo(() => _timer.SwitchOn()).MustHaveHappened();
			A.CallTo(() => _view.Invoke(A<Action>._)).MustHaveHappened();
		}

		[Test]
		public void IsSwitchedOff_If_ShowCountdownFalse_And_WasSwitchedOn()
		{
			A.CallTo(() => _timer.IsSwitchedOn).Returns(true);

			_changeHandler.ConfigUpdated(new ConfigSettings {ShowCountdown = false});

			A.CallTo(() => _timer.SwitchOff()).MustHaveHappened();
			A.CallTo(() => _view.Invoke(A<Action>._)).MustHaveHappened();
		}
	}
}