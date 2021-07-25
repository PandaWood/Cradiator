using System;
using System.Windows.Data;
using Cradiator.Converters;
using Cradiator.Model;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class OneBreakerConverter_Tests : ConverterTestBase
	{
		IBuildBuster _buildBuster;

		protected override IValueConverter CreateConverter()
		{
			_buildBuster = A.Fake<IBuildBuster>();
			return new OneBreakerConverter(_buildBuster);
		}

		[Test]
		public void CanConvert_If_CurrentMessage_Has_1_Breaker()
		{
			// NB we only test what is the responsibility of the OneBreakerConverter 
			// which is just formatting the results of IBuildBuster (which is already tested, DRY)

			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns("bob");
			DoConvert("irrelevant").ShouldBe("(bob)");
		}

		[Test]
		public void CanConvert_If_CurrentMessage_IsEmptyString()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns("");
			DoConvert("irrelevant").ShouldBe("");
		}

		[Test]
		public void CanConvert_If_CurrentMessage_IsNull()
		{
			A.CallTo(() => _buildBuster.FindBreaker(A<string>._)).Returns(null);
			DoConvert("irrelevant").ShouldBe("");
		}

		[Test]
		public void CanConvertBackIsNotImplemented()
		{
			var converter = new OneBreakerConverter(_buildBuster);
			
			Assert.Throws<NotImplementedException>(() => 
				converter.ConvertBack(null, null, null, null)
			);
		}
	}
}