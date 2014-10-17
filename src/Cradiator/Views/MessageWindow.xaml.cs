using System;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Cradiator.Views
{
	public partial class MessageWindow
	{
		const double PercentageOfPollFrequency = 1.2;

		readonly ICradiatorView _mainView;
		DispatcherTimer _timer;
		private Storyboard _fadeStoryBoard;

		public MessageWindow()
		{
			InitializeComponent();
		}

		public MessageWindow(ICradiatorView view) : this()
		{
			_mainView = view;
		}

		public void ShowMessage(int pollFrequency, string message)
		{
			Message.Text = message;

			// set the timer to auto-close this window before next screenupdate (delay = 80% of pollFrequency)
			_timer = new DispatcherTimer
					 {
						Interval = TimeSpan.FromSeconds(pollFrequency / PercentageOfPollFrequency)
					 };

			_timer.Tick += Timer_Tick;
			_timer.Start();

			if (_mainView != null)
			{
				_mainView.ScreenUpdating += MainScreenUpdating;
				_mainView.Closing += MainWindowClosing;
			}

			Show();
		}

		void MainWindowClosing(object sender, CancelEventArgs e)
		{
			Close();
		}

		// if main screen was updated abruptly/manually (ie outside the timer-mechanism) then force a close
		void MainScreenUpdating(object sender, EventArgs e)
		{
			_timer.Stop();
			CloseWithFade();
		}

		private void Timer_Tick(object timer, EventArgs e)
		{
			((DispatcherTimer)timer).Stop();
			CloseWithFade();
		}

		private void CloseWithFade()
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
			{
				_fadeStoryBoard = (Storyboard) FindResource("FadeAway");
				_fadeStoryBoard.Completed += FadeAway_Completed;
				BeginStoryboard(_fadeStoryBoard);
			}));
		}

		private void FadeAway_Completed(object sender, EventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (_mainView != null)
			{
				_mainView.ScreenUpdating -= MainScreenUpdating;
				_mainView.Closing -= MainWindowClosing;
			}
			if (_timer != null)
			{
				_timer.Tick -= Timer_Tick;
			}
			if (_fadeStoryBoard != null)
			{
				_fadeStoryBoard.Completed -= FadeAway_Completed;
			}

			base.OnClosing(e);
		}
	}
}