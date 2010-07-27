using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using Cradiator.Extensions;
using log4net;

namespace Cradiator.Config
{
	/// <summary>
	/// configuration settings
	/// </summary>
	public class ConfigSettings : INotifyPropertyChanged, IConfigSettings
	{
		const int DefaultPollingFrequency = 30;

		const string PollFrequencyKey = "PollFrequency";
		const string UrlKey = "URL";
		const string SkinKey = "Skin";
		const string ProjectNameRegexKey = "ProjectNameRegEx";
		const string CategoryRegexKey = "CategoryRegEx";
		const string ShowCountdownKey = "ShowCountdown";
		const string ShowProgressKey = "ShowProgress";
		const string PlaySoundsKey = "PlaySounds";
		const string BrokenBuildSoundKey = "BrokenBuildSound";
		const string FixedBuildSoundKey = "FixedBuildSound";
		const string BrokenBuildTextKey = "BrokenBuildText";
		const string FixedBuildTextKey = "FixedBuildText";
		const string PlaySpeechKey = "PlaySpeech";
		const string SpeechVoiceNameKey = "SpeechVoiceName";
		const string BreakerGuiltStrategyKey = "BreakerGuiltStrategy";

		readonly IList<IConfigObserver> _configObservers = new List<IConfigObserver>();
		static readonly ILog _log = LogManager.GetLogger(typeof(ConfigSettings).Name);
		IDictionary<string, string> _usernameMap = new Dictionary<string, string>();
		readonly UserNameMappingReader _userNameMappingReader = new UserNameMappingReader(new ConfigLocation());

		private string _url;
		public string URL
		{
			get { return _url; }
			set
			{
				if (_url == value) return;
				_url = value;
				Notify("URL");
			}
		}

		private string _skinName;
		public string SkinName
		{
			get { return _skinName; }
			set
			{
				if (_skinName == value) return;
				_skinName = value;
				Notify("SkinName");
			}
		}

		/// <summary> interval at which to poll (in seconds) </summary>
		private int  _pollFrequency;
		public int PollFrequency
		{
			get { return _pollFrequency; }
			set
			{
				if (_pollFrequency == value) return;
				_pollFrequency = value;
				Notify("PollFrequency");
			}
		}

		private string _projectNameRegEx;
		public string ProjectNameRegEx
		{
			get { return _projectNameRegEx.GetRegEx(); }
			set
			{
				if (_projectNameRegEx == value) return;
				_projectNameRegEx = value;
				Notify("ProjectNameRegEx");
			}
		}

		private string _categoryRegEx;
		public string CategoryRegEx
		{
			get { return _categoryRegEx.GetRegEx(); }
			set
			{
				if (_categoryRegEx == value) return;
				_categoryRegEx = value;
				Notify("CategoryRegEx");
			}
		}

		private bool _showCountdown;
		public bool ShowCountdown
		{
			get { return _showCountdown; }
			set
			{
				if (_showCountdown == value) return;
				_showCountdown = value;
				Notify("ShowCountdown");
			}
		}

		private bool _showProgress;
		public bool ShowProgress
		{
			get { return _showProgress; }
			set
			{
				if (_showProgress == value) return;
				_showProgress = value;
				Notify("ShowProgress");
			}
		}

		public IDictionary<string, string> UsernameMap
		{
			get { return _usernameMap; }
			set { _usernameMap = value; }
		}

		private string _brokenBuildSound;
		public string BrokenBuildSound
		{
			get { return _brokenBuildSound; }
			set
			{
				if (_brokenBuildSound == value) return;
				_brokenBuildSound = value;
				Notify("BrokenBuildSound");
			}
		}

		private string _fixedBuildSound;
		public string FixedBuildSound
		{
			get { return _fixedBuildSound; }
			set
			{
				if (_fixedBuildSound == value) return;
				_fixedBuildSound = value;
				Notify("FixedBuildSound");
			}
		}

		private string _brokenBuildText;
		public string BrokenBuildText
		{
			get { return _brokenBuildText; }
			set
			{
				if (_brokenBuildText == value) return;
				_brokenBuildText = value;
				Notify("BrokenBuildText");
			}
		}

		private string _fixedBuildText;
		public string FixedBuildText
		{
			get { return _fixedBuildText; }
			set
			{
				if (_fixedBuildText == value) return;
				_fixedBuildText = value;
				Notify("FixedBuildText");
			}
		}

		private bool _playSounds;
		public bool PlaySounds
		{
			get { return _playSounds; }
			set
			{
				if (_playSounds == value) return;
				_playSounds = value;
				Notify("PlaySounds");
			}
		}

		private bool _playSpeech;
		public bool PlaySpeech
		{
			get { return _playSpeech; }
			set
			{
				if (_playSpeech == value) return;
				_playSpeech = value;
				Notify("PlaySpeech");
			}
		}

		private string _speechVoiceName;
		public string SpeechVoiceName
		{
			get { return _speechVoiceName; }
			set
			{
				if (_speechVoiceName == value) return;
				_speechVoiceName = value;
				Notify("SpeechVoiceName");
			}
		}

