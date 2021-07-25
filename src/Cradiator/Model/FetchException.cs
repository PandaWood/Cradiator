using System;

namespace Cradiator.Model
{
	public class FetchException : Exception
	{
		private readonly string _url;

		public FetchException(string url, Exception exception)
			: base("Unable to contact " + url, exception)
		{
			_url = url;
		}

		public FetchException(string url)
			: base($"Invalid URL '{url}'")
		{
			_url = url;
		}

		public string Url => _url;
	}
}