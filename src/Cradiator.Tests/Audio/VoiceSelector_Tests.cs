using System.Collections.Generic;
using Cradiator.Audio;
using FakeItEasy;
using NUnit.Framework;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class VoiceSelector_Tests
	{
		const string CepstralWilliam = "Cepstral William";

		ISpeechSynthesizer _speechSynth;
		VoiceSelector _voiceSelector;

		[SetUp]
		public void SetUp()
		{
			_speechSynth = A.Fake<ISpeechSynthesizer>();
			_voiceSelector = new VoiceSelector(_speechSynth);
		}

		[Test]
		public void CanNotSet_SpeechVoiceName_IfNameNotIn_InstalledVoices()
		{
			A.CallTo(() => _speechSynth.SelectedVoice).Returns(new CradiatorInstalledVoice("Bob"));
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice("Bob"),
			});

			_voiceSelector.SelectInstalledVoice("DodgyVoiceName");
			A.CallTo(() => _speechSynth.SelectVoice(A<string>.That.IsEqualTo("Bob"))).MustHaveHappened();
		}

		[Test]
		public void CanSet_SpeechVoiceName_From_2_InstalledVoices()
		{
			const string Bob = "Bob";
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice(Bob),
				new CradiatorInstalledVoice("Mary")
			});
			
			_voiceSelector.SelectInstalledVoice(Bob);
			A.CallTo(() => _speechSynth.SelectVoice(A<string>.That.IsEqualTo("Bob"))).MustHaveHappened();
		}

		[Test]
		public void CanSet_SpeechVoiceName_FromMany_InstalledVoices()
		{
			const string Terry = "Terry";
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice("Bob"),
				new CradiatorInstalledVoice("Mary"),
				new CradiatorInstalledVoice("Mike"),
				new CradiatorInstalledVoice(Terry),
				new CradiatorInstalledVoice("Dexter")
			});

			_voiceSelector.SelectInstalledVoice(Terry);
			A.CallTo(() => _speechSynth.SelectVoice(A<string>.That.IsEqualTo(Terry))).MustHaveHappened();
		}

		/// <summary>
		/// we don't want to have to get the name exactly right ("william" is good enough to get "Cepstral William")
		/// if it's ever in doubt, then it's up to the user to specify the voice name exactly - or get the 'first one' that matches
		/// </summary>
		[Test]
		public void CanSet_SpeechVoiceName_IfTextContainsVoiceName_IeIsNotExactMatch()
		{
			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice(CepstralWilliam),
			});

			_voiceSelector.SelectInstalledVoice("william");
			A.CallTo(() => _speechSynth.SelectVoice(A<string>.That.IsEqualTo(CepstralWilliam))).MustHaveHappened();
		}

		[Test]
		public void CanChoose_FirstOrderedVoiceName_If_MoreThan_1_Voice_Matches()
		{
			const string AppleWilliam = "Apple William ";

			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice("Microsoft William"),
				new CradiatorInstalledVoice(AppleWilliam),
				new CradiatorInstalledVoice(CepstralWilliam),
			});

			_voiceSelector.SelectInstalledVoice("william");
			A.CallTo(() => _speechSynth.SelectVoice(A<string>.That.IsEqualTo(AppleWilliam))).MustHaveHappened();
		}

		[Test]
		public void CanGetInstalledVoice_WithoutSelecting()
		{
			A.CallTo(() => _speechSynth.SelectedVoice).Returns(new CradiatorInstalledVoice("Bob"));

			A.CallTo(() => _speechSynth.GetInstalledVoices()).Returns(new List<CradiatorInstalledVoice>
			{
				new CradiatorInstalledVoice("Bob"),
			});

			Assert.That(_voiceSelector.GetClosestMatchingInstalledVoice("DodgyVoiceName"),
				Is.EqualTo(new CradiatorInstalledVoice("Bob")));

			A.CallTo(() => _speechSynth.SelectVoice(A<string>._)).MustNotHaveHappened();
		}

		[Test]
		public void CanReturnSelectedVoiceIfNullOrEmpty()
		{
			A.CallTo(() => _speechSynth.SelectedVoice).Returns(new CradiatorInstalledVoice("Bob"));
			Assert.That(_voiceSelector.GetClosestMatchingInstalledVoice(null).Name, Is.EqualTo("Bob"));
		}
	}
}