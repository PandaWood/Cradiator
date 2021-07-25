using System;
using System.Text.RegularExpressions;
using Cradiator.Extensions;
using log4net;

namespace Cradiator.Model
{
	public class UrlParser
	{
		static readonly ILog _log = LogManager.GetLogger(nameof(UrlParser));
		private readonly string _url;

		public UrlParser(string url)
		{
			_url = url;
		}

		public string Url => _url;

		public bool IsValid
		{
			get
			{	
				var isValid = IsDebug ||
								Regex.IsMatch(_url, @"^((https?)://+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
								// just to prevent basic typos and misunderstandings
				if (!isValid) _log.WarnFormat("Skipping invalid URL: '{0}'", _url);
				return isValid;
			}
		}

		public bool IsNotValid => !IsValid;

		public bool IsDebug => _url.HasValue() && _url.ToLower().StartsWith("debug");
	}
}