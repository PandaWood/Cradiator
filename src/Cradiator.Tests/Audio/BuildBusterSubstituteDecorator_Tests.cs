using System.Collections.Generic;
using Cradiator.Config;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class BuildBusterSubstituteDecorator_Tests
	{
		BuildBusterFullNameDecorator _buildBusterDecorator;
		IBuildBuster _buildBuster;

		[SetUp]
		public void SetUp()
		{
			_buildBuster = A.Fake<IBuildBuster>();

			_buildBusterDecorator =
				new BuildBusterFullNameDecorator(_buildBuster, new ConfigSettings
				{
					UsernameMap = new Dictionary<string, string>
				    {
						{"jbloggs", "Joe Bloggs"},
						{"am", "Ace McAwesome"}
				    }}
				);
		}

		[Test]
		public void CanSubstitute_Username_1stEntry()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>.Ignored)).Returns("jbloggs");
			var breaker = _buildBusterDecorator.FindBreaker("ignored");

			breaker.ShouldBe("Joe Bloggs");
		}

		[Test]
		public void CanSubstitute_Username_2ndEntry()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>.Ignored)).Returns("am");
			var breaker = _buildBusterDecorator.FindBreaker("ignored");

			breaker.ShouldBe("Ace McAwesome");
		}
	}
}