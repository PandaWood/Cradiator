using System;

namespace Cradiator.Services
{
	public interface IWebClient : IDisposable
	{
		string DownloadString(string url);
	}
}