using System;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class PollIntervalChangeHandler_Tests
	{
		IPollTimer _pollTimer;
		ICountdownTimer _countdownChangeHandler;
		IConfigChangeHandler _pollingChangeHandler;

		[SetUp]
		public void SetUp()
		{
			_pollTimer = A.Fake<IPollTimer>();
			_countdownChangeHandler = A.Fake<ICountdownTimer>();
			_pollingChangeHandler = new PollIntervalChangeHandler(_pollTimer, _countdownChangeHandler);
		}

		[Test]
		public void DoesNotUpdate_If_IntervalAlreadyEqual()
		{
			A.CallTo(() => _pollTimer.Interval).Returns(TimeSpan.FromSeconds(5));

			_pollingChangeHandler.ConfigUpdated(new ConfigSettings {PollFrequency = 5});

			A.CallTo(() => _pollTimer.Stop()).MustNotHaveHappened();
			A.CallTo(() => _countdownChangeHandler.Reset()).MustNotHaveHappened();
			A.CallTo(() => _pollTimer.Start()).MustNotHaveHappened();
		}

		[Test]
		public void CanUpdate_If_IntervalNotEqual()
		{
			A.CallTo(() => _pollTimer.Interval).Returns(TimeSpan.FromSeconds(1));
			_pollTimer.Interval = TimeSpan.FromSeconds(2);
			_countdownChangeHandler.PollFrequency = TimeSpan.FromSeconds(2);

			_pollingChangeHandler.ConfigUpdated(new ConfigSettings {PollFrequency = 2});

			A.CallTo(() => _pollTimer.Stop()).MustHaveHappened();
			A.CallTo(() => _countdownChangeHandler.Reset()).MustHaveHappened();
			A.CallTo(() => _pollTimer.Start()).MustHaveHappened();
		}
	}
}