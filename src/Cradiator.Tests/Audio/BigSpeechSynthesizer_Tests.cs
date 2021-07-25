using System.Linq;
using System.Speech.Synthesis;
using Cradiator.Audio;
using NUnit.Framework;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class BigSpeechSynthesizer_Tests
	{
		[Test]
		public void CanGetInstalledVoices()
		{
			var bigSynth = new CradiatorSpeechSynthesizer(new SpeechSynthesizer()) { Rate = -2 };
			Assert.That(bigSynth.GetInstalledVoices().Count(), Is.GreaterThanOrEqualTo(1));
			Assert.That(bigSynth.GetInstalledVoices().Count(v => v.Name.StartsWith("Microsoft")), Is.GreaterThanOrEqualTo(1));
		}
	}
}