using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cradiator.Config;
using Cradiator.Services;

namespace Cradiator.Model
{
	public class BuildDataFetcher : IConfigObserver, IDisposable
	{
		readonly ViewUrl _viewUrl;
		readonly IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		public BuildDataFetcher(ViewUrl viewUrl, IConfigSettings configSettings,
								IWebClientFactory webClientFactory)
		{
			_viewUrl = viewUrl;
			_webClientFactory = webClientFactory;
			_webClient = webClientFactory.GetWebClient(configSettings.URL);
			configSettings.AddObserver(this);
		}

		public IEnumerable<string> Fetch()
		{
			return _viewUrl.UriList.Select(url =>
			{
				try
				{
					return _webClient.DownloadString(url);
				}
				catch (WebException webException)
				{
					throw new FetchException(url, webException);
				}
			}).ToList();
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_viewUrl.Url = newSettings.URL;

			// dispose the client being replaced, otherwise each config change
			// leaks the underlying WebClient
			var previousWebClient = _webClient;
			_webClient = _webClientFactory.GetWebClient(newSettings.URL);
			previousWebClient?.Dispose();
		}

		public void Dispose()
		{
			_webClient?.Dispose();
		}
	}
}