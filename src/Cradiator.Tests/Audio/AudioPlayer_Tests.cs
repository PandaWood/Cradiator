using System.Collections.Generic;
using System.Speech.Synthesis;
using Cradiator.Audio;
using Cradiator.Config;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class AudioPlayer_Tests
	{
		ISpeechSynthesizer _speechSynth;
		IAppLocation _appLocation;

		[SetUp]
		public void SetUp()
		{
			_speechSynth = A.Fake<ISpeechSynthesizer>();
			_appLocation = A.Fake<IAppLocation>();
			A.CallTo(() => _appLocation.DirectoryName).Returns(@"c:\bla");
		}

		[Test]
		public void DoesNotSay_If_PlaySpeech_IsOff()
		{
			//arrange
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>());

			//act
			var audioPlayer = new AudioPlayer(_speechSynth,
											  new ConfigSettings { PlaySpeech = false },
											  new VoiceSelector(_speechSynth), _appLocation);
			audioPlayer.Say(new PromptBuilder());

			//assert
			A.CallTo(() => _speechSynth.SpeakAsync(A<PromptBuilder>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void DoesSay_If_PlaySpeech_IsOn()
		{
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice("Bob")
			});

			var audioPlayer = new AudioPlayer(_speechSynth,
											  new ConfigSettings { PlaySpeech = true, SpeechVoiceName = "Bob" },
											  new VoiceSelector(_speechSynth), _appLocation);
			audioPlayer.Say(new PromptBuilder());

			A.CallTo(() => _speechSynth.SpeakAsync(A<PromptBuilder>.Ignored)).MustHaveHappened();
		}

		[Test]
		public void CanPlay_WAV_File()
		{
			//TODO make this test not so whimpy
			Assert.DoesNotThrow(()=>new AudioPlayer(@"c:\windows\media"));
		}
	}
}