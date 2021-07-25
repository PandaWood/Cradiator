using System.Collections.Generic;
using Cradiator.Audio;
using Cradiator.Config;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class DiscJockey_Tests
	{
		IAudioPlayer _audioPlayer;
		DiscJockey _discJockey;
		ConfigSettings _configSettings;
		List<ProjectStatus> _projectList1Success;
		List<ProjectStatus> _projectList1Failure;
		List<ProjectStatus> _projectList2BothSuccess;
		List<ProjectStatus> _projectList2BothFailure;
		SpeechMaker _speechMaker;
		IBuildBuster _buildBuster;

		const string BrokenSound = "broken.wav";
		const string FixedSound = "fixed.wav";

		[SetUp]
		public void SetUp()
		{
			_audioPlayer = A.Fake<IAudioPlayer>();
			_buildBuster = A.Fake<IBuildBuster>();

			_configSettings = new ConfigSettings
			{
				BrokenBuildSound = BrokenSound,
				FixedBuildSound = FixedSound,
			    PlaySounds = true
			};

			_speechMaker = new SpeechMaker(_configSettings, new SpeechTextParser(_buildBuster));

			_discJockey = new DiscJockey(_configSettings, _audioPlayer, _speechMaker);
			_projectList1Success = new List<ProjectStatus>
			{
				new ProjectStatus("bla")
				{
					LastBuildStatus = ProjectStatus.SUCCESS
				}
			};
			_projectList1Failure = new List<ProjectStatus>
			{
				new ProjectStatus("bla")
				{
					LastBuildStatus = ProjectStatus.FAILURE
				}
			};

			_projectList2BothSuccess = new List<ProjectStatus>
			{
				new ProjectStatus("bla") {LastBuildStatus = ProjectStatus.SUCCESS},
				new ProjectStatus("bla2") {LastBuildStatus = ProjectStatus.SUCCESS}
			};

			_projectList2BothFailure = new List<ProjectStatus>
			{
				new ProjectStatus("bla") {LastBuildStatus = ProjectStatus.FAILURE},
				new ProjectStatus("bla2") {LastBuildStatus = ProjectStatus.FAILURE}
			};
		}

		[Test]
		public void CanPlay_BuildStatus_If_SuccessThenFailure()
		{
			_discJockey.PlaySounds(_projectList1Success);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();
			
			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustHaveHappened();
		}

		[Test]
		public void DoesNotPlay_BuildStatus_If_2_FailuresInARow()
		{
			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();

			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();
		}

		[Test]
		public void CanPlay_BuildStatus_If_SuccessThenFail_WithMultipleStatus()
		{
			_discJockey.PlaySounds(_projectList2BothSuccess);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();
		
			_discJockey.PlaySounds(_projectList2BothFailure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustHaveHappened();

		}

		[Test]
		public void DoesNotPlay_BuildStatus_If_OnlyFailures_WithMultipleStatus()
		{
			_discJockey.PlaySounds(_projectList2BothFailure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();

			_discJockey.PlaySounds(_projectList2BothFailure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();
		}

		[Test]
		public void CanPlaySound_ThenUpdateConfig_ThenNotPlaySound()
		{
			_discJockey = new DiscJockey(new ConfigSettings { PlaySounds = false }, _audioPlayer, _speechMaker);

			_discJockey.PlaySounds(_projectList1Success);
			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustNotHaveHappened();

			_discJockey.ConfigUpdated(new ConfigSettings { PlaySounds = true, BrokenBuildSound = BrokenSound});

			_discJockey.PlaySounds(_projectList1Success);
			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(BrokenSound)).MustHaveHappened();
		}

		[Test]
		public void CanPlay_Success_If_FailureThenSuccess()
		{
			_discJockey.PlaySounds(_projectList1Failure);
			A.CallTo(() => _audioPlayer.Play(FixedSound)).MustNotHaveHappened();

			_discJockey.PlaySounds(_projectList1Success);
			A.CallTo(() => _audioPlayer.Play(FixedSound)).MustHaveHappened();

		}

		[Test]
		public void DoesNotPlay_Success_If_2_SuccessesInARow()
		{
			_discJockey.PlaySounds(_projectList1Success);
			A.CallTo(() => _audioPlayer.Play(FixedSound)).MustNotHaveHappened();


			_discJockey.PlaySounds(_projectList1Success);
			A.CallTo(() => _audioPlayer.Play(FixedSound)).MustNotHaveHappened();
		}
	}
}