		private string _breakerGuiltStrategy;
		public GuiltStrategyType BreakerGuiltStrategy
		{
			get { return _breakerGuiltStrategy == "First" ? GuiltStrategyType.First : GuiltStrategyType.Last; }
		}

		public TimeSpan PollFrequencyTimeSpan
		{
			get { return TimeSpan.FromSeconds(PollFrequency); }
		}

		public void Save()
		{
			try
			{
				var config = OpenExeConfiguration();

				config.AppSettings.Settings[UrlKey].Value = URL;
				config.AppSettings.Settings[SkinKey].Value = SkinName;
				config.AppSettings.Settings[PollFrequencyKey].Value = PollFrequency.ToString();
				config.AppSettings.Settings[ProjectNameRegexKey].Value = ProjectNameRegEx;
				config.AppSettings.Settings[CategoryRegexKey].Value = CategoryRegEx;
				config.AppSettings.Settings[ShowCountdownKey].Value = ShowCountdown.ToString();
				config.AppSettings.Settings[ShowProgressKey].Value = ShowProgress.ToString();
                config.AppSettings.Settings[PlaySoundsKey].Value = PlaySounds.ToString();
                config.AppSettings.Settings[PlaySpeechKey].Value = PlaySpeech.ToString();
				config.AppSettings.Settings[BrokenBuildSoundKey].Value = BrokenBuildSound;
				config.AppSettings.Settings[FixedBuildSoundKey].Value = FixedBuildSound;
				config.AppSettings.Settings[BrokenBuildTextKey].Value = BrokenBuildText;
				config.AppSettings.Settings[FixedBuildTextKey].Value = FixedBuildText;
				config.AppSettings.Settings[SpeechVoiceNameKey].Value = SpeechVoiceName;
				config.AppSettings.Settings[BreakerGuiltStrategyKey].Value = _breakerGuiltStrategy;
				config.Save(ConfigurationSaveMode.Minimal);
			}
			catch (Exception ex)
			{
				// config may be edited in the file (manually) - we cannot show an error dialog here
				// because it's entirely reasonable that the user doesn't have access to the machine running the exe to close it
				_log.Error(ex.Message, ex);		
			}
		}

	    public void Load()
	    {
	        var config = OpenExeConfiguration();

            URL = config.GetProperty(UrlKey).Value;
			SkinName = config.GetProperty(SkinKey).Value;
            PollFrequency = config.GetIntProperty(PollFrequencyKey, DefaultPollingFrequency);
			ProjectNameRegEx = config.GetProperty(ProjectNameRegexKey).Value;
			CategoryRegEx = config.GetProperty(CategoryRegexKey).Value;
            ShowCountdown = config.GetBoolProperty(ShowCountdownKey);
            ShowProgress = config.GetBoolProperty(ShowProgressKey);
			PlaySounds = config.GetBoolProperty(PlaySoundsKey);
			PlaySpeech = config.GetBoolProperty(PlaySpeechKey);
			BrokenBuildSound = config.GetProperty(BrokenBuildSoundKey).Value;
			FixedBuildSound = config.GetProperty(FixedBuildSoundKey).Value;
			BrokenBuildText = config.GetProperty(BrokenBuildTextKey).Value;
			FixedBuildText = config.GetProperty(FixedBuildTextKey).Value;
			SpeechVoiceName = config.GetProperty(SpeechVoiceNameKey).Value;
			_breakerGuiltStrategy = config.GetProperty(BreakerGuiltStrategyKey).Value;

	    	_usernameMap = _userNameMappingReader.Read();
	    }

		private static Configuration OpenExeConfiguration()
		{
			return ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
		}

		public void AddObserver(IConfigObserver observer)
		{
			_configObservers.Add(observer);
		}

		public void NotifyObservers()
		{
			foreach (var observer in _configObservers)
			{
				observer.ConfigUpdated(this);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void Notify(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public override string ToString()
		{
			return string.Format("Url={0}, SkinName={1}, PollFrequency={2}, ProjectNameRegEx={3}, ShowCountdown={4}, ShowCountdown={5}, PlaySounds={6}, PlaySpeech={7}, BrokenBuildSound={8}, BrokenBuildText={9}, FixedBuildSound={10}, FixedBuildText={11}, SpeechVoiceName={12}, CategoryRegEx={13}, BreakerGuiltStrategy={14}",
								 _url, _skinName, _pollFrequency, _projectNameRegEx, _showCountdown, _showProgress, _playSounds, _playSpeech, _brokenBuildSound, _brokenBuildText, _fixedBuildSound, _fixedBuildText, _speechVoiceName, _categoryRegEx, _breakerGuiltStrategy);
		}
	}

	public enum GuiltStrategyType
	{
		First,
		Last
	}

	public class ConfigSettingsException : Exception
    {
        public ConfigSettingsException(string key) : 
			base(string.Format("Configuration setting missing: '{0}'", key)) 
		{  }
    }
}