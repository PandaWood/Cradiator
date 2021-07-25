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
	public class ImagePathConverter_Tests : ConverterTestBase
	{
		IBuildBuster _buildBuster;

		protected override IValueConverter CreateConverter()
		{
			_buildBuster = A.Fake<IBuildBuster>();
			return new ImagePathConverter(_buildBuster);
		}

		[Test]
		public void CanCall_BuildBuster_IfValue_IsNotNullOrEmpty()
		{
			const string currentMessage = "Breakers: Bob, Mary";

			DoConvert(currentMessage);

			// this is all we want to know - the fact that the BuildBuster "is called" (with the correct arguments)
			// (the BuildBuster is tested elsewhere - ie DRY)
			A.CallTo(() => _buildBuster.FindBreaker(A<string>.That.IsEqualTo(currentMessage))).MustHaveHappened();
		}

		[Test]
		public void CanConvertBackIsNotImplemented()
		{
			Assert.Throws<NotImplementedException>(
				() => new ImagePathConverter(_buildBuster).ConvertBack(null, null, null, null)
			);
		}
	}
}