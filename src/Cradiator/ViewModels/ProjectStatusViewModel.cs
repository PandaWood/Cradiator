using System;
using System.Windows;

namespace Cradiator.ViewModels
{
	public class ProjectStatusViewModel : NotifyingClass
	{
		private string _name;
		private string _currentState;
		private string _CurrentMessage;
		private string _ProjectActivity;
		private string _serverName;
		private bool _isBroken;
		private bool _isSuccessful;
		private Visibility _serverNameVisible;
		private DateTime _lastBuildTime;

		[Obsolete("only used by xaml")]
		public ProjectStatusViewModel()
		{
		}


		public ProjectStatusViewModel(Model.ProjectStatus ps, Config.IViewSettings vs)
		{
			this.CurrentMessage = ps.CurrentMessage;
			this.CurrentState = ps.CurrentState;
			this.IsBroken = ps.IsBroken;
			this.IsSuccessful = ps.IsSuccessful;
			this.Name = ps.Name;
			this.ProjectActivity = ps.ProjectActivity.ToString();
			this.ServerName = ps.ServerName;
			this.LastBuildTime = ps.LastBuildTime;

			this.ServerNameVisible = vs.ShowServerName ? Visibility.Visible : Visibility.Collapsed;
		}

		#region ProjectStatusProps

		public string Name
		{
			get => _name;
			set
			{
				if (_name == value) return;
				_name = value;
				Notify("Name");
			}
		}

		public string CurrentState
		{
			get => _currentState;
			set
			{
				if (_currentState == value) return;
				_currentState = value;
				Notify("CurrentState");
			}
		}

		public string CurrentMessage
		{
			get => _CurrentMessage;
			set
			{
				if (_CurrentMessage == value) return;
				_CurrentMessage = value;
				Notify("CurrentMessage");
			}
		}

		public string ProjectActivity
		{
			get => _ProjectActivity;
			set
			{
				if (_ProjectActivity == value) return;
				_ProjectActivity = value;
				Notify("ProjectActivity");
			}
		}

		public bool IsBroken
		{
			get => _isBroken;
			set
			{
				if (_isBroken == value) return;
				_isBroken = value;
				Notify("IsBroken");
			}
		}

		public bool IsSuccessful
		{
			get => _isSuccessful;
			set
			{
				if (_isSuccessful == value) return;
				_isSuccessful = value;
				Notify("IsSuccessful");
			}
		}


		public string ServerName
		{
			get => _serverName;

			set
			{
				if (_serverName == value) return;
				_serverName = value;
				Notify("ServerName");
			}
		}


		public DateTime LastBuildTime
		{
			get => _lastBuildTime;
			set
			{
				if (_lastBuildTime == value) return;
				_lastBuildTime = value;
				Notify("LastBuildTime");
			}
		}


		#endregion


		#region SettingProps

		public Visibility ServerNameVisible
		{
			get => _serverNameVisible;
			set
			{
				if (_serverNameVisible == value) return;
				_serverNameVisible = value;
				Notify("ServerNameVisible");
			}
		}

		#endregion
	}
}
