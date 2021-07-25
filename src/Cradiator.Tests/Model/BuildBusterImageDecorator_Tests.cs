using Cradiator.Config;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class BuildBusterImageDecorator_Tests
	{
		BuildBusterImageDecorator _buildBusterDecorator;
		IBuildBuster _buildBuster;
		IAppLocation _appLocation;
		const string DirectoryName = @"c:\dummy\DirectoryName\file\system\is\not\used";

		[SetUp]
		public void SetUp()
		{
			_buildBuster = A.Fake<IBuildBuster>();
			_appLocation = A.Fake<IAppLocation>();

			// it is very important that we don't rely on (and therefore retest) the 'decorated' BuildBuster object
			// If we did, then that class failing would fail this test too (a classic case of fickle (and therefore bad) tests
			A.CallTo(() => _appLocation.DirectoryName).Returns(DirectoryName);
			_buildBusterDecorator = new BuildBusterImageDecorator(_buildBuster, _appLocation);
		}

		[Test]
		public void CanDecorate_WithImageExtension()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns("bob");

			// we want to test the decoration of the string 'bob' - nothing else
			var breaker = _buildBusterDecorator.FindBreaker("don't care - the internal BuildBuster is stubbed to return 'bob'");

			breaker.ShouldBe(DirectoryName + @"\images\bob.jpg");
		}

		[Test]
		public void CanReplace_Slash_InFilename()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns(@"b\smith");

			var breaker = _buildBusterDecorator.FindBreaker("dontcare");

			breaker.ShouldBe(DirectoryName + @"\images\bsmith.jpg");
		}

		[Test]
		public void CanReplace_AnyInvalidFilenameChar()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns(@"b*?<>""smith");

			var breaker = _buildBusterDecorator.FindBreaker("dontcare");

			breaker.ShouldBe(DirectoryName + @"\images\bsmith.jpg");
		}
	}
}