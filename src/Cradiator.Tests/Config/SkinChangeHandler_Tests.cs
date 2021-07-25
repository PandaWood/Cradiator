using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class SkinChangeHandler_Tests
	{
		IConfigChangeHandler _skinChangeHandler;
		ISkinLoader _skinLoader;

		[SetUp]
		public void SetUp()
		{
			_skinLoader = A.Fake<ISkinLoader>();
			_skinChangeHandler = new SkinChangeHandler(_skinLoader);
		}

		[Test]
		public void CanUpdateSkin_FirstTime_WhenCurrentSkinIsNull()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});
			A.CallTo(() => _skinLoader.Load(A<Skin>.That.Matches(s => s.Name == "newSkin"))).MustHaveHappened();
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull_2times()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});

			A.CallTo(() => _skinLoader.Load(A<Skin>.That.Matches(s => s.Name == "newSkin"))).MustHaveHappened(1, Times.Exactly);
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull_3times()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});

			A.CallTo(() => _skinLoader.Load(A<Skin>.That.Matches(s => s.Name == "newSkin"))).MustHaveHappened(1, Times.Exactly);
		}

		[Test]
		public void CanUpdateSkin_Whenever_NewSkin_IsDifferent()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newSkin"});

			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "newerSkin"});
			A.CallTo(() => _skinLoader.Load(A<Skin>.That.Matches(s => s.Name == "newerSkin"))).MustHaveHappened();

			_skinChangeHandler.ConfigUpdated(new ConfigSettings {SkinName = "yetNewerSkin"});
			A.CallTo(() => _skinLoader.Load(A<Skin>.That.Matches(s => s.Name == "yetNewerSkin"))).MustHaveHappened();
		}
	}
}