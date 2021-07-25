using Cradiator.Audio;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class SpeechTextParser_Tests
	{
		IBuildBuster _buildBuster;
		SpeechTextParser _speechTextParser;
		ProjectStatus _failedProject;

		[SetUp]
		public void SetUp()
		{
			_buildBuster = A.Fake<IBuildBuster>();
			_speechTextParser = new SpeechTextParser(_buildBuster);

			_failedProject = new ProjectStatus("ProjectZombie")
			{
				LastBuildStatus = ProjectStatus.FAILURE,
			};
		}

		[Test]
		public void CanParseSentence_WithVariables_ProjectName_And_Breaker()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>.Ignored)).Returns("PandaWood");

			var parsedSentence = _speechTextParser.Parse(
				"$ProjectName$, is broken.$Breaker$, you're fired!", _failedProject);

			parsedSentence.ShouldBe("ProjectZombie, is broken.PandaWood, you're fired!");
		}

		[Test]
		public void CanParse_Sentence_With_NoVariables_ForBackwardsCompatibility_And_Minimalism()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>.Ignored)).Returns("PandaWood");

			var parsedSentence = _speechTextParser.Parse("is broken", _failedProject);

			parsedSentence.ShouldBe("ProjectZombie, is broken");
		}
	}
}