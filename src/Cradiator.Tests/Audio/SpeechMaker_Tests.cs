using System.Collections.Generic;
using Cradiator.Audio;
using Cradiator.Config;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class SpeechMaker_Tests
	{
		const string ProjectFixedText = "is fixed";
		const string ProjectBrokenText = "is broken";

		List<ProjectStatus> _projectStatusList;
		SpeechMaker _speaker;
		ISpeechTextParser _speechTextParser;

		[SetUp]
		public void SetUp()
		{
			_speechTextParser = A.Fake<ISpeechTextParser>();

			_projectStatusList = new List<ProjectStatus>
			{
				new ProjectStatus("Project2")
				{
					LastBuildStatus = ProjectStatus.SUCCESS
				}
			};

			_speaker = new SpeechMaker(new ConfigSettings
			{
				FixedBuildText = ProjectFixedText,
				BrokenBuildText = ProjectBrokenText
			}, _speechTextParser);
		}

		[Test]
		public void CanMakeSpeech_ForBuildIsFixed()
		{
			A.CallTo(() => _speechTextParser.Parse(A<string>.Ignored, A<ProjectStatus>.Ignored)).Returns(ProjectFixedText);

			var speech = _speaker.BuildIsFixed(_projectStatusList);
			speech.ToXml().ShouldContain(ProjectFixedText);
		}

		[Test]
		public void CanMakeSpeech_ForBuildIsBroken()
		{
			A.CallTo(() => _speechTextParser.Parse(A<string>.Ignored, A<ProjectStatus>.Ignored)).Returns(ProjectBrokenText);

			var speech = _speaker.BuildIsBroken(_projectStatusList);
            speech.ToXml().ShouldContain(ProjectBrokenText);
		}

		[Test]
		public void CanUpdateSettings()
		{
			A.CallTo(() => _speechTextParser.Parse(ProjectFixedText, A<ProjectStatus>.Ignored)).Returns("dont care");

			_speaker.BuildIsFixed(_projectStatusList);

			A.CallTo(() => _speechTextParser.Parse("is different", A<ProjectStatus>.Ignored)).Returns("dont care");

			_speaker.ConfigUpdated(new ConfigSettings { FixedBuildText = "is different" });
			_speaker.BuildIsFixed(_projectStatusList);
		}
	}
